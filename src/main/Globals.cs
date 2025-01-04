using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame;

public static class Globals
{
    public static Settings Settings = new Settings();
    public static SaveFile SaveFile = new SaveFile();

    public static void Update()
    {
        Settings = Settings.LoadSettings(Game.settingsFilePath);
        SaveFile = SaveFile.LoadSavefile(Game.savefileFilePath);
        SaveFile.Update();
    }
}