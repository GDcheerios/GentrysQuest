using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public class MapScene
    {
        private readonly List<DrawableEnemyEntity> enemies = [];
        private DrawablePlayableEntity player;
        private readonly DrawableMap map = new DrawableMap();

        private void moveCamera(Vector2 direction, double speed)
        {
            var value = (float)(GameClock.FrameTime * speed);
            map.MoveTo(map.Position + -direction * value);
            enemies.ForEach(e => e.MoveTo(e.Position + -direction * value));
        }

        public void AddPlayer(DrawablePlayableEntity player)
        {
            this.player = player;

            player.OnMove += moveCamera;
        }

        public void RemovePlayer(DrawablePlayableEntity player) => this.player = null;

        public void AddEnemy(DrawableEnemyEntity enemyEntity) => enemies.Add(enemyEntity);
        public void RemoveEnemy(DrawableEnemyEntity enemyEntity) => enemies.Remove(enemyEntity);

        public void LoadMap(Map map) => this.map.Load(map);
        public void UnloadMap() => map.Unload();
    }
}
