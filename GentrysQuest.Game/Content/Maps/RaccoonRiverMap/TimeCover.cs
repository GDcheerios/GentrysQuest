using GentrysQuest.Game.Location;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Maps.RaccoonRiverMap
{
    /// <summary>
    /// TimeCover class represents a 
    /// </summary>
    public partial class TimeCover : MapObject
    {
        private readonly int cycleTime;
        private readonly int dayStrength;
        private readonly int nightStrength;

        public TimeCover(int cycleTime, int dayStrength, int nightStrength)
        {
            Name = "Time Cover";

            this.cycleTime = cycleTime;
            this.dayStrength = dayStrength;
            this.nightStrength = nightStrength;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Colour = new Colour4(0, 0, 0, 255);
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            this.FadeTo(dayStrength, cycleTime * 0.1).Then()
                .Delay(dayStrength).Then()
                .FadeTo(nightStrength, cycleTime * 0.1).Then()
                .Delay(cycleTime);
        }
    }
}
