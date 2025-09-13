using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class Artifact : EntityBase
    {
        public virtual Family family
        {
            get { throw new NotImplementedException(); }
            protected set { throw new NotImplementedException(); }
        }

        public Buff MainAttribute { get; set; }
        public List<Buff> Attributes { get; set; }
        public virtual List<StatType> ValidMainAttributes { get; set; } = new();
        public virtual List<int> ValidStarRatings { get; set; } = new() { 1, 2, 3, 4, 5 };
        public virtual AllowedPercentMethod AllowedPercentMethod { get; set; } = AllowedPercentMethod.Allowed;
        public Character Holder;

        public Artifact()
        {
            Initialize(ValidStarRatings[Random.Shared.Next(ValidStarRatings.Count)]);
        }

        public override void LevelUp()
        {
            if (Experience.Level.Current.Value >= Experience.Level.Limit.Value) return;

            base.LevelUp();
            Difficulty = (byte)(1 + Experience.Level.Current.Value / 4);
            if (Experience.Level.Current.Value % 4 == 0) AddBuff();
            MainAttribute.Improve();
            Holder?.UpdateStats();
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

                case AllowedPercentMethod.NotAllowed:
                    break;

                case AllowedPercentMethod.OnlyPercent:
                    isPercent = true;
                    break;
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
                MainBuff = MainAttribute.ToJson(),
                FamilyName = family.Name
            };
            List<JsonBuff> buffs = new List<JsonBuff>();
            foreach (Buff buff in Attributes) buffs.Add(buff.ToJson());
            jsonEntity.Buffs = buffs;

            return jsonEntity;
        }

        public void LoadJson(JsonArtifact jsonArtifact)
        {
            LoadJsonBase(jsonArtifact);
            MainAttribute = new Buff(jsonArtifact.MainBuff);
            foreach (JsonBuff jsonBuff in jsonArtifact.Buffs) Attributes.Add(new Buff(jsonBuff));
        }

        public override void CalculateXpRequirement()
        {
            Experience.Xp.Requirement.Value = (Experience.CurrentLevel() * (100 * StarRating)) + ((Experience.CurrentLevel() / 4) * (1000 * StarRating));
        }

        private void initializeAttributes()
        {
            Attributes = new();
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

                if (!duplicate)
                {
                    newBuff.ParentEntity = this;
                    Attributes.Add(newBuff);
                }
            }
        }
    }
}
