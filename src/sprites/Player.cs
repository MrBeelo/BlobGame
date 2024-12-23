using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;


namespace BlobGame
{
    public class Player : CollMoveableSprite
    {
        public static int playerSizeW = 42;
        public static int playerSizeH = 64;
        public float Health {get; set;} = 100;
        public int stamina = 500;
        public static SoundEffect successSound;
        private static SoundEffect jumpSound;
        private static SoundEffect speedStartSound;
        private static SoundEffect speedEndSound;
        private static SoundEffect powerUpSound;
        private static SoundEffect laserShootSound;
        public static SoundEffect hitSound;
        public Texture2D[] idleTextures;
        int idleCounter;
        int idleActiveFrame;
        public Texture2D[] walkingTextures;
        int walkingCounter;
        int walkingActiveFrame;
        public Texture2D[] jumpingTextures;
        private float flickerTime;
        public bool alive = true;
        public static int gravity = 1;
        public bool isSanic = false;
        public bool isDashing = false;
        public bool horizColl = false;
        public bool vertColl = false;
        public bool hazardHorizColl = false;
        public bool hazardVertColl = false;
        public bool justCollided = false;
        public int justDrankMilk = 0;
        public int coyoteTime = 0;
        public static int dashTime = -1;
        public int sanicTime = 500;
        public bool doubleJump = false;
        public int djReset = 0;
        public int Immunity = 0;
        public bool Immune = false;
        public Matrix translation;
        public static int xartomantila = 0;
        

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
            hitSound = game.Content.Load<SoundEffect>("assets/sounds/hitBlob");

            successSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
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
                sanicTime--;
            }

            if(sanicTime == 0 && isSanic && !isDashing)
            {
                speed = 3;
                isSanic = false;
                speedEndSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
            } else if(sanicTime < 0 && isSanic && !isDashing)
            {
                speed = 3;
                isSanic = false;
            }

            if(sanicTime < 500 && !isSanic && !isDashing)
            {
                speed = 3;
                sanicTime++;
            }

            if(stamina < 500)
            {
                stamina++;
            }

            //! Handling Y Movement

            if(!isDashing)
            {
                Velocity.Y += 0.5f;
            }

            if((Main.inputManager.PJump && !isInAir) || (Main.inputManager.PJump && isInAir && doubleJump)) {
                Velocity.Y = -10;
                jumpSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                coyoteTime = 0;
                justCollided = false;
                doubleJump = false;
            }

            Velocity.Y = Math.Min(40.0f, Velocity.Y);

            SetBounds();
            Drect.Location = PointClamp(Drect.Location, minPos, maxPos);
            
            // Horizontal Collision Resolution
            Drect.X += (int)Velocity.X;
            horizColl = false;
            hazardHorizColl = false;
            horizontalCollisions = GetIntersectingTiles(Drect);
            
            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
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
                    else if(value == 1 && !Immune) //! Hazard
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
                        if(!horizColl)
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
                    }
                    else if(value == 5) //! Boost Crystal
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            if(sanicTime >= 500)
                            {
                                isSanic = true;
                                speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                                Tilemap.excludedNormalTiles.Add(new Vector3(26, tile.X, tile.Y));
                                Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            }
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
                            dashTime = 3;
                        }
                    }
                    else if(value == 8) //! Double Jump Crystal
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            doubleJump = true;
                            djReset = 250;
                            speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                            Tilemap.excludedNormalTiles.Add(new Vector3(31, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                        }
                    }
                    else if(value == 9) //! Milk
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            if(Health < 100)
                            {
                                Health += 50;
                                speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                                Tilemap.excludedNormalTiles.Add(new Vector3(32, tile.X, tile.Y));
                                Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                                justDrankMilk = 50;
                            }
                        }
                    }
                    else if(value == 10) //! Xartomantila
                    {
                        if(!Tilemap.permaExcludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.permaExcludedNormalTiles.Add(new Vector3(33, tile.X, tile.Y));
                            Tilemap.permaExcludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            xartomantila++;
                            Globals.SaveFile.SaveSavefile(Main.savefileFilePath);
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
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 0) //! Solid
                    {
                        vertColl = true;
                        justCollided = true;
                        coyoteTime = 0;
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            TakeFallDmg();
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
                    else if(value == 1 && !Immune) //! Hazard
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
                            Tilemap.MoveLevel();
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
                        if(!vertColl)
                        {
                            if(Velocity.Y > 0)
                            {
                                Velocity.Y = 1;
                            }
                            isInAir = true;
                        }
                    }
                    else if(value == 5) //! Crystal
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            if(sanicTime >= 500)
                            {
                                isSanic = true;
                                speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                                Tilemap.excludedNormalTiles.Add(new Vector3(26, tile.X, tile.Y));
                                Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            }
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
                            dashTime = 3;
                        }
                    }
                    else if(value == 8) //! Double Jump Crystal
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            doubleJump = true;
                            djReset = 250;
                            speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                            Tilemap.excludedNormalTiles.Add(new Vector3(31, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                        }
                    }
                    else if(value == 9) //! Milk
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            if(Health < 100)
                            {
                                Health += 50;
                                speedStartSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                                Tilemap.excludedNormalTiles.Add(new Vector3(32, tile.X, tile.Y));
                                Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            }
                        }
                    }
                    else if(value == 10) //! Xartomantila
                    {
                        if(!Tilemap.permaExcludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.permaExcludedNormalTiles.Add(new Vector3(33, tile.X, tile.Y));
                            Tilemap.permaExcludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            xartomantila++;
                            Globals.SaveFile.SaveSavefile(Main.savefileFilePath);
                        }
                    }
                } else {
                    if(sanicTime == 500 && !isSanic)
                    {
                        Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 26);
                        Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 5);
                    }
                }
            }

            if(Drect.Y >= maxPos.Y)
            {
                isInAir = false;
            }

            //! Handling Double Jump

            if(djReset > 0)
            {
                djReset--;
            }

            if(djReset == 1)
            {
                Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 31);
                Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 8);
            }

            if(djReset == 0)
            {
                djReset = 500;
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
                Die();
            }

            if(!vertColl && hazardVertColl)
            {
                Die();
            }

            //! Handling the Fireball

            if(Main.inputManager.PFireball && (stamina >= 500 || isSanic))
            {
                Fireball fireball = new Fireball(Fireball.fireTextures[1], new Rectangle(Drect.Center.X, Drect.Center.Y - 15, 32, 32), new Rectangle(0, 0, 16, 16), Globals.Graphics, isLeft);
                Main.fireballs.Add(fireball);
                laserShootSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                if(!isSanic)
                {
                    stamina -= 100;
                }
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

            //! Handling Health

            if(Health > 100)
            {
                Health = 100;
            }

            if(Health <= 0)
            {
                alive = false;
            }

            if(Immunity > 0)
            {
                Immunity--;
            } 

            //! Handling Milk

            if(justDrankMilk > 0)
            {
                justDrankMilk--;
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

            if (Main.hasF3On)
            {
                foreach (var rect in horizontalCollisions)
                {
                    Main.DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                foreach (var rect in verticalCollisions)
                {
                    Main.DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                Main.DrawRectHollow(Globals.SpriteBatch, Drect, 4, Color.Blue);
            }
        }

        public override string[] GetDebugInfo()
        {
            return new string[] 
            {
                "----PLAYER----",
                "Position: " + Position,
                "Tile Position: " + TilePosition,
                "Velocity: " + Velocity,
                "Speed: " + speed,
                "Stamina: " + stamina,
                "Is in Air: " + isInAir,
                "Is Moving: " + isMoving,
                "Is looking Left: " + isLeft,
                "Direction: " + Direction,
                "Alive: " + alive,
                "Is Sanic: " + isSanic,
                "Is Dashing: " + isDashing,
                "Colliding Horizontally: " + horizColl,
                "Colliding Vertically: " + vertColl,
                "CollHazard Horizontally: " + hazardHorizColl,
                "CollHazard Verically: " + hazardVertColl,
                "Just Collided: " + justCollided,
                "Coyote Time: " + coyoteTime,
                "Dash Time: " + dashTime,
                "Double Jump: " + doubleJump,
                "Immunity: " + Immunity,
                "Immune: " + Immune,
                "Sanic Time: " + sanicTime,
                "Double Jump Reset: " + djReset,
                "Xartomantila: " + xartomantila
            };
        }

        public static void ResetPos(Player player)
        {
            Tilemap.Eval();
        }

        public static void ResetState(Player player)
        {
            player.Velocity = new Vector2(0, 0.5f);
            player.isLeft = false;
            player.stamina = 500;
            player.speed = 3;
            player.isSanic = false;
            player.isDashing = false;
            player.horizColl = false;
            player.vertColl = false;
            player.hazardHorizColl = false;
            player.hazardVertColl = false;
            player.sanicTime = 500;
            Main.fireballs.Clear();
        }

        public static void Respawn(Player player)
        {
            player.alive = true;
            player.Health = 100;
            ResetPos(player);
            ResetState(player);
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
                float t = (MathF.Sin(flickerTime) + 1) / 2;
                return Color.Lerp(Color.White, Color.LightBlue, t);
            }

            if(Immunity > 0)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2;
                return Color.Lerp(Color.White, Color.Red, t);
            }

            if(justDrankMilk > 0)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2;
                return Color.Lerp(Color.White, Color.LightGreen, t);
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

        public void CalculateTranslation()
        {
            var dx = (Settings.SimulationSize.X / 2) - Main.player.Drect.X - Main.player.Drect.Width;
            dx = MathHelper.Clamp(dx, -Main.tilemap.Mapsize.X + Settings.SimulationSize.X, 0);
            var dy = (Settings.SimulationSize.Y / 2) - Main.player.Drect.Y - Main.player.Drect.Height;
            dy = MathHelper.Clamp(dy, -Main.tilemap.Mapsize.Y + Settings.SimulationSize.Y, 0);
            translation = Matrix.CreateTranslation(dx, dy, 0f);
        }

        public static void TakeFallDmg()
        {
            switch(Main.player.Velocity.Y)
            {
                case >= 40:
                    Damage(15);
                    break;

                case > 35:
                    Damage(12.5f);
                    break;

                case > 30:
                    Damage(10);
                    break;

                case > 25:
                    Damage(7.5f);
                    break;

                case > 20:
                    Damage(5);
                    break;
            }
        }

        public static void Damage(float dmgAmount)
        {
            Main.player.Health -= dmgAmount;
            if(Main.player.Health > 0)
            {
                hitSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
            }
            Main.player.Immunity = 50;
        }

        public static void Die()
        {
            Main.player.alive = false;
            Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 31);
            Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 8);
            Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 32);
            Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 9);
            ResetState(Main.player);
            hitSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
        }

        public static void Teleport(int x, int y)
        {
            Main.player.Drect = new Rectangle(x, y, playerSizeW, playerSizeH);
        }
    }
}
