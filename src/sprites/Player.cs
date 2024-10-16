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
        public static int playerSizeW = 53;
        public static int playerSizeH = 80;
        public int speed = 3;
        public int stamina = 0;
        private SoundEffect successSound;
        private SoundEffect jumpSound;
        private SoundEffect speedStartSound;
        private SoundEffect speedEndSound;
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

        public bool isInAir = true;
        public static int gravity = 1;
        public bool isMoving = false;
        public bool isLeft = false;

        public enum Direction 
        {
            Up,
            Down,
            Left,
            Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight,
            NA
        }

        KeyboardState prevkstate;

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

            successSound.Play(Globals.Settings.Volume, 0.0f, 0.0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

            KeyboardState kstate = Keyboard.GetState();
            Velocity.X = 0;
            Velocity.Y += 0.5f;

            Velocity.Y = Math.Min(25.0f, Velocity.Y);
            
            if (kstate.IsKeyDown(Keys.A))
            {
                Velocity.X = -speed;
            }
            
            if (kstate.IsKeyDown(Keys.D))
            {
                Velocity.X = speed;
            }

            if(Main.IsKeyPressed(kstate, prevkstate, Keys.Space) && !isInAir) {
                Velocity.Y = -10;
                jumpSound.Play(Globals.Settings.Volume, 0.0f, 0.0f);
            }

            if(Main.IsKeyPressed(kstate, prevkstate, Keys.LeftShift) && stamina == 0) {
                stamina = 1000;
                speedStartSound.Play(Globals.Settings.Volume, 0.0f, 0.0f);
            }

            if(stamina > 600) {
                speed = 6;
                stamina--;
            } else if (stamina == 600)
            {
                speed = 6;
                stamina--;
                speedEndSound.Play(Globals.Settings.Volume, 0.0f, 0.0f);
            } else if(stamina >= 1)
            {
                speed = 3;
                stamina--;
            } else {
                speed = 3;
            }
            
            //! Previous logic from main for moving and hitboxes
            
            /*Drect.X += (int)velocity.X;
            Main.intersections = getIntersectingTilesHorizontal(Drect);
    
            foreach (var rect in Main.intersections)
            {
                if(Main.collision.TryGetValue(new Vector2(rect.X, rect.Y), out int value))
                {
                    Rectangle collision = new Rectangle(rect.X * Main.tilesize, rect.Y * Main.tilesize, Main.tilesize, Main.tilesize);
    
                    if(velocity.X > 0.0f)
                    {
                        Drect.X = collision.Left - Drect.Width;
                    } else if(velocity.X < 0.0f)
                    {
                        Drect.X = collision.Right;
                    }
                }
            }
    
            Drect.Y += (int)velocity.Y;
            Main.intersections = getIntersectingTilesVertical(Drect);
    
            isInAir = true;
            foreach (var rect in Main.intersections)
            {
                if(Main.collision.TryGetValue(new Vector2(rect.X, rect.Y), out int value))
                {
                    Rectangle collision = new Rectangle(rect.X * Main.tilesize, rect.Y * Main.tilesize, Main.tilesize, Main.tilesize);
    
                    if(velocity.Y > 0.0f)
                    {
                        Drect.Y = collision.Top - Drect.Height;
                        velocity.Y = 0.0f;
                        isInAir = false;
                    } else if(velocity.Y < 0.0f)
                    {
                        Drect.Y = collision.Bottom;
                    } else {
                    }
    
                }
            }*/
            
            // Horizontal Collision Resolution
            Drect.X += (int)Velocity.X;
            horizontalCollisions = GetIntersectingTiles(Drect);
            
            foreach (var tile in horizontalCollisions)
            {
                if (Main.collision.TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    Rectangle collision = new Rectangle(tile.X * Main.tilesize, tile.Y * Main.tilesize, Main.tilesize, Main.tilesize);
            
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
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);
            
            isInAir = true;
            foreach (var tile in verticalCollisions)
            {
                if (Main.collision.TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    Rectangle collision = new Rectangle(tile.X * Main.tilesize, tile.Y * Main.tilesize, Main.tilesize, Main.tilesize);
            
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

            isMoving = Velocity.X != 0;

            if(Velocity.X < 0)
            {
                isLeft = true;
            } else if(Velocity.X > 0)
            {
                isLeft = false;
            }

            prevkstate = kstate; //Used for one-shot
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
                    Color.White,
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

            if(!isMoving && !isInAir)
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
                "Position: " + Position,
                "Tile Position: " + TilePosition,
                "Velocity: " + Velocity,
                "Speed: " + speed,
                "Stamina: " + stamina,
                "Is in Air: " + isInAir,
                "Is Moving: " + isMoving,
                "Is looking Left: " + isLeft,
                "Direction: " + direction
            };
        }
        
        /*public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target) {
    
            List<Rectangle> intersections = new();
    
            for (int x = 0; x <= Main.playerSizeW; x += Main.tilesize) {
                for (int y = 0; y <= Main.playerSizeH; y += Main.tilesize) {
    
                    intersections.Add(new Rectangle(
    
                        (target.X + x) / Main.tilesize,
                        (target.Y + y / Main.tilesize *(Main.tilesize-1)) / Main.tilesize,
                        Main.tilesize,
                        Main.tilesize
                    ));
                }
            }
    
            if (Main.playerSizeW % Main.tilesize != 0) {
            intersections.Add(new Rectangle(
                (target.X + Main.playerSizeW) / Main.tilesize,
                (target.Y) / Main.tilesize,
                Main.tilesize,
                Main.tilesize
            ));
            }
    
            return intersections;
        }
        public List<Rectangle> getIntersectingTilesVertical(Rectangle target) {
    
            List<Rectangle> intersections = new();
    
            for (int x = 0; x <= Main.playerSizeW; x += Main.tilesize) {
                for (int y = 0; y <= Main.playerSizeH; y += Main.tilesize) {
    
                    intersections.Add(new Rectangle(
    
                        (target.X + x / Main.tilesize *(Main.tilesize-1)) / Main.tilesize,
                        (target.Y + y) / Main.tilesize,
                        Main.tilesize,
                        Main.tilesize
    
                    ));
    
                }
            }
    
            if (Main.playerSizeH % Main.tilesize != 0) {
            intersections.Add(new Rectangle(
                (target.X) / Main.tilesize,
                (target.Y + Main.playerSizeH) / Main.tilesize,
                Main.tilesize,
                Main.tilesize
            ));
            }
    
            return intersections;
        }*/
        
        public List<Point> GetIntersectingTiles(Rectangle target)
        {
            List<Point> tiles = new List<Point>();
        
            int leftTile = target.Left / Main.tilesize;
            int rightTile = (target.Right - 1) / Main.tilesize;
            int topTile = target.Top / Main.tilesize;
            int bottomTile = (target.Bottom - 1) / Main.tilesize;
        
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    tiles.Add(new Point(x, y));
                }
            }
        
            return tiles;
        }
    }
}
