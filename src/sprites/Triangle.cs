using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace BlobGame
{
    public class Triangle : CollMoveableSprite
    {
        public static Texture2D[] idleTextures;
        int idleCounter;
        int idleActiveFrame;
        public static Texture2D[] walkingTextures;
        int walkingCounter;
        int walkingActiveFrame;
        public static Texture2D[] jumpingTextures;
        public static int triangleSizeW = 42;
        public static int triangleSizeH = 64;
        Random random = new Random();
        bool randomBool;
        int delay = 0;
        int onGroundDelay = 0;
        bool alive = true;
        int switchTick = new Random().Next(1, 101);
        bool stop = false;
        int health = 80;

        public Triangle(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
        }

        public override void LoadContent(Game game)
        {
            base.LoadContent(game);

            idleTextures = new Texture2D[2];
            walkingTextures = new Texture2D[4];
            jumpingTextures = new Texture2D[2];

            idleTextures[0] = LoadTexture("sprites/triangle/TriangleIdle1.png");
            idleTextures[1] = LoadTexture("sprites/triangle/TriangleIdle2.png");

            jumpingTextures[0] = LoadTexture("sprites/triangle/TriangleJump1.png");
            jumpingTextures[1] = LoadTexture("sprites/triangle/TriangleJump2.png");

            walkingTextures[0] = LoadTexture("sprites/triangle/TriangleWalk1.png");
            walkingTextures[1] = LoadTexture("sprites/triangle/TriangleWalk2.png");
            walkingTextures[2] = LoadTexture("sprites/triangle/TriangleWalk1.png");
            walkingTextures[3] = LoadTexture("sprites/triangle/TriangleWalk3.png");

        }

        public override void Update()
        {
            base.Update();

            idleCounter++;
            if (idleCounter > 29)
            {
                idleCounter = 0;
                idleActiveFrame++;

                if (idleActiveFrame > idleTextures.Length - 1)
                {
                    idleActiveFrame = 0;
                }
            }

            walkingCounter++;
            if (walkingCounter > 44 - (speed * 6))
            {
                walkingCounter = 0;
                walkingActiveFrame++;

                if (walkingActiveFrame > walkingTextures.Length - 1)
                {
                    walkingActiveFrame = 0;
                }
            }

            if(switchTick == 75)
            {
                if(!isLeft)
                {
                    isLeft = true;
                } else {
                    isLeft = false;
                }
            }

            if(Drect.X <= minPos.X + 10)
            {
                isLeft = false;
            }

            if(Drect.X >= maxPos.X - Drect.Width - 10)
            {
                isLeft = true;
            }

            if (!isLeft)
            {
                Velocity.X = speed;
            }
            else if (isLeft)
            {
                Velocity.X = -speed;
            }

            if (delay > 0)
            {
                delay--;
            }

            if (onGroundDelay > 0)
            {
                onGroundDelay--;
            }

            Velocity.Y += 0.5f;

            Velocity.Y = Math.Min(25.0f, Velocity.Y);

            SetBounds();
            Drect.Position = Vector2.Clamp(Drect.Position, minPos, maxPos);

            // Horizontal Collision Resolution
            if(!stop){Drect.X += (int)Velocity.X;}
            horizontalCollisions = GetIntersectingTiles(Drect);

            isInAir = true;

            if (onGroundDelay == 0 && !isInAir)
            {
                Velocity.Y = -10;
                onGroundDelay = 300;
            }

            randomBool = random.Next(2) == 1; // Generates 0 or 1, then checks if it equals 1

            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[(int)Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if (!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if (value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);

                            if (Velocity.X > 0) // Moving Right
                            {
                                Drect.X = collision.Left() - Drect.Width;

                                if (randomBool)
                                {
                                    isLeft = true;
                                }
                                else if (!randomBool && delay == 0)
                                {
                                    Velocity.Y = -10;
                                    delay = 40;
                                }
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right();

                                if (randomBool)
                                {
                                    isLeft = false;
                                }
                                else if (!randomBool && delay == 0)
                                {
                                    Velocity.Y = -10;
                                    delay = 40;
                                }
                            }
                            Velocity.X = 0; // Stop horizontal movement upon collision
                        }
                        else if (value == 4) //! Water
                        {
                            if (Velocity.X > 1)
                            {
                                Velocity.X = 1;
                            }
                            else if (Velocity.X < -1)
                            {
                                Velocity.X = -1;
                            }
                        }
                    }
                }
            }

            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);

            foreach (var tile in verticalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if (!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if (value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);

                            if (Velocity.Y > 0) // Falling Down
                            {
                                Drect.Y = collision.Top() - Drect.Height;
                                Velocity.Y = 0.5f;
                                isInAir = false;
                            }
                            else if (Velocity.Y < 0) // Moving Up
                            {
                                Drect.Y = collision.Bottom();
                                Velocity.Y = 0;
                            }
                        }
                        else if (value == 4) //! Water
                        {
                            if (Velocity.Y > 0)
                            {
                                Velocity.Y = 1;
                            }
                            isInAir = true;
                        }
                    }
                }
            }

            if(switchTick > 0)
            {
                switchTick--;
            } else {
                switchTick = 100;
            }

            if(switchTick > 25)
            {
                stop = false;
            } else {
                stop = true;
            }

            foreach (Fireball fireball in Game.fireballs)
            {
                if (fireball.Drect.Intersects(Drect) && !fireball.bad)
                {
                    fireball.Die();
                    health -= 20;
                }
            }

            if (health == 0 && alive)
            {
                alive = false;
            }

            if (Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                alive = false;
            }

            if (Drect.Intersects(Game.player.Drect) && !Game.player.isSanic)
            {
                Player.Damage(40);
            }

            if (!alive)
            {
                Game.triangles.Remove(this);
            }
        }

        public override void Draw()
        {
            float flip = isLeft ? -1.0f : 1.0f;
            Srect = new(0, 0, 20 * flip, 30);

            if(isMoving && !isInAir && !stop)
            {
                DrawTexturePro(
                    walkingTextures[walkingActiveFrame],
                    Srect,
                    Drect,
                    Vector2.Zero,
                    0f,
                    Color.White
                );
            }

            if(isInAir)
            {
                if(Velocity.Y >= 5 || Velocity.Y <= -5)
                {
                    DrawTexturePro(
                        jumpingTextures[0],
                        Srect,
                        Drect,
                        Vector2.Zero,
                        0f,
                        Color.White
                    );
                }
                else
                {
                    DrawTexturePro(
                        jumpingTextures[1],
                        Srect,
                        Drect,
                        Vector2.Zero,
                        0f,
                        Color.White
                    );
                }
            }

            if((!isMoving || stop) && !isInAir)
            {
                DrawTexturePro(
                    idleTextures[idleActiveFrame],
                    Srect,
                    Drect,
                    Vector2.Zero,
                    0f,
                    Color.White
                );
            }

            if (Game.hasF3On)
            {
                foreach (var rect in horizontalCollisions)
                {
                    DrawRectangleLinesEx(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                foreach (var rect in verticalCollisions)
                {
                    DrawRectangleLinesEx(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                DrawRectangleLinesEx(Drect, 4, Color.Blue);
            }
        }

        public override string[] GetDebugInfo()
        {
            return new string[]
            {
                "---TRIANGLE---",
                "Random Bool: " + randomBool,
                "Delay: " + delay,
                "Is In Air: " + isInAir,
                "Is Left: " + isLeft,
                "Drect: " + Drect
            };
        }

        public static void Summon(Vector2 pos)
        {
            Triangle triangle = new Triangle(idleTextures[1], new Rectangle((int)pos.X, (int)pos.Y, triangleSizeW, triangleSizeH), new Rectangle(0, 0, 20, 30));
            Game.triangles.Add(triangle);
        }

        public void Die()
        {
            alive = false;
        }

        public static void ClearAll()
        {
            Game.triangles.Clear();
        }
    }
}
