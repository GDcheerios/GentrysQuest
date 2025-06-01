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
        private readonly List<DrawableEnemyEntity> enemies = [];
        private DrawablePlayableEntity player;
        private readonly DrawableMap map = new();
        private readonly List<Projectile> projectiles = new();
        private LocationText locationText;

        private void moveCamera(Vector2 direction, double speed)
        {
            var value = (float)(GameClock.FrameTime * speed);
            map.MoveTo(map.Position + -direction * value);
            enemies.ForEach(e =>
            {
                e.MoveTo(e.Position + -direction * value);
                e.FocusedPosition += -direction * value;
            });
            projectiles.ForEach(p => p.MoveTo(p.Position + -direction * value));
        }

        public void AddPlayer(DrawablePlayableEntity player)
        {
            this.player = player;

            foreach (DrawableEnemyEntity enemy in enemies) enemy.FollowEntity(player);
            AddInternal(player);

            player.OnMove += moveCamera;
        }

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

        private void removeNpc(DrawableEntity npc) => map.RemoveNpc(npc);
        public void RemoveNpc(DrawableEntity npc) => removeNpc(npc);
        public void RemoveNpc(int index) => removeNpc(map.Npcs[index]);

        public void LoadMap(Map mapInfo)
        {
            map.Load(mapInfo);
            map.MoveTo(map.MapReference.SpawnPoint);

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
                    // projectile.Position = new Vector2(500, -500);
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
