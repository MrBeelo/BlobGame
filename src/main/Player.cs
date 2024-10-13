using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace BlobGame
{
    public class Player : Sprite
    {
        public Vector2 velocity;
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

        public bool isInAir = true;
        public static int gravity = 1;
        public bool isMoving = false;
        public bool isLeft = false;

        KeyboardState prevkstate;

        public Player(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            velocity = new();
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
            /*switch(settings.Level)
            {
                case 1:
                Main.levelStartPos.X = 50;
                Main.levelStartPos.Y = 600;
                break;

                case 2:
                Main.levelStartPos.X = 200;
                Main.levelStartPos.Y = 400;
                break;
            }*/

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
            velocity.X = 0;
            velocity.Y += 0.5f;

            velocity.Y = Math.Min(25.0f, velocity.Y);
            
            if (kstate.IsKeyDown(Keys.A))
            {
                velocity.X = -speed;
            }
            
            if (kstate.IsKeyDown(Keys.D))
            {
                velocity.X = speed;
            }

            if(Main.IsKeyPressed(kstate, prevkstate, Keys.Space) && !isInAir) {
                velocity.Y = -10;
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

            isMoving = velocity.X != 0;

            if(velocity.X < 0)
            {
                isLeft = true;
            } else if(velocity.X > 0)
            {
                isLeft = false;
            }
            
            //! Previous logic from main for moving and hitboxes
            
            Drect.X += (int)velocity.X;
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
                        velocity.Y = 0.5f;
                        isInAir = false;
                    } else if(velocity.Y < 0.0f)
                    {
                        Drect.Y = collision.Bottom;
                    } else {
                    }
    
                }
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
                if(velocity.Y >= 5 || velocity.Y <= -5)
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
                "Veocity: " + velocity,
                "Speed: " + speed,
                "Stamina: " + stamina,
                "Is in Air: " + isInAir,
                "Is Moving: " + isMoving,
                "Is looking Left: " + isLeft
            };
        }
        
        public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target) {
    
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
        }
    }
}
