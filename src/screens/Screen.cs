using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame
{
    public class Screen
    {
        public Font indexFont;
        public int selectedIndex;
        public Vector2[] itemPosition = {Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero};
        public Color normalColor = Color.White;
        public Color selectedColor = Color.Yellow;
        public Game game;
        public float[] itemScales; // Scale for each menu item
        public const float ScaleStep = 0.05f; // Step for increasing or decreasing scale
        public const float MaxScale = 1.2f; // Maximum scale for the selected item
        public const float MinScale = 1.0f; // Minimum scale for unselected items

        public virtual string[] MenuItems()
        {
            return [];
        }

        public Screen(Font font)
        {
            indexFont = font;
            selectedIndex = 0;
            itemScales = new float[MenuItems().Length];
            for (int i = 0; i < MenuItems().Length; i++)
            {
                itemScales[i] = MinScale;
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(Settings.SimulationSize.X / 2 - (MeasureText(item, Game.indexSize) / 2), Settings.SimulationSize.Y / 3); // Set the position of the menu
            }
        }

        public virtual void Update()
        {
            Settings.LoadSettings(Game.settingsFilePath);

            for (int i = 0; i < MenuItems().Length; i++)
            {
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(Settings.SimulationSize.X / 2 - (MeasureText(item, Game.indexSize) / 2), Settings.SimulationSize.Y / 3); // Set the position of the menu
            }

            if (Game.inputManager.PDown)
            {
                selectedIndex++;
                if (selectedIndex >= MenuItems().Length)
                {
                    selectedIndex = 0;
                }
            }

            if (Game.inputManager.PUp)
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

        public virtual void Draw()
        {
            for (int i = 0; i < MenuItems().Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                float scale = itemScales[i];
                Vector2 textSize = MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize, 0);
                Vector2 origin = textSize / 2;
                Vector2 scaledPosition = itemPosition[i] + new Vector2(0, i * 60) + (textSize / 2);
                DrawTextPro(indexFont, MenuItems()[i], scaledPosition, origin, 0f, 26 * scale, 0, textColor);
            }
        }

    }
}