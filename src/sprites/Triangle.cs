using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;


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
        bool TriangleIsAlive = true;
        public static int bossTriangleSizeW = 210;
        public static int bossTriangleSizeH = 320;

        public Triangle(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            Velocity = new();
        }

        public override void LoadContent(Game game)
        {
            base.LoadContent(game);

            idleTextures = new Texture2D[2];
            walkingTextures = new Texture2D[4];
            jumpingTextures = new Texture2D[2];

            idleTextures[0] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleIdle1");
            idleTextures[1] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleIdle2");

            jumpingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleJump1");
            jumpingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleJump2");

            walkingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleWalk1");
            walkingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleWalk2");
            walkingTextures[2] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleWalk1");
            walkingTextures[3] = game.Content.Load<Texture2D>("assets/sprites/triangle/TriangleWalk3");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

            // Horizontal Collision Resolution
            Drect.X += (int)Velocity.X;
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
                if (Tilemap.Collision[(int)Tilemap.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if (!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if (value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);

                            if (Velocity.X > 0) // Moving Right
                            {
                                Drect.X = collision.Left - Drect.Width;

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
                                Drect.X = collision.Right;

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
                if (Tilemap.Collision[(int)Tilemap.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if (!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if (value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);

                            if (Velocity.Y > 0) // Falling Down
                            {
                                Drect.Y = collision.Top - Drect.Height;
                                Velocity.Y = 0.5f;
                                isInAir = false;
                            }
                            else if (Velocity.Y < 0) // Moving Up
                            {
                                Drect.Y = collision.Bottom;
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

            if (Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                TriangleIsAlive = false;
            }

            if (Drect.Intersects(Main.player.Drect) && Main.player.Immunity == 0 && !Main.player.Immune && !Main.player.isSanic)
            {
                //Player.hitSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                //Main.player.Health -= 20;
                //Main.player.Immunity = 50;
                Player.Damage(20);
            }

            if (!TriangleIsAlive)
            {
                Main.triangles.Remove(this);
                //Main.sprites.Remove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = isLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (isMoving && !isInAir)
            {
                Globals.SpriteBatch.Draw(
                    walkingTextures[walkingActiveFrame],
                    Drect,
                    Srect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    spriteEffects,
                    0f
                );
            }

            if (isInAir)
            {
                if (Velocity.Y >= 5 || Velocity.Y <= -5)
                {
                    Globals.SpriteBatch.Draw(
                        jumpingTextures[0],
                        Drect,
                        Srect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        spriteEffects,
                        0f
                    );
                }
                else
                {
                    Globals.SpriteBatch.Draw(
                        jumpingTextures[1],
                        Drect,
                        Srect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        spriteEffects,
                        0f
                    );
                }
            }

            if (!isMoving && !isInAir)
            {
                Globals.SpriteBatch.Draw(
                    idleTextures[idleActiveFrame],
                    Drect,
                    Srect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    spriteEffects,
                    0f
                );
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
            Triangle triangle = new Triangle(idleTextures[1], new Rectangle((int)pos.X, (int)pos.Y, triangleSizeW, triangleSizeH), new Rectangle(0, 0, 20, 30), Globals.Graphics);
            Main.triangles.Add(triangle);
        }

        public static void SummonBoss(Vector2 pos)
        {
            Triangle triangle = new Triangle(idleTextures[1], new Rectangle((int)pos.X, (int)pos.Y, bossTriangleSizeW, bossTriangleSizeH), new Rectangle(0, 0, 20, 30), Globals.Graphics);
            Main.triangles.Add(triangle);
        }

        public static void ClearAll()
        {
            Main.triangles.Clear();
        }
    }
}
