using GentrysQuest.Game.Audio.Music;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;

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

        private readonly Bindable<double> gameVolume = new(1);
        private readonly Bindable<double> musicVolume = new(1);
        private readonly Bindable<double> soundVolume = new(1);

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
            var oldMusic = gameMusic;

            DrawableTrack track = new DrawableTrack(trackStore.Get(song.FileName));
            track.Looping = true;
            gameMusic = track;

            AddInternal(gameMusic);
            gameMusic.Volume.Value = musicVolume.Value;
            gameMusic.Start();
            oldMusic?.Expire();
        }

        public void StopMusic() => gameMusic?.Stop();

        public void FadeOutMusic(int time = FADE_TIME)
        {
            Logger.Log($"Fade out music for {time}ms");
            if (gameMusic == null) return;

            gameMusic.ClearTransforms();

            if (time <= 0)
            {
                gameMusic.Volume.Value = 0;
            }
            else
            {
                gameMusic.VolumeTo(0, time);
            }
        }

        public void FadeInMusic(int time = FADE_TIME)
        {
            if (gameMusic != null)
            {
                gameMusic.Volume.Value = 0;
                gameMusic.VolumeTo(musicVolume.Value, time);
            }
        }

        public void PlaySound(DrawableSample sample)
        {
            sample.Volume.Value = soundVolume.Value;
            sample.Play();
        }
    }
}
