using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class MainMenuScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Continue Game", "Options", "Exit"};
        }

        public MainMenuScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
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
                        Main.currentGameState = Main.GameState.Playing;
                        break;
                    case 1:
                        // Options
                        Main.options.cameFrom = SettingsScreen.CameFrom.MainMenu;
                        Globals.Settings = Settings.LoadSettings(Main.settingsFilePath);
                        Main.currentGameState = Main.GameState.Options;
                        break;
                    case 2:
                        // Exit
                        Main.currentGameState = Main.GameState.Quit;
                        //settings.PlayerPos = Main.player.Position;
                        break;
                }
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);

            string message = "Blob Game";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, Main.credits, new Vector2(graphics.PreferredBackBufferWidth - menuFont.MeasureString(Main.credits).X - 20, graphics.PreferredBackBufferHeight - 70), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, Main.version, new Vector2(20, graphics.PreferredBackBufferHeight - 70), Color.Black);
        }
    }
}
