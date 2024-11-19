using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class PausedScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Continue", "Options", "Quit"};
        }

        public PausedScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
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
                    case 1:
                        Main.options.cameFrom = SettingsScreen.CameFrom.Paused;
                        Globals.Settings = Settings.LoadSettings(Main.settingsFilePath);
                        Main.currentGameState = Main.GameState.Options;
                        break;
                    case 2:
                        Main.currentGameState = Main.GameState.MainMenu;
                        break;
                }
            }
            
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);

            string message = "Paused";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(Settings.SimulationSize.X / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
        }
    }
}
