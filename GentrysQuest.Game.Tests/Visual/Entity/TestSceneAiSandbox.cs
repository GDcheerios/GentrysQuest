using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.AI;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using GameEntity = GentrysQuest.Game.Entity.Entity;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneAiSandbox : GentrysQuestTestScene
    {
        private readonly Container playfield;
        private readonly Container obstacleLayer;
        private readonly Container actorLayer;
        private readonly FillFlowContainer debugPanel;
        private readonly List<SpriteText> debugLines = [];
        private readonly List<Projectile> projectiles = [];

        private DrawableEntity target;
        private DrawableEnemyEntity agent;
        private AiProfile profile = AiProfile.Balanced();
        private Func<Weapon> weaponFactory = () => new Knife();
        private string obstaclePreset = "None";

        protected override string TestName { get; init; } = "AI Sandbox";

        public TestSceneAiSandbox()
        {
            Add(playfield = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DarkSlateGray,
                    Alpha = 0.45f
                }
            });

            playfield.Add(obstacleLayer = new Container { RelativeSizeAxes = Axes.Both });
            playfield.Add(actorLayer = new Container { RelativeSizeAxes = Axes.Both });

            Add(debugPanel = new FillFlowContainer
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Position = new Vector2(12),
                Width = 430,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 4),
                Depth = -10
            });

            addDebugLine(Colour4.White);
            addDebugLine(Colour4.LightGreen);
            addDebugLine(Colour4.LightSkyBlue);
            addDebugLine(Colour4.LightGoldenrodYellow);
            addDebugLine(Colour4.Orange);

            resetScenario();
        }

        [Test]
        public void TestSandboxControls()
        {
            AddStep("reset scenario", resetScenario);

            AddStep("melee knife", () => setWeapon(() => new Knife()));
            AddStep("mid sword", () => setWeapon(() => new Sword()));
            AddStep("ranged bow", () => setWeapon(() => new Bow()));

            AddStep("balanced", () => setProfile(AiProfile.Balanced()));
            AddStep("aggressive", () => setProfile(AiProfile.Aggressive()));
            AddStep("defensive", () => setProfile(AiProfile.Defensive()));
            AddStep("ranged profile", () => setProfile(AiProfile.Ranged()));

            AddStep("range auto", () => updateProfile(p => p.RangeStyle = AiRangeStyle.Auto));
            AddStep("short range", () => updateProfile(p => p.RangeStyle = AiRangeStyle.ShortRange));
            AddStep("long range", () => updateProfile(p => p.RangeStyle = AiRangeStyle.LongRange));

            AddStep("no obstacles", () => setObstaclePreset("None"));
            AddStep("single wall", () => setObstaclePreset("Wall"));
            AddStep("corridor", () => setObstaclePreset("Corridor"));
            AddStep("box maze", () => setObstaclePreset("Box Maze"));

            AddStep("full health", () => setAgentHealth(1));
            AddStep("low health", () => setAgentHealth(0.25f));

            AddSliderStep("vision", 100, 2500, (int)profile.VisionRange, value => updateProfile(p => p.VisionRange = value));
            AddSliderStep("lose target", 100, 3500, (int)profile.LoseTargetRange, value => updateProfile(p => p.LoseTargetRange = value));
            AddSliderStep("preferred distance", 0, 1200, (int)profile.PreferredDistance, value => updateProfile(p => p.PreferredDistance = value));
            AddSliderStep("range tolerance", 0, 400, (int)profile.RangeTolerance, value => updateProfile(p => p.RangeTolerance = value));
            AddSliderStep("attack padding", 0, 300, (int)profile.AttackRangePadding, value => updateProfile(p => p.AttackRangePadding = value));
            AddSliderStep("wander radius", 0, 1600, (int)profile.WanderRadius, value => updateProfile(p => p.WanderRadius = value));
            AddSliderStep("windup min", 0, 1000, (int)profile.AttackWindupMinimum, value => updateProfile(p => p.AttackWindupMinimum = value));
            AddSliderStep("windup max", 0, 1600, (int)profile.AttackWindupMaximum, value => updateProfile(p => p.AttackWindupMaximum = Math.Max(value, p.AttackWindupMinimum)));
            AddSliderStep("cooldown min", 0, 2000, (int)profile.AttackCooldownMinimum, value => updateProfile(p => p.AttackCooldownMinimum = value));
            AddSliderStep("cooldown max", 0, 3000, (int)profile.AttackCooldownMaximum, value => updateProfile(p => p.AttackCooldownMaximum = Math.Max(value, p.AttackCooldownMinimum)));
            AddSliderStep("melee hold min", 0, 1200, (int)profile.MeleeAttackHoldMinimum, value => updateProfile(p => p.MeleeAttackHoldMinimum = value));
            AddSliderStep("melee hold max", 0, 1800, (int)profile.MeleeAttackHoldMaximum, value => updateProfile(p => p.MeleeAttackHoldMaximum = Math.Max(value, p.MeleeAttackHoldMinimum)));
            AddSliderStep("ranged hold min", 0, 2500, (int)profile.RangedAttackHoldMinimum, value => updateProfile(p => p.RangedAttackHoldMinimum = value));
            AddSliderStep("ranged hold max", 0, 3500, (int)profile.RangedAttackHoldMaximum, value => updateProfile(p => p.RangedAttackHoldMaximum = Math.Max(value, p.RangedAttackHoldMinimum)));
        }

        private void resetScenario()
        {
            HitBoxScene.Clear();
            projectiles.Clear();
            actorLayer.Clear();
            obstacleLayer.Clear();
            agent = null;
            target = null;

            target = createTarget(new Vector2(0, 0));
            actorLayer.Add(target);

            spawnAgent(new Vector2(100, 100));
            applyObstaclePreset();
        }

        private DrawableEntity createTarget(Vector2 position)
        {
            var entity = new GameEntity { Name = "Target" };
            entity.SetWeapon(new Sword());
            entity.Stats.Health.SetAdditional(999999);
            entity.Stats.Speed.SetDefaultValue(300);
            entity.Stats.AttackSpeed.SetDefaultValue(1);
            entity.DamageModifier = 0;

            return new DrawableEntity(entity, AffiliationType.Player, false)
            {
                Position = position,
                Colour = Colour4.CornflowerBlue
            };
        }

        private void spawnAgent(Vector2 position)
        {
            if (agent != null)
            {
                actorLayer.Remove(agent, true);
                HitBoxScene.Remove(agent.HitBox);
                HitBoxScene.Remove(agent.ColliderBox);
            }

            var entity = new Enemy { Name = "AI Agent", AiProfile = profile };
            entity.SetWeapon(weaponFactory());
            entity.Stats.Health.SetAdditional(999999);
            entity.Stats.AttackSpeed.SetDefaultValue(1);

            agent = new DrawableEnemyEntity(entity)
            {
                Position = position,
                Colour = Colour4.IndianRed
            };

            agent.FollowEntity(target);
            actorLayer.Add(agent);
        }

        private void setWeapon(Func<Weapon> factory)
        {
            weaponFactory = factory;
            Vector2 position = agent?.Position ?? new Vector2(230, 360);
            spawnAgent(position);
        }

        private void setProfile(AiProfile nextProfile)
        {
            profile = nextProfile;
            applyProfileToAgent();
        }

        private void updateProfile(Action<AiProfile> change)
        {
            change(profile);
            normalizeAttackWindows();
            applyProfileToAgent();
        }

        private void normalizeAttackWindows()
        {
            profile.AttackWindupMaximum = Math.Max(profile.AttackWindupMaximum, profile.AttackWindupMinimum);
            profile.AttackCooldownMaximum = Math.Max(profile.AttackCooldownMaximum, profile.AttackCooldownMinimum);
            profile.MeleeAttackHoldMaximum = Math.Max(profile.MeleeAttackHoldMaximum, profile.MeleeAttackHoldMinimum);
            profile.RangedAttackHoldMaximum = Math.Max(profile.RangedAttackHoldMaximum, profile.RangedAttackHoldMinimum);
        }

        private void applyProfileToAgent()
        {
            if (agent != null)
                agent.GetBase().AiProfile = profile;
        }

        private void moveTarget(Vector2 position)
        {
            if (target != null)
                target.Position = position;
        }

        private void moveAgent(Vector2 position)
        {
            if (agent != null)
                agent.Position = position;
        }

        private void setAgentHealth(float percent)
        {
            if (agent == null)
                return;

            double total = agent.GetBase().Stats.Health.Total();
            agent.GetBase().Stats.Health.Current.Value = Math.Clamp(total * percent, 1, total);
        }

        private void setObstaclePreset(string preset)
        {
            obstaclePreset = preset;
            applyObstaclePreset();
        }

        private void applyObstaclePreset()
        {
            obstacleLayer.Clear();

            switch (obstaclePreset)
            {
                case "Wall":
                    addObstacle(new Vector2(560, 170), new Vector2(70, 390));
                    break;

                case "Corridor":
                    addObstacle(new Vector2(390, 210), new Vector2(460, 60));
                    addObstacle(new Vector2(390, 510), new Vector2(460, 60));
                    addObstacle(new Vector2(800, 270), new Vector2(60, 240));
                    break;

                case "Box Maze":
                    addObstacle(new Vector2(390, 150), new Vector2(70, 360));
                    addObstacle(new Vector2(620, 300), new Vector2(70, 360));
                    addObstacle(new Vector2(850, 150), new Vector2(70, 360));
                    addObstacle(new Vector2(460, 580), new Vector2(460, 55));
                    break;
            }
        }

        private void addObstacle(Vector2 position, Vector2 size)
        {
            obstacleLayer.Add(new MapObject
            {
                Position = position,
                Size = size,
                HasCollider = true,
                Colour = Colour4.DimGray,
                Alpha = 0.95f
            });
        }

        private void addDebugLine(Colour4 colour)
        {
            var text = new SpriteText
            {
                Font = FontUsage.Default.With(size: 18),
                Colour = colour,
                Shadow = true
            };

            debugLines.Add(text);
            debugPanel.Add(text);
        }

        protected override void Update()
        {
            base.Update();
            updateDebugPanel();
            updateProjectiles(agent);
        }

        private void updateDebugPanel()
        {
            if (agent == null || target == null)
                return;

            AiCommand command = agent.CurrentAiCommand;
            float distance = (float)MathBase.GetDistance(agent.Position, target.Position);
            Weapon weapon = agent.GetBase().Weapon;

            debugLines[0].Text = $"Profile: {profile.Temperament} / {profile.RangeStyle} / weapon {weapon?.Name ?? "None"} ({weapon?.Distance ?? 0}) / obstacles {obstaclePreset}";
            debugLines[1].Text = $"State: {agent.AiState} / attack {agent.CurrentAttackState} / wants attack {command?.ShouldAttack ?? false}";
            debugLines[2].Text = $"Movement: {command?.MovementMode.ToString() ?? "None"} / has movement {command?.HasMovement ?? false} / distance {distance:0.0}";
            debugLines[3].Text =
                $"Agent: {agent.Position.X:0},{agent.Position.Y:0} / Target: {target.Position.X:0},{target.Position.Y:0} / health {agent.GetBase().Stats.Health.Current.Value:0}/{agent.GetBase().Stats.Health.Total():0}";
            debugLines[4].Text = $"Ranges: vision {profile.VisionRange:0} / preferred {profile.PreferredDistance:0} / tolerance {profile.RangeTolerance:0} / padding {profile.AttackRangePadding:0}";
        }

        private void updateProjectiles(DrawableEntity shooter)
        {
            if (shooter == null || shooter.QueuedProjectiles.Count == 0)
                return;

            foreach (Projectile projectile in shooter.QueuedProjectiles.ToList())
            {
                projectile.ShootFrom(shooter);
                shooter.QueuedProjectiles.Remove(projectile);
                projectiles.Add(projectile);
                actorLayer.Add(projectile);
                Scheduler.AddDelayed(() =>
                {
                    projectile.Expire();
                    HitBoxScene.Remove(projectile.HitBox);
                    projectiles.Remove(projectile);
                }, projectile.Lifetime);
            }
        }
    }
}
