using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Users
{
    public static class EventSessionInventoryScope
    {
        private static readonly Dictionary<IUser, Snapshot> snapshots = new();

        public static void Begin(IUser user)
        {
            if (user == null)
                return;

            ensureInventoryLists(user);

            if (!snapshots.ContainsKey(user))
            {
                snapshots[user] = new Snapshot
                {
                    Characters = new List<Character>(user.Characters),
                    Artifacts = new List<Artifact>(user.Artifacts),
                    Weapons = new List<Weapon>(user.Weapons),
                    EquippedCharacter = user.EquippedCharacter,
                    Money = user.Money,
                    MoneyAmount = user.MoneyHandler?.Amount.Value ?? user.Money,
                    ExperienceLevel = user.Experience?.Level.Current.Value ?? 1,
                    ExperienceCurrent = user.Experience?.Xp.Current.Value ?? 0,
                    ExperienceRequirement = user.Experience?.Xp.Requirement.Value ?? 0
                };
            }

            resetToEventBaseline(user, snapshots[user]);
        }

        public static void Restart(IUser user)
        {
            if (user == null)
                return;

            if (!snapshots.TryGetValue(user, out Snapshot snapshot))
            {
                Begin(user);
                return;
            }

            resetToEventBaseline(user, snapshot);
        }

        public static void End(IUser user)
        {
            if (user == null)
                return;

            if (!snapshots.TryGetValue(user, out Snapshot snapshot))
            {
                user.SessionMode = UserSessionMode.Normal;
                return;
            }

            ensureInventoryLists(user);
            user.Characters.Clear();
            user.Artifacts.Clear();
            user.Weapons.Clear();
            user.Characters.AddRange(snapshot.Characters);
            user.Artifacts.AddRange(snapshot.Artifacts);
            user.Weapons.AddRange(snapshot.Weapons);
            user.EquippedCharacter = snapshot.EquippedCharacter;
            user.Money = snapshot.Money;
            if (user.MoneyHandler != null) user.MoneyHandler.Amount.Value = snapshot.MoneyAmount;

            if (user.Experience != null)
            {
                user.Experience.Level.Current.Value = snapshot.ExperienceLevel;
                user.Experience.Xp.Current.Value = snapshot.ExperienceCurrent;
                user.Experience.Xp.Requirement.Value = snapshot.ExperienceRequirement;
            }

            snapshots.Remove(user);

            user.SessionMode = UserSessionMode.Normal;
        }

        private static void ensureInventoryLists(IUser user)
        {
            user.Characters ??= [];
            user.Artifacts ??= [];
            user.Weapons ??= [];
        }

        private static void resetToEventBaseline(IUser user, Snapshot snapshot)
        {
            ensureInventoryLists(user);

            user.Characters.Clear();
            user.Artifacts.Clear();
            user.Weapons.Clear();
            user.EquippedCharacter = null;

            user.Money = snapshot.Money;
            if (user.MoneyHandler != null) user.MoneyHandler.Amount.Value = snapshot.MoneyAmount;

            if (user.Experience != null)
            {
                user.Experience.Level.Current.Value = snapshot.ExperienceLevel;
                user.Experience.Xp.Current.Value = snapshot.ExperienceCurrent;
                user.Experience.Xp.Requirement.Value = snapshot.ExperienceRequirement;
            }

            user.SessionMode = UserSessionMode.Event;
        }

        private sealed class Snapshot
        {
            public List<Character> Characters { get; init; }
            public List<Artifact> Artifacts { get; init; }
            public List<Weapon> Weapons { get; init; }
            public Character EquippedCharacter { get; init; }
            public int Money { get; init; }
            public int MoneyAmount { get; init; }
            public int ExperienceLevel { get; init; }
            public int ExperienceCurrent { get; init; }
            public int ExperienceRequirement { get; init; }
        }
    }
}
