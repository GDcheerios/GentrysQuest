using System.Collections.Generic;

namespace GentrysQuest.Game.Entity.Weapon
{
    /// <summary>
    /// A holder of keyframes making an animation.
    /// </summary>
    public class AttackAnimation
    {
        private readonly List<AttackKeyframe> keyframes = [];

        public void AddEvent(AttackKeyframe keyframe) => keyframes.Add(keyframe);

        public List<AttackKeyframe> GetEvents() => keyframes;

        public AttackKeyframe GetLastEvent() => keyframes[^1];
    }
}
