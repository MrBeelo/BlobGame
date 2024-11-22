using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BlobGame;

public static class Globals
{
    public static float TotalSeconds { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDeviceManager Graphics {get; set;}
    public static GraphicsDevice GraphicsDevice {get; set;}
    public static Settings Settings = new Settings();
    public static SaveFile SaveFile = new SaveFile();

    public static void Update(GameTime gameTime)
    {
        TotalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Settings = Settings.LoadSettings(Main.settingsFilePath);
        SaveFile = SaveFile.LoadSavefile(Main.savefileFilePath);
        SaveFile.Update();
        Graphics.PreferredBackBufferWidth = Settings.WindowSize.X;
        Graphics.PreferredBackBufferHeight = Settings.WindowSize.Y;
        Graphics.ApplyChanges();
        Main.main.canvas.SetDestinationRectangle();
    }
}