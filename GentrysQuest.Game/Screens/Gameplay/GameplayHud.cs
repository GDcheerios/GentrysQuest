using GentrysQuest.Game.Entity;
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
        private EntityBase.EntityEvent healthEventHandler;
        private Entity.Entity.SwapWeaponEvent weaponSwapHandler;

        private FillFlowContainer barsContainer;

        private readonly GameplayBar healthBar;
        private readonly GameplayBar experienceBar;
        private readonly SkillOverlay skillOverlay;

        private readonly SpriteText levelText;

        public GameplayHud()
        {
            RelativeSizeAxes = Axes.Both;
            Depth = -2;

            InternalChildren =
            [
                barsContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    AutoSizeAxes = Axes.X,
                    Margin = new MarginPadding { Bottom = 20 },
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Children =
                    [
                        experienceBar = new GameplayBar
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Position = new Vector2(0, 0),
                            Size = new Vector2(500, 75),
                            BackgroundColour = new Colour4(70, 70, 70, 255),
                            ForegroundColour = new Colour4(149, 239, 255, 255)
                        },
                        healthBar = new GameplayBar
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Position = new Vector2(0, -56),
                            Size = new Vector2(500, 150),
                            BackgroundColour = new Colour4(70, 70, 70, 255),
                            ForegroundColour = new Colour4(28, 201, 84, 255)
                        }
                    ]
                },
                skillOverlay = new SkillOverlay
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(500, 200),
                    Margin = new MarginPadding { Right = 30, Bottom = 20 }
                }
            ];
        }

        public void SetEntity(Entity.Entity theEntity)
        {
            if (entityTracker != null)
            {
                if (healthEventHandler != null) entityTracker.OnHealthEvent -= healthEventHandler;
                if (weaponSwapHandler != null) entityTracker.OnSwapWeapon -= weaponSwapHandler;
            }

            healthBar.SetProgressSize(new Vector2(400, 75));
            healthBar.SetLabel("Health");
            experienceBar.SetProgressSize(new Vector2(350, 25));
            experienceBar.SetLabel($"Level: {theEntity.Experience.CurrentLevel()}");

            entityTracker = theEntity;

            healthEventHandler = () =>
            {
                healthBar.Current.Value = (float)entityTracker.Stats.Health.GetCurrent();
                healthBar.Max.Value = (float)entityTracker.Stats.Health.Total();
            };
            entityTracker.OnHealthEvent += healthEventHandler;
            entityTracker.OnUpdateStats += healthEventHandler;

            weaponSwapHandler = _ =>
            {
                skillOverlay.ClearSkills();
                skillOverlay.SetUpSkills(entityTracker);
            };
            entityTracker.OnSwapWeapon += weaponSwapHandler;

            theEntity.Experience.Xp.Current.ValueChanged += _ =>
            {
                experienceBar.Current.Value = (float)theEntity.Experience.Xp.Current.Value;
                experienceBar.Max.Value = (float)theEntity.Experience.Xp.Requirement.Value;
            };
            theEntity.Experience.Level.Current.ValueChanged += _ => experienceBar.SetLabel($"Level: {theEntity.Experience.CurrentLevel()}");
            healthBar.Current.Value = (float)theEntity.Stats.Health.GetCurrent();
            healthBar.Max.Value = (float)theEntity.Stats.Health.Total();
            experienceBar.Current.Value = (float)theEntity.Experience.Xp.Current.Value;
            experienceBar.Max.Value = (float)theEntity.Experience.Xp.Requirement.Value;
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
