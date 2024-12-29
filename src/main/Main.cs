using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace BlobGame;

public class Main : Game
{
    public static Main main;
    public float deltaTime;
    public static string credits = "Made by MrBeelo";
    public static string version = "v0.45";
    public static string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "settings.json");
    public static string savefileFilePath = Path.Combine(AppContext.BaseDirectory, "data", "savefile.json");
    public static Player player { get; set; }
    public static Fireball fireball { get; set; }
    public static Tilemap tilemap { get; set; }
    public static Triangle triangle { get; set; }
    public static Circle circle { get; set; }
    public static List<Triangle> triangles = new();
    public static List<Circle> circles = new();
    public static List<Fireball> fireballs = new();
    public static List<Sprite> sprites = new();
    public static bool hasF3On = false;
    public static Texture2D pixelTexture;
    public static SpriteFont statsFont;
    public static SpriteFont typeFont;
    public static SpriteFont headerFont;
    public static SpriteFont indexFont;
    public static SpriteFont debugFont;
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
    KeyboardState prevkstate;
    public int frameCounter;
    public TimeSpan timeSpan;
    public int FPS;
    public static InputManager inputManager = new InputManager();
    public Canvas canvas;
    public bool TypingMode = false;
    public string InputText = "";
    public Background background = new();
    public Song menuMusic;
    public Song playMusic;
    public enum GameState
    { MainMenu, Playing, Paused, Options, Quit, Death, Win, Pass, Info }
    public Main()
    {
        Window.Title = "Blob Game";
        Globals.Graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "content";
        IsMouseVisible = true;

        main = this;
    }

    protected override void Initialize()
    {
        Settings.LoadSettings(settingsFilePath);
        SaveFile.LoadSavefile(savefileFilePath);

        Globals.Graphics.PreferredBackBufferWidth = Globals.Settings.WindowSize.X;
        Globals.Graphics.PreferredBackBufferHeight = Globals.Settings.WindowSize.Y;

        canvas = new Canvas(GraphicsDevice, 1920, 1080);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.GraphicsDevice = GraphicsDevice;

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        //! Definition of a texture and a position for the sprite class is needed here.
        //! Apart from that, you can do whatever the fuck you want with all entities after this point.

        statsFont = Content.Load<SpriteFont>("assets/fonts/statsFont");
        typeFont = Content.Load<SpriteFont>("assets/fonts/typeFont");
        headerFont = Content.Load<SpriteFont>("assets/fonts/headerFont");
        indexFont = Content.Load<SpriteFont>("assets/fonts/indexFont");
        debugFont = Content.Load<SpriteFont>("assets/fonts/debugFont");

        Texture2D playerTexture = Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");
        Texture2D fireTexture = Content.Load<Texture2D>("assets/sprites/fireball/Fireball1");
        Texture2D triangleTexture = Content.Load<Texture2D>("assets/sprites/triangle/TriangleIdle1");
        Texture2D circleTexture = Content.Load<Texture2D>("assets/sprites/circle/CircleIdle1");

        menuMusic = Content.Load<Song>("assets/music/menumusic");
        playMusic = Content.Load<Song>("assets/music/playmusic");

        background.LoadContent();

        mainMenu = new MainMenuScreen(indexFont, Globals.Graphics);
        paused = new PausedScreen(indexFont, Globals.Graphics);
        quit = new QuitScreen(indexFont, Globals.Graphics);
        options = new SettingsScreen(indexFont, Globals.Graphics);
        death = new DeathScreen(indexFont, Globals.Graphics);
        win = new WinScreen(indexFont, Globals.Graphics);
        pass = new PassScreen(indexFont, Globals.Graphics);
        info = new InfoScreen(indexFont, Globals.Graphics);

        tilemap = new Tilemap();
        tilemap.LoadContent(this);

        player = new Player(playerTexture, new Rectangle(0, 0, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30), Globals.Graphics);

        Globals.SaveFile.Initialize();

        player.LoadContent(this);
        sprites.Add(player);

        fireball = new Fireball(fireTexture, Rectangle.Empty, Rectangle.Empty, Globals.Graphics, false);
        fireball.LoadContent(this);

        triangle = new Triangle(triangleTexture, new Rectangle(0, 0, Triangle.triangleSizeW, Triangle.triangleSizeH), new(0, 0, 20, 30), Globals.Graphics);
        triangle.LoadContent(this);

        circle = new Circle(circleTexture, new Rectangle(0, 0, Circle.circleSizeW, Circle.circleSizeH), new(0, 0, 20, 30), Globals.Graphics);
        circle.LoadContent(this);

        Player.Respawn(player);

        MediaPlayer.Play(menuMusic);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        main = this;

        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Globals.Update(gameTime);

        KeyboardState kstate = Keyboard.GetState();

        inputManager.Update(gameTime);

        if (TypingMode)
        {
            foreach (var key in kstate.GetPressedKeys())
            {
                if (InputManager.IsKeyPressed(kstate, prevkstate, key))
                {
                    if (key == Keys.Back && InputText.Length > 0)
                    {
                        // Handle Backspace
                        InputText = InputText.Substring(0, InputText.Length - 1);
                    }
                    else
                    {
                        // Add the character for the pressed key
                        char? character = InputManager.KeyToChar(key, kstate.IsKeyDown(Keys.LeftShift) || kstate.IsKeyDown(Keys.RightShift));
                        if (character != null)
                        {
                            InputText += character.Value;
                        }
                    }
                }
            }
        }

        LoweredVolume = Globals.Settings.Volume * 0.4;

        timeSpan += gameTime.ElapsedGameTime;
        frameCounter++;

        if (timeSpan > TimeSpan.FromSeconds(1))
        {
            timeSpan -= TimeSpan.FromSeconds(1);
            FPS = frameCounter;
            frameCounter = 0;
        }

        if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.F11))
        {
            Globals.Settings.SetFullScreen();
        }

        if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == false)
        {
            hasF3On = true;
        }
        else if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == true)
        {
            hasF3On = false;
        }

        //! Handling Typing Mode

        if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.OemTilde))
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

        if (TypingMode && InputManager.IsKeyPressed(kstate, prevkstate, Keys.Enter))
        {
            switch (InputText)
            {
                case "/yipee":
                    Player.successSound.Play((float)LoweredVolume, 0.0f, 0.0f);
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
                    Triangle.Summon(new Vector2(player.Drect.X, player.Drect.Y));
                    break;

                case "/summonBossTriangle":
                    Triangle.SummonBoss(new Vector2(player.Drect.X, player.Drect.Y));
                    break;

                case "/clearTriangle":
                    Triangle.ClearAll();
                    break;

                case "/summonCircle":
                    Circle.Summon(new Vector2(player.Drect.X, player.Drect.Y));
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

        if (TypingMode && InputManager.IsKeyPressed(kstate, prevkstate, Keys.Escape))
        {
            InputText = "";
            TypingMode = false;
        }

        //! Updating for all Game States

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Update(gameTime);
                if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.Escape) && !TypingMode)
                {
                    currentGameState = GameState.Quit;
                }
                break;

            case GameState.Playing:
                tilemap.Update(this);
                player.Update(gameTime);
                player.CalculateTranslation();

                foreach (var fireball in fireballs.ToList())
                {
                    fireball.Update(gameTime);
                }

                foreach (var triangle in triangles.ToList())
                {
                    triangle.Update(gameTime);
                }

                foreach (var circle in circles.ToList())
                {
                    circle.Update(gameTime);
                }

                if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.Escape) && !TypingMode)
                {
                    currentGameState = GameState.Paused;
                }
                break;

            case GameState.Paused:
                if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.Escape) && !TypingMode)
                {
                    currentGameState = GameState.MainMenu;
                }
                paused.Update(gameTime);
                break;

            case GameState.Options:
                options.Update(gameTime);
                break;

            case GameState.Quit:
                quit.Update(gameTime);
                break;

            case GameState.Death:
                death.Update(gameTime);
                break;

            case GameState.Win:
                win.Update(gameTime);
                break;

            case GameState.Pass:
                pass.Update(gameTime);
                break;

            case GameState.Info:
                info.Update(gameTime);
                break;
        }

        if(currentGameState == GameState.Playing || currentGameState == GameState.Paused || currentGameState == GameState.Pass || currentGameState == GameState.Win || currentGameState == GameState.Death || (currentGameState == GameState.Options && options.cameFrom == SettingsScreen.CameFrom.Paused))
        {
            if (MediaPlayer.Queue.ActiveSong != playMusic)
            { MediaPlayer.Play(playMusic); }
            MediaPlayer.Volume = (float)LoweredVolume - 0.1f;
        } else {
            if (MediaPlayer.Queue.ActiveSong != menuMusic)
            { MediaPlayer.Play(menuMusic); }
            MediaPlayer.Volume = (float)LoweredVolume;
        }

        if(currentGameState != GameState.Playing)
        { background.Update(gameTime); }

        MediaPlayer.IsRepeating = true;


        prevkstate = kstate;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        canvas.Activate();

        GraphicsDevice.Clear(Color.CornflowerBlue);

        background.DrawBG(gameTime);

        //!Beggining Play Sprite Batch
        Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: player.translation);

        switch (currentGameState)
        {
            case GameState.Playing:
                tilemap.Draw(gameTime);

                player.Draw(Globals.SpriteBatch);

                foreach (var fireball in fireballs.ToList())
                {
                    fireball.Draw(Globals.SpriteBatch);
                }

                foreach (var triangle in triangles.ToList())
                {
                    triangle.Draw(Globals.SpriteBatch);
                }

                foreach (var circle in circles.ToList())
                {
                    circle.Draw(Globals.SpriteBatch);
                }
                break;
        }

        //!Ending Play Sprite Batch
        Globals.SpriteBatch.End();

        //!Beginning UI Sprite Batch
        Globals.SpriteBatch.Begin();

        switch (currentGameState)
        {
            case GameState.MainMenu:
                mainMenu.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Playing:
                string health = "Health: " + player.Health.ToString() + "/100";
                string level = "Level: " + Tilemap.level;
                string xartomantila = "Xartomantila: " + Player.xartomantila;

                Globals.SpriteBatch.DrawString(statsFont, health, new Vector2(Settings.SimulationSize.X - statsFont.MeasureString(health).X - 20, 10), Color.Black);
                Globals.SpriteBatch.DrawString(statsFont, level, new Vector2(Settings.SimulationSize.X - statsFont.MeasureString(level).X - 20, 60), Color.Black);
                Globals.SpriteBatch.DrawString(statsFont, xartomantila, new Vector2(Settings.SimulationSize.X - statsFont.MeasureString(xartomantila).X - 20, 110), Color.Black);
                break;

            case GameState.Paused:
                paused.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Options:
                options.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Quit:
                quit.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Death:
                death.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Win:
                win.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Pass:
                pass.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Info:
                info.Draw(Globals.SpriteBatch, Globals.Graphics);
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
                Globals.SpriteBatch.DrawString(debugFont, info, pos, Color.Black);
                pos.Y += 20;
            }
        }

        //! Handling Typing Mode

        if (TypingMode)
        {
            DrawTypingZone();
        }

        //!Ending UI Sprite Batch
        Globals.SpriteBatch.End();

        base.Draw(gameTime);

        canvas.Draw(Globals.SpriteBatch);
    }

    public static void ExitGame()
    {
        Environment.Exit(0);
    }

    public static void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color)
    {
        spriteBatch.Draw(
            pixelTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            color
        );
        spriteBatch.Draw(
            pixelTexture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            color
        );
        spriteBatch.Draw(
            pixelTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            color
        );
        spriteBatch.Draw(
            pixelTexture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            color
        );
    }

    public void DrawTypingZone()
    {
        Globals.SpriteBatch.Draw(pixelTexture, new Rectangle(Globals.Settings.WindowSize.X / 20, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10, Globals.Settings.WindowSize.X - Globals.Settings.WindowSize.X / 10, Globals.Settings.WindowSize.Y / 20), new Color(Color.Black, 0.35f));
        Globals.SpriteBatch.DrawString(typeFont, InputText, new Vector2(Globals.Settings.WindowSize.X / 20 + 10, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10 + 5), Color.White);
    }

    public static bool TryParseTwoArgs(string input, out int arg1, out int arg2)
    {
        arg1 = arg2 = 0;
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 && int.TryParse(parts[0], out arg1) && int.TryParse(parts[1], out arg2);
    }
}
