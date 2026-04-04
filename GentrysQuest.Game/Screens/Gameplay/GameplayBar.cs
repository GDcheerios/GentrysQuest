using System;
using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Screens.Gameplay;

public partial class GameplayBar : CompositeDrawable
{
    public SpriteText Label { get; set; }
    public Colour4 BackgroundColour { get; set; } = new Colour4(55, 55, 55, 255);
    public Colour4 ForegroundColour { get; set; } = Colour4.White;

    public Bindable<float> Current { get; } = new Bindable<float>(0);
    public Bindable<float> Max { get; } = new Bindable<float>(1);

    private ProgressBar progressBar = null!;
    private SpriteText valueText = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                X = -10,
                Child = new FillFlowContainer
                {
                    AutoSizeAxes = Axes.X,
                    RelativeSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children =
                    [
                        new Container
                        {
                            Size = new Vector2(150, 32),
                            Margin = new MarginPadding { Left = 10 },
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Size = new Vector2(1.1f, 1.1f),
                                    Colour = ColourInfo.GradientHorizontal(new Colour4(50, 50, 50, 167), new Colour4(0, 0, 0, 0))
                                },
                                Label = new SpriteText
                                {
                                    Text = "",
                                    Font = FontUsage.Default.With(size: 32, weight: "SemiBold"),
                                    Margin = new MarginPadding { Left = 10 },
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Colour = Colour4.White
                                }
                            ]
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Horizontal,
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Children =
                            [
                                progressBar = new ProgressBar
                                {
                                    Masking = true,
                                    BorderThickness = 5,
                                    BorderColour = Colour4.Black,
                                    CornerExponent = 2,
                                    CornerRadius = 6,
                                    Size = new Vector2(100, 25),
                                    Shear = new Vector2(0.35f, 0),
                                    BackgroundColour = BackgroundColour,
                                    ForegroundColour = ForegroundColour
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Y,
                                    Width = 100,
                                    Child = valueText = new SpriteText
                                    {
                                        Font = FontUsage.Default.With(size: 24, weight: "Bold"),
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Colour = Colour4.White,
                                        Margin = new MarginPadding { Bottom = 2 }
                                    }
                                }
                            ]
                        }
                    ]
                }
            }
        ];

        progressBar.Current.BindTo(Current);
        progressBar.Max.BindTo(Max);

        Current.ValueChanged += _ => updateValueText();
        Max.ValueChanged += _ => updateValueText();
        updateValueText();
    }

    private void updateValueText()
    {
        valueText.Text = $"{formatCompact(Current.Value)}/{formatCompact(Max.Value)}";
    }

    public void SetLabel(string label) => Label.Text = label;

    public void SetProgressSize(Vector2 size) => progressBar.Size = size;

    private static string formatCompact(float value)
    {
        float safeValue = Math.Max(0, value);

        if (safeValue >= 1000f)
        {
            float truncatedThousands = MathF.Floor((safeValue / 1000f) * 10f) / 10f;
            return $"{truncatedThousands:0.#}k";
        }

        return ((int)MathF.Floor(safeValue)).ToString();
    }
}
