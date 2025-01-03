using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class PassScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Next Level"};
        }

        public PassScreen(Font font) : base(font)
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
                        Game.currentGameState = Game.GameState.Playing;
                        break;
                }
                selectedIndex = 0;
            }
        }

        public override void Draw()
        {
            base.Draw();
            
            string message = "You Passed the Level!";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.zerove, message, Game.headerSize, 0).X / 2f), 30), Game.headerSize, 0, Color.White);
        }
    }
}
