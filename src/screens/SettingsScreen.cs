
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
            return menuItems ?? new string[] {"PLACEHOLDER","Back"};
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
            menuItems = new string[] {$"Volume: {Globals.Settings.Volume:F2}","Back"};
        }
    }
}
