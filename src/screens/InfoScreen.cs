using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class InfoScreen
    {
        public string MenuItem = "Back";
        public Color selectedColor = Color.Yellow;

        public InfoScreen(Font font)
        {
        }

        public void Update()
        {

            if (Game.inputManager.PConfirm)
            {
                Game.currentGameState = Game.GameState.MainMenu;
            }
        }

        public void Draw()
        {
            string[] lines = {
                "Hey!",
                "This is a project made to test my non-existant coding skills!",
                "'Blob' was a meme made by my friend Nick_Greek as a test for Minecraft modeling.",
                "I ended up liking it so much that I made it into a game!",
                "",
                "",
                "",
                "CONTROLS:",
                "AD to move",
                "Space to jump",
                "F to fire a fireball",
                "J + held direction (WASD) to dash",
                "",
                "",
                "",
                "CREDITS:",
                "Most of the art, coding, sound effects and menu music (made with BandCamp) made by MrBeelo.",
                "Inspiration and a little help with art by Nick_Greek.",
                "Some coding fundementals by 'Coding with Sphere' and 'GameDev Quickie'.",
                "Font: Zerove and Rijusans by GGBot.",
                "",
                "",
                "",
                "SPECIAL THANKS:",
                "You <3"
                };

            for (int i = 0; i < lines.Length; i++)
            {
                DrawTextEx(Game.rijusans, lines[i], new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.rijusans, lines[i], Game.indexSize, 0).X / 4), i * 30 + i * 5 + 50), 26, 0, Color.White);
            }

            

            DrawTextEx(Game.rijusans, MenuItem, new Vector2(Settings.SimulationSize.X / 2 - (MeasureTextEx(Game.rijusans, MenuItem, Game.indexSize, 0).X / 4), Settings.SimulationSize.Y - Settings.SimulationSize.Y / 10), 26, 0, selectedColor);
        }
    }
}
