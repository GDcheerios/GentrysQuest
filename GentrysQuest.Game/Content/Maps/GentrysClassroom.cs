using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class GentrysClassroom : Map
    {
        // classroom layout
        public const int CLASSROOM_WIDTH = 1500;
        public const int CLASSROOM_HEIGHT = 1700;

        // colors
        private static readonly Colour4 FRONT_WALL_COLOUR = new Colour4(167, 66, 46, 255);
        private static readonly Colour4 WALL_COLOUR = Colour4.LightGray;
        private static readonly Colour4 FLOOR_COLOUR = new Colour4(48, 58, 62, 255);

        // desks
        private const int COLUMNS = 3;
        private const int ROWS = 4;
        private const float DESK_WIDTH = 350;
        private const float DESK_HEIGHT = 100;

        private DrawableEntity mrGentryNpc;

        private readonly Character[] seatedCharacters =
        [
            new BraydenMesserschmidt(),
            new PhilipMcClure(),
            new MekhiElliot()
        ];

        public override void Load()
        {
            Name = "Gentry's Classroom";
            DifficultyScales = false;
            Size = new Vector2(CLASSROOM_WIDTH, CLASSROOM_HEIGHT);

            #region Layout

            // Floor
            Objects.Add(new MapObject
            {
                RelativeSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Colour = FLOOR_COLOUR
            });

            // Walls
            // Bottom
            Objects.Add(new MapObject
            {
                Size = new Vector2(1, 50),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Colour = WALL_COLOUR,
                HasCollider = true
            });

            // Top
            Objects.Add(new MapObject
            {
                Size = new Vector2(1, 50),
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                Colour = FRONT_WALL_COLOUR,
                HasCollider = true
            });

            // Left
            Objects.Add(new MapObject
            {
                Size = new Vector2(50, 1),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Colour = WALL_COLOUR,
                HasCollider = true
            });

            // Right
            Objects.Add(new MapObject
            {
                Size = new Vector2(50, 1200),
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Colour = WALL_COLOUR,
                HasCollider = true
            });

            // Door1
            Objects.Add(new MapObject
            {
                Size = new Vector2(25, 200),
                Position = new Vector2(0, 50),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Colour = Colour4.Black,
                HasCollider = true
            });

            // Door2
            Objects.Add(new MapObject
            {
                Size = new Vector2(25, 200),
                Position = new Vector2(0, -50),
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Colour = Colour4.Black,
                HasCollider = true
            });

            #endregion

            #region Desks

            // Student's desks

            int counter = 0;

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    float x = (col - 1) * (CLASSROOM_WIDTH / 3f);
                    float y = -100 + (row - (ROWS - 1) / 2f) * (DESK_HEIGHT + 150f);

                    Objects.Add(new MapObject
                    {
                        Size = new Vector2(DESK_WIDTH, DESK_HEIGHT),
                        Position = new Vector2(x, y),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Colour4.Gray,
                        HasCollider = true
                    });

                    for (int i = 0; i < 3; i++)
                    {
                        DrawableEntity studentEntity;

                        if (counter >= 0 && counter < seatedCharacters.Length && seatedCharacters[counter] is not null)
                        {
                            studentEntity = new DrawableEntity(seatedCharacters[counter])
                            {
                                Position = new Vector2(x - DESK_WIDTH * 0.5f + i * (DESK_WIDTH / 3) + 50, y + 80),
                            };
                        }
                        else
                        {
                            studentEntity = new DrawableEntity(new Entity.Entity())
                            {
                                Position = new Vector2(x - DESK_WIDTH * 0.5f + i * (DESK_WIDTH / 3) + 50, y + 80),
                            };
                        }

                        Npcs.Add(studentEntity);

                        studentEntity.GetBase().Stats.Defense.SetDefaultValue(9999999);

                        studentEntity.HideBar();

                        counter++;
                    }
                }
            }

            // Mr.Gentry's cool desk
            Objects.Add(new MapObject
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.TopLeft,
                Colour = Colour4.DarkSalmon,
                Position = new Vector2(65, 300),
                Size = new Vector2(100, 80),
                HasCollider = true
            });
            Objects.Add(new MapObject
            {
                Origin = Anchor.TopLeft,
                Anchor = Anchor.TopLeft,
                Colour = Colour4.Gray,
                Position = new Vector2(55, 75),
                Size = new Vector2(80, 120),
                HasCollider = true
            });

            #endregion

            #region Decoration

            // Projector
            Objects.Add(new MapObject
            {
                Colour = Colour4.White,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                Size = new Vector2(0.3f, 10),
                Position = new Vector2(0, 60)
            });

            Npcs.Add(mrGentryNpc = new DrawableEntity(new GMoney())
            {
                Position = new Vector2(0, -600),
            });
            mrGentryNpc.EntityBar.OnlyShowName();

            #endregion
        }
    }
}
