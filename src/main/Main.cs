using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace BlobGame;

public class Main : Game
{
    public static Main main;
    public float deltaTime;
    public static string credits = "Made by MrBeelo";
    public static string version = "v0.39.2";
    public static string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "settings.json");
    public static string savefileFilePath = Path.Combine(AppContext.BaseDirectory, "data", "savefile.json");
    public static Player player {get; set;}
    public static Fireball fireball {get; set;}
    public static Tilemap tilemap {get; set;}
    public static Triangle triangle {get; set;}
    public static Circle circle {get; set;}
    public static List<Triangle> triangles = new();
    public static List<Circle> circles = new();
    public static List<Fireball> fireballs = new();
    public static List<Sprite> sprites = new();
    public static bool hasF3On = false;
    public static Texture2D pixelTexture;
    public static SpriteFont font;
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
    Texture2D background;
    public static InputManager inputManager = new InputManager();
    public Canvas canvas;
    public bool TypingMode = false;
    public string InputText = "";
    public enum GameState
    {MainMenu, Playing, Paused, Options, Quit, Death, Win, Pass, Info}
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

        font = Content.Load<SpriteFont>("assets/fonts/font");
        debugFont = Content.Load<SpriteFont>("assets/fonts/debugFont");
        background = Content.Load<Texture2D>("assets/bg");

        Texture2D playerTexture = Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");
        Texture2D fireTexture = Content.Load<Texture2D>("assets/sprites/fireball/Fireball1");
        Texture2D triangleTexture = Content.Load<Texture2D>("assets/sprites/triangle/TriangleIdle1");
        Texture2D circleTexture = Content.Load<Texture2D>("assets/sprites/circle/CircleIdle1");

        mainMenu = new MainMenuScreen(font, Globals.Graphics);
        paused = new PausedScreen(font, Globals.Graphics);
        quit = new QuitScreen(font, Globals.Graphics);
        options = new SettingsScreen(font, Globals.Graphics);
        death = new DeathScreen(font, Globals.Graphics);
        win = new WinScreen(font, Globals.Graphics);
        pass = new PassScreen(font, Globals.Graphics);
        info = new InfoScreen(font, Globals.Graphics);

        Globals.SaveFile.Initialize();

        player = new Player(playerTexture, new Rectangle((int)Globals.SaveFile.Level.Y, (int)Globals.SaveFile.Level.Z, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30), Globals.Graphics);
        player.LoadContent(this);
        sprites.Add(player);

        tilemap = new Tilemap();
        tilemap.LoadContent(this);

        fireball = new Fireball(fireTexture, Rectangle.Empty, Rectangle.Empty, Globals.Graphics, false);
        fireball.LoadContent(this);

        triangle = new Triangle(triangleTexture, new Rectangle((int)Globals.SaveFile.Level.Y, (int)Globals.SaveFile.Level.Z, Triangle.triangleSizeW, Triangle.triangleSizeH), new(0, 0, 20, 30), Globals.Graphics);
        triangle.LoadContent(this);

        circle = new Circle(circleTexture, new Rectangle((int)Globals.SaveFile.Level.Y, (int)Globals.SaveFile.Level.Z, Circle.circleSizeW, Circle.circleSizeH), new(0, 0, 20, 30), Globals.Graphics);
        circle.LoadContent(this);

        Player.Respawn(player);
    }

    protected override void Update(GameTime gameTime)
    {
        main = this;

        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Globals.Update(gameTime);

        KeyboardState kstate = Keyboard.GetState();

        inputManager.Update(gameTime);

        if(TypingMode)
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

        if(InputManager.IsKeyPressed(kstate, prevkstate, Keys.F11))
        {
            Globals.Settings.SetFullScreen();
        }

        if(InputManager.IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == false)
        {
            hasF3On = true;
        } else if (InputManager.IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == true)
        {
            hasF3On = false;
        }

        //! Handling Typing Mode

        if(InputManager.IsKeyPressed(kstate, prevkstate, Keys.OemTilde))
        {
            if(!TypingMode)
            {
                TypingMode = true;
            } else if (TypingMode)
            {
                TypingMode = false;
            }
        }

        if(TypingMode && InputManager.IsKeyPressed(kstate, prevkstate, Keys.Enter))
        {
            switch(InputText)
            {
                case "/yipee":
                    Player.successSound.Play((float)LoweredVolume, 0.0f, 0.0f);
                    break;
                
                case "/kill":
                    player.alive = false;
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
                    if(Tilemap.level.X < Tilemap.Collision.Length - 1)
                    {
                        Tilemap.MoveLevel();
                    }
                    break;

                case "/immune":
                    if(!player.Immune)
                    {
                        player.Immune = true;
                    } else if(player.Immune)
                    {
                        player.Immune = false;
                    }
                    break;

                case "/summonTriangle":
                    Triangle.Summon(new Vector2(player.Drect.X, player.Drect.Y));
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

                case string s when s.StartsWith("/moveTo"):
                    string levelPart = s.Substring("/moveTo ".Length).Trim();
                    if (int.TryParse(levelPart, out int level))
                    {
                        if(level >= 0 && level < Tilemap.Collision.Length)
                        {
                            Tilemap.EvaluateLevel(level);
                            Tilemap.level.X = level;
                            Player.Respawn(player);
                            Globals.SaveFile.SaveSavefile(savefileFilePath);
                        }
                    }
                    break;

                default:
                    Debug.WriteLine("INVALID COMMAND");
                    break;
            }
            Debug.WriteLine(InputText);
            InputText = "";
            TypingMode = false;
        }

        if(TypingMode && InputManager.IsKeyPressed(kstate, prevkstate, Keys.Escape))
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

                foreach(var fireball in fireballs.ToList())
                {
                    fireball.Update(gameTime);
                }

                foreach(var triangle in triangles.ToList())
                {
                    triangle.Update(gameTime);
                }

                foreach(var circle in circles.ToList())
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

        prevkstate = kstate;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        canvas.Activate();

        GraphicsDevice.Clear(Color.CornflowerBlue);

        if(currentGameState == GameState.Playing)
        {
            DrawBG();
        }

        //!Beggining Play Sprite Batch
        Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: player.translation);

        switch (currentGameState) 
        {
            case GameState.Playing:
                tilemap.Draw(gameTime);

                if(hasF3On)
                {
                    foreach(var rect in player.horizontalCollisions)
                    {
                        DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                    }
                    foreach(var rect in player.verticalCollisions)
                    {
                        DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                    }
                    DrawRectHollow(Globals.SpriteBatch, player.Drect, 4, Color.Blue);
                }

                player.Draw(Globals.SpriteBatch);

                foreach(var fireball in fireballs.ToList())
                {
                    fireball.Draw(Globals.SpriteBatch);
                }

                foreach(var triangle in triangles.ToList())
                {
                    triangle.Draw(Globals.SpriteBatch);
                }

                foreach(var circle in circles.ToList())
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
                string level = "Level: " + Tilemap.level.X;
                string xartomantila = "Xartomantila: " + Player.xartomantila;

                Globals.SpriteBatch.DrawString(font, health, new Vector2(Settings.SimulationSize.X - font.MeasureString(health).X - 20, 10), Color.Black);
                Globals.SpriteBatch.DrawString(font, level, new Vector2(Settings.SimulationSize.X - font.MeasureString(level).X - 20, 60), Color.Black);
                Globals.SpriteBatch.DrawString(font, xartomantila, new Vector2(Settings.SimulationSize.X - font.MeasureString(xartomantila).X - 20, 110), Color.Black);
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

        if(hasF3On)
            {
                List<string> otherDebugInfoList = new List<string>();

                foreach(var sprite in sprites)
                {
                    string[] spriteDebugInfo = sprite.GetDebugInfo();
                    otherDebugInfoList.AddRange(spriteDebugInfo);
                }

                foreach(var fireball in fireballs)
                {
                    string[] fireballDebugInfo = fireball.GetDebugInfo();
                    otherDebugInfoList.AddRange(fireballDebugInfo);
                }

                foreach(var triangle in triangles)
                {
                    string[] triangleDebugInfo = triangle.GetDebugInfo();
                    otherDebugInfoList.AddRange(triangleDebugInfo);
                }

                foreach(var circle in circles)
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

                foreach(var info in combinedDebugInfo)
                {
                    Globals.SpriteBatch.DrawString(debugFont, info, pos, Color.Black);
                    pos.Y += 20;
                }
            }

        //! Handling Typing Mode

        if(TypingMode)
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

    public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color) {
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

    public void DrawBG()
    {
        Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Globals.SpriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);
        Globals.SpriteBatch.End();
    }

    public void DrawTypingZone()
    {
        Globals.SpriteBatch.Draw(pixelTexture, new Rectangle(Globals.Settings.WindowSize.X / 20, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10, Globals.Settings.WindowSize.X - Globals.Settings.WindowSize.X / 10, Globals.Settings.WindowSize.Y / 20), new Color(Color.Black, 0.35f));
        Globals.SpriteBatch.DrawString(font, InputText, new Vector2(Globals.Settings.WindowSize.X / 20 + 10, Globals.Settings.WindowSize.Y - Globals.Settings.WindowSize.Y / 10 + 5), Color.White);
    }
}