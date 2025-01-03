using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class QuitScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Yes", "No"};
        }
        public QuitScreen(Font font) : base(font)
        {
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            
            string message = "Want to quit?";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.zerove, message, Game.headerSize, 0).X / 2f), 30), Game.headerSize, 0, Color.White);
        }

        public override void AcceptIndex()
        {
            switch (selectedIndex)
                {
                    case 0:
                        Game.ExitGame();
                        break;
                    case 1:
                        Game.currentGameState = Game.GameState.MainMenu;
                        break;
                }
                selectedIndex = 0;
        }
    }
}
