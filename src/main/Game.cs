using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Diagnostics;
using System.Numerics;

namespace BlobGame;

public class Game
{
    public static Game game;
    public float deltaTime;
    public static string credits = "Made by MrBeelo";
    public static string version = "v0.45";
    public static string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "settings.json");
    public static string savefileFilePath = Path.Combine(AppContext.BaseDirectory, "data", "savefile.json");
    public static Player player { get; set; }
    public static TriangleBoss triangleBoss {get; set;}
    public static Fireball fireball { get; set; }
    public static Tilemap tilemap { get; set; }
    public static Triangle triangle { get; set; }
    public static Circle circle { get; set; }
    public static List<TriangleBoss> triangleBosses = new();
    public static List<Triangle> triangles = new();
    public static List<Circle> circles = new();
    public static List<Fireball> fireballs = new();
    public static List<Sprite> sprites = new();
    public static bool hasF3On = false;
    public static Texture2D pixelTexture;
    public static Font rijusans = new();
    public static Font zerove = new();
    public static int statsSize = 34 * 2;
    public static int typeSize = 26 * 2;
    public static int headerSize = 60 * 2;
    public static int indexSize = 26 * 2;
    public static int debugSize = 14 * 2;
    public static GameState currentGameState = GameState.MainMenu;
    private MainMenuScreen mainMenu;
    private PausedScreen paused;
    private QuitScreen quit;
    public static SettingsScreen options;
    public DeathScreen death;
    public WinScreen win;
    public PassScreen pass;
    public InfoScreen info;
    public static double LoweredVolume = Globals.Settings.Volume * 0.4;
    public int FPS;
    public static InputManager inputManager = new InputManager();
    public Canvas canvas;
    public bool TypingMode = false;
    public string InputText = "";
    public Background background = new();
    public static Music menuMusic;
    public static Music playMusic;
    public enum GameState
    { MainMenu, Playing, Paused, Options, Quit, Death, Win, Pass, Info }
    public void Run()
    {
        InitWindow(1920, 1080, "Blob Game");
        InitAudioDevice();
        SetWindowIcon(LoadImage("assets/other/icon.png"));
        SetExitKey(KeyboardKey.Null);
        SetTargetFPS(60);
        //ToggleFullscreen();
        ShowCursor();

        game = this;

        Settings.LoadSettings(settingsFilePath);
        SaveFile.LoadSavefile(savefileFilePath);

        SetWindowMinSize(Globals.Settings.WindowSize.X, Globals.Settings.WindowSize.Y);
        SetWindowMaxSize(Globals.Settings.WindowSize.X, Globals.Settings.WindowSize.Y);

        canvas = new Canvas(1920, 1080);

        //! Definition of a texture and a position for the sprite class is needed here.
        //! Apart from that, you can do whatever the fuck you want with all entities after this point.

        rijusans = LoadFont("assets/fonts/Rijusans-Regular.ttf");
        zerove = LoadFont("assets/fonts/Zerove.ttf");

        Texture2D playerTexture = LoadTexture("assets/sprites/player/PlayerIdle1.png");
        Texture2D fireTexture = LoadTexture("assets/sprites/fireball/Fireball1.png");
        Texture2D triangleTexture = LoadTexture("assets/sprites/triangle/TriangleIdle1.png");
        Texture2D circleTexture = LoadTexture("assets/sprites/circle/CircleIdle1.png");

        menuMusic = LoadMusicStream("assets/music/menumusic.mp3");
        playMusic = LoadMusicStream("assets/music/playmusic.mp3");

        background.LoadContent();

        mainMenu = new MainMenuScreen(rijusans);
        paused = new PausedScreen(rijusans);
        quit = new QuitScreen(rijusans);
        options = new SettingsScreen(rijusans);
        death = new DeathScreen(rijusans);
        win = new WinScreen(rijusans);
        pass = new PassScreen(rijusans);
        info = new InfoScreen(rijusans);

        tilemap = new Tilemap();
        tilemap.LoadContent(this);

        player = new Player(playerTexture, new Rectangle(0, 0, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30));

        Globals.SaveFile.Initialize();

        player.LoadContent(this);
        sprites.Add(player);

        triangleBoss = new TriangleBoss(triangleTexture, new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0));
        triangleBoss.LoadContent(this);

        fireball = new Fireball(fireTexture, new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), false, false);
        fireball.LoadContent(this);

        triangle = new Triangle(triangleTexture, new Rectangle(0, 0, Triangle.triangleSizeW, Triangle.triangleSizeH), new(0, 0, 20, 30));
        triangle.LoadContent(this);

        circle = new Circle(circleTexture, new Rectangle(0, 0, Circle.circleSizeW, Circle.circleSizeH), new(0, 0, 20, 30));
        circle.LoadContent(this);

        Player.Respawn(player);

        PlayMusicStream(menuMusic);

        while (!WindowShouldClose())
        {
            Update();
            Draw();
        }
    }

    public void Update()
    {
        //! UPDATE LOOP
            game = this;

            Globals.Update();
            inputManager.Update();

            UpdateMusicStream(menuMusic);
            UpdateMusicStream(playMusic); 

            FPS = GetFPS();
            deltaTime = GetFrameTime();

        if (TypingMode)
        {
            KeyboardKey key = (KeyboardKey)GetKeyPressed();
            if (key == KeyboardKey.Back && InputText.Length > 0)
                {
                    // Handle Backspace
                    InputText = InputText.Substring(0, InputText.Length - 1);
                }
                else
                {
                    // Add the character for the pressed key
                    char? character = InputManager.KeyToChar(key, IsKeyDown(KeyboardKey.LeftShift) || IsKeyDown(KeyboardKey.RightShift));
                    if (character != null)
                    {
                        InputText += character.Value;
                    }
                }
        }

        LoweredVolume = Globals.Settings.Volume * 0.4;

        if (IsKeyPressed(KeyboardKey.F11))
        {
            ToggleFullscreen();
        }

        if (IsKeyPressed(KeyboardKey.F3) && hasF3On == false)
        {
            hasF3On = true;
        }
        else if (IsKeyPressed(KeyboardKey.F3) && hasF3On == true)
        {
            hasF3On = false;
        }

        //! Handling Typing Mode

        if (IsKeyPressed(KeyboardKey.Grave))
        {
            if (!TypingMode)
            {
                TypingMode = true;
            }
            else if (TypingMode)
            {
                TypingMode = false;
            }
        }

        if (TypingMode && IsKeyPressed(KeyboardKey.Enter))
        {
            switch (InputText)
            {
                case "/yipee":
                    PlaySound(Player.successSound);
                    break;

                case "/kill":
                    Player.Die();
                    break;

                case "/resetState":
                    Player.ResetState(player);
                    break;

                case "/resetPos":
                    Player.ResetPos(player);
                    break;

                case "/respawn":
                    Player.Respawn(player);
                    break;

                case "/moveLevel":
                    if (Tilemap.level < Tilemap.Collision.Length - 1)
                    {
                        Tilemap.MoveLevel();
                    }
                    break;

                case "/immune":
                    if (!player.Immune)
                    {
                        player.Immune = true;
                    }
                    else if (player.Immune)
                    {
                        player.Immune = false;
                    }
                    break;

                case "/summonTriangle":
                    Triangle.Summon(new Vector2(player.Drect.X - Triangle.triangleSizeW, player.Drect.Y - Triangle.triangleSizeH));
                    break;

                case "/clearTriangle":
                    Triangle.ClearAll();
                    break;

                case "/summonBossTriangle":
                    TriangleBoss.Summon(new Vector2(player.Drect.X - TriangleBoss.bossTriangleSizeW, player.Drect.Y - TriangleBoss.bossTriangleSizeH));
                    break;

                case "/clearBossTriangle":
                    TriangleBoss.ClearAll();
                    break;

                case "/summonCircle":
                    Circle.Summon(new Vector2(player.Drect.X - Circle.circleSizeW, player.Drect.Y - Circle.circleSizeH));
                    break;

                case "/clearCircle":
                    Circle.ClearAll();
                    break;

                case "/killAll":
                    player.alive = false;
                    Triangle.ClearAll();
                    Circle.ClearAll();
                    break;

                case string s when s.StartsWith("/moveTo") && int.TryParse(s.Substring("/moveTo ".Length).Trim(), out int level):
                    Tilemap.MoveTo(level);
                    break;

                case string s when s.StartsWith("/dmg") && int.TryParse(s["/dmg ".Length..].Trim(), out int damage):
                    Player.Damage(damage);
                    break;

                case string s when s.StartsWith("/tp ") && TryParseTwoArgs(s["/tp ".Length..].Trim(), out int x, out int y):
                    Player.Teleport(x, y);
                    break;

                default:
                    Debug.WriteLine("Player Inputted Invalid Command: " + InputText);
                    break;
            }
            Debug.WriteLine("Player Inputted Command: " + InputText);
            InputText = "";
            TypingMode = false;
        }

        if (TypingMode && IsKeyPressed(KeyboardKey.Escape))
        {
            InputText = "";
            TypingMode = false;
        }

        //! Updating for all Game States

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Update();
                if (IsKeyPressed(KeyboardKey.Escape) && !TypingMode)
                {
                    currentGameState = GameState.Quit;
                }
                break;

            case GameState.Playing:
                tilemap.Update(this);
                player.Update();
                player.CalculateTranslation();

                foreach (var fireball in fireballs.ToList())
                {
                    fireball.Update();
                }

                foreach (var triangleBoss in triangleBosses.ToList())
                {
                    triangleBoss.Update();
                }

                foreach (var triangle in triangles.ToList())
                {
                    triangle.Update();
                }

                foreach (var circle in circles.ToList())
                {
                    circle.Update();
                }

                if (IsKeyPressed(KeyboardKey.Escape) && !TypingMode)
                {
                    currentGameState = GameState.Paused;
                }
                break;

            case GameState.Paused:
                if (IsKeyPressed(KeyboardKey.Escape) && !TypingMode)
                {
                    currentGameState = GameState.MainMenu;
                }
                paused.Update();
                break;

            case GameState.Options:
                options.Update();
                break;

            case GameState.Quit:
                quit.Update();
                break;

            case GameState.Death:
                death.Update();
                break;

            case GameState.Win:
                win.Update();
                break;

            case GameState.Pass:
                pass.Update();
                break;

            case GameState.Info:
                info.Update();
                break;
        }

        if(currentGameState == GameState.Playing || currentGameState == GameState.Paused || currentGameState == GameState.Pass || currentGameState == GameState.Win || currentGameState == GameState.Death || (currentGameState == GameState.Options && options.cameFrom == SettingsScreen.CameFrom.Paused))
        {
            if (!IsMusicStreamPlaying(playMusic))
            { PlayMusicStream(playMusic); }
            SetMusicVolume(playMusic, (float)LoweredVolume - 0.1f);
            StopMusicStream(menuMusic);
        } else {
            if (!IsMusicStreamPlaying(menuMusic))
            { PlayMusicStream(menuMusic); }
            SetMusicVolume(menuMusic, (float)LoweredVolume);
            StopMusicStream(playMusic);
        }

        if(currentGameState != GameState.Playing)
        { background.Update(); }
    }

    public void Draw()
    {
        //! DRAW LOOP

        //canvas.Activate();

        ClearBackground(Color.SkyBlue);

        //!Beggining Sprite Batch
        BeginDrawing();

        background.DrawBG();

        switch (currentGameState)
        {
            case GameState.Playing:
                tilemap.Draw();

                player.Draw();

                foreach (var fireball in fireballs.ToList())
                {
                    fireball.Draw();
                }

                foreach (var triangleBoss in triangleBosses.ToList())
                {
                    triangleBoss.Draw();
                }

                foreach (var triangle in triangles.ToList())
                {
                    triangle.Draw();
                }

                foreach (var circle in circles.ToList())
                {
                    circle.Draw();
                }
                break;
        }

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Draw();
                break;

            case GameState.Playing:
                string health = "Health: " + player.Health.ToString() + "/100";
                string level = "Level: " + Tilemap.level;
                string xartomantila = "Xartomantila: " + Player.xartomantila;

                DrawTextEx(rijusans, health, new Vector2(Settings.SimulationSize.X - MeasureTextEx(rijusans, health, statsSize, 0).X - 20, 10), statsSize, 0, Color.Black);
                DrawTextEx(rijusans, level, new Vector2(Settings.SimulationSize.X - MeasureTextEx(rijusans, level, statsSize, 0).X - 20, 60), statsSize, 0, Color.Black);
                DrawTextEx(rijusans, xartomantila, new Vector2(Settings.SimulationSize.X - MeasureTextEx(rijusans, xartomantila, statsSize, 0).X - 20, 110), statsSize, 0, Color.Black);
                foreach(TriangleBoss triangleBoss in triangleBosses)
                { triangleBoss.DrawBar(); }
                break;

            case GameState.Paused:
                paused.Draw();
                break;

            case GameState.Options:
                options.Draw();
                break;

            case GameState.Quit:
                quit.Draw();
                break;

            case GameState.Death:
                death.Draw();
                break;

            case GameState.Win:
                win.Draw();
                break;

            case GameState.Pass:
                pass.Draw();
                break;

            case GameState.Info:
                info.Draw();
                break;
        }

        //! Handle F3 Info

        if (hasF3On)
        {
            List<string> otherDebugInfoList = new List<string>();

            foreach (var sprite in sprites)
            {
                string[] spriteDebugInfo = sprite.GetDebugInfo();
                otherDebugInfoList.AddRange(spriteDebugInfo);
            }

            foreach (var fireball in fireballs)
            {
                string[] fireballDebugInfo = fireball.GetDebugInfo();
                otherDebugInfoList.AddRange(fireballDebugInfo);
            }

            foreach (var triangle in triangles)
            {
                string[] triangleDebugInfo = triangle.GetDebugInfo();
                otherDebugInfoList.AddRange(triangleDebugInfo);
            }

            foreach (var circle in circles)
            {
                string[] circleDebugInfo = circle.GetDebugInfo();
                otherDebugInfoList.AddRange(circleDebugInfo);
            }

            string[] mainDebugInfo =
            {
                    "Current Game State: " + currentGameState,
                    "FPS: " + FPS,
                    "Delta Time: " + deltaTime,
                    "Level: " + Tilemap.level,
                    "Savefile Level: " + Globals.SaveFile.Level,
                    "Mapsize: " + tilemap.Mapsize,
                    "Pressed Direction: " + inputManager.pressedDirection,
                    "Typing Mode: " + TypingMode
                };

            string[] otherDebugInfo = otherDebugInfoList.ToArray();

            string[] combinedDebugInfo = mainDebugInfo.Concat(otherDebugInfo).ToArray();

            Vector2 pos = Vector2.Zero;

            foreach (var info in combinedDebugInfo)
            {
                DrawTextEx(rijusans, info, pos, debugSize, 0, Color.Black);
                pos.Y += 20;
            }
        }

        //! Handling Typing Mode

        if (TypingMode)
        {
            DrawTypingZone();
        }

        //!Ending Sprite Batch
        EndDrawing();

        //canvas.Draw();
    }

    public static void ExitGame()
    {
        Environment.Exit(0);
    }

    public static void DrawRectHollow(Rectangle rect, int thickness, Color color) {
    // Draw top side
    DrawRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, thickness, color);
    // Draw bottom side
    DrawRectangle((int)rect.X, (int)(rect.Y + rect.Height - thickness), (int)rect.Width, thickness, color);
    // Draw left side
    DrawRectangle((int)rect.X, (int)rect.Y, thickness, (int)rect.Height, color);
    // Draw right side
    DrawRectangle((int)(rect.X + rect.Width - thickness), (int)rect.Y, thickness, (int)rect.Height, color);
    }


    public void DrawTypingZone()
    {
        DrawRectangle(Globals.Settings.WindowSize.X / 20, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10, Globals.Settings.WindowSize.X - Globals.Settings.WindowSize.X / 10, Globals.Settings.WindowSize.Y / 20, new Color(0, 0, 0, 89));
        DrawTextEx(rijusans, InputText, new Vector2(Globals.Settings.WindowSize.X / 20 + 10, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10 + 5), typeSize, 0, Color.White);
    }

    public static bool TryParseTwoArgs(string input, out int arg1, out int arg2)
    {
        arg1 = arg2 = 0;
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 && int.TryParse(parts[0], out arg1) && int.TryParse(parts[1], out arg2);
    }
}
