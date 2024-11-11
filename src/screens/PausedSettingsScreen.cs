using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BlobGame
{
    public class PausedSettingsScreen : Screen
    {
        public override string[] MenuItems() {
            return menuItems ?? new string[] {"PLACEHOLDER","Back"};
        }

        private string[] menuItems;

        public PausedSettingsScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
        {
            // Load settings and parse menu items
            UpdateMenuItems();
        }

        public override void Update(GameTime gameTime)
        {
            Globals.Settings = Settings.LoadSettings(Main.settingsFilePath);
            KeyboardState kstate = Keyboard.GetState();

            base.Update(gameTime);

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0: // Volume
                        if(Globals.Settings.Volume == 1.0f)
                        {
                            Globals.Settings.Volume = 0.0f;
                        }
                        else
                        {
                            Globals.Settings.ChangeVolume(0.1f);
                        }
                        break;
                    case 1:
                        Main.currentGameState = Main.GameState.Paused;
                        break;
                }
                
                Globals.Settings.SaveSettings(Main.settingsFilePath); // Save changes to the file
                UpdateMenuItems();
            }
            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);

            string message = "Settings";

            spriteBatch.DrawString(menuFont, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);
        }

        public void UpdateMenuItems()
        {
            menuItems = new string[] {
                $"Volume: {Globals.Settings.Volume:F2}", // Format volume to 2 decimal places
                "Back"
            };
        }
    }
}