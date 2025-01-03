using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

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

        public SettingsScreen(Font font) : base(font)
        {
            // Load settings and parse menu items
            UpdateMenuItems();
        }

        public override void Update()
        {
            base.Update();

            if (Game.inputManager.PConfirm)
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
                        if (Globals.Settings.WindowSize.X == 1920 && Globals.Settings.WindowSize.Y == 1080)
                        {
                            Globals.Settings.SetResolution(800, 480);
                        } else if (Globals.Settings.WindowSize.X == 800 && Globals.Settings.WindowSize.Y == 480)
                        {
                            Globals.Settings.SetResolution(1920, 1080);
                        }
                        break;
                    case 2:
                        switch(cameFrom)
                        {
                            case CameFrom.MainMenu:
                                Game.currentGameState = Game.GameState.MainMenu;
                                break;

                            case CameFrom.Paused:
                                Game.currentGameState = Game.GameState.Playing;
                                break;
                        }
                        break;
                }
                selectedIndex = 0;
                Globals.Settings.SaveSettings(Game.settingsFilePath); // Save changes to the file
            }

            Globals.Settings = Settings.LoadSettings(Game.settingsFilePath);
            UpdateMenuItems();
        }

        public override void Draw()
        {
            base.Draw();

            string message = "Settings";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.zerove, message, Game.headerSize, 0).X / 2f), 30), Game.headerSize, 0, Color.White);
        }

        public void UpdateMenuItems()
        {
            menuItems = new string[] {$"Volume: {Globals.Settings.Volume:F2}", $"Resolution: {GetScreenWidth():F0} x {GetScreenHeight():F0}", "Back"};
        }
    }
}
