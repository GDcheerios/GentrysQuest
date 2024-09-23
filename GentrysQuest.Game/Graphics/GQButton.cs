using System;
using GentrysQuest.Game.Audio;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Graphics
{
    public partial class GQButton : Button
    {
        public AudioMapping Sounds = new();
        private ISampleStore samples;
        private Action action;

        public GQButton()
        {
            Sounds.Add("Default_Hover", "sounds_menu_dHover.mp3");
            Sounds.Add("Default_Click", "sounds_menu_dClick.mp3");
        }

        [BackgroundDependencyLoader]
        private void load(ISampleStore sampleStore) => samples = sampleStore;

        public void SetAction(Action action) => this.action = action;

        protected override bool OnHover(HoverEvent e)
        {
            string sound = "Default_Hover";
            if (hasSound("Hover")) sound = "Hover";
            AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Sounds.Get(sound))));
            return base.OnHover(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            string sound = "Default_Click";
            if (hasSound("Click")) sound = "Click";
            AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Sounds.Get(sound))));
            action?.Invoke();
            return base.OnClick(e);
        }

        private bool hasSound(string value) => Sounds.Get(value) != "Death.mp3";
    }
}
