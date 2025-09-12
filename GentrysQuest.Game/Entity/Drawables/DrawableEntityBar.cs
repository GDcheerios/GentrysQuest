using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables;

public partial class DrawableEntityBar : CompositeDrawable
{
    // objects
    public readonly ProgressBar HealthProgressBar;
    public readonly ProgressBar TenacityBar;
    public FillFlowContainer StatusEffects;
    public SpriteText EntityName;
    public SpriteText EntityLevel;
    public SpriteText HealthText;

    public DrawableEntityBar(Entity entity)
    {
        Anchor = Anchor.TopCentre;
        Origin = Anchor.BottomCentre;
        RelativeSizeAxes = Axes.Both;
        RelativePositionAxes = Axes.Both;
        Y -= 0.1f;
        Size = new Vector2(1.25f, 0.125f);
        InternalChildren = new Drawable[]
        {
            EntityName = new SpriteText
            {
                Text = entity.Name,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.BottomCentre,
                Font = FontUsage.Default.With(size: 50)
            },
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Child = EntityLevel = new SpriteText
                        {
                            Text = entity.Experience.Level.Current.Value.ToString(),
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Font = FontUsage.Default.With(size: 32)
                        }
                    },
                    HealthProgressBar = new ProgressBar
                    {
                        RelativeSizeAxes = Axes.X,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(0.98f, 20)
                    },
                    TenacityBar = new ProgressBar
                    {
                        BackgroundColour = Colour4.Transparent,
                        ForegroundColour = Colour4.Yellow,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.X,
                        Y = -10,
                        Size = new Vector2(0.98f, 2)
                    },
                    HealthText = new SpriteText
                    {
                        Text = "0",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = FontUsage.Default.With(size: 24)
                    },
                }
            },
            StatusEffects = new FillFlowContainer
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Direction = FillDirection.Horizontal,
            }
        };
        HealthProgressBar.ForegroundColour = Colour4.Lime;
        HealthProgressBar.BackgroundColour = Colour4.Red;

        entity.OnHealthEvent += delegate
        {
            HealthProgressBar.Current.Value = (float)entity.Stats.Health.Current.Value;
            HealthText.Text = entity.Stats.Health.Current.Value.ToString();
            TenacityBar.Current.Value = entity.CurrentTenacity;
            TenacityBar.Max.Value = (float)entity.Stats.Tenacity.Total();
        };
        entity.OnUpdateStats += delegate
        {
            EntityLevel.Text = entity.Experience.Level.Current.Value.ToString();
            HealthProgressBar.Current.Value = (float)entity.Stats.Health.GetCurrent();
            HealthProgressBar.Max.Value = (float)entity.Stats.Health.Total();
            HealthText.Text = entity.Stats.Health.Current.Value.ToString();
            TenacityBar.Current.Value = entity.CurrentTenacity;
            TenacityBar.Max.Value = (float)entity.Stats.Tenacity.Total();
        };
        entity.OnDeath += delegate { this.FadeOut(); };
        entity.OnSpawn += delegate { this.FadeIn(); };
        entity.OnEffect += delegate { updateEffects(entity); };
    }

    public void OnlyShowName()
    {
        EntityLevel.Hide();
        HealthProgressBar.Hide();
        TenacityBar.Hide();
        HealthText.Hide();
    }

    public void ShowAll()
    {
        EntityLevel.Show();
        HealthProgressBar.Show();
        TenacityBar.Show();
        HealthText.Show();
    }

    private void updateEffects(Entity entity)
    {
        StatusEffects.Clear();

        foreach (StatusEffect effect in entity.Effects)
        {
            StatusEffects.Add(new DrawableStatusEffect(effect));
        }
    }
}
