using GentrysQuest.Game.Overlays.SkillOverlay;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Screens.Gameplay
{
    public partial class GameplayHud : CompositeDrawable
    {
        private Entity.Entity entityTracker;

        private Container barsContainer;

        private readonly GameplayBar healthBar;
        private readonly GameplayBar experienceBar;
        private readonly SkillOverlay skillOverlay;

        private readonly SpriteText levelText;

        public GameplayHud()
        {
            RelativeSizeAxes = Axes.Both;
            Depth = -2;

            InternalChildren = new Drawable[]
            {
                barsContainer = new Container()
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,

                    CornerRadius = 4,
                    CornerExponent = 2,
                    Masking = true,

                    Size = new Vector2(0.4f, 0.15f),
                    Margin = new MarginPadding { Left = 30, Bottom = 10 },

                    Children = new Drawable[]
                    {
                        healthBar = new GameplayBar
                        {
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Size = new Vector2(1, 0.5f),
                            ForegroundColour = Colour4.LimeGreen
                        },
                        experienceBar = new GameplayBar
                        {
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.Both,
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            BackgroundColour = Colour4.Gray,
                            ForegroundColour = Colour4.LightBlue,
                            Size = new Vector2(1, 0.5f)
                        },
                        levelText = new SpriteText
                        {
                            Text = "Level 0",
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.TopLeft,
                            RelativePositionAxes = Axes.Both,
                            Font = FontUsage.Default.With(size: 24),
                            Margin = new MarginPadding { Left = 10 },
                            Position = new Vector2(0)
                        }
                    }
                },
                skillOverlay = new SkillOverlay
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(500, 200),
                    Margin = new MarginPadding { Right = 30, Bottom = 20 }
                }
            };
        }

        public void SetEntity(Entity.Entity theEntity)
        {
            theEntity.OnHealthEvent += () =>
            {
                healthBar.Current.Value = (float)theEntity.Stats.Health.GetCurrent();
                healthBar.Max.Value = (float)theEntity.Stats.Health.Total();
            };
            theEntity.Experience.Xp.Current.ValueChanged += _ =>
            {
                experienceBar.Current.Value = (float)theEntity.Experience.Xp.Current.Value;
                experienceBar.Max.Value = (float)theEntity.Experience.Xp.Requirement.Value;
            };
            levelText.Text = $"Level {theEntity.Experience.CurrentLevel()}";
            skillOverlay.ClearSkills();
            skillOverlay.SetUpSkills(theEntity);
        }

        public void Disappear()
        {
            barsContainer.MoveToY(1, 250, Easing.OutQuint);
            skillOverlay.MoveToY(1, 250, Easing.OutQuint);
        }

        public void Appear()
        {
            barsContainer.MoveToY(0, 250, Easing.OutQuint);
            skillOverlay.MoveToY(0, 250, Easing.OutQuint);
        }
    }
}
