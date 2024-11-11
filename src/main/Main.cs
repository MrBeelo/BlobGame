using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Linq;

namespace BlobGame;

public class Main : Game
{
    public static string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "settings.json");
    public static Player player {get; set;}
    public static Fireball fireball {get; set;}
    public static Tilemap tilemap {get; set;}
    public static List<Fireball> fireballs = new();
    public static List<Sprite> sprites = new();
    public static bool hasF3On = false;
    public static bool hasF11On = false;
    public static Texture2D pixelTexture;
    public static SpriteFont font;
    public static SpriteFont debugFont;
    public static GameState currentGameState = GameState.MainMenu;
    private MainMenuScreen mainMenu;
    private PausedScreen paused;
    private QuitScreen quit;
    public SettingsScreen options;
    public PausedSettingsScreen poptions;
    public DeathScreen death;
    public WinScreen win;
    public PassScreen pass;
    public static double LoweredVolume = Globals.Settings.Volume * 0.4;
    
    public FollowCamera camera;
    KeyboardState prevkstate;
    public int frameCounter;
    public TimeSpan timeSpan;
    public int FPS;
    Texture2D background;
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Options,
        Quit,
        POptions,
        Death,
        Win,
        Pass
    }
    public Main()
    {
        Window.Title = "Blob Game";
        Globals.Graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "content";
        IsMouseVisible = true;

        Globals.Graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
        Globals.Graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
    }

    protected override void Initialize()
    {
        Settings.LoadSettings(settingsFilePath);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Settings.LoadSettings(settingsFilePath);
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        //! Definition of a texture and a position for the sprite class is needed here.
        //! Apart from that, you can do whatever the fuck you want with all entities after this point.

        font = Content.Load<SpriteFont>("assets/fonts/font");
        debugFont = Content.Load<SpriteFont>("assets/fonts/debugFont");
        background = Content.Load<Texture2D>("assets/bg");

        Texture2D playerTexture = Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");
        Texture2D fireTexture = Content.Load<Texture2D>("assets/sprites/fireball/Fireball1");

        mainMenu = new MainMenuScreen(font, Globals.Graphics);
        paused = new PausedScreen(font, Globals.Graphics);
        quit = new QuitScreen(font, Globals.Graphics);
        options = new SettingsScreen(font, Globals.Graphics);
        poptions = new PausedSettingsScreen(font, Globals.Graphics);
        death = new DeathScreen(font, Globals.Graphics);
        win = new WinScreen(font, Globals.Graphics);
        pass = new PassScreen(font, Globals.Graphics);

        player = new Player(playerTexture, new Rectangle((int)Tilemap.level.Y, (int)Tilemap.level.Z, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30), Globals.Graphics);
        player.LoadContent(this);
        sprites.Add(player);

        fireball = new Fireball(fireTexture, Rectangle.Empty, Rectangle.Empty, Globals.Graphics, false);
        fireball.LoadContent(this);

        tilemap = new Tilemap();
        tilemap.LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kstate = Keyboard.GetState();

        LoweredVolume = Globals.Settings.Volume * 0.6;

        timeSpan += gameTime.ElapsedGameTime;
        frameCounter++;

        if (timeSpan > TimeSpan.FromSeconds(1))
        {
            timeSpan -= TimeSpan.FromSeconds(1);
            FPS = frameCounter;
            frameCounter = 0;
        }

        if(hasF11On)
        {
            Globals.Graphics.IsFullScreen = true;
            IsMouseVisible = false;
            Globals.Graphics.ApplyChanges();
        } else {
            Globals.Graphics.IsFullScreen = false;
            IsMouseVisible = true;
            Globals.Graphics.ApplyChanges();
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.F11) && hasF11On == false)
        {
            hasF11On = true;
        } else if(IsKeyPressed(kstate, prevkstate, Keys.F11) && hasF11On == true)
        {
            hasF11On = false;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == false)
                {
                    hasF3On = true;
                } else if (IsKeyPressed(kstate, prevkstate, Keys.F3) && hasF3On == true)
                {
                    hasF3On = false;
                }

        switch (currentGameState) 
        {
            case GameState.MainMenu:
                mainMenu.Update(gameTime);
                if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
                {
                    currentGameState = GameState.Quit;
                }
                break;

            case GameState.Playing:
                player.Update(gameTime);

                foreach(Fireball fireball in fireballs.ToList())
                {
                    fireball.Update(gameTime);
                }

                if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
                {
                    currentGameState = GameState.Paused;
                }
                break;

            case GameState.Paused:
                if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
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
            
            case GameState.POptions:
                poptions.Update(gameTime);
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
        }

        prevkstate = kstate;

        //camera.Follow(player.Drect, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
        Globals.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //Beginning Sprite Batch
        Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        switch (currentGameState) 
        {
            case GameState.MainMenu:
                mainMenu.Draw(Globals.SpriteBatch, Globals.Graphics);
                break;

            case GameState.Playing:
                Globals.SpriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);

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

                foreach(Fireball fireball in fireballs.ToList())
                {
                    fireball.Draw(Globals.SpriteBatch);
                }
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
            
            case GameState.POptions:
                poptions.Draw(Globals.SpriteBatch, Globals.Graphics);
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
                string[] mainDebugInfo = 
                {
                    "Current Game State: " + currentGameState,
                    "FPS: " + FPS,
                    "Level: " + Tilemap.level
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
            
        //Ending Sprite Batch
        Globals.SpriteBatch.End();
        
        base.Draw(gameTime);
    }

    public static bool IsKeyPressed(KeyboardState kstate, KeyboardState prevkstate, Keys key)
        {
            return kstate.IsKeyDown(key) && !prevkstate.IsKeyDown(key);
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
}