using System;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class SkillDrawable : CompositeDrawable
    {
        private readonly bool hasSkill;
        private readonly Skill skillReference;
        private readonly SpriteText skillName;
        private readonly ProgressBar percentageDisplay;
        private readonly Sprite skillDisplay;
        private readonly SpriteText skillStack;
        private readonly SpriteText cooldownText;

        public SkillDrawable(Skill skillReference, string keyBind = "")
        {
            if (skillReference == null)
            {
                hasSkill = false;
                Hide();
            }
            else
            {
                hasSkill = true;
                Width = 112;
                Height = 180;
                this.skillReference = skillReference;

                InternalChildren =
                [
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = 8,
                        CornerExponent = 2,
                        Children =
                        [
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = new Colour4(0, 0, 0, 140)
                            },
                            new Container
                            {
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                Position = new Vector2(6),
                                Size = new Vector2(30, 18),
                                Masking = true,
                                CornerRadius = 5,
                                CornerExponent = 2,
                                Children =
                                [
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(255, 255, 255, 40)
                                    },
                                    new SpriteText
                                    {
                                        Text = keyBind,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Font = FontUsage.Default.With(size: 13)
                                    }
                                ]
                            },
                            new Container
                            {
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.TopRight,
                                Position = new Vector2(-6, 6),
                                Size = new Vector2(36, 18),
                                Masking = true,
                                CornerRadius = 5,
                                CornerExponent = 2,
                                Children =
                                [
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(255, 255, 255, 40)
                                    },
                                    skillStack = new SpriteText
                                    {
                                        Text = "0",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Font = FontUsage.Default.With(size: 12)
                                    }
                                ]
                            },
                            new Container
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Position = new Vector2(0, -10),
                                Size = new Vector2(86),
                                Masking = true,
                                CornerRadius = 8,
                                CornerExponent = 2,
                                Children =
                                [
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(255, 255, 255, 25)
                                    },
                                    skillDisplay = new Sprite
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Size = new Vector2(0.88f),
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre
                                    }
                                ]
                            },
                            cooldownText = new SpriteText
                            {
                                Text = "Ready",
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Y = 38,
                                Font = FontUsage.Default.With(size: 13)
                            },
                            skillName = new SpriteText
                            {
                                Text = skillReference.Name,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Y = -18,
                                Font = FontUsage.Default.With(size: 16)
                            },
                            new Container
                            {
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Y = -4,
                                Size = new Vector2(96, 10),
                                Masking = true,
                                CornerRadius = 5,
                                CornerExponent = 2,
                                Child = percentageDisplay = new ProgressBar
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    BackgroundColour = new Colour4(80, 80, 80, 200),
                                    ForegroundColour = new Colour4(255, 255, 255, 220),
                                    MaxInit = 100,
                                    Animate = false
                                }
                            }
                        ]
                    }
                ];
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            if (skillDisplay != null) skillDisplay.Texture = textureStore.Get(skillReference.TextureMapping.Get("Icon"));
        }

        protected override void Update()
        {
            base.Update();

            if (!hasSkill) return;

            double cooldown = Math.Max(skillReference.Cooldown, 1);
            double elapsed = Math.Max(0, GameClock.CurrentTime - skillReference.LastUseTime);
            double remaining = Math.Max(0, cooldown - elapsed);
            bool isRecharging = skillReference.UsesAvailable < skillReference.MaxStack;
            float percent = isRecharging
                ? (float)Math.Clamp((elapsed / cooldown) * 100, 0, 100)
                : 100;

            percentageDisplay.Current.Value = (float)(percent * 0.01);
            skillStack.Text = skillReference.MaxStack > 1
                ? $"{skillReference.UsesAvailable}/{skillReference.MaxStack}"
                : skillReference.UsesAvailable.ToString();
            cooldownText.Text = isRecharging ? $"{remaining / 1000:0.0}s" : "Ready";
            skillDisplay.Alpha = skillReference.UsesAvailable > 0 ? 1f : 0.55f;
            cooldownText.Colour = isRecharging ? Colour4.White : Colour4.LightGreen;
        }
    }
}
