using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class MainMenuScreen : Screen
    {
        private string start = "Start Game";
        public override string[] MenuItems() {
            return new string[] {start, "Options", "Credits & Info", "Exit"};
        }

        public MainMenuScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
        {
            if(Globals.SaveFile.Level <= 0)
            {
                start = "Start Game";
            } else {
                start = "Continue Game";
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(Globals.SaveFile.Level <= 0)
            {
                start = "Start Game";
            } else {
                start = "Continue Game";
            }

            KeyboardState kstate = Keyboard.GetState();

            base.Update(gameTime);

            if (Main.inputManager.PConfirm)
            {
                switch (selectedIndex)
                {
                    case 0:
                        // Start Game
                        Main.currentGameState = Main.GameState.Playing;
                        break;
                    case 1:
                        // Options
                        Main.options.cameFrom = SettingsScreen.CameFrom.MainMenu;
                        Globals.Settings = Settings.LoadSettings(Main.settingsFilePath);
                        Main.currentGameState = Main.GameState.Options;
                        break;
                    case 2:
                        //Info
                        Main.currentGameState = Main.GameState.Info;
                        break;
                    case 3:
                        // Exit
                        Main.currentGameState = Main.GameState.Quit;
                        break;
                }
                selectedIndex = 0;
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);

            string message = "Blob Game";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(Settings.SimulationSize.X / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, Main.credits, new Vector2(Settings.SimulationSize.X - menuFont.MeasureString(Main.credits).X - 20, Settings.SimulationSize.Y - 70), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, Main.version, new Vector2(20, Settings.SimulationSize.Y - 70), Color.Black);
        }
    }
}
