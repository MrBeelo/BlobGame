using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class QuitScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Yes", "No"};
        }
        public QuitScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
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
                        // Start Game
                        Main.ExitGame();
                        break;
                    case 1:
                        // Options
                        Main.currentGameState = Main.GameState.MainMenu;
                        break;
                }
            }
            
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);
            
            string message = "Want to quit?";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
        }
    }
}
