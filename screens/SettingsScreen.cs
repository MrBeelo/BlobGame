using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BlobGame
{
    public class Settings
    {
        public float Volume { get; set; } = 0.3f; // Default volume
        public Resolution Resolution { get; set; } = new Resolution { Width = 1920, Height = 1080 }; // Default resolution

        public void ChangeVolume(float delta)
        {
            Volume = MathHelper.Clamp(Volume + delta, 0f, 1f); // Clamp between 0 and 1
        }

        public void ChangeResolution(int width, int height)
        {
            Resolution.Width = width;
            Resolution.Height = height;
        }
    }

    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class SettingsScreen
    {
        KeyboardState prevkstate;
        private SpriteFont menuFont;
        private int selectedIndex;
        private string[] menuItems;
        private Vector2[] itemPosition;
        private Color normalColor = Color.White;
        private Color selectedColor = Color.Yellow;
        private string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "settings.json");
        public Settings settings = new Settings();

        public SettingsScreen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            menuFont = font;
            selectedIndex = 0;

            // Load settings and parse menu items
            settings = LoadSettings(settingsFilePath);
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
            if(settings.Volume >= 1.0f)
            {
                settings.Volume = 0.0f;
            }
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
                        settings.ChangeVolume(0.1f);
                        break;
                    case 1: // Resolution
                        settings.ChangeResolution(1280, 720); // Change to desired resolution
                        break;
                    case 2:
                        Main.currentGameState = Main.GameState.MainMenu;
                        break;
                }
                SaveSettings(settingsFilePath); // Save changes to the file
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

        public Settings LoadSettings(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            return new Settings(); // Return default settings if file does not exist
        }

        public void SaveSettings(string filePath)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void UpdateMenuItems()
        {
            menuItems = new string[]
            {
                $"Volume: {settings.Volume:F2}", // Format volume to 2 decimal places
                $"Resolution: {settings.Resolution.Width}x{settings.Resolution.Height}",
                "Back"
            };
        }
    }
}

