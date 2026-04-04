using System;
using System.Threading.Tasks;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Online.API.Requests.Gameplay;
using GentrysQuest.Game.Online.API.Requests.Leaderboard;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osuTK;

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
        private bool ending;
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

        private Visitation visitation;

        protected IUser User;
        protected int Score => score;
        protected int? LeaderboardID => ID;
        protected Visitation CurrentVisitation => visitation;

        public event Action<GameplayEndReason> GameplayEnded;
        public event Action ScoreSubmitted;

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

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
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
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
            this.TransformTo(nameof(DisplayedScore), DisplayedScore, 0);
        }

        public void LoadGameplay(IUser user, Map map) => Schedule(() => loadGameplay(user, map));

        private void loadGameplay(IUser user, Map map)
        {
            if (map == null || user?.EquippedCharacter == null) return;

            startTime = Time.Current;
            started = true;
            lastSpawnGameplayTime = 0;
            pausedTimeOffset = 0;
            pauseStartTime = null;

            mapScene.LoadMap(map);
            playerEntity = new DrawablePlayableEntity(user.EquippedCharacter);
            mapScene.AddPlayer(playerEntity);
            mapScene.GetPlayer().SetupClickContainer();

            User = user;
            User.SessionMode = SessionMode;
            score = 0;
            DisplayedScore = 0;
            ending = false;
            gameMenuOverlay.SetAccessLocked(false);

            if (user is OnlineUser onlineUser)
            {
                _ = beginVisitationAsync(onlineUser.ID, map.ContentID ?? 0);
            }

            user.EquippedCharacter.OnHitEntity += details =>
            {
                registerStatistic(new Statistic(StatTypes.HitEnemy, 1)
                {
                    Character = User.EquippedCharacter,
                    Enemy = details.Receiver as Enemy,
                    Weapon = details.Sender.Weapon,
                    Leaderboard = ID,
                    Map = map
                });
            };

            user.EquippedCharacter.OnGetHit += details =>
            {
                registerStatistic(new Statistic(StatTypes.HitPlayer, 1)
                {
                    Character = User.EquippedCharacter,
                    Enemy = details.Sender as Enemy,
                    Weapon = details.Receiver.Weapon,
                    Leaderboard = ID,
                    Map = map
                });
            };

            user.EquippedCharacter.OnDamage += details =>
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

            user.EquippedCharacter.OnDeath += () => _ = EndAsync(GameplayEndReason.Death);

            AddInternal(gameplayHud = new GameplayHud());
            gameplayHud.SetEntity(user.EquippedCharacter);

            gameMenuOverlay.BackButton.HideButton();

            started = true;
        }

        public void End(GameplayEndReason reason = GameplayEndReason.Ended) => _ = EndAsync(reason);

        public async Task EndAsync(GameplayEndReason reason = GameplayEndReason.Ended)
        {
            if (ending)
                return;

            ending = true;
            started = false;
            gameMenuOverlay.SetAccessLocked(true);
            gameMenuOverlay.Disappear();
            mapScene.RemovePlayer(playerEntity);
            LoadingIndicator loadingIndicator = new LoadingIndicator("Loading results...")
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(2)
            };

            try
            {
                AddInternal(loadingIndicator);
                loadingIndicator.FadeInFromZero(100);
                await OnEnding(reason);
                if (visitation != null) await visitation?.DepartAsync()!;

                if (LeaderboardID != null)
                {
                    OnlineUser user = User as OnlineUser;
                    await new SubmitScoreRequest((int)LeaderboardID, user!.ID, Score, visitation!.ID).PerformAsync();
                    user.RefreshRanking();
                    ScoreSubmitted?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Gameplay end hook failed: {ex.Message}", LoggingTarget.Network, LogLevel.Important);
            }
            finally
            {
                RemoveInternal(loadingIndicator, true);
                gameMenuOverlay.SetAccessLocked(false);
                mapScene.UnloadMap();
                gameMenuOverlay.BackButton.ShowButton();

                if (gameplayHud != null)
                {
                    RemoveInternal(gameplayHud, true);
                }
            }

            OnEnded(reason);
            GameplayEnded?.Invoke(reason);
        }

        protected virtual Task OnEnding(GameplayEndReason reason) => Task.CompletedTask;

        protected virtual void OnEnded(GameplayEndReason reason)
        {
        }

        private async Task beginVisitationAsync(int userId, int locationId)
        {
            var request = new VisitRequest(userId, locationId);

            try
            {
                await request.PerformAsync();
                var visited = request.Response;
                if (visited == null)
                    return;

                visitation = visited;
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to start visitation: {ex.Message}", LoggingTarget.Network, LogLevel.Important);
            }
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

            bool isPaused = gameMenuOverlay?.IsVisible == true;

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
            if (statistic == null || User == null || ending)
                return;

            statistic.Visitation = visitation?.ID;
            _ = User.AddStatistic(statistic);
            int scoreIncrease = (int)(statistic.ScoreReward * statistic.Value);
            if (scoreIncrease <= 0) scoreIncrease = statistic.ScoreReward;
            if (scoreIncrease <= 0) return;

            score += scoreIncrease;
            this.TransformTo(nameof(DisplayedScore), score, 350, Easing.OutQuint);
        }

        private void updateScoreText(int value)
        {
            if (scoreText != null) scoreText.Text = $"{value:N0} score";
        }
    }
}
