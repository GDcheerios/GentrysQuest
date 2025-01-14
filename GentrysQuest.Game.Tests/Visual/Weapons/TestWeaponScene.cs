using System.Linq;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osuTK;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace GentrysQuest.Game.Tests.Visual.Weapons
{
    public partial class TestWeaponScene : GentrysQuestTestScene
    {
        private DrawableEntity entity;
        private Weapon weapon = new Bow();
        private DrawableEnemyEntity enemy;
        private int holdTime;

        public TestWeaponScene()
        {
            enemy = new DrawableEnemyEntity(new Game.Entity.Entity()) { Position = new Vector2(500, 0) };
            enemy.GetBase().DamageModifier = 0;
            enemy.GetBase().Stats.Health.SetAdditional(999999);
            enemy.GetBase().Stats.Tenacity.SetAdditional(999999);
            entity = new DrawableEntity(new Game.Entity.Entity());
            entity.GetBase().SetWeapon(weapon);
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
            Add(entity);
            Add(enemy);
        }

        [Test]
        public void Test()
        {
            AddSliderStep("Angle", 0, 360, 0, i => entity.DirectionLooking = i);
            AddSliderStep("Hold Time", 0, 10000, 300, i => holdTime = i);
            AddStep("Click", clickAttack);
            AddStep("Hold", holdAttack);
        }

        private Vector2 getAngle() => MathBase.GetAngleToVector(0);

        private void clickAttack()
        {
            click();
            release();
        }

        private void holdAttack()
        {
            click();
            Scheduler.AddDelayed(release, holdTime);
        }

        private void click()
        {
            entity.DoAttack(getAngle());
            Logger.Log("Clicked");
        }

        private void release()
        {
            entity.OnRelease();
            Logger.Log("Released");
        }

        protected override void Update()
        {
            base.Update();

            if (!IsLoaded || entity.QueuedProjectiles.Count <= 0) return;

            foreach (Projectile projectile in entity.QueuedProjectiles.ToList())
            {
                projectile.Position = new Vector2(20, 20);
                projectile.Alpha = 1;
                projectile.ShootFrom(entity);
                Scheduler.AddDelayed(() =>
                {
                    Remove(projectile, false);
                    HitBoxScene.Remove(projectile.HitBox);
                }, projectile.Lifetime);
                entity.QueuedProjectiles.Remove(projectile);
                Add(projectile);
            }
        }
    }
}
