using GentrysQuest.Game.Content.Weapons;
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
        private Weapon weapon = new Sword();
        private int holdTime;

        public TestWeaponScene()
        {
            entity = new DrawableEntity(new Game.Entity.Entity());
            entity.GetBase().SetWeapon(weapon);
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
            Add(entity);
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
    }
}
