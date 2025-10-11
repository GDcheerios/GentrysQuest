using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.IO;
using JetBrains.Annotations;

namespace GentrysQuest.Game.Entity
{
    public abstract class EntityBase
    {
        public int ID { get; set; }
        public virtual string Name { get; set; } = "Entity";
        public virtual StarRating StarRating { get; protected set; } = new StarRating(1);
        public virtual string Description { get; protected set; } = "This is a description";
        public Experience Experience { get; protected set; } = new();

        [CanBeNull]
        public TextureMapping TextureMapping { get; protected set; } = null;

        public DrawableTexture DrawableTexture { get; protected set; } = new();

        public AudioMapping AudioMapping { get; protected set; } = new();
        public byte Difficulty { get; protected set; }

        public delegate void EntityEvent();

        // Experience events
        public event EntityEvent OnGainXp;
        public event EntityEvent OnLevelUp;

        public void AddXp(int amount)
        {
            while (Experience.Xp.add_xp(ref amount)) LevelUp();
            OnGainXp?.Invoke();
        }

        public virtual void LevelUp()
        {
            Experience.Level.AddLevel();
            CalculateXpRequirement();
            OnLevelUp?.Invoke();
        }

        public int CalculateRequirement(int level, int starRating)
        {
            int difficulty = (level / 20);
            int starRatingExperience = starRating * 25;
            int levelExperience = level * 10;

            return level * difficulty * difficulty * 100 + levelExperience + starRatingExperience;
        }

        public void LoadJsonBase(IJsonEntity jsonEntity)
        {
            ID = jsonEntity.ID;
            Experience.Level.Current.Value = jsonEntity.Level;
            Experience.Xp.Current.Value = jsonEntity.CurrentXp;
        }

        public virtual void CalculateXpRequirement() => Experience.Xp.Requirement.Value = CalculateRequirement(Experience.CurrentLevel(), StarRating.Value);
    }
}
