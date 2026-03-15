using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Screens
{
    public partial class GameplayScreen : GqScreen
    {
        private SpriteText scoreText;
        private DrawablePlayableEntity playerEntity;
        private GameplayHud gameplayHud;
        private bool started = false;

        private const double FAILED_SPAWN_RETRY_MS = 500;

        private MapScene mapScene = new();

        private double lastSpawnGameplayTime;

        /// <summary>
        /// The gameplay time tracker
        /// </summary>
        private double gameplayTime;

        /// <summary>
        /// The time the game started
        /// </summary>
        private double startTime;

        /// <summary>
        /// The accumulated time spent paused
        /// </summary>
        private double pausedTimeOffset;

        /// <summary>
        /// The time when the game was paused
        /// </summary>
        private double? pauseStartTime;

        private IUser user;

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(mapScene);
        }

        public void LoadGameplay(IUser user, Map map)
        {
            if (map == null) return;

            startTime = Time.Current;
            lastSpawnGameplayTime = 0;
            pausedTimeOffset = 0;
            pauseStartTime = null;

            mapScene.LoadMap(map);
            mapScene.AddPlayer(new DrawablePlayableEntity(user.EquippedCharacter));
            mapScene.GetPlayer().SetupClickContainer();

            this.user = user;

            AddInternal(gameplayHud = new GameplayHud());
            gameplayHud.SetEntity(user.EquippedCharacter);

            started = true;
        }

        protected override void Update()
        {
            base.Update();
            if (!started) return;

            gameplayTime = Time.Current - startTime - pausedTimeOffset;

            double spawnInterval = mapScene.GetMap().Reference.TimeToSpawnEnemies;
            if (gameplayTime - lastSpawnGameplayTime < spawnInterval) return;

            var spawnedEnemies = mapScene.SpawnEnemies();

            foreach (Enemy enemy in spawnedEnemies)
            {
                enemy.SetRelativeLevel(user.EquippedCharacter.Experience.CurrentLevel());
                enemy.Heal();
                enemy.AddEffect(new Paused(duration: 2000) { IsInfinite = false });
                enemy.OnDeath += () =>
                {
                    foreach (Artifact artifact in enemy.GetArtifactReward())
                    {
                        user.MoneyHandler.Hand(enemy.GetMoneyReward());
                        Weapon weapon = enemy.GetWeaponReward();
                        if (weapon != null) user.AddItem(weapon);
                        artifact.Initialize(MathBase.GetStarRating(enemy.Difficulty));
                        user.AddItem(artifact);
                    }
                };
            }

            if (spawnedEnemies.Count > 0) lastSpawnGameplayTime = gameplayTime;
            else lastSpawnGameplayTime = gameplayTime - spawnInterval + FAILED_SPAWN_RETRY_MS;
        }

        public override bool UpdateSubTree()
        {
            if (!started) return base.UpdateSubTree();

            bool isPaused = gameMenuOverlay.IsVisible;

            if (isPaused)
            {
                pauseStartTime ??= Time.Current;
                return false;
            }

            if (pauseStartTime != null)
            {
                pausedTimeOffset += Time.Current - pauseStartTime.Value;
                pauseStartTime = null;
            }

            return base.UpdateSubTree();
        }
    }
}
