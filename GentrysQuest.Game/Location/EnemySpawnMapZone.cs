using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Location
{
    public partial class EnemySpawnZone : MapZone
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Colour = Colour4.Yellow;
        }
    }
}
