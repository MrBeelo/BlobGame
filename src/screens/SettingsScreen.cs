
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BlobGame
{
    public class SettingsScreen : Screen
    {
        public enum CameFrom
        {
            MainMenu,
            Paused
        }
        public CameFrom cameFrom = CameFrom.MainMenu;
        public override string[] MenuItems() {
            return menuItems ?? new string[] {"PLACEHOLDER", "PLACEHOLDER", "Back"};
        }

        private string[] menuItems;

        public SettingsScreen(SpriteFont font, GraphicsDeviceManager graphics) : base(font, graphics)
        {
            // Load settings and parse menu items
            UpdateMenuItems();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            base.Update(gameTime);

            if (Main.inputManager.PConfirm)
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
                        switch(Globals.Settings.WindowSize)
                        {
                            case Point(1920, 1080):
                                Globals.Settings.SetResolution(800, 480);
                                break;

                            case Point(800, 480):
                                Globals.Settings.SetResolution(1920, 1080);
                                break;
                        }
                        break;
                    case 2:
                        switch(cameFrom)
                        {
                            case CameFrom.MainMenu:
                                Main.currentGameState = Main.GameState.MainMenu;
                                break;

                            case CameFrom.Paused:
                                Main.currentGameState = Main.GameState.Playing;
                                break;
                        }
                        break;
                }
                selectedIndex = 0;
                Globals.Settings.SaveSettings(Main.settingsFilePath); // Save changes to the file
            }

            Globals.Settings = Settings.LoadSettings(Main.settingsFilePath);
            UpdateMenuItems();

            prevkstate = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(spriteBatch, graphics);

            string message = "Settings";

            spriteBatch.DrawString(Main.headerFont, message, new Vector2(Settings.SimulationSize.X / 2 - (Main.headerFont.MeasureString(message).X / 2f), 30), Color.Black);
        }

        public void UpdateMenuItems()
        {
            menuItems = new string[] {$"Volume: {Globals.Settings.Volume:F2}", $"Resolution: {Globals.Graphics.PreferredBackBufferWidth:F0} x {Globals.Graphics.PreferredBackBufferHeight:F0}", "Back"};
        }
    }
}
