using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace BlobGame
{
    public class TriangleBoss : CollMoveableSprite
    {
        public static Texture2D[] idleTextures;
        int idleCounter;
        int idleActiveFrame;
        public static Texture2D[] walkingTextures;
        int walkingCounter;
        int walkingActiveFrame;
        public static Texture2D[] jumpingTextures;
        Random random = new Random();
        bool randomBool;
        int delay = 0;
        int onGroundDelay = 0;
        bool TriangleIsAlive = true;
        public static int bossTriangleSizeW = 126;
        public static int bossTriangleSizeH = 192;
        int switchTick = new Random().Next(1, 101);
        bool stop = false;
        public int health = 3600;

        public TriangleBoss(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
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

            idleTextures[0] = LoadTexture("assets/sprites/triangle/TriangleIdle1.png");
            idleTextures[1] = LoadTexture("assets/sprites/triangle/TriangleIdle2.png");

            jumpingTextures[0] = LoadTexture("assets/sprites/triangle/TriangleJump1.png");
            jumpingTextures[1] = LoadTexture("assets/sprites/triangle/TriangleJump2.png");

            walkingTextures[0] = LoadTexture("assets/sprites/triangle/TriangleWalk1.png");
            walkingTextures[1] = LoadTexture("assets/sprites/triangle/TriangleWalk2.png");
            walkingTextures[2] = LoadTexture("assets/sprites/triangle/TriangleWalk1.png");
            walkingTextures[3] = LoadTexture("assets/sprites/triangle/TriangleWalk3.png");

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

            if(switchTick % 500 == 400)
            {
                if(!isLeft && randomBool)
                {
                    isLeft = true;
                } else if(randomBool) {
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
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
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

            //! AI

            if(switchTick > 0)
            {
                switchTick--;
            } else {
                switchTick = 499;
            }

            /*if(switchTick % 100 <= 25)
            {
                stop = true;
            } else {
                stop = false;
            }*/

            if(switchTick == 250)
            {
                int value = new Random().Next(1, 3);
                switch(value)
                {
                    case 1:
                        Circle.Summon(Drect.Position);
                        break;

                    case 2:
                        Triangle.Summon(Drect.Position);
                        break;
                }
            }

            if (switchTick % 50 == 0)
            {
                Fireball.FireBad(Drect, isLeft);
            }
            
            foreach (Fireball fireball in Game.fireballs)
            {
                if (fireball.Drect.Intersects(Drect) && !fireball.bad)
                {
                    fireball.Die();
                    health -= 20;
                }
            }

            if (health == 0)
            {
                TriangleIsAlive = false;
            }

            if (Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                TriangleIsAlive = false;
            }

            if (Drect.Intersects(Game.player.Drect) && Game.player.Immunity == 0 && !Game.player.Immune && !Game.player.isSanic)
            {
                Player.Damage(30);
            }

            if (!TriangleIsAlive)
            {
                Game.triangleBosses.Remove(this);
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
                    Game.DrawRectHollow(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                foreach (var rect in verticalCollisions)
                {
                    Game.DrawRectHollow(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                Game.DrawRectHollow(Drect, 4, Color.Blue);
            }
        }

        public override string[] GetDebugInfo()
        {
            return new string[]
            {
                "---TRIANGLE BOSS---",
                "Random Bool: " + randomBool,
                "Delay: " + delay,
                "Is In Air: " + isInAir,
                "Is Left: " + isLeft,
                "Drect: " + Drect
            };
        }

        public static void Summon(Vector2 pos)
        {
            TriangleBoss triangleBoss = new TriangleBoss(idleTextures[1], new Rectangle((int)pos.X, (int)pos.Y, bossTriangleSizeW, bossTriangleSizeH), new Rectangle(0, 0, 20, 30));
            Game.triangleBosses.Add(triangleBoss);
        }

        public static void ClearAll()
        {
            Game.triangleBosses.Clear();
        }

        public void DrawBar()
        {
            string healthStat = "Boss Health: " + health + "/3600";
            DrawTextEx(Game.rijusans, healthStat, new Vector2((Settings.SimulationSize.X / 2) - (MeasureTextEx(Game.rijusans, healthStat, Game.statsSize, 0).X / 2), 10), Game.statsSize, 0, Color.Black);
        }
    }
}
