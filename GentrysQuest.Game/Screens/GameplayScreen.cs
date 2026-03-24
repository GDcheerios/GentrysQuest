using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Screens
{
    public partial class GameplayScreen : GqScreen
    {
        protected virtual UserSessionMode SessionMode => UserSessionMode.Normal;
        private SpriteText scoreText;
        private int score;
        private int displayedScore;
        private DrawablePlayableEntity playerEntity;
        private GameplayHud gameplayHud;
        private bool started = false;
        protected int? ID { get; set; }

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

        protected IUser User;

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        private int DisplayedScore
        {
            get => displayedScore;
            set
            {
                displayedScore = value;
                updateScoreText(displayedScore);
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(mapScene);
            AddInternal(scoreText = new SpriteText
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Margin = new MarginPadding { Top = 24, Right = 24 },
                Font = FontUsage.Default.With(size: 38, weight: "SemiBold"),
                Depth = -2
            });
            updateScoreText(0);
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

            User = user;
            User.SessionMode = SessionMode;
            score = 0;
            DisplayedScore = 0;
            this.TransformTo(nameof(DisplayedScore), DisplayedScore, 0);

            user.EquippedCharacter.OnDamage += (details) =>
            {
                var statistic = new Statistic(StatTypes.PlayerDamage, details.Damage)
                {
                    Enemy = details.Sender as Enemy,
                    Character = User.EquippedCharacter,
                    Weapon = details.Receiver.Weapon,
                    Leaderboard = ID,
                    Map = map
                };
                registerStatistic(statistic);

                if (user.EquippedCharacter.IsDead)
                {
                    registerStatistic(new Statistic(StatTypes.Death, 1)
                    {
                        Enemy = details.Sender as Enemy,
                        Character = User.EquippedCharacter,
                        Weapon = details.Receiver.Weapon,
                        Leaderboard = ID,
                        Map = map
                    });
                }
            };

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
                enemy.SetRelativeLevel(User.EquippedCharacter.Experience.CurrentLevel());
                enemy.Heal();
                enemy.AddEffect(new Paused(duration: 2000) { IsInfinite = false });
                enemy.OnDamage += (details) =>
                {
                    registerStatistic(new Statistic(StatTypes.EnemyDamage, details.Damage)
                        {
                            Character = User.EquippedCharacter,
                            Enemy = enemy,
                            Weapon = details.Sender.Weapon,
                            Leaderboard = ID,
                            Map = mapScene.GetMap().Reference
                        }
                    );
                };
                enemy.OnDeath += () =>
                {
                    registerStatistic(new Statistic(StatTypes.Kill, 1)
                        {
                            Character = User.EquippedCharacter,
                            Enemy = enemy,
                            Weapon = User.EquippedCharacter?.Weapon,
                            Leaderboard = ID,
                            Map = mapScene.GetMap().Reference
                        }
                    );

                    foreach (Artifact artifact in enemy.GetArtifactReward())
                    {
                        User.MoneyHandler.Hand(enemy.GetMoneyReward());
                        Weapon weapon = enemy.GetWeaponReward();
                        if (weapon != null) User.AddItem(weapon);
                        artifact.Initialize(MathBase.GetStarRating(enemy.Difficulty));
                        User.AddItem(artifact);
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

        private void registerStatistic(Statistic statistic)
        {
            _ = User.AddStatistic(statistic);
            int scoreIncrease = (int)(statistic.ScoreReward * statistic.Value);
            if (scoreIncrease <= 0) scoreIncrease = statistic.ScoreReward;
            if (scoreIncrease <= 0) return;

            score += scoreIncrease;
            this.TransformTo(nameof(DisplayedScore), score, 350, Easing.OutQuint);
        }

        private void updateScoreText(int value) => scoreText.Text = $"{value:N0} score";
    }
}
