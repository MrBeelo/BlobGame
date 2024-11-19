using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class WinScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Back to Main Menu", "Play Again"};
        }

        public WinScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            base.Update(gameTime);

            if (Main.inputManager.PConfirm)
            {
                switch (selectedIndex)
                {
                    case 0:
                        Main.currentGameState = Main.GameState.MainMenu;
                        Tilemap.Reset(Main.player);
                        break;
                    case 1:
                        Main.currentGameState = Main.GameState.Playing;
                        Tilemap.Reset(Main.player);
                        break;
                }
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);
            
            string message = "You Won!";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(Settings.SimulationSize.X / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Gold);
        }
    }
}
