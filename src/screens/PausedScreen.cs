using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class PausedScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Continue", "Options", "Quit"};
        }

        public PausedScreen(Font font) : base(font)
        {
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();

            string message = "Paused";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.zerove, message, Game.headerSize, 0).X / 2f), 30), Game.headerSize, 0, Color.White);
        }

        public override void AcceptIndex()
        {
            base.AcceptIndex();
            
            switch (selectedIndex)
                {
                    case 0:
                        Game.currentGameState = Game.GameState.Playing;
                        break;
                    case 1:
                        Game.options.cameFrom = SettingsScreen.CameFrom.Paused;
                        Globals.Settings = Settings.LoadSettings(Game.settingsFilePath);
                        Game.currentGameState = Game.GameState.Options;
                        break;
                    case 2:
                        Game.currentGameState = Game.GameState.MainMenu;
                        break;
                }
                selectedIndex = 0;
        }
    }
}
