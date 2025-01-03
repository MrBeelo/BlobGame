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
        public Vector2 startingIndexVec;

        public virtual string[] MenuItems()
        {
            return [];
        }

        public Screen(Font font, Vector2 startInVec = default)
        {
            indexFont = font;
            selectedIndex = 0;
            startingIndexVec = default ? new Vector2(Settings.SimulationSize.X / 2, Settings.SimulationSize.Y / 3) : startInVec;
            startingIndexVec = (startInVec == default) ? new Vector2(Settings.SimulationSize.X / 2, Settings.SimulationSize.Y / 3) : startInVec;
            itemScales = new float[MenuItems().Length];
            for (int i = 0; i < MenuItems().Length; i++)
            {
                itemScales[i] = MinScale;
                string item = MenuItems()[i];
                itemPosition[i] = new Vector2(startingIndexVec.X + (MeasureTextEx(Game.rijusans, item, Game.indexSize, 0).X / 2), startingIndexVec.Y); // Set the position of the menu
            }
        }

        public virtual void Update()
        {
            Settings.LoadSettings(Game.settingsFilePath);

            for (int i = 0; i < MenuItems().Length; i++)
            {
                itemPosition[i] = new Vector2(startingIndexVec.X + (MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize, 0).X / 2), startingIndexVec.Y); // Set the position of the menu
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

            if (Game.inputManager.PConfirm || IsMouseButtonPressed(MouseButton.Left))
            {
                AcceptIndex();
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

                if(Game.indexRects.TryGetValue(i, out Rectangle rect))
                {
                    if(CheckCollisionPointRec(GetMousePosition(), rect))
                    {
                        selectedIndex = i;
                    }
                }
            }
        }

        public virtual void Draw()
        {
            for (int i = 0; i < MenuItems().Length; i++)
            {
                Color textColor = (i == selectedIndex) ? selectedColor : normalColor;
                Vector2 origin = MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize, 0) / 2;
                Vector2 scaledPosition = itemPosition[i] - origin * itemScales[i] + new Vector2(0, i * 70);
                DrawTextPro(indexFont, MenuItems()[i], scaledPosition, origin, 0f, Game.indexSize * itemScales[i], 0, textColor);

                Rectangle indexHitbox = new Rectangle(scaledPosition.X - MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize, 0).X / 2 - 20, 
                                                    scaledPosition.Y - MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize, 0).Y / 2 - 10, 
                                                    (20 * 2) + MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize * itemScales[i], 0).X, 
                                                    (10 * 2) + MeasureTextEx(Game.rijusans, MenuItems()[i], Game.indexSize * itemScales[i], 0).Y);

                if(!Game.indexRects.ContainsKey(i))
                {
                    Game.indexRects.Add(i, indexHitbox);
                }

                Game.DrawRectHollow(indexHitbox, 3, textColor);
            }
        }

        public virtual void AcceptIndex()
        {

        }
    }
}