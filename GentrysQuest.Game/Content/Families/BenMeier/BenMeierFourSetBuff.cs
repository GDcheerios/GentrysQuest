using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Families.BenMeier
{
    public class BenMeierFourSetBuff : FourSetBuff
    {
        private readonly Entity.Entity.EntityHitEvent buff;

        public BenMeierFourSetBuff()
        {
            buff += details =>
            {
                Entity.Entity sender = details.Sender;
                sender.Damage((int)(details.Damage * 0.25));
            };
        }

        public override void ApplyToCharacter(Character character) => character.OnGetHit += buff;
        public override void RemoveFromCharacter(Character character) => character.OnGetHit -= buff;
        public override string Explanation { get; protected set; } = "Return [unit]25%[/unit] of damage to the attacker.";
    }
}
