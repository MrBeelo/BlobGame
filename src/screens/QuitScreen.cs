using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class QuitScreen
    {
        
        KeyboardState prevkstate;
        private SpriteFont menuFont;
        private int selectedIndex;
        private string[] menuItems = { "Yes", "No"};
        private Vector2[] itemPosition = {Vector2.Zero, Vector2.Zero};
        private Color normalColor = Color.White;
        private Color selectedColor = Color.Yellow;

        public QuitScreen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            menuFont = font;
            selectedIndex = 1; // Start with the first menu item selected

            for (int i = 0; i < menuItems.Length; i++)
            {
                string item = menuItems[i];
                itemPosition[i] = new Vector2(graphics.PreferredBackBufferWidth / 2f - (menuFont.MeasureString(item).X / 2f), 400); // Set the position of the menu
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Down) || Main.IsKeyPressed(kstate, prevkstate, Keys.S))
            {
                selectedIndex++;
                if (selectedIndex >= menuItems.Length)
                {
                    selectedIndex = 0;
                }
            }

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Up) || Main.IsKeyPressed(kstate, prevkstate, Keys.W))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }

            if (Main.IsKeyPressed(kstate, prevkstate, Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        // Start Game
                        Main.ExitGame();
                        break;
                    case 1:
                        // Options
                        Main.currentGameState = Main.GameState.MainMenu;
                        break;
                }
            }
            
            prevkstate = kstate;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            string message = "Want to quit?";

            Globals.SpriteBatch.DrawString(Main.font, message, new Vector2(graphics.PreferredBackBufferWidth / 2 - (menuFont.MeasureString(message).X / 2f), 30), Color.Black);

            for (int i = 0; i < menuItems.Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                spriteBatch.DrawString(menuFont, menuItems[i], itemPosition[i] + new Vector2(0, i * 40), textColor);
            }
        }
    }
}