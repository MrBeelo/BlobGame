using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame
{
    public class Screen
    {
        public KeyboardState prevkstate;
        public SpriteFont menuFont;
        public int selectedIndex;
        public Vector2[] itemPosition = {Vector2.Zero, Vector2.Zero, Vector2.Zero};
        public Color normalColor = Color.White;
        public Color selectedColor = Color.Yellow;
        public Main main;

        public virtual string[] MenuItems()
        {
            return [];
        }

        public Screen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            menuFont = font;
            selectedIndex = 0; // Start with the first menu item selected
            for (int i = 0; i < MenuItems().Length; i++)
            {
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(graphics.PreferredBackBufferWidth / 2f - (menuFont.MeasureString(item).X / 2f), 400); // Set the position of the menu
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Settings.LoadSettings(Main.settingsFilePath);

            if (Main.keyManager.PDown)
            {
                selectedIndex++;
                if (selectedIndex >= MenuItems().Length)
                {
                    selectedIndex = 0;
                }
            }

            if (Main.keyManager.PUp)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = MenuItems().Length - 1;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            for (int i = 0; i < MenuItems().Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                spriteBatch.DrawString(menuFont, MenuItems()[i], itemPosition[i] + new Vector2(0, i * 40), textColor);
            }
        }

    }
}