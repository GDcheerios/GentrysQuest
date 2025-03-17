using System;
using GentrysQuest.Game.Audio.Music;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Audio
{
    public partial class AudioManager : CompositeComponent
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance ??= new AudioManager();
        private ITrackStore trackStore;
        private ISampleStore sampleStore;

        public delegate void PlayMusicEvent();

        public event PlayMusicEvent OnPlayMusic;

        private readonly Volume gameVolume = new(1);
        private readonly Volume musicVolume = new(1);
        private readonly Volume soundVolume = new(1);

        [BackgroundDependencyLoader]
        private void load(ITrackStore trackStore, ISampleStore sampleStore)
        {
            RelativeSizeAxes = Axes.Both;
            this.trackStore = trackStore;
            this.sampleStore = sampleStore;
        }

        [CanBeNull]
        private DrawableTrack gameMusic;

        public ISong CurrentSong;

        private const int FADE_TIME = 5000;

        public void ChangeMusic(ISong song)
        {
            CurrentSong = song;
            OnPlayMusic?.Invoke();
            DrawableTrack track = new DrawableTrack(trackStore.Get(song.FileName));
            track.Looping = true;

            Action modifyTrack = () =>
            {
                gameMusic = track;
                gameMusic.Start();
                gameMusic.VolumeTo(musicVolume.Amount, FADE_TIME);
            };

            if (gameMusic == null)
            {
                gameMusic = track;
                gameMusic.VolumeTo(0);
                gameMusic.Start();
                gameMusic.VolumeTo(musicVolume.Amount, FADE_TIME);
            }
            else
            {
                gameMusic.VolumeTo(0, FADE_TIME).Then().Finally(_ => modifyTrack());
            }
        }

        public void StopMusic() => gameMusic?.Stop();
        public void FadeOutMusic(int time = FADE_TIME) => gameMusic.FadeOut(time);
        public void FadeInMusic(int time = FADE_TIME) => gameMusic.FadeIn(time);

        public void PlaySound(DrawableSample sample)
        {
            sample.VolumeTo(soundVolume.Amount);
            sample.Play();
        }

        public void ChangeMusicVolume(int percent) { musicVolume.Amount = (double)percent / 100 / gameVolume.Amount; }

        private void adjustMusicVolume() { ChangeMusicVolume((int)musicVolume.Amount * 100); }

        public void ChangeSoundVolume(int percent) { soundVolume.Amount = (double)percent / 100 / gameVolume.Amount; }

        private void adjustSoundVolume() { ChangeSoundVolume((int)soundVolume.Amount * 100); }

        public void ChangeGameVolume(int percent)
        {
            gameVolume.Amount = (double)percent / 100;
            adjustMusicVolume();
            adjustSoundVolume();
        }
    }
}
