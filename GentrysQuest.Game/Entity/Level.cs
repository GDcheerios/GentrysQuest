using osu.Framework.Bindables;

namespace GentrysQuest.Game.Entity
{
    public class Level
    {
        public Bindable<int> Current { get; private set; } = new Bindable<int>(0);
        public Bindable<int> Limit { get; } = new Bindable<int>(0);

        public Level(int current = 1)
        {
            Current.Value = current;
            Limit.Value = 0;
        }

        public Level(int current, int limit)
        {
            Current.Value = current;
            Limit.Value = limit;
        }

        public void AddLevel()
        {
            if (!IsMax()) Current.Value++;
        }

        public bool IsMax() => Current.Value == Limit.Value;

        public override string ToString()
        {
            return $"Level {Current.Value}{(Limit.Value > 0 ? $"/{Limit.Value}" : "")}";
        }
    }
}
