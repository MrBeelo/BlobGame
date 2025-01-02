using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class DeathScreen : Screen
    {
        public override string[] MenuItems() {
            return new string[] {"Retry", "Back to Main Menu"};
        }

        public DeathScreen(Font font) : base(font)
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
                        Player.ResetPos(Game.player);
                        Player.Respawn(Game.player);
                        break;
                    case 1:
                        Game.currentGameState = Game.GameState.MainMenu;
                        Player.ResetPos(Game.player);
                        Player.Respawn(Game.player);
                        break;
                }

                selectedIndex = 0;
            }
        }

        public override void Draw()
        {
            base.Draw();
            
            string message = "You Died!";

            DrawTextEx(Game.zerove, message, new Vector2(Settings.SimulationSize.X / 2 - (MeasureText(message, Game.headerSize) / 2f), 30), Game.headerSize, 0, Color.Red);
        }
    }
}
