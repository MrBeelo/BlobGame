using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace BlobGame
{
    public class Player : MoveableSprite
    {
        public static int playerSizeW = 42;
        public static int playerSizeH = 64;
        public int speed = 3;
        public int stamina = 500;
        private static SoundEffect successSound;
        private static SoundEffect jumpSound;
        private static SoundEffect speedStartSound;
        private static SoundEffect speedEndSound;
        private static SoundEffect powerUpSound;
        private static SoundEffect laserShootSound;
        public Texture2D[] idleTextures;
        int idleCounter;
        int idleActiveFrame;
        public Texture2D[] walkingTextures;
        int walkingCounter;
        int walkingActiveFrame;
        public Texture2D[] jumpingTextures;
        public Direction direction = Direction.NA;
        public List<Point> horizontalCollisions;
        public List<Point> verticalCollisions;
        private float flickerTime;
        public bool alive = true;
        public bool isInAir = true;
        public static int gravity = 1;
        public bool isMoving = false;
        public bool isLeft = false;
        public bool isSanic = false;
        public bool isDashing = false;
        public bool horizColl = false;
        public bool vertColl = false;
        public bool hazardHorizColl = false;
        public bool hazardVertColl = false;
        public bool justCollided = false;
        public int coyoteTime = 0;
        public static int dashTime = -1;
        public enum Direction 
        {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NA}

        public Player(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            Velocity = new();
        }

        public void Initialize()
        {
        }

        public override void LoadContent(Game game)
        {

            idleTextures = new Texture2D[2];
            walkingTextures = new Texture2D[4];
            jumpingTextures = new Texture2D[2];

            idleTextures[0] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");
            idleTextures[1] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerIdle2");

            jumpingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerJump1");
            jumpingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerJump2");

            walkingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerWalk1");
            walkingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerWalk2");
            walkingTextures[2] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerWalk1");
            walkingTextures[3] = game.Content.Load<Texture2D>("assets/sprites/player/PlayerWalk3");

            successSound = game.Content.Load<SoundEffect>("assets/sounds/success");
            jumpSound = game.Content.Load<SoundEffect>("assets/sounds/jump");
            speedStartSound = game.Content.Load<SoundEffect>("assets/sounds/speedStart");
            speedEndSound = game.Content.Load<SoundEffect>("assets/sounds/speedEnd");
            powerUpSound = game.Content.Load<SoundEffect>("assets/sounds/powerUp");
            laserShootSound = game.Content.Load<SoundEffect>("assets/sounds/laserShoot");

            successSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);

            horizontalCollisions = GetIntersectingTiles(Drect);
            verticalCollisions = GetIntersectingTiles(Drect);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            flickerTime += Globals.TotalSeconds * 5; // Adjust speed by changing multiplier

            if(alive == false)
            {
                Main.currentGameState = Main.GameState.Death;
            }

            if(Drect.Y > 1500)
            {
                Drect.Y = 1500;
                alive = false;
            }
            else if(Drect.Y < -3000)
            {
                Drect.Y = -3000;
            }

            idleCounter++;
            if(idleCounter > 29)
            {
                idleCounter = 0;
                idleActiveFrame++;

                if(idleActiveFrame > idleTextures.Length - 1)
                {
                    idleActiveFrame = 0;
                }
            }

            walkingCounter++;
            if(walkingCounter > 44 - (speed * 6))
            {
                walkingCounter = 0;
                walkingActiveFrame++;

                if(walkingActiveFrame > walkingTextures.Length - 1)
                {
                    walkingActiveFrame = 0;
                }
            }

            //! Handling X Movement

            if(!isDashing)
            {
                Velocity.X = 0;
            }
            
            if (Main.inputManager.DLeft && !isDashing)
            {
                Velocity.X = -speed;
            } 
            else 
            if (Main.inputManager.DLeft && isDashing)
            {
                Velocity.X --;
            }
            
            if (Main.inputManager.DRight && !isDashing)
            {
                Velocity.X = speed;
            }
            else 
            if (Main.inputManager.DRight && isDashing)
            {
                Velocity.X ++;
            }

            if(isSanic && !isDashing)
            {
                speed = 6;
                stamina--;
            }

            if(stamina == 0 && isSanic && !isDashing)
            {
                speed = 3;
                isSanic = false;
                speedEndSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
            } else if(stamina < 0 && isSanic && !isDashing)
            {
                speed = 3;
                isSanic = false;
            }

            if(stamina < 500 && !isSanic && !isDashing)
            {
                speed = 3;
                stamina++;
            }

            //! Handling Y Movement

            if(!isDashing)
            {
                Velocity.Y += 0.5f;
            }

            if(Main.inputManager.PJump && !isInAir) {
                Velocity.Y = -10;
                jumpSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                coyoteTime = 0;
                justCollided = false;
            }

            Velocity.Y = Math.Min(25.0f, Velocity.Y);
            
            // Horizontal Collision Resolution
            Drect.X += (int)Velocity.X;
            horizColl = false;
            hazardHorizColl = false;
            horizontalCollisions = GetIntersectingTiles(Drect);
            
            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[(int)Tilemap.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 0) //! Solid
                    {
                    horizColl = true;
                    justCollided = true;
                    coyoteTime = 0;
                    Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right;
                        }
                        Velocity.X = 0; // Stop horizontal movement upon collision
                    }
                    else if(value == 1) //! Hazard
                    {
                    hazardHorizColl = true;
                    Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right;
                        }
                        Velocity.X = 0; // Stop horizontal movement upon collision
                    }
                    else if (value == 3) //! Win (Nothing happens from sides)
                    {
                    Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right;
                        }
                        Velocity.X = 0; // Stop horizontal movement upon collision
                    }
                    else if (value == 4) //! Water
                    {
                        if(Velocity.X > 1)
                        {
                            Velocity.X = 1;
                        }
                        else if(Velocity.X < -1)
                        {
                            Velocity.X = -1;
                        }
                    }
                    else if(value == 5) //! Crystal
                    {
                        if(stamina >= 500)
                        {
                            isSanic = true;
                            speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                            Tilemap.excludedNormalTiles.Add(new Vector3(26, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                        }
                    }
                    else if(value == 6) //! Breakable Block
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                            if (Velocity.X > 0) // Moving Right
                            {
                                Drect.X = collision.Left - Drect.Width;
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right;
                            }
                            Velocity.X = 0; // Stop horizontal movement upon collision
                        }
                    }
                    else if(value == 7) //! Dash Block
                    {
                        if(!isDashing)
                        {
                            horizColl = true;
                            justCollided = true;
                            coyoteTime = 0;
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                            if (Velocity.X > 0) // Moving Right
                            {
                                Drect.X = collision.Left - Drect.Width;
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right;
                            }
                            Velocity.X = 0; // Stop horizontal movement upon collision
                        }
                        else if(isDashing)
                        {
                            dashTime = 1;
                        }
                    }
                } else {
                    if(stamina == 500 && !isSanic)
                    {
                        Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 26);
                        Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 5);
                    }
                }
            }
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            vertColl = false;
            hazardVertColl = false;
            verticalCollisions = GetIntersectingTiles(Drect);
            
            isInAir = true;
            foreach (var tile in verticalCollisions)
            {
                if (Tilemap.Collision[(int)Tilemap.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 0) //! Solid
                    {
                        vertColl = true;
                        justCollided = true;
                        coyoteTime = 0;
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
                    else if(value == 1) //! Hazard
                    {
                        hazardVertColl = true;
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
                    else if(value == 2) //! Pass
                    {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            MoveLevel(this);
                            Main.currentGameState = Main.GameState.Pass;
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom;
                            Velocity.Y = 0;
                        }
                    }
                    else if (value == 3) //! Win
                    {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            Drect.Y = collision.Top - Drect.Height;
                            Velocity.Y = 0.5f;
                            isInAir = false;
                            Main.currentGameState = Main.GameState.Win;
                            Tilemap.excludedNormalTiles.Clear();
                            Tilemap.excludedCollisionTiles.Clear();
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom;
                            Velocity.Y = 0;
                        }
                    }
                    else if (value == 4) //! Water
                    {
                        if(Velocity.Y > 0)
                        {
                            Velocity.Y = 1;
                        }
                        isInAir = true;
                    }
                    else if(value == 5) //! Crystal
                    {
                        if(stamina >= 500)
                        {
                            isSanic = true;
                            speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                            Tilemap.excludedNormalTiles.Add(new Vector3(26, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                        }
                    }
                    else if(value == 6) //!Breakable Block
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
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
                    }
                    else if(value == 7) //! Dash Block
                    {
                        if(!isDashing)
                        {
                            vertColl = true;
                            justCollided = true;
                            coyoteTime = 0;
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
                        else if(isDashing)
                        {
                            dashTime = 1;
                        }
                    }
                } else {
                    if(stamina == 500 && !isSanic)
                    {
                        Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 26);
                        Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 5);
                    }
                }
            }

            //! Handling Coyote Time

            if(!horizColl && !vertColl && justCollided == true)
            {
                justCollided = false;
                coyoteTime = 5;
            }

            if(coyoteTime > 0)
            {
                coyoteTime--;
            }

            //! Handling Deaths

            if(!horizColl && hazardHorizColl)
            {
                alive = false;
            }

            if(!vertColl && hazardVertColl)
            {
                alive = false;
            }

            //! Handling the Fireball

            if(Main.inputManager.PFireball && (stamina >= 500 || isSanic))
            {
                Fireball fireball = new Fireball(Fireball.fireTextures[1], new Rectangle(Drect.Center.X, Drect.Center.Y - 15, 32, 32), new Rectangle(0, 0, 16, 16), Globals.Graphics, isLeft);
                Main.fireballs.Add(fireball);
                Main.sprites.Add(fireball);
                stamina -= 100;
                laserShootSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
            }

            //! Handling the Dash

            if(Main.inputManager.PDash && stamina >= 500 && dashTime < 0)
            {
                Dash(Main.inputManager.pressedDirection, 25, 10);
                powerUpSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                isDashing = true;
                stamina -= 100;
            }

            if(dashTime > -1)
            {
                dashTime --;
            }

            if(dashTime == 0)
            {
                isDashing = false;
                Velocity.Y = -2.5f;
            }

            //! Logic For Direction and all other bools

            if(Velocity.Y > 0.5 && Velocity.X > 0)
            {
                direction = Direction.DownRight;
            }
            else if(Velocity.Y > 0.5 && Velocity.X < 0)
            {
                direction = Direction.DownLeft;
            }
            else if(Velocity.Y < 0 && Velocity.X > 0)
            {
                direction = Direction.UpRight;
            }
            else if(Velocity.Y < 0 && Velocity.X < 0)
            {
                direction = Direction.UpLeft;
            }
            else if(Velocity.X > 0)
            {
                direction = Direction.Right;
            }
            else if(Velocity.X < 0)
            {
                direction = Direction.Left;
            }
            else if(Velocity.Y > 0.5)
            {
                direction = Direction.Down;
            }
            else if(Velocity.Y < 0)
            {
                direction = Direction.Up;
            }
            else if(Velocity.X == 0 && Velocity.Y == 0.5)
            {
                direction = Direction.NA;
            }

            KeyboardState kstate = Keyboard.GetState();

            /*if(kstate.IsKeyDown(Keys.S) && kstate.IsKeyDown(Keys.D))
            {
                pressedDirection = PressedDirection.DownRight;
            }
            else if(kstate.IsKeyDown(Keys.S) && kstate.IsKeyDown(Keys.A))
            {
                pressedDirection = PressedDirection.DownLeft;
            }
            else if(kstate.IsKeyDown(Keys.W) && kstate.IsKeyDown(Keys.D))
            {
                pressedDirection = PressedDirection.UpRight;
            }
            else if(kstate.IsKeyDown(Keys.W) && kstate.IsKeyDown(Keys.A))
            {
                pressedDirection = PressedDirection.UpLeft;
            }
            else if(kstate.IsKeyDown(Keys.D))
            {
                pressedDirection = PressedDirection.Right;
            }
            else if(kstate.IsKeyDown(Keys.A))
            {
                pressedDirection = PressedDirection.Left;
            }
            else if(kstate.IsKeyDown(Keys.S))
            {
                pressedDirection = PressedDirection.Down;
            }
            else if(kstate.IsKeyDown(Keys.W))
            {
                pressedDirection = PressedDirection.Up;
            }
            else
            {
                pressedDirection = PressedDirection.NA;
            }*/

            isMoving = Velocity.X != 0;

            if(Velocity.X < 0)
            {
                isLeft = true;
            } else if(Velocity.X > 0)
            {
                isLeft = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = isLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if(isMoving && !isInAir)
            {
                Globals.SpriteBatch.Draw(
                    walkingTextures[walkingActiveFrame],
                    Drect,
                    Srect,
                    PlayerColor(),
                    0f,
                    Vector2.Zero,
                    spriteEffects,
                    0f
                );
            }

            if(isInAir)
            {
                if(Velocity.Y >= 5 || Velocity.Y <= -5)
                {
                    Globals.SpriteBatch.Draw(
                        jumpingTextures[0],
                        Drect,
                        Srect,
                        PlayerColor(),
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
                        PlayerColor(),
                        0f,
                        Vector2.Zero,
                        spriteEffects,
                        0f
                    );
                }
            }

            if(!isMoving && !isInAir)
            {
                Globals.SpriteBatch.Draw(
                    idleTextures[idleActiveFrame],
                    Drect,
                    Srect,
                    PlayerColor(),
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
                "Position: " + Position,
                "Tile Position: " + TilePosition,
                "Velocity: " + Velocity,
                "Speed: " + speed,
                "Stamina: " + stamina,
                "Is in Air: " + isInAir,
                "Is Moving: " + isMoving,
                "Is looking Left: " + isLeft,
                "Direction: " + direction,
                "Alive: " + alive,
                "Is Sanic: " + isSanic,
                "Is Dashing: " + isDashing,
                "Colliding Horizontally: " + horizColl,
                "Colliding Vertically: " + vertColl,
                "CollHazard Horizontally: " + hazardHorizColl,
                "CollHazard Verically: " + hazardVertColl,
                "Just Collided: " + justCollided,
                "Coyote Time: " + coyoteTime,
                "Dash Time: " + dashTime
            };
        }
        public List<Point> GetIntersectingTiles(Rectangle target)
        {
            List<Point> tiles = new List<Point>();
        
            int leftTile = target.Left / Tilemap.Tilesize;
            int rightTile = (target.Right - 1) / Tilemap.Tilesize;
            int topTile = target.Top / Tilemap.Tilesize;
            int bottomTile = (target.Bottom - 1) / Tilemap.Tilesize;
        
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    tiles.Add(new Point(x, y));
                }
            }
        
            return tiles;
        }

        public static void ResetPos(Player player)
        {
            player.Drect = new Rectangle((int)Tilemap.level.Y, (int)Tilemap.level.Z, playerSizeW, playerSizeH);
        }

        public static void ResetState(Player player)
        {
            player.Velocity = new Vector2(0, 0.5f);
            player.alive = true;
            player.isLeft = false;
            player.stamina = 500;
            player.speed = 3;
            player.isSanic = false;
            player.isDashing = false;
            player.horizColl = false;
            player.vertColl = false;
            player.hazardHorizColl = false;
            player.hazardVertColl = false;
            Main.fireballs.Clear();
        }

        public Color PlayerColor()
        {
            if(stamina > 0 && stamina < 500 && !isSanic)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2; // Normalizes to range 0-1
                return Color.Lerp(Color.White, Color.LightSalmon, t); // Blends from white to red based on t
            }
            else if(isSanic)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2; // Normalizes to range 0-1
                return Color.Lerp(Color.White, Color.LightBlue, t); // Blends from white to red based on t
            }
            return Color.White;
        }

        public static void Dash(InputManager.PressedDirection pressedDirection, int power, int DashTime)
        {
            dashTime = DashTime;
            double hpower = power * 0.6; //! Horizontal Power

            switch(pressedDirection)
            {
                case InputManager.PressedDirection.Right:
                    Main.player.Velocity.X = (int)hpower;
                    Main.player.Velocity.Y = 0.5f;
                    break;

                case InputManager.PressedDirection.Left:
                    Main.player.Velocity.X = (int)-hpower;
                    Main.player.Velocity.Y = 0.5f;
                    break;

                case InputManager.PressedDirection.Down:
                    Main.player.Velocity.Y = power;
                    break;

                case InputManager.PressedDirection.Up:
                    Main.player.Velocity.Y = -power;
                    break;

                case InputManager.PressedDirection.DownRight:
                    Main.player.Velocity.X = (int)hpower;
                    Main.player.Velocity.Y = power;
                    break;

                case InputManager.PressedDirection.DownLeft:
                    Main.player.Velocity.X = (int)-hpower;
                    Main.player.Velocity.Y = power;
                    break;
                
                case InputManager.PressedDirection.UpRight:
                    Main.player.Velocity.X = (int)hpower;
                    Main.player.Velocity.Y = -power;
                    break;

                case InputManager.PressedDirection.UpLeft:
                    Main.player.Velocity.X = (int)-hpower;
                    Main.player.Velocity.Y = -power;
                    break;
                
                case InputManager.PressedDirection.NA:
                    if(!Main.player.isLeft)
                    {
                        Main.player.Velocity.X = (int)hpower;
                        Main.player.Velocity.Y = 0.5f;
                    }
                    else if(Main.player.isLeft)
                    {
                        Main.player.Velocity.X = (int)-hpower;
                        Main.player.Velocity.Y = 0.5f;
                    }
                    break;
            }
        }

        public static void MoveLevel(Player player)
        {
            Tilemap.level.X++;

            switch(Tilemap.level.X)
            {
                case 0:
                    Tilemap.level.Y = 50;
                    Tilemap.level.Z = 600;
                    break;

                case 1:
                    Tilemap.level.Y = 50;
                    Tilemap.level.Z = 200;
                    break;

                case 2:
                    Tilemap.level.Y = 220;
                    Tilemap.level.Z = 150;
                    break;

            }

            Tilemap.excludedNormalTiles.Clear();
            Tilemap.excludedCollisionTiles.Clear();
            ResetState(player);
        }
    }
}
