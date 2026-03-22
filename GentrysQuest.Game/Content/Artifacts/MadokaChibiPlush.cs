using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class MadokaChibiPlush : Artifact
    {
        public override string Name { get; set; } = "Madoka Chibi Plush";

        public override string Description { get; protected set; } = "Brayden's trusty old Madoka Chibi Plush! "
                                                                     + "[condition]When hit[/condition] you have a [unit]10% chance[/unit] [details]+ 5% per stack[/details] to not take damage.";

        public override int? ContentID { get; set; } = 1;

        public override void OnEquip(Entity.Entity entity) => entity.OnGetHit += handleHit;

        public override void OnUnequip(Entity.Entity entity) => entity.OnGetHit -= handleHit;

        private bool procs() => MathBase.RandomPercent() < 10 + Stack * 5;

        private void handleHit(DamageDetails details)
        {
            if (procs()) details.Damage = 0;
        }

        public MadokaChibiPlush()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_brayden_messerschmidt_madoka_chibi_plush.jpg");
        }
    }
}
