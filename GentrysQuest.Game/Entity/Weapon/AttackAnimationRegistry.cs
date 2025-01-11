using System.Collections.Generic;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class AttackAnimationRegistry
    {
        private readonly Dictionary<string, AttackAnimation> animationRegistry = new();
        private AttackAnimation selectedAnimation;

        /// <summary>
        /// Register an animation.
        /// Will automatically select for ease of use.
        /// </summary>
        /// <param name="name">The name for the animation.</param>
        public void RegisterAnimation(string name)
        {
            AttackAnimation newAnimation = new AttackAnimation();
            selectedAnimation = newAnimation;
            animationRegistry.TryAdd(name, newAnimation);
        }

        /// <summary>
        /// Select an animation with a given name.
        /// </summary>
        /// <param name="name">Name of the animation.</param>
        public void SelectAnimation(string name) => selectedAnimation = animationRegistry[name];

        /// <summary>
        /// Adds a keyframe to the selected animation from the registry.
        /// </summary>
        /// <param name="attackKeyframe">The keyframe</param>
        public void AddKeyframe(AttackKeyframe attackKeyframe) => selectedAnimation.AddEvent(attackKeyframe);

        /// <summary>
        /// Obtain an animation with a given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AttackAnimation GetAnimation(string name) => animationRegistry[name];

        public int GetListAmount() => animationRegistry.Count;
    }
}
