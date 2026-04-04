using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class Artifact : EntityBase
    {
        public Buff MainAttribute { get; set; }
        public List<Buff> Attributes { get; set; } = [];
        public virtual List<StatType> ValidMainAttributes { get; set; } = new();
        public virtual List<int> ValidStarRatings { get; set; } = new() { 1, 2, 3, 4, 5 };
        public virtual AllowedPercentMethod AllowedPercentMethod { get; set; } = AllowedPercentMethod.Allowed;
        public Character Holder;

        /// <summary>
        /// Abstract method for handling artifact equip events. Subclasses should override this to provide specific behavior.
        /// </summary>
        /// <param name="entity">The entity that is equipping the artifact.</param>
        public virtual void OnEquip(Entity entity) { }

        /// <summary>
        /// Abstract method for handling artifact unequip events. Subclasses should override this to provide specific behavior.
        /// </summary>
        /// <param name="entity">The entity that is unequipping the artifact.</param>
        public virtual void OnUnequip(Entity entity) { }

        /// <summary>
        /// Get the stack of this artifact.
        /// </summary>
        public int Stack => Holder != null ? Holder.Artifacts.GetArtifactCountByName(Name) - 1 : 0;

        public Artifact() => Initialize(MathBase.RandomGachaStarRating());
        public Artifact(int starRating) => Initialize(starRating);

        public override void LevelUp()
        {
            if (Experience.Level.Current.Value >= Experience.Level.Limit.Value) return;

            base.LevelUp();
            Difficulty = (byte)(Experience.Level.Current.Value / 4);
            if (Experience.Level.Current.Value % 4 == 0) AddBuff();
            MainAttribute.Improve();
            Holder?.UpdateStats();
        }

        public override void CalculateXpRequirement()
        {
            int xp = Experience.CurrentLevel() * 100 * StarRating;
            xp += Experience.CurrentLevel() / 4 * 250 * StarRating;
            Experience.Xp.Requirement.Value = xp;
        }

        public void Initialize(int starRating)
        {
            StarRating.Value = starRating;
            StatType stat = Buff.GetRandomStat();
            bool isPercent = false;

            if (ValidMainAttributes.Count > 0) stat = ValidMainAttributes[Random.Shared.Next(ValidMainAttributes.Count)];

            switch (AllowedPercentMethod)
            {
                case AllowedPercentMethod.Allowed:
                    isPercent = MathBase.RandomBool();
                    break;

                case AllowedPercentMethod.OnlyPercent:
                    isPercent = true;
                    break;

                case AllowedPercentMethod.NotAllowed:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            MainAttribute = new Buff(this, stat, isPercent);

            initializeAttributes();
            CalculateXpRequirement();
        }

        public JsonArtifact ToJson()
        {
            JsonArtifact jsonEntity = new JsonArtifact
            {
                Name = Name,
                Level = Experience.CurrentLevel(),
                StarRating = StarRating.Value,
                ID = ID,
                CurrentXp = Experience.CurrentXp(),
                MainBuff = MainAttribute.ToJson()
            };
            List<JsonBuff> buffs = Attributes.Select(buff => buff.ToJson()).ToList();
            jsonEntity.Buffs = buffs;

            return jsonEntity;
        }

        public override void LoadJson(IJsonEntity jsonEntity)
        {
            JsonArtifact jsonArtifact = (JsonArtifact)jsonEntity;
            LoadJsonBase(jsonArtifact);
            MainAttribute = new Buff(jsonArtifact.MainBuff);
            foreach (JsonBuff jsonBuff in jsonArtifact.Buffs) Attributes.Add(new Buff(jsonBuff));
        }

        private void initializeAttributes()
        {
            Attributes = [];
            Experience = new Experience(new Xp(), new Level(1, StarRating.Value * 4));
            int counter = StarRating.Value;

            while (counter > 2)
            {
                AddBuff();
                counter--;
            }
        }

        public void AddBuff(Buff buff) => handleBuff(buff);

        public void AddBuff() => handleBuff(new Buff(this));

        private void handleBuff(Buff newBuff)
        {
            if (newBuff.StatType == MainAttribute.StatType && newBuff.IsPercent == MainAttribute.IsPercent) handleBuff(new Buff(this));
            else
            {
                bool duplicate = false;

                foreach (var buff in Attributes.Where(buff => newBuff.StatType == buff.StatType && newBuff.IsPercent == buff.IsPercent))
                {
                    buff.Improve();
                    duplicate = true;
                }

                if (duplicate) return;

                newBuff.ParentEntity = this;
                Attributes.Add(newBuff);
            }
        }
    }
}
