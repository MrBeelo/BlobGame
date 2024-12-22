using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class PassScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Next Level"};
        }

        public PassScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
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
                        Main.currentGameState = Main.GameState.Playing;
                        break;
                }
                selectedIndex = 0;
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);
            
            string message = "You Passed the Level!";

            Globals.SpriteBatch.DrawString(Main.headerFont, message, new Vector2(Settings.SimulationSize.X / 2 - (Main.headerFont.MeasureString(message).X / 2f), 30), Color.Black);
        }
    }
}
