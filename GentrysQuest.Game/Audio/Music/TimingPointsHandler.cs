using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Audio.Music
{
    public class TimingPointsHandler
    {
        private readonly List<TimingPoint> timingPoints = [];

        public void AddPoint(TimingPoint newPoint) => timingPoints.Add(newPoint);

        public int GetPoint(string name) => (from timingPoint in timingPoints where timingPoint.Name == name select timingPoint.TimeMs).FirstOrDefault();
    }
}
