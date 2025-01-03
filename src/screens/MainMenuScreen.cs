using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class MainMenuScreen : Screen
    {
        float time = 0f;
        private string start = "Start Game";
        public override string[] MenuItems() {
            return new string[] {start, "Options", "Credits & Info", "Exit"};
        }

        public MainMenuScreen(Font font) : base(font)
        {
            if(Globals.SaveFile.Level <= 0)
            {
                start = "Start Game";
            } else {
                start = "Continue Game";
            }
        }

        public override void Update()
        {
            time += GetFrameTime();

            if(Globals.SaveFile.Level <= 0)
            {
                start = "Start Game";
            } else {
                start = "Continue Game";
            }

            base.Update();

            if (Game.inputManager.PConfirm)
            {
                switch (selectedIndex)
                {
                    case 0:
                        // Start Game
                        Game.currentGameState = Game.GameState.Playing;
                        break;
                    case 1:
                        // Options
                        Game.options.cameFrom = SettingsScreen.CameFrom.MainMenu;
                        Globals.Settings = Settings.LoadSettings(Game.settingsFilePath);
                        Game.currentGameState = Game.GameState.Options;
                        break;
                    case 2:
                        //Info
                        Game.currentGameState = Game.GameState.Info;
                        break;
                    case 3:
                        // Exit
                        Game.currentGameState = Game.GameState.Quit;
                        break;
                }
                selectedIndex = 0;
            }
        }

        public override void Draw()
        {
            base.Draw();

            string message = "Blob Game";
            float amplitude = 10f;
            float rotation = amplitude * (float)Math.Sin(time);

            DrawTextPro(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2, 70 + (MeasureTextEx(Game.rijusans, Game.credits, Game.indexSize, 0).Y / 2f)), new Vector2(MeasureTextEx(Game.zerove, message, Game.headerSize, 0).X / 2, MeasureTextEx(Game.zerove, message, Game.headerSize, 0).Y / 2) , rotation, Game.headerSize, 0, Color.White);
            DrawTextEx(Game.rijusans, Game.credits, new Vector2(Settings.SimulationSize.X - MeasureTextEx(Game.rijusans, Game.credits, Game.indexSize, 0).X - 20, Settings.SimulationSize.Y - 70), Game.indexSize, 0, Color.White);
            DrawTextEx(Game.rijusans, Game.version, new Vector2(20, Settings.SimulationSize.Y - 70), Game.indexSize, 0, Color.White);
        }
    }
}
