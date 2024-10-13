using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlobGame;

public static class Globals
{
    public static float TotalSeconds { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDeviceManager Graphics {get; set;}
    public static GraphicsDevice GraphicsDevice {get; set;}
    public static Settings Settings = new Settings(); 
    public static Point WindowSize {get; set;} = new Point(x: 1920, y: 1080);

    public static void Update(GameTime gameTime)
    {
        TotalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Settings = Settings.LoadSettings(Main.settingsFilePath);
    }
}