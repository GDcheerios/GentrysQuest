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
    private SpriteText difficultyText;
    private InventoryButton levelUpButton;
    private InventoryButton exchangeButton;
    private Container equipContainer;
    private Container equipsContainer;
    private InventoryLevelUpBox levelUpBox;
    private Container moneyControl;

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
                                        Child = experienceBar = new ProgressBar
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
                    new Container
                    {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Margin = new MarginPadding { Left = 25 },
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.25f, 200),
                        Y = -100,
                        Children =
                        [
                            levelUpButton = new InventoryButton("Level Up")
                            {
                                RelativeSizeAxes = Axes.Both,
                                Width = 0.8f,
                                Height = 0.45f
                            },
                            moneyControl = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Width = 1f,
                                Height = 0.45f,
                                Y = 140,
                                Children = new Drawable[]
                                {
                                    levelUpBox = new InventoryLevelUpBox()
                                    {
                                        Width = 150
                                    },
                                    new InventoryButton("-")
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Width = 0.2f,
                                        Height = 0.3f,
                                        X = -100,
                                        Action = () => { levelUpBox.DecreaseAmount(); }
                                    },
                                    new InventoryButton("+")
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Width = 0.2f,
                                        Height = 0.3f,
                                        X = 100,
                                        Action = () => { levelUpBox.IncreaseAmount(); }
                                    },
                                }
                            },
                            exchangeButton = new InventoryButton("Exchange")
                            {
                                RelativeSizeAxes = Axes.Both,
                                Width = 0.8f,
                                Height = 0.3f,
                                Y = 150
                            },
                        ]
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
                    }
                ]
            },
        ];
    }

    public async Task SetEntity(EntityBase entity)
    {
        this.entity = entity;

        const int textDuration = 200;
        const int transitionDuration = 100;
        const int iconStartDelay = 50;
        const int starRatingStartDelay = 100;

        nameContainer.Clear();

        float individualCharacterTime = textDuration / entity.Name.Length;

        for (var index = 0; index < entity.Name.Length; index++)
        {
            char character = entity.Name[index];
            SpriteText characterSpriteText = new SpriteText
            {
                Text = character.ToString(),
                Font = new FontUsage(size: 48),
                Alpha = 0,
                Scale = new Vector2(0, 1)
            };
            Scheduler.AddDelayed(() =>
            {
                nameContainer.AddText(characterSpriteText);
                characterSpriteText.FadeInFromZero(individualCharacterTime);
                characterSpriteText.ScaleTo(1, individualCharacterTime);
            }, index * individualCharacterTime);
        }

        nameContainer.AddText(difficultyText = new SpriteText
        {
            Text = "",
            Font = new FontUsage(size: 48),
            Alpha = 0,
            Scale = Vector2.Zero,
            Margin = new MarginPadding { Right = 10 }
        });

        setDifficulty();

        icon.Texture = textureStore.Get(entity.TextureMapping!.Get("Icon"));
        icon.FadeOut().ScaleTo(new Vector2(1, 1)).Then()
            .Delay(iconStartDelay).FadeInFromZero(transitionDuration).ScaleTo(new Vector2(1, 1), transitionDuration);

        starRatingContainer.starRating.Value = 0;
        starRatingContainer.FadeOut().Then()
                           .Delay(starRatingStartDelay).Then()
                           .FadeInFromZero(transitionDuration).Finally(_ => starRatingContainer.starRating.Value = entity.StarRating.Value);

        descriptionContainer.Clear();
        string description = entity.Description;

        if (entity is Artifact)
        {
            Artifact artifact = (Artifact)entity;
            description += "\nApart of the " + artifact.family.Name + " "
                           + "\nBuffs:";

            string twoSetDescription = "\nTwo Set - "
                                       + artifact.family.TwoSetBuff.BuffExplanation();
            description += twoSetDescription;

            if (artifact.family.FourSetBuff != null)
            {
                string fourSetDescription = "\nFour Set - "
                                            + artifact.family.FourSetBuff.Explanation;
                description += fourSetDescription;
            }
        }

        descriptionContainer.SetTaggedText(description);

        individualCharacterTime = textDuration / descriptionContainer.GetSpriteTexts().ToList().Count;

        var counter = 0;

        foreach (Drawable drawable in descriptionContainer.GetSpriteTexts())
        {
            drawable.FadeOut();
            Scheduler.AddDelayed(() => drawable.FadeInFromZero(textDuration), counter * individualCharacterTime);
            counter++;
        }

        statContainer.Clear();

        createStatContainer();
        handleExperienceChange();

        equipContainer.FadeOut();

        switch (entity)
        {
            case Character character:
                equipContainer.FadeInFromZero(transitionDuration);
                equipsContainer.Clear();
                EquipPanel weaponPanel = new EquipPanel(character.Weapon)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Left = 50 },
                    Name = "Weapon",
                    Action = () => { inventoryReference.ClickWeapon(); }
                };
                weaponPanel.SetRemoveAction(() => inventoryReference.RemoveWeapon());
                weaponPanel.SetSwapAction(() => inventoryReference.SwapWeapon());
                equipsContainer.Add(weaponPanel);

                for (int i = 0; i < 5; i++)
                {
                    var i1 = i;
                    EquipPanel artifactPanel = new EquipPanel(character.Artifacts.Get(i))
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Name = $"Artifact{i}",
                        X = 175 + (i * 85),
                        Action = () => { inventoryReference.ClickArtifact(i1); }
                    };
                    artifactPanel.SetRemoveAction(() => inventoryReference.RemoveArtifact(i1));
                    artifactPanel.SetSwapAction(() => inventoryReference.SwapArtifact(i1));
                    equipsContainer.Add(artifactPanel);
                }

                break;

            default:
                equipContainer.FadeOut();
                break;
        }

        exchangeButton.FadeIn();

        if (entity is Artifact)
        {
            moneyControl.FadeOut();
            levelUpButton.FadeOut();
            exchangeButton.SetAction(inventoryReference.StartArtifactExchange);
        }
        else
        {
            moneyControl.FadeIn();
            levelUpButton.FadeIn();
            levelUpButton.SetAction(handleLevelUp);
            exchangeButton.SetAction(inventoryReference.StartWeaponExchange);
        }

        if (entity is Character)
        {
            exchangeButton.FadeOut();
        }
    }

    private void handleLevelUp()
    {
        if (entity == null) return;

        int previousLevel = entity.Experience.CurrentLevel();

        switch (entity)
        {
            case Character character:
                inventoryReference.LevelUpWithMoney(character, levelUpBox.GetAmount());
                break;

            case Weapon weapon:
                inventoryReference.LevelUpWithMoney(weapon, levelUpBox.GetAmount());
                break;
        }

        if (entity.Experience.CurrentLevel() != previousLevel)
        {
            setDifficulty();
            const int delay = 50;

            switch (entity)
            {
                case Character character:
                    foreach (Stat stat in character.Stats.GetStats())
                    {
                        StatDrawable statDrawable = statContainer.GetStatDrawable(stat.Name);
                        statDrawable.AdditionalValue.Value = stat.Additional.Value;
                        if (statDrawable.Value.Value != stat.Total()) statDrawable.UpdateValue(stat.Total());
                    }

                    break;

                case Weapon weapon:
                    StatDrawable drawable = statContainer.GetStatDrawable("Damage");
                    drawable.AdditionalValue.Value = weapon.Damage.Additional.Value;
                    drawable.UpdateValue(weapon.Damage.Total());

                    drawable = statContainer.GetStatDrawable(weapon.Buff.StatType.ToString());
                    drawable.AdditionalValue.Value = weapon.Buff.Level;
                    drawable.UpdateValue(weapon.Buff.Value.Value);
                    break;

                case Artifact artifact:
                    StatDrawable artifactStat = statContainer.GetStatDrawable(artifact.MainAttribute.StatType.ToString());
                    artifactStat.AdditionalValue.Value = artifact.Experience.Level.Current.Value;
                    artifactStat.UpdateValue(artifact.MainAttribute.Value.Value);

                    for (int i = 0; i < artifact.Attributes.Count; i++)
                    {
                        Buff attribute = artifact.Attributes[i];
                        artifactStat = statContainer.GetStatDrawable(artifact.Attributes[i].StatType.ToString());

                        if (artifactStat == null) statContainer.AddStat(new StatDrawable(attribute));
                        else
                        {
                            artifactStat.AdditionalValue.Value = attribute.Level;
                            artifactStat.UpdateValue(attribute.Value.Value);
                        }
                    }

                    break;
            }
        }

        handleExperienceChange();
    }

    private void handleExperienceChange()
    {
        if (entity == null) return;

        experienceBar.Max.Value = entity.Experience.Xp.Requirement.Value;
        experienceBar.Current.Value = entity.Experience.CurrentXp();
        levelText.Text = entity.Experience.CurrentLevel().ToString();
        xpText.Text = $"{entity.Experience.CurrentXp()} / {entity.Experience.Xp.Requirement.Value}";
    }

    private void createStatContainer()
    {
        const int delay = 50;
        statContainer.Clear();

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
    }

    private void setDifficulty()
    {
        difficultyText.Text = entity?.Difficulty.ToString();
        difficultyText.Colour = Colour4.FromHSV((float)((1 - ((entity?.Difficulty - 1) / 4f)) * 0.333f), 1, 1);
        difficultyText.Delay(500).Then().ScaleTo(1, 200).FadeIn(200);
    }
}
