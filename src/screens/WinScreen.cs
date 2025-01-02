using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class WinScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Back to Main Menu", "Play Again"};
        }

        public WinScreen(Font font) : base(font)
        {
        }

        public override void Update()
        {
            base.Update();

            if (Game.inputManager.PConfirm)
            {
                switch (selectedIndex)
                {
                    case 0:
                        Game.currentGameState = Game.GameState.MainMenu;
                        Tilemap.Reset(Game.player);
                        break;
                    case 1:
                        Game.currentGameState = Game.GameState.Playing;
                        Tilemap.Reset(Game.player);
                        break;
                }
                selectedIndex = 0;
            }
        }

        public override void Draw()
        {
            base.Draw();
            
            string message = "You Won!";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureText(message, Game.headerSize) / 2f), 30), Game.headerSize, 0, Color.Gold);
        }
    }
}
