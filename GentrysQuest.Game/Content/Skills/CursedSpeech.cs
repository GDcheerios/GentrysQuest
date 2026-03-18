using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Content.Skills
{
    public class CursedSpeech : Skill
    {
        public override string Name { get; protected set; } = "Airxy Secondary";
        public override string Description { get; protected set; } = "Shoots hastags and stuff";
        public override double Cooldown { get; protected set; } = 500;

        private static readonly string[] text_options = ["!", "@", "#", "$", "%", "^", "&", "*"];
        private const int max_text_length = 4;

        protected override void SkillDo()
        {
            User.GetBase().AddProjectile(new ProjectileParameters
            {
                Design = new Container
                {
                    Size = new Vector2(150, 50),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Child = new SpriteText
                    {
                        Text = getString(),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Colour4.Black,
                        Font = FontUsage.Default.With(size: 50)
                    }
                },
                Speed = 10,
            });
        }

        private string getString()
        {
            string text = "";
            for (int i = 0; i < max_text_length; i++) text += text_options[MathBase.RandomChoice(text_options.Length)];
            return text;
        }
    }
}
