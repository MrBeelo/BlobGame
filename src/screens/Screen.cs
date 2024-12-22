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
        public SpriteFont indexFont;
        public int selectedIndex;
        public Vector2[] itemPosition = {Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero};
        public Color normalColor = Color.White;
        public Color selectedColor = Color.Yellow;
        public Main main;
        public float[] itemScales; // Scale for each menu item
        public const float ScaleStep = 0.05f; // Step for increasing or decreasing scale
        public const float MaxScale = 1.2f; // Maximum scale for the selected item
        public const float MinScale = 1.0f; // Minimum scale for unselected items

        public virtual string[] MenuItems()
        {
            return [];
        }

        public Screen(SpriteFont font, GraphicsDeviceManager graphics)
        {
            indexFont = font;
            selectedIndex = 0;
            itemScales = new float[MenuItems().Length];
            for (int i = 0; i < MenuItems().Length; i++)
            {
                itemScales[i] = MinScale;
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(Settings.SimulationSize.X / 2 - (indexFont.MeasureString(item).X / 2), Settings.SimulationSize.Y / 3); // Set the position of the menu
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Settings.LoadSettings(Main.settingsFilePath);

            for (int i = 0; i < MenuItems().Length; i++)
            {
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(Settings.SimulationSize.X / 2 - (indexFont.MeasureString(item).X / 2), Settings.SimulationSize.Y / 3); // Set the position of the menu
            }

            if (Main.inputManager.PDown)
            {
                selectedIndex++;
                if (selectedIndex >= MenuItems().Length)
                {
                    selectedIndex = 0;
                }
            }

            if (Main.inputManager.PUp)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = MenuItems().Length - 1;
                }
            }

            for (int i = 0; i < MenuItems().Length; i++)
            {
                if (i == selectedIndex)
                {
                    itemScales[i] = Math.Min(itemScales[i] + ScaleStep, MaxScale);
                }
                else
                {
                    itemScales[i] = Math.Max(itemScales[i] - ScaleStep, MinScale);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            for (int i = 0; i < MenuItems().Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                float scale = itemScales[i];
                Vector2 textSize = indexFont.MeasureString(MenuItems()[i]);
                Vector2 origin = textSize / 2;
                Vector2 scaledPosition = itemPosition[i] + new Vector2(0, i * 60) + (textSize / 2);
                spriteBatch.DrawString(indexFont, MenuItems()[i], scaledPosition, textColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }

    }
}