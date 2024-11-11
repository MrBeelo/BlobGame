using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class MainMenuScreen
    {
        
        KeyboardState prevkstate;
        private SpriteFont menuFont;
        private int selectedIndex;
        private string[] menuItems = {"Continue Game", "Options", "Exit"};
        private Vector2[] itemPosition = {Vector2.Zero, Vector2.Zero, Vector2.Zero};
        private Color normalColor = Color.White;
        private Color selectedColor = Color.Yellow;
        private Settings settings = new Settings();

        public MainMenuScreen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            menuFont = font;
            selectedIndex = 0; // Start with the first menu item selected
            for (int i = 0; i < menuItems.Length; i++)
            {
                string item = menuItems[i];
                itemPosition[i] = new Vector2(graphics.PreferredBackBufferWidth / 2f - (menuFont.MeasureString(item).X / 2f), 400); // Set the position of the menu
            }
        }

        public void Update(GameTime gameTime)
        {
            Settings.LoadSettings(Main.settingsFilePath);
            KeyboardState kstate = Keyboard.GetState();

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Down) || Main.IsKeyPressed(kstate, prevkstate, Keys.S))
            {
                selectedIndex++;
                if (selectedIndex >= menuItems.Length)
                {
                    selectedIndex = 0;
                }
            }

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Up) || Main.IsKeyPressed(kstate, prevkstate, Keys.W))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        // Start Game
                        Main.currentGameState = Main.GameState.Playing;
                        break;
                    case 1:
                        // Options
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

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            string message = "Blob Game";
            string credits = "Made by MrBeelo";
            string version = "v0.24";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, credits, new Vector2(graphics.PreferredBackBufferWidth - menuFont.MeasureString(credits).X - 20, graphics.PreferredBackBufferHeight - 70), Color.Black);
            Globals.SpriteBatch.DrawString(Main.font, version, new Vector2(20, graphics.PreferredBackBufferHeight - 70), Color.Black);

            for (int i = 0; i < menuItems.Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                spriteBatch.DrawString(menuFont, menuItems[i], itemPosition[i] + new Vector2(0, i * 40), textColor);
            }
        }
    }
}
