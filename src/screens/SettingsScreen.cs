using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BlobGame
{
    public class SettingsScreen
    {
        KeyboardState prevkstate;
        private SpriteFont menuFont;
        private int selectedIndex;
        private string[] menuItems;
        private Vector2[] itemPosition;
        private Color normalColor = Color.White;
        private Color selectedColor = Color.Yellow;
        public Settings settings = new Settings();

        public SettingsScreen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            menuFont = font;
            selectedIndex = 0;

            // Load settings and parse menu items
            settings = Settings.LoadSettings(Main.settingsFilePath);
            UpdateMenuItems();

            itemPosition = new Vector2[menuItems.Length];
            for (int i = 0; i < menuItems.Length; i++)
            {
                string item = menuItems[i];
                itemPosition[i] = new Vector2(graphics.PreferredBackBufferWidth / 2f - (menuFont.MeasureString(item).X / 2f), 400 + i * 40);
            }
        }

        public void Update(GameTime gameTime)
        {
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
                    case 0: // Volume
                        if(settings.Volume == 1.0f)
                        {
                            settings.Volume = 0.0f;
                        }
                        else
                        {
                            settings.ChangeVolume(0.1f);
                        }
                        break;
                    /*case 1: // Resolution
                        settings.ChangeResolution(1920, 1080); // Change to desired resolution
                        break;*/
                    case 1:
                        Main.currentGameState = Main.GameState.MainMenu;
                        break;
                }
                
                settings.SaveSettings(Main.settingsFilePath); // Save changes to the file
                UpdateMenuItems();
            }
            prevkstate = kstate;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            string message = "Settings";

            spriteBatch.DrawString(menuFont, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);

            for (int i = 0; i < menuItems.Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                spriteBatch.DrawString(menuFont, menuItems[i], itemPosition[i], textColor);
            }

            spriteBatch.End();
        }

        public void UpdateMenuItems()
        {
            menuItems = new string[]
            {
                $"Volume: {settings.Volume:F2}", // Format volume to 2 decimal places
                "Back"
            };
        }
    }
}

