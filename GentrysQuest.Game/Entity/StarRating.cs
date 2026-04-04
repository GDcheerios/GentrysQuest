using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Entity
{
    public class StarRating
    {
        private LimitedInt value = new LimitedInt(5, 1);

        public StarRating(int value)
        {
            this.value.Value = value;
        }

        public int Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        public static implicit operator int(StarRating starRating) => starRating.Value;

        private Colour4 getColor(int starRating)
        {
            switch (starRating)
            {
                case 1:
                    return Colour4.Gray;

                case 2:
                    return Colour4.LimeGreen;

                case 3:
                    return Colour4.Aqua;

                case 4:
                    return Colour4.DeepPink;

                case 5:
                    return Colour4.Gold;

                default:
                    return Colour4.Gray;
            }
        }

        public Colour4 GetColor(StarRating starRating) => getColor(starRating.Value);
        public Colour4 GetColor() => getColor(this.Value);
    }
}
