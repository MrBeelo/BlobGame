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
            return menuItems ?? new string[] {"PLACEHOLDER", "PLACEHOLDER", "Save and Exit"};
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

            if(IsWindowResized())
            {
                Globals.Settings.WindowSize.X = GetScreenWidth();
                Globals.Settings.WindowSize.Y = GetScreenHeight();
                Globals.Settings.SaveSettings(Game.settingsFilePath);
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
            menuItems = new string[] {$"Volume: {Globals.Settings.Volume:F2}", $"Resolution: {GetScreenWidth():F0} x {GetScreenHeight():F0}", "Save and Exit"};
        }

        public override void AcceptIndex()
        {
            base.AcceptIndex();

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
                        switch(Globals.Settings.WindowSize.X, Globals.Settings.WindowSize.Y)
                        {
                            case (1920, 1080):
                                Globals.Settings.SetResolution(960, 540, true);
                                break;

                            case (960, 540):
                                Globals.Settings.SetResolution(1400, 650, true);
                                break;

                            case (1400, 650):
                                Globals.Settings.SetResolution(1100, 950, true);
                                break;

                            case (1100, 950):
                                Globals.Settings.SetResolution(1920, 1080, true);
                                break;

                            default:
                                Globals.Settings.SetResolution(1920, 1080, true);
                                break;
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
                        Globals.Settings.SaveSettings(Game.settingsFilePath);
                        break;
                }
                selectedIndex = 0;
                Globals.Settings.SaveSettings(Game.settingsFilePath); // Save changes to the file
        }
    }
}
