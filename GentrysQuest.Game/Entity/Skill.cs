using System;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public abstract class Skill
    {
        public Action OnAct;

        /// <summary>
        /// The name of the skill
        /// </summary>
        public abstract string Name { get; protected set; }

        /// <summary>
        /// The description of the skill
        /// </summary>
        public abstract string Description { get; protected set; }

        /// <summary>
        /// The cooldown for the skill
        /// </summary>
        public abstract double Cooldown { get; protected set; }

        /// <summary>
        /// The time since last start
        /// </summary>
        public double LastUseTime { get; protected set; }

        /// <summary>
        /// How much longer until the skill is ready to use again
        /// </summary>
        /// <returns></returns>
        public int PercentToDone { get; protected set; }

        /// <summary>
        /// The parent to this skill
        /// </summary>
        public DrawableEntity User { get; set; }

        /// <summary>
        /// How many uses of the skill you have
        /// </summary>
        public int UsesAvailable = 1;

        /// <summary>
        /// The maximum uses of a skill you can have at once
        /// </summary>
        public virtual int MaxStack { get; protected set; } = 1;

        /// <summary>
        /// How this skill works
        /// </summary>
        public virtual void Act()
        {
            // base logic
            if (CanUse())
            {
                LastUseTime = GameClock.CurrentTime;
                UsesAvailable--;
                PercentToDone = 0;
                OnAct?.Invoke();
            }

            // override this method to implement logic.
        }

        /// <summary>
        /// The texture mapping for this skill
        /// </summary>
        public TextureMapping TextureMapping { get; protected set; } = new();

        public bool CanUse() => PercentToDone == 100 || UsesAvailable > 0;

        public void SetPercent(double currentTime)
        {
            var elapsedTime = currentTime - LastUseTime;

            if (PercentToDone < 100 && UsesAvailable < MaxStack) { PercentToDone = (int)((elapsedTime / (float)Cooldown) * 100); }
            else
            {
                if (UsesAvailable < MaxStack)
                {
                    UsesAvailable++;
                    PercentToDone = 0;
                    LastUseTime = currentTime;
                }
                else PercentToDone = 100;
            }
        }
    }
}
