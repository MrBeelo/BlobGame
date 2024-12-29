using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class MainMenuScreen : Screen
    {
        float time = 0f;
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
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            float amplitude = 0.1745f;
            float rotation = amplitude * (float)Math.Sin(time);

            Globals.SpriteBatch.DrawString(Main.headerFont, message, new Vector2(Settings.SimulationSize.X / 2, 60 + (Main.headerFont.MeasureString(message).Y / 2f)), Color.White, rotation, new Vector2(Main.headerFont.MeasureString(message).X / 2, Main.headerFont.MeasureString(message).Y / 2), 1.0f, SpriteEffects.None, 1f);
            Globals.SpriteBatch.DrawString(Main.indexFont, Main.credits, new Vector2(Settings.SimulationSize.X - Main.indexFont.MeasureString(Main.credits).X - 20, Settings.SimulationSize.Y - 70), Color.White);
            Globals.SpriteBatch.DrawString(Main.indexFont, Main.version, new Vector2(20, Settings.SimulationSize.Y - 70), Color.White);
        }
    }
}
