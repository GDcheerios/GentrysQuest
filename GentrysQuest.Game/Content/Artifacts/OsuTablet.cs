using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class OsuTablet : Artifact
    {
        public override List<StatType> ValidMainAttributes { get; set; } = [StatType.CritRate];
        public override string Name { get; set; } = "Osu Tablet";

        public override string Description { get; protected set; } = "Brayden's Osu Tablet. "
                                                                     + "[condition]Every 10 hits[/condition][details]-1 for the star rating[/details] "
                                                                     + "guarantees you to hit a [type]critical attack[/type] with an additional "
                                                                     + "[unit]50%[/unit][details] + 20% per stack[/details] damage.";

        public override void OnEquip(Entity.Entity entity) => entity.OnGetHit += handleHit;
        public override void OnUnequip(Entity.Entity entity) => entity.OnGetHit -= handleHit;

        private void handleHit(DamageDetails details)
        {
            if (Holder.Weapon != null && Holder.Weapon.AttackAmount % (10 - StarRating.Value) == 0)
            {
                int percent = 50 + (20 * Stack);
                details.Damage += (int)MathBase.GetPercent(details.Damage, percent);
                details.IsCrit = true;
            }
        }

        public OsuTablet()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_brayden_messerschmidt_osu_tablet.png");
        }
    }
}
