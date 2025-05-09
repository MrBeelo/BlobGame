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
    public static string version = "v0.46.3";
    public static string settingsFilePath = "data/settings.json";
    public static string savefileFilePath = "data/savefile.json";
    public static Player player { get; set; }
    public static TriangleBoss triangleBoss {get; set;}
    public static Fireball fireball { get; set; }
    public static Tilemap tilemap { get; set; }
    public static Triangle triangle { get; set; }
    public static Circle circle { get; set; }
    public static CrystalEvent crystalEvent { get; set; }
    public static List<TriangleBoss> triangleBosses = new();
    public static List<Triangle> triangles = new();
    public static List<Circle> circles = new();
    public static List<Fireball> fireballs = new();
    public static List<Sprite> sprites = new();
    public static bool hasF3On = false;
    public static Texture2D pixelTexture;
    public static Font rijusans = new();
    public static Font zerove = new();
    public static int statsSize = 34 + 34 / 2;
    public static int typeSize = 26 + 26 / 2;
    public static int headerSize = 60 + 60 / 2;
    public static int indexSize = 26 + 26 / 2;
    public static int debugSize = 14 + 14 / 2;
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
    public bool TypingMode = false;
    public string InputText = "";
    public Background background = new();
    public static Music menuMusic;
    public static Music playMusic;
    public RenderTexture2D target;
    public float scale;
    public static Dictionary<int, Rectangle> indexRects = new();
    public static List<Event> events = new();
    public enum GameState
    { MainMenu, Playing, Paused, Options, Quit, Death, Win, Pass, Info }
    public void Run()
    {
        if (!Extensions.SearchAndSetResourceDir("assets"))
        {
            Console.WriteLine("Resources directory not found. Exiting...");
            return;
        }

        Globals.Update();

        SetConfigFlags(ConfigFlags.ResizableWindow);
        SetConfigFlags(ConfigFlags.VSyncHint);
        SetConfigFlags(ConfigFlags.Msaa4xHint);
        ClearWindowState(ConfigFlags.FullscreenMode);
        
        InitWindow(Globals.Settings.WindowSize.X, Globals.Settings.WindowSize.Y, "Blob Game");
        SetWindowMinSize(Settings.SimulationSize.X / 2, Settings.SimulationSize.Y / 2);

        if(IsWindowFullscreen())
        {
            ToggleFullscreen();
        }

        InitAudioDevice();
        SetWindowIcon(LoadImage("other/icon.png"));
        SetExitKey(KeyboardKey.Null);
        SetTargetFPS(60);
        ShowCursor();

        target = LoadRenderTexture(Settings.SimulationSize.X, Settings.SimulationSize.Y);
        SetTextureFilter(target.Texture, TextureFilter.Bilinear);

        game = this;

        //! Definition of a texture and a position for the sprite class is needed here.
        //! Apart from that, you can do whatever the fuck you want with all entities after this point.

        rijusans = LoadFontEx("fonts/Rijusans-Regular.ttf", 60, null, 0);
        zerove = LoadFontEx("fonts/Zerove.ttf", 100, null, 0);

        Texture2D playerTexture = LoadTexture("sprites/player/PlayerIdle1.png");
        Texture2D fireTexture = LoadTexture("sprites/fireball/Fireball1.png");
        Texture2D triangleTexture = LoadTexture("sprites/triangle/TriangleIdle1.png");
        Texture2D circleTexture = LoadTexture("sprites/circle/CircleIdle1.png");

        menuMusic = LoadMusicStream("music/menumusic.mp3");
        playMusic = LoadMusicStream("music/playmusic.mp3");

        background.LoadContent();

        mainMenu = new MainMenuScreen(rijusans);
        paused = new PausedScreen(rijusans);
        quit = new QuitScreen(rijusans);
        options = new SettingsScreen(rijusans);
        death = new DeathScreen(rijusans);
        win = new WinScreen(rijusans);
        pass = new PassScreen(rijusans);
        info = new InfoScreen(rijusans, new Vector2(Settings.SimulationSize.X / 2, Settings.SimulationSize.Y * 14 / 15));

        tilemap = new Tilemap();
        tilemap.LoadContent(this);

        player = new Player(playerTexture, new Rectangle(0, 0, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30));

        Globals.SaveFile.Initialize();

        player.LoadContent(this);
        sprites.Add(player);

        triangleBoss = new TriangleBoss(triangleTexture, new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0));
        triangleBoss.LoadContent(this);

        fireball = new Fireball(fireTexture, new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), false, false, 5);
        fireball.LoadContent(this);

        triangle = new Triangle(triangleTexture, new Rectangle(0, 0, Triangle.triangleSizeW, Triangle.triangleSizeH), new(0, 0, 20, 30));
        triangle.LoadContent(this);

        circle = new Circle(circleTexture, new Rectangle(0, 0, Circle.circleSizeW, Circle.circleSizeH), new(0, 0, 20, 30));
        circle.LoadContent(this);

        Player.Respawn(player);

        crystalEvent = new CrystalEvent();
        crystalEvent.LoadContent(this);

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

            scale = Math.Min((float)GetScreenWidth()/Settings.SimulationSize.X, (float)GetScreenHeight()/Settings.SimulationSize.Y);

            Globals.Update();
            inputManager.Update();

            UpdateMusicStream(menuMusic);
            UpdateMusicStream(playMusic); 

            FPS = GetFPS();
            deltaTime = GetFrameTime();

        if (TypingMode)
        {
            if(IsKeyPressed(KeyboardKey.Backspace) && InputText.Length > 0)
            {
                InputText = InputText.Substring(0, InputText.Length - 1);
            } else {
                KeyboardKey key = (KeyboardKey)GetKeyPressed();
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
            if(!IsWindowFullscreen())
            {
                Globals.Settings.SetResolution(GetMonitorWidth(GetCurrentMonitor()), GetMonitorHeight(GetCurrentMonitor()), true);
            }
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

                case "/becomeSuper":
                    player.insanity = 100;
                    player.isSuper = true;
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

        if (TypingMode && inputManager.PBack)
        {
            InputText = "";
            TypingMode = false;
        }

        //! Updating for all Game States

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Update();
                if (inputManager.PBack && !TypingMode)
                {
                    currentGameState = GameState.Quit;
                }
                break;

            case GameState.Playing:
                tilemap.Update(this);
                player.Update();

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

                foreach (var Event in events.ToList())
                {
                    Event.Update();
                }

                if (inputManager.PBack && !TypingMode)
                {
                    currentGameState = GameState.Paused;
                }
                break;

            case GameState.Paused:
                if (inputManager.PBack && !TypingMode)
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

        ClearBackground(Color.SkyBlue);

        //!Beggining Texture Mode
        BeginTextureMode(target);

        background.DrawBG();

        BeginMode2D(Player.camera);

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

                foreach (var Event in events.ToList())
                {
                    Event.Draw();
                }
                break;
        }

        EndMode2D();

        DrawTexture(Player.flash, 0, 0, new Color(255, 255, 255, player.flashProgress));

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Draw();
                break;

            case GameState.Playing:
                string level = "Level: " + Tilemap.level;
                string xartomantila = "Xartomantila: " + Player.xartomantila;

                player.DrawHealthBar();
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

            /*foreach (Rectangle rect in indexRects.Values)
            {
                DrawRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height, Color.White);
            }*/

            string[] mainDebugInfo =
            {
                    "Current Game State: " + currentGameState,
                    "FPS: " + FPS,
                    "Delta Time: " + deltaTime,
                    "Level: " + Tilemap.level,
                    "Savefile Level: " + Globals.SaveFile.Level,
                    "Mapsize: " + Tilemap.Mapsize,
                    "Pressed Direction: " + inputManager.pressedDirection,
                    "Typing Mode: " + TypingMode
                };

            string[] otherDebugInfo = otherDebugInfoList.ToArray();

            string[] combinedDebugInfo = mainDebugInfo.Concat(otherDebugInfo).ToArray();

            Vector2 pos = Vector2.Zero;

            foreach (var info in combinedDebugInfo)
            {
                DrawTextEx(rijusans, info, pos, debugSize, 0, Color.Black);
                pos.Y += 25;
            }
        }

        //! Handling Typing Mode

        if (TypingMode)
        {
            DrawTypingZone();
        }

        inputManager.DrawController();

        //!Ending Texture Mode
        EndTextureMode();

        BeginDrawing();
            ClearBackground(Color.Black);

            DrawTexturePro(target.Texture, new Rectangle (0, 0, target.Texture.Width, -target.Texture.Height), new Rectangle((float)(GetScreenWidth() - Settings.SimulationSize.X*scale)*0.5f,
                (GetScreenHeight() - Settings.SimulationSize.Y*scale)*0.5f, Settings.SimulationSize.X*scale,Settings.SimulationSize.Y*scale), Vector2.Zero, 0f, Color.White);
        EndDrawing();
    }

    public static void ExitGame()
    {
        Environment.Exit(0);
    }

    public void DrawTypingZone()
    {
        DrawRectangle(Settings.SimulationSize.X / 20, Settings.SimulationSize.Y - Settings.SimulationSize.Y / 10, Settings.SimulationSize.X - Settings.SimulationSize.X / 10, Settings.SimulationSize.Y / 20, new Color(0, 0, 0, 89));
        DrawTextEx(rijusans, InputText, new Vector2(Settings.SimulationSize.X / 20 + 10, Settings.SimulationSize.Y - Settings.SimulationSize.Y / 10 + 7), typeSize, 0, Color.White);
    }

    public static bool TryParseTwoArgs(string input, out int arg1, out int arg2)
    {
        arg1 = arg2 = 0;
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 && int.TryParse(parts[0], out arg1) && int.TryParse(parts[1], out arg2);
    }
}
