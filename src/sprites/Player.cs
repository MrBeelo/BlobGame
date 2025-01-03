using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace BlobGame
{
    public class Player : CollMoveableSprite
    {
        public static Camera2D camera = new();
        public static int playerSizeW = 42;
        public static int playerSizeH = 64;
        public float Health {get; set;} = 100;
        public int stamina = 500;
        public static Sound successSound;
        private static Sound jumpSound;
        private static Sound speedStartSound;
        private static Sound speedEndSound;
        private static Sound powerUpSound;
        public static Sound hitSound;
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
        public Matrix3x2 translation;
        public static int xartomantila = 0;
        public static Vector2 playerMaxPos;
        

        public Player(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
        }

        public override void LoadContent(Game game)
        {

            idleTextures = new Texture2D[2];
            walkingTextures = new Texture2D[4];
            jumpingTextures = new Texture2D[2];

            idleTextures[0] = LoadTexture("assets/sprites/player/PlayerIdle1.png");
            idleTextures[1] = LoadTexture("assets/sprites/player/PlayerIdle2.png");

            jumpingTextures[0] = LoadTexture("assets/sprites/player/PlayerJump1.png");
            jumpingTextures[1] = LoadTexture("assets/sprites/player/PlayerJump2.png");

            walkingTextures[0] = LoadTexture("assets/sprites/player/PlayerWalk1.png");
            walkingTextures[1] = LoadTexture("assets/sprites/player/PlayerWalk2.png");
            walkingTextures[2] = LoadTexture("assets/sprites/player/PlayerWalk1.png");
            walkingTextures[3] = LoadTexture("assets/sprites/player/PlayerWalk3.png");

            successSound = LoadSound("assets/sounds/success.wav");
            jumpSound = LoadSound("assets/sounds/jump.wav");
            speedStartSound = LoadSound("assets/sounds/speedStart.wav");
            speedEndSound = LoadSound("assets/sounds/speedEnd.wav");
            powerUpSound = LoadSound("assets/sounds/powerUp.wav");
            hitSound = LoadSound("assets/sounds/hitBlob.wav");

            PlaySound(successSound);

            camera.Zoom = 1;
            camera.Target = Drect.Position;
            camera.Offset = new Vector2(Settings.SimulationSize.X / 2, Settings.SimulationSize.Y / 2);
        }

        public override void Update()
        {
            base.Update();

            flickerTime += GetFrameTime() * 5; // Adjust speed by changing multiplier

            if(alive == false)
            {
                Game.currentGameState = Game.GameState.Death;
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
            
            if (Game.inputManager.DLeft && !isDashing)
            {
                Velocity.X = -speed;
            } 
            else 
            if (Game.inputManager.DLeft && isDashing)
            {
                Velocity.X --;
            }
            
            if (Game.inputManager.DRight && !isDashing)
            {
                Velocity.X = speed;
            }
            else 
            if (Game.inputManager.DRight && isDashing)
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
                PlaySound(speedEndSound);
                SetSoundVolume(speedEndSound, (float)Game.LoweredVolume);
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

            if((Game.inputManager.PJump && !isInAir) || (Game.inputManager.PJump && isInAir && doubleJump)) {
                Velocity.Y = -10;
                PlaySound(jumpSound);
                SetSoundVolume(jumpSound, (float)Game.LoweredVolume);
                coyoteTime = 0;
                justCollided = false;
                doubleJump = false;
            }

            Velocity.Y = Math.Min(40.0f, Velocity.Y);

            SetBounds();
            SetPlayerBounds();
            Drect.Position = Vector2.Clamp(Drect.Position, minPos, playerMaxPos);
            UpdateCamera();
            
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
                            Drect.X = collision.Left() - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right();
                        }
                        Velocity.X = 0; // Stop horizontal movement upon collision
                    }
                    else if((value == 1) && !Immune) //! Hazard
                    {
                    hazardHorizColl = true;
                    Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left() - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right();
                        }
                        Velocity.X = 0; // Stop horizontal movement upon collision
                    }
                    else if (value == 3) //! Win (Nothing happens from sides)
                    {
                    Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left() - Drect.Width;
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right();
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
                                PlaySound(speedStartSound);
                                SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                                Drect.X = collision.Left() - Drect.Width;
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right();
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
                                Drect.X = collision.Left() - Drect.Width;
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right();
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
                            PlaySound(speedStartSound);
                            SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                                PlaySound(speedStartSound);
                                SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                            Globals.SaveFile.SaveSavefile(Game.savefileFilePath);
                        }
                    }
                    else if(value >= 11 && value <= 14 && !Immune)
                    {
                        foreach(Rectangle spike in Tilemap.spikes)
                        {
                            if(Drect.Intersects(spike))
                            {
                                hazardHorizColl = true;
                            }
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
                    else if(value == 1 && !Immune) //! Hazard
                    {
                        hazardVertColl = true;
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
                    else if(value == 2) //! Pass
                    {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            Tilemap.MoveLevel();
                            Game.currentGameState = Game.GameState.Pass;
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom();
                            Velocity.Y = 0;
                        }
                    }
                    else if (value == 3) //! Win
                    {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            Drect.Y = collision.Top() - Drect.Height;
                            Velocity.Y = 0.5f;
                            isInAir = false;
                            Game.currentGameState = Game.GameState.Win;
                            Tilemap.excludedNormalTiles.Clear();
                            Tilemap.excludedCollisionTiles.Clear();
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom();
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
                                PlaySound(speedStartSound);
                                SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                            PlaySound(speedStartSound);
                            SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                                PlaySound(speedStartSound);
                                SetSoundVolume(speedStartSound, (float)Game.LoweredVolume);
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
                            Globals.SaveFile.SaveSavefile(Game.savefileFilePath);
                        }
                    }
                    else if(value >= 11 && value <= 14 && !Immune)
                    {
                        foreach(Rectangle spike in Tilemap.spikes)
                        {
                            if(Drect.Intersects(spike))
                            {
                                hazardVertColl = true;
                            }
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

            if(Game.inputManager.PFireball && (stamina >= 500 || isSanic))
            {
                Fireball.Fire(Drect, isLeft);
                if(!isSanic)
                {
                    stamina -= 100;
                }
            }

            //! Handling the Dash

            if(Game.inputManager.PDash && stamina >= 500 && dashTime < 0)
            {
                Dash(Game.inputManager.pressedDirection, 25, 10);
                PlaySound(powerUpSound);
                SetSoundVolume(powerUpSound, (float)Game.LoweredVolume);
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

        public override void Draw()
        {
            float flip = isLeft ? -1.0f : 1.0f;
            Srect = new(0, 0, 20 * flip, 30);

            if(isMoving && !isInAir)
            {
                DrawTexturePro(
                    walkingTextures[walkingActiveFrame],
                    Srect,
                    Drect,
                    Vector2.Zero,
                    0f,
                    PlayerColor()
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
                        PlayerColor()
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
                        PlayerColor()
                    );
                }
            }

            if((!isMoving) && !isInAir)
            {
                DrawTexturePro(
                    idleTextures[idleActiveFrame],
                    Srect,
                    Drect,
                    Vector2.Zero,
                    0f,
                    PlayerColor()
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
            Game.fireballs.Clear();
        }

        public static void Respawn(Player player)
        {
            player.alive = true;
            player.Health = 100;
            ResetPos(player);
            ResetState(player);
        }

        public static Color DamageColor() { return new Color(255, 150, 143);}

        public Color PlayerColor()
        {
            if((stamina > 0 && stamina < 500 && !isSanic && !isDashing) || Immunity > 0)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2; // Normalizes to range 0-1
                return ColorLerp(Color.White, DamageColor(), t); // Blends from white to red based on t
            }
            else if(isSanic)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2;
                return ColorLerp(Color.White, Color.SkyBlue, t);
            }

            if(justDrankMilk > 0)
            {
                float t = (MathF.Sin(flickerTime) + 1) / 2;
                return ColorLerp(Color.White, Color.Green, t);
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
                    Game.player.Velocity.X = (int)hpower;
                    Game.player.Velocity.Y = 0.5f;
                    break;

                case InputManager.PressedDirection.Left:
                    Game.player.Velocity.X = (int)-hpower;
                    Game.player.Velocity.Y = 0.5f;
                    break;

                case InputManager.PressedDirection.Down:
                    Game.player.Velocity.Y = power;
                    break;

                case InputManager.PressedDirection.Up:
                    Game.player.Velocity.Y = -power;
                    break;

                case InputManager.PressedDirection.DownRight:
                    Game.player.Velocity.X = (int)hpower;
                    Game.player.Velocity.Y = power;
                    break;

                case InputManager.PressedDirection.DownLeft:
                    Game.player.Velocity.X = (int)-hpower;
                    Game.player.Velocity.Y = power;
                    break;
                
                case InputManager.PressedDirection.UpRight:
                    Game.player.Velocity.X = (int)hpower;
                    Game.player.Velocity.Y = -power;
                    break;

                case InputManager.PressedDirection.UpLeft:
                    Game.player.Velocity.X = (int)-hpower;
                    Game.player.Velocity.Y = -power;
                    break;
                
                case InputManager.PressedDirection.NA:
                    if(!Game.player.isLeft)
                    {
                        Game.player.Velocity.X = (int)hpower;
                        Game.player.Velocity.Y = 0.5f;
                    }
                    else if(Game.player.isLeft)
                    {
                        Game.player.Velocity.X = (int)-hpower;
                        Game.player.Velocity.Y = 0.5f;
                    }
                    break;
            }
        }

        /*public void CalculateTranslation()
        {
            var dx = (Settings.SimulationSize.X / 2) - Game.player.Drect.X - Game.player.Drect.Width;
            dx = Math.Clamp(dx, -Game.tilemap.Mapsize.X + Settings.SimulationSize.X, 0);
            var dy = (Settings.SimulationSize.Y / 2) - Game.player.Drect.Y - Game.player.Drect.Height;
            dy = Math.Clamp(dy, -Game.tilemap.Mapsize.Y + Settings.SimulationSize.Y, 0);
            translation = Matrix3x2.CreateTranslation(new Vector2(dx, dy));
        }*/

        public static void TakeFallDmg()
        {
            switch(Game.player.Velocity.Y)
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
            Game.player.Health -= dmgAmount;
            if(Game.player.Health > 0)
            {
                PlaySound(hitSound);
                SetSoundVolume(hitSound, (float)Game.LoweredVolume);
            }
            Game.player.Immunity = 50;
        }

        public static void Die()
        {
            Game.player.alive = false;
            Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 31);
            Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 8);
            Tilemap.excludedNormalTiles.RemoveAll(removeTile => removeTile.X == 32);
            Tilemap.excludedCollisionTiles.RemoveAll(removeTile => removeTile.X == 9);
            ResetState(Game.player);
            PlaySound(hitSound);
            SetSoundVolume(hitSound, (float)Game.LoweredVolume);
        }

        public static void Teleport(int x, int y)
        {
            Game.player.Drect = new Rectangle(x, y, playerSizeW, playerSizeH);
        }

        public void SetPlayerBounds()
        {
            playerMaxPos = maxPos;
            playerMaxPos.X -= Drect.Width;
            playerMaxPos.Y -= Drect.Height;
        }

        public void UpdateCamera()
        {
            camera.Target = Drect.Position;

            float halfX = Settings.SimulationSize.X / 2 / camera.Zoom;
            float halfY = Settings.SimulationSize.Y / 2 / camera.Zoom;

            camera.Offset = new Vector2(halfX, halfY);

            float clampX = Math.Clamp(camera.Target.X, minPos.X + halfX, maxPos.X - halfX);
            float clampY = Math.Clamp(camera.Target.Y, minPos.Y + halfY, maxPos.Y - halfY);

            Debug.WriteLine(clampX + ", " + clampY);

            camera.Target = new Vector2(clampX, clampY);
        }
    }
}
