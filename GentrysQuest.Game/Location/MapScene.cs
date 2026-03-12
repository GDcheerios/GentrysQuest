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
            if (player != null) RemoveInternal(player, false);
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
            RemoveInternal(enemyEntity, false);
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

                    CollisonHitBox spawnProbe = new CollisonHitBox(new MapObject
                    {
                        Position = spawnPosition,
                        Size = new Vector2(ENEMY_COLLIDER_SIZE),
                        Affiliation = AffiliationType.Enemy
                    });

                    validPosition = !HitBoxScene.Collides(spawnProbe);
                    HitBoxScene.Remove(spawnProbe);

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

            AddInternal(locationText = new LocationText(mapInfo.Name));
        }

        public void UnloadMap()
        {
            map.Unload();
            RemoveInternal(locationText, false);
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
                        Remove(projectile, false);
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
                        RemoveInternal(projectile, false);
                        HitBoxScene.Remove(projectile.HitBox);
                        removeProjectile(projectile);
                    }, projectile.Lifetime);
                }
            }
        }
    }
}
