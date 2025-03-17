using System.Collections.Generic;
using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Screens
{
    public partial class GameplayScreen : GqScreen
    {
        public int Score { get; set; } = new();
        private int spendableScore;
        private TextFlowContainer scoreFlowContainer;
        private SpriteText scoreText;
        private DrawablePlayableEntity playerEntity;
        private GameplayHud gameplayHud;
        private InventoryOverlay inventoryOverlay;
        private StatTracker currentStats;
        private bool started = false;
        private readonly List<DrawableEntity> enemies = new();
        private readonly List<Projectile> projectiles = new();


        private bool showingInventory = false;

        /// <summary>
        /// if this is connected to a leaderboard
        /// </summary>
        private readonly int? leaderboardId;

        /// <summary>
        /// Maximum enemies allowed to spawn at once
        /// </summary>
        private int enemySpawnLimit;

        /// <summary>
        /// How many enemies are allowed on the screen
        /// </summary>
        private int enemyLimit;

        /// <summary>
        /// The gameplay difficulty
        /// </summary>
        private int gameplayDifficulty;

        /// <summary>
        /// The gameplay time tracker
        /// </summary>
        private double gameplayTime;

        /// <summary>
        /// if the gameplay is paused
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// The amount of pause time
        /// </summary>
        private double pauseTime;

        private const double MAX_TIME_TO_SPAWN = 20000;

        private delegate void GameplayEvent();

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Colour4.Gray,
                    RelativeSizeAxes = Axes.Both
                },
                gameplayHud = new GameplayHud(),
                scoreFlowContainer = new TextFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                },
            };
            scoreFlowContainer.AddText(scoreText = new SpriteText
            {
                Text = "0",
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 64)
            });
            scoreFlowContainer.AddText(new SpriteText
            {
                Text = "score",
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 52),
                Padding = new MarginPadding { Left = 15 }
            });
        }

        /// <summary>
        /// Add an enemy to the gameplay scene
        /// </summary>
        /// <param name="level">Current level of the character</param>
        /// <param name="enemy">For if you want to use custom enemies</param>
        public void AddEnemy(int level, Enemy enemy = null)
        {
            enemy!.Experience.Level.Current.Value = level;
            enemy.UpdateStats();
            DrawableEnemyEntity newEnemy = new DrawableEnemyEntity(enemy);
            newEnemy.Position = new Vector2(MathBase.RandomInt(-500, 500), MathBase.RandomInt(-500, 500));
            AddInternal(newEnemy);
            enemies.Add(newEnemy);
            enemy.SetWeapon();
            newEnemy.GetBase().OnDeath += delegate { Scheduler.AddDelayed(() => RemoveEnemy(newEnemy), 100); };
            newEnemy.FollowEntity(playerEntity);
        }

        /// <summary>
        /// Spawns enemies in bulk
        /// </summary>
        public void SpawnEntities()
        {
            int currentAmount = 0;

            while (enemies.Count < enemyLimit)
            {
                AddEnemy(HelpMe.GetScaledLevel(gameplayDifficulty, playerEntity.GetBase().Experience.Level.Current.Value));
                currentAmount++;
                if (currentAmount > enemySpawnLimit) break;
            }
        }

        public void Pause()
        {
            playerEntity.GetBase().AddEffect(new Paused());

            foreach (DrawableEntity enemy in enemies)
            {
                enemy.GetBase().AddEffect(new Paused());
            }

            isPaused = true;
        }

        public void UnPause()
        {
            playerEntity.GetBase().RemoveEffect("Paused");

            foreach (DrawableEntity enemy in enemies)
            {
                enemy.GetBase().RemoveEffect("Paused");
            }

            isPaused = false;
        }

        /// <summary>
        /// Get the right time to spawn enemies
        /// </summary>
        /// <returns>if it's right to spawn</returns>
        private void spawnEnemyClock()
        {
            if (!isPaused)
            {
                double elapsedTime = Clock.CurrentTime - (gameplayTime - pauseTime);
                pauseTime = 0;

                if (MathBase.RandomInt(1, 10000 / 1 + gameplayDifficulty) < 5)
                {
                    if (!atEnemyLimit()) AddEnemy(HelpMe.GetScaledLevel(gameplayDifficulty, playerEntity.GetBase().Experience.Level.Current.Value));
                }

                if (elapsedTime > MAX_TIME_TO_SPAWN)
                {
                    gameplayTime = Clock.CurrentTime;
                    SpawnEntities();
                }
            }
            else
            {
                pauseTime = Clock.CurrentTime;
            }
        }

        private bool atEnemyLimit()
        {
            return enemies.Count == enemyLimit;
        }

        /// <summary>
        /// Remove an enemy from the gameplay scene
        /// </summary>
        /// <param name="enemy">The enemy to remove</param>
        public void RemoveEnemy(DrawableEnemyEntity enemy)
        {
            HitBoxScene.Remove(enemy.HitBox);
            HitBoxScene.Remove(enemy.ColliderBox);
            HitBoxScene.Remove(enemy.Weapon.HitBox);
            enemy.EnemyController.Destroy();
            enemies.Remove(enemy);
            RemoveInternal(enemy, false);
        }

        private void removeAllEnemies()
        {
            List<DrawableEntity> newEnemyList = new List<DrawableEntity>();

            foreach (var drawableEntity in enemies)
            {
                newEnemyList.Add(drawableEntity);
            }

            foreach (var drawableEntity in newEnemyList)
            {
                var enemy = (DrawableEnemyEntity)drawableEntity;
                RemoveEnemy(enemy);
            }
        }

        /// <summary>
        /// Sets up the gameplay scene
        /// </summary>
        public void SetUp(IUser user)
        {
            if (playerEntity is null)
            {
                AddInternal(playerEntity = new DrawablePlayableEntity(user.EquippedCharacter));
                if (user.EquippedCharacter.Weapon != null) user.EquippedCharacter.SetWeapon(user.EquippedCharacter.Weapon);
                playerEntity.GetBase().OnDeath += End;
            }

            Scheduler.AddDelayed(() =>
            {
                AudioManager.Instance.ChangeMusic(new Overwhelm());
            }, 100);

            gameplayHud.SetEntity(user.EquippedCharacter);
            playerEntity.SetupClickContainer();
            gameplayTime = Clock.CurrentTime;
            user.EquippedCharacter.Spawn();
            currentStats.ScoreStatistic.OnScoreChange += change =>
            {
                spendableScore += change;
                this.TransformTo(nameof(Score), (int)currentStats.ScoreStatistic.Value, 1000, Easing.Out);
            };
            started = true;
        }

        /// <summary>
        /// Manages how entities move depending on the direction.
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <param name="speed">The speed</param>
        /// <param name="drawable">The drawable to invoke movement on</param>
        private void manage_direction(Vector2 direction, double speed, Drawable drawable)
        {
            var value = (float)(Clock.ElapsedFrameTime * speed);
            drawable.MoveTo(drawable.Position + -direction * value);
        }

        private void manage_direction(Vector2 direction, double speed, DrawableEntity drawable)
        {
            var value = (float)(Clock.ElapsedFrameTime * speed);
            drawable.MoveTo(drawable.Position + -direction * value);
            drawable.FocusedPosition += -direction * value;
        }

        /// <summary>
        /// Manages how to end the gameplay scene
        /// </summary>
        public virtual void End()
        {
            Pause();

            foreach (DrawableEntity enemy in enemies) enemy.GetBase().Die();

            playerEntity.RemoveClickContainer();
            Container deathContainer = new Container
            {
                Alpha = 0,
                Depth = -4,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Text = "You died",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Colour4.Red,
                        Font = FontUsage.Default.With(size: 86)
                    }
                }
            };
            AddInternal(deathContainer);
            gameplayHud.Delay(3000).Then().FadeOut();
            deathContainer.FadeIn(3000);
            Scheduler.AddDelayed(this.Exit, 3000);
            started = false;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.C:
                    inventoryOverlay.ToggleDisplay();
                    if (isPaused) UnPause();
                    else Pause();
                    break;

                case Key.T:
                    playerEntity.Position += Vector2.One;
                    break;
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e) => this.FadeInFromZero(500, Easing.OutQuint);

        protected override void Update()
        {
            base.Update();
            if (!started) return;

            spawnEnemyClock();
            scoreText.Text = "" + Score;
        }
    }
}
