using System.Linq;
using System.Threading.Tasks;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory;

public partial class ItemDisplayContainer : Container
{
    // main variables
    [CanBeNull]
    private EntityBase entity;

    private InventoryOverlay inventoryReference;

    // design
    private TextureStore textureStore;
    private TextFlowContainer nameContainer;
    private Sprite icon;
    private TaggedTextContainer descriptionContainer;
    private StarRatingContainer starRatingContainer;
    private StatDrawableContainer statContainer;
    private ProgressBar experienceBar;
    private SpriteText levelText;
    private SpriteText xpText;
    private InventoryButton levelUpButton;
    private Container equipContainer;
    private Container upgradeContainer;
    private Container equipsContainer;

    private const int DELAY = 25;

    public ItemDisplayContainer(InventoryOverlay inventoryReference) => this.inventoryReference = inventoryReference;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        this.textureStore = textureStore;
        RelativeSizeAxes = Axes.Both;
        Children =
        [
            new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 50,
                Anchor = Anchor.TopLeft,
                Margin = new MarginPadding { Top = 10 },
                Child = nameContainer = new TextFlowContainer
                {
                    Margin = new MarginPadding { Left = 25 },
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                }
            },
            new FillFlowContainer
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Y = 75,
                Margin = new MarginPadding { Left = 25 },
                Children =
                [
                    icon = new Sprite
                    {
                        Size = new Vector2(200),
                        Margin = new MarginPadding { Top = 10 }
                    },
                    starRatingContainer = new StarRatingContainer(1)
                    {
                        Size = new Vector2(200, 40),
                        Margin = new MarginPadding { Top = 10 }
                    },
                    new Container
                    {
                        Margin = new MarginPadding { Top = 10 },
                        Size = new Vector2(200, 100),
                        Children =
                        [
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                Size = new Vector2(1, 40),
                                Children =
                                [
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Size = new Vector2(1, 20),
                                        Children =
                                        [
                                            levelText = new SpriteText
                                            {
                                                Text = "",
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft
                                            },
                                            xpText = new SpriteText
                                            {
                                                Text = "",
                                                Anchor = Anchor.CentreRight,
                                                Origin = Anchor.CentreRight
                                            }
                                        ]
                                    },
                                    new Container
                                    {
                                        Masking = true,
                                        CornerExponent = 2,
                                        CornerRadius = 10,
                                        RelativeSizeAxes = Axes.X,
                                        Size = new Vector2(1, 20),
                                        Y = 20,
                                        Child = experienceBar = new ProgressBar(0, 1)
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            ForegroundColour = Colour4.LightBlue
                                        }
                                    }
                                ]
                            }
                        ]
                    },
                ]
            },
            descriptionContainer = new TaggedTextContainer
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Size = new Vector2(250, 100),
                X = 230,
                Y = 75
            },
            new Container
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Size = new Vector2(350, 400),
                Y = 50,
                Margin = new MarginPadding { Right = 25 },
                Child = statContainer = new StatDrawableContainer()
            },
            new Container
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                RelativeSizeAxes = Axes.X,
                Height = 250,
                Children =
                [
                    levelUpButton = new InventoryButton("Level Up")
                    {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Margin = new MarginPadding { Left = 25 },
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.25f, 64),
                        Y = -100
                    },
                    equipContainer = new Container
                    {
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        Size = new Vector2(0.75f, 1),
                        Children =
                        [
                            new SpriteText
                            {
                                Text = "Weapon",
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                X = 50,
                                Y = 25
                            },
                            new SpriteText
                            {
                                Text = "Artifacts",
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                X = 300,
                                Y = 25
                            },
                            equipsContainer = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        ]
                    },
                    upgradeContainer = new Container
                    {
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        Size = new Vector2(0.75f, 1),
                        Children =
                        [
                        ]
                    }
                ]
            },
        ];
    }

    public async Task SetEntity(EntityBase entity)
    {
        levelUpButton.Action = () => handleLevelUp(entity);

        const int textDuration = 200;
        const int transitionDuration = 100;
        const int iconStartDelay = 50;
        const int starRatingStartDelay = 100;

        nameContainer.Clear();

        float individualCharacterTime = textDuration / entity.Name.Length;

        for (var index = 0; index < entity.Name.Length; index++)
        {
            char character = entity.Name[index];
            SpriteText charcterSpriteText = new SpriteText
            {
                Text = character.ToString(),
                Font = new FontUsage(size: 48),
                Alpha = 0,
                Scale = new Vector2(0, 1)
            };
            Scheduler.AddDelayed(() =>
            {
                nameContainer.AddText(charcterSpriteText);
                charcterSpriteText.FadeInFromZero(individualCharacterTime);
                charcterSpriteText.ScaleTo(1, individualCharacterTime);
            }, index * individualCharacterTime);
        }

        icon.Texture = textureStore.Get(entity.TextureMapping!.Get("Icon"));
        icon.FadeOut().ScaleTo(new Vector2(1, 1)).Then()
            .Delay(iconStartDelay).FadeInFromZero(transitionDuration).ScaleTo(new Vector2(1, 1), transitionDuration);

        starRatingContainer.starRating.Value = 0;
        starRatingContainer.FadeOut().Then()
                           .Delay(starRatingStartDelay).Then()
                           .FadeInFromZero(transitionDuration).Finally(_ => starRatingContainer.starRating.Value = entity.StarRating.Value);

        descriptionContainer.Clear();
        descriptionContainer.SetTaggedText(entity.Description);

        individualCharacterTime = textDuration / descriptionContainer.GetSpriteTexts().ToList().Count;

        var counter = 0;

        foreach (Drawable drawable in descriptionContainer.GetSpriteTexts())
        {
            drawable.FadeOut();
            Scheduler.AddDelayed(() => drawable.FadeInFromZero(textDuration), counter * individualCharacterTime);
            counter++;
        }

        statContainer.Clear();

        const int delay = 50;

        switch (entity)
        {
            case Character character:

                var stats = character.Stats.GetStats();

                for (var index = 0; index < stats.Length; index++)
                {
                    var stat = stats[index];
                    Scheduler.AddDelayed(() =>
                    {
                        statContainer.AddStat(new StatDrawable(stat));
                    }, index * delay);
                }

                break;

            case Weapon weapon:
                Scheduler.AddDelayed(() => { statContainer.AddStat(new StatDrawable(weapon.Damage)); }, 1 * delay);
                Scheduler.AddDelayed(() => { statContainer.AddStat(new StatDrawable(weapon.Buff)); }, 2 * delay);
                break;

            case Artifact artifact:
                Scheduler.AddDelayed(() =>
                {
                    statContainer.AddStat(new StatDrawable(artifact.MainAttribute));
                }, 1 * delay);

                for (var index = 0; index < artifact.Attributes.Count; index++)
                {
                    var t = artifact.Attributes[index];
                    Scheduler.AddDelayed(() =>
                    {
                        statContainer.AddStat(new StatDrawable(t));
                    }, (2 + index) * delay);
                }

                break;
        }

        experienceBar.Min = entity.CalculateRequirement(entity.Experience.CurrentLevel(), entity.StarRating.Value);
        experienceBar.Max = entity.Experience.Xp.Requirement.Value;
        experienceBar.Current = 0;
        Scheduler.AddDelayed(() => experienceBar.Current = entity.Experience.CurrentXp(), 500);

        levelText.Text = entity.Experience.CurrentLevel().ToString();
        xpText.Text = $"{entity.Experience.CurrentXp()} / {entity.Experience.Xp.Requirement.Value}";

        equipContainer.FadeOut();
        upgradeContainer.FadeOut();

        switch (entity)
        {
            case Character character:
                equipContainer.FadeInFromZero(transitionDuration);
                equipsContainer.Clear();
                equipsContainer.Add(new EquipPanel(character.Weapon)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Left = 50 },
                    Name = "Weapon",
                    Action = () => { inventoryReference.SwapWeapon(); }
                });

                for (int i = 0; i < 5; i++)
                {
                    var i1 = i;
                    equipsContainer.Add(new EquipPanel(character.Artifacts.Get(i))
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Name = $"Artifact{i}",
                        X = 175 + (i * 85),
                        Action = () => { inventoryReference.SwapArtifact(i1); }
                    });
                }

                break;
        }
    }

    private void handleLevelUp(EntityBase entity)
    {
    }
}
