using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class DrawableBuffIcon : Container
    {
        [CanBeNull]
        private readonly Buff buff;

        private SpriteIcon icon = new SpriteIcon
        {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
            Icon = FontAwesome.Solid.Circle
        };

        private SpriteText stats = new SpriteText
        {
            Text = $"",
            Position = new Vector2(12, 0),
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreLeft,
            Font = FontUsage.Default.With(size: 32)
        };

        private SpriteText name = new SpriteText
        {
            Text = $"Empty",
            Anchor = Anchor.TopCentre,
            Origin = Anchor.BottomCentre,
            Font = FontUsage.Default.With(size: 24)
        };

        public DrawableBuffIcon([CanBeNull] Buff buff, bool sideView = false)
        {
            Size = new Vector2(48);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            if (sideView)
            {
                name.Hide();
                stats.Anchor = Anchor.CentreLeft;
                stats.Origin = Anchor.CentreRight;
                if (buff != null) stats.Text = buff.Level + (buff.IsPercent ? "%" : "");
                stats.Font = FontUsage.Default.With(size: 12);
                stats.Position = new Vector2(-6, 0);
                Padding = new MarginPadding { Top = 2, Bottom = 2 };
            }

            if (buff == null) return;

            this.buff = buff;

            icon.Icon = buff.StatType switch
            {
                StatType.Health => FontAwesome.Solid.Plus,
                StatType.Attack => FontAwesome.Solid.FistRaised,
                StatType.Defense => FontAwesome.Solid.ShieldAlt,
                StatType.CritRate => FontAwesome.Solid.ArrowCircleUp,
                StatType.CritDamage => FontAwesome.Solid.ArrowUp,
                StatType.Speed => FontAwesome.Solid.ShoePrints,
                StatType.AttackSpeed => FontAwesome.Solid.Wind,
                StatType.RegenSpeed => FontAwesome.Solid.Recycle,
                StatType.RegenStrength => FontAwesome.Solid.Vial,
                StatType.Tenacity => FontAwesome.Solid.LayerGroup,
                _ => icon.Icon
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children =
            [
                icon,
                stats,
                name
            ];

            if (buff == null) return;
            stats.Text = $"{buff.Value.Value:0.##}{(buff.IsPercent ? "%" : "")}";
            name.Text = buff.StatType.ToString();
        }
    }
}
