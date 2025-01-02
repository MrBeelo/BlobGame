using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame;

public static class Globals
{
    public static float TotalSeconds { get; set; }
    public static Settings Settings = new Settings();
    public static SaveFile SaveFile = new SaveFile();

    public static void Update()
    {
        Settings = Settings.LoadSettings(Game.settingsFilePath);
        SaveFile = SaveFile.LoadSavefile(Game.savefileFilePath);
        SaveFile.Update();
        SetWindowMinSize(Settings.WindowSize.X, Settings.WindowSize.Y);
        SetWindowMaxSize(Settings.WindowSize.X, Settings.WindowSize.Y);
        Game.game.canvas.SetDestinationRectangle();
    }
}