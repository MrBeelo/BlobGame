using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class DeathScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Back to Main Menu", "Retry"};
        }

        public DeathScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            base.Update(gameTime);

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        // Start Game
                        Main.currentGameState = Main.GameState.MainMenu;
                        Player.ResetPos(Main.player);
                        Player.ResetState(Main.player);
                        break;
                    case 1:
                        // Options
                        Main.currentGameState = Main.GameState.Playing;
                        Player.ResetPos(Main.player);
                        Player.ResetState(Main.player);
                        break;
                }
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);
            
            string message = "You Died!";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Red);
        }
    }
}
