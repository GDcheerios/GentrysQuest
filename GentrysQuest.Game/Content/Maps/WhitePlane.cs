using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Maps.WhitePlaneMap;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class WhitePlane : Map
    {
        private DrawableEntity GentryNpc;

        public WhitePlane()
        {
            Name = "";
            Size = new Vector2(10000);
            SpawnPoint = Size;
        }

        public override void Load()
        {
            Objects.Add(new WhitePlaneBackground());
            Npcs.Add(GentryNpc = new DrawableEntity(new GMoney()));
            GentryNpc.EntityBar.Hide();
        }
    }
}
