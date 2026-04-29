using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Location.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public partial class MapScene : Container
    {
        private const float MIN_SPAWN_RADIUS = 300f;
        private const float MAX_SPAWN_RADIUS = 600f;
        private const int MAX_SPAWN_ATTEMPTS = 40;
        private const float ENEMY_COLLIDER_SIZE = DrawableEntity.SIZE * 0.3f;

        private readonly List<DrawableEnemyEntity> enemies = [];
        private DrawablePlayableEntity player;
        private readonly DrawableMap map = new();
        private readonly List<Projectile> projectiles = new();
        private LocationText locationText;

        private void moveCamera(Vector2 direction, double speed)
        {
            var value = (float)(GameClock.FrameTime * speed);
            map.Move(-direction * value);
            enemies.ForEach(e =>
            {
                e.Position += -direction * value;
                e.FocusedPosition += -direction * value;
                e.OffsetAiPositions(-direction * value);
            });
            projectiles.ForEach(p => p.Position += -direction * value);
        }

        public void AddPlayer(DrawablePlayableEntity player)
        {
            this.player = player;

            foreach (DrawableEnemyEntity enemy in enemies) enemy.FollowEntity(player);
            AddInternal(player);

            player.OnMove += moveCamera;
        }

        public DrawablePlayableEntity GetPlayer() => player;

        public void RemovePlayer(DrawablePlayableEntity player)
        {
            player?.Expire();
            this.player = null;
        }

        public void AddEnemy(DrawableEnemyEntity enemyEntity)
        {
            enemies.Add(enemyEntity);
            AddInternal(enemyEntity);
            if (player != null) enemyEntity.FollowEntity(player);
            enemyEntity.GetBase().OnDeath += () => RemoveEnemy(enemyEntity);
        }

        public void RemoveEnemy(DrawableEnemyEntity enemyEntity)
        {
            enemies.Remove(enemyEntity);
            enemyEntity?.Expire();
        }

        public List<Enemy> SpawnEnemies()
        {
            List<Enemy> spawnedEnemies = [];
            if (player == null || !map.Reference.AllowRandomSpawning || enemies.Count >= map.Reference.MaxEnemySpawn) return spawnedEnemies;

            int enemiesToSpawn = MathBase.RandomInt(map.Reference.MinEnemySpawn, map.Reference.MaxEnemySpawn);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Enemy enemy = map.Reference.Enemies[MathBase.RandomChoice(map.Reference.Enemies.Count)].Copy();
                Vector2 spawnPosition = Vector2.Zero;
                bool validPosition = false;

                for (int attempt = 0; attempt < MAX_SPAWN_ATTEMPTS; attempt++)
                {
                    float angle = MathBase.RandomFloat(0, 360);
                    float distance = MathBase.RandomFloat(MIN_SPAWN_RADIUS, MAX_SPAWN_RADIUS);
                    spawnPosition = player.Position + (MathBase.GetAngleToVector(angle) * distance);

                    validPosition = canSpawnEnemyAt(spawnPosition);

                    if (validPosition) break;
                }

                if (!validPosition) continue;

                spawnedEnemies.Add(enemy);
                var drawableEnemy = new DrawableEnemyEntity(enemy) { Position = spawnPosition };
                enemy.SetWeapon();
                AddEnemy(drawableEnemy);
            }

            return spawnedEnemies;
        }

        private bool canSpawnEnemyAt(Vector2 position)
        {
            if (!isInsideMapBounds(position))
                return false;

            float halfSize = ENEMY_COLLIDER_SIZE * 0.5f;
            Vector2 topLeft = ToScreenSpace(position - new Vector2(halfSize));
            Vector2 bottomRight = ToScreenSpace(position + new Vector2(halfSize));

            RectangleF spawnBounds = new RectangleF(
                topLeft.X,
                topLeft.Y,
                bottomRight.X - topLeft.X,
                bottomRight.Y - topLeft.Y
            );

            return !HitBoxScene.Collides(spawnBounds, AffiliationType.Enemy);
        }

        private bool isInsideMapBounds(Vector2 scenePosition)
        {
            float halfSize = ENEMY_COLLIDER_SIZE * 0.5f;
            Vector2 mapPosition = scenePosition - map.Position;

            return mapPosition.X >= halfSize
                   && mapPosition.Y >= halfSize
                   && mapPosition.X <= map.DrawWidth - halfSize
                   && mapPosition.Y <= map.DrawHeight - halfSize;
        }

        private void removeNpc(DrawableEntity npc) => map.RemoveNpc(npc);
        public void RemoveNpc(DrawableEntity npc) => removeNpc(npc);
        public void RemoveNpc(int index) => removeNpc(map.Npcs[index]);
        public DrawableEntity GetNpc(int index) => map.Npcs[index];
        public DrawableEntity GetNpc(string name) => map.Npcs.FirstOrDefault(n => n.GetBase().Name == name);

        public Vector2 GetSpawnPoint() => -map.Reference.SpawnPoint;

        public void LoadMap(Map mapInfo)
        {
            map.Load(mapInfo);
            map.Position = GetSpawnPoint();

            Scheduler.Add(_ => AddInternal(locationText = new LocationText(mapInfo.Name)), 1);
        }

        public void UnloadMap()
        {
            foreach (DrawableEnemyEntity enemy in enemies.ToList())
                RemoveEnemy(enemy);

            foreach (Projectile projectile in projectiles.ToList())
            {
                RemoveInternal(projectile, true);
                removeProjectile(projectile);
            }

            map.Unload();

            if (locationText != null) RemoveInternal(locationText, true);
            if (player != null) RemovePlayer(player);

            locationText = null;
        }

        public DrawableMap GetMap() => map;

        private void removeProjectile(Projectile projectile) => projectiles.Remove(projectile);

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(map);
        }

        protected override void Update()
        {
            base.Update();

            if (player != null)
            {
                foreach (Projectile projectile in player.QueuedProjectiles.ToList())
                {
                    projectile.ShootFrom(player);
                    player.QueuedProjectiles.Remove(projectile);
                    projectiles.Add(projectile);
                    AddInternal(projectile);
                    Scheduler.AddDelayed(() =>
                    {
                        projectile.Expire();
                        HitBoxScene.Remove(projectile.HitBox);
                        removeProjectile(projectile);
                    }, projectile.Lifetime);
                }
            }

            foreach (DrawableEnemyEntity enemy in enemies)
            {
                // transferParticles(enemy);

                foreach (Projectile projectile in enemy.QueuedProjectiles.ToList())
                {
                    projectile.ShootFrom(enemy);
                    projectiles.Add(projectile);
                    AddInternal(projectile);
                    enemy.QueuedProjectiles.Remove(projectile);
                    Scheduler.AddDelayed(() =>
                    {
                        projectile.Expire();
                        HitBoxScene.Remove(projectile.HitBox);
                        removeProjectile(projectile);
                    }, projectile.Lifetime);
                }
            }
        }
    }
}
