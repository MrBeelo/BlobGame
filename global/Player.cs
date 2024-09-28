using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


namespace BlobGame
{
    public class Player : Sprite
    {
        public Vector2 velocity;
        public int blobSpeed = 3;
        public int blobStamina = 0;
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
        Settings settings = new Settings();

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

            successSound.Play(settings.Volume, 0.0f, 0.0f);
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

            settings = Settings.LoadSettings(Main.settingsFilePath);
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
            if(walkingCounter > 44 - (blobSpeed * 6))
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
                velocity.X = -blobSpeed;
            }
            
            if (kstate.IsKeyDown(Keys.D))
            {
                velocity.X = blobSpeed;
            }

            if(Main.IsKeyPressed(kstate, prevkstate, Keys.Space) && !isInAir) {
                velocity.Y = -10;
                jumpSound.Play(settings.Volume, 0.0f, 0.0f);
            }

            if(Main.IsKeyPressed(kstate, prevkstate, Keys.LeftShift) && blobStamina == 0) {
                blobStamina = 1000;
                speedStartSound.Play(settings.Volume, 0.0f, 0.0f);
            }

            if(blobStamina > 600) {
                blobSpeed = 6;
                blobStamina--;
            } else if (blobStamina == 600)
            {
                blobSpeed = 6;
                blobStamina--;
                speedEndSound.Play(settings.Volume, 0.0f, 0.0f);
            } else if(blobStamina >= 1)
            {
                blobSpeed = 3;
                blobStamina--;
            } else {
                blobSpeed = 3;
            }

            isMoving = velocity.X != 0;

            if(velocity.X < 0)
            {
                isLeft = true;
            } else if(velocity.X > 0)
            {
                isLeft = false;
            }

            prevkstate = kstate; //Used for one-shot
        } 
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteEffects spriteEffects = isLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if(isMoving && !isInAir)
            {
                Main.spriteBatch.Draw(
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
                    Main.spriteBatch.Draw(
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
                    Main.spriteBatch.Draw(
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
                Main.spriteBatch.Draw(
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

            Main.spriteBatch.End();
        }

        public override string[] GetDebugInfo()
        {
            return new string[] 
            {
                "Position: " + Position,
                "Tile Position: " + TilePosition,
                "Veocity: " + velocity,
                "Speed: " + blobSpeed,
                "Stamina: " + blobStamina,
                "Is in Air: " + isInAir,
                "Is Moving: " + isMoving,
                "Is looking Left: " + isLeft
            };
        }
    }
}
