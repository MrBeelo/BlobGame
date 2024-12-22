using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class InfoScreen
    {
        public string MenuItem = "Back";
        public Color selectedColor = Color.Yellow;
        KeyboardState prevkstate;

        public InfoScreen(SpriteFont font, GraphicsDeviceManager graphics)
        {
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            if (Main.inputManager.PConfirm)
            {
                Main.currentGameState = Main.GameState.MainMenu;
            }
            prevkstate = kstate;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
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
                "Most of the art, coding and sound effects made by MrBeelo.",
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
                Globals.SpriteBatch.DrawString(Main.indexFont, lines[i], new Vector2(Settings.SimulationSize.X / 2 - (Main.indexFont.MeasureString(lines[i]).X / 2), i * 30 + i * 5 + 50), Color.White);
            }

            

            for (int i = 0; i < MenuItem.Length; i++)
            {
                spriteBatch.DrawString(Main.indexFont, MenuItem, new Vector2(Settings.SimulationSize.X / 2 - (Main.indexFont.MeasureString(MenuItem).X / 2), Settings.SimulationSize.Y - Settings.SimulationSize.Y / 10), selectedColor);
            }
        }
    }
}
