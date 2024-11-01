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
    public static List<Sprite> sprites;
    public static bool hasF3On = false;
    public static bool hasF11On = false;
    public static Texture2D pixelTexture;
    public static SpriteFont font;
    public static GameState currentGameState = GameState.MainMenu;
    private MainMenuScreen mainMenu;
    private PausedScreen paused;
    private QuitScreen quit;
    public SettingsScreen options;
    public PausedSettingsScreen poptions;
    public DeathScreen death;
    public WinScreen win;
    public static Dictionary<Vector2, int> normal;
    public static Dictionary<Vector2, int> collision;
    public Texture2D textureAtlas;
    public Texture2D hitboxAtlas;
    public static int tilesize = 30; //Display Tilesize
    
    public FollowCamera camera;
    KeyboardState prevkstate;
    public int frameCounter;
    public TimeSpan timeSpan;
    public int FPS;
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Options,
        Quit,
        POptions,
        Death,
        Win
    }
    public Main()
    {
        //IsFixedTimeStep = false;
        Globals.Graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "content";
        IsMouseVisible = true;
        normal = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_normal.csv"));
        collision = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_collision.csv"));

        camera = new(Vector2.Zero);

        Globals.Graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
        Globals.Graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;

        sprites = new();

        //TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 240.0); 
    }

    public Dictionary<Vector2, int> LoadMap(string filepath)
    {
        Dictionary<Vector2, int> result = new();

        StreamReader reader = new (filepath);
        int y = 0;
        string line;
        while((line = reader.ReadLine()) != null) {
            string[] Items = line.Split(',');
            for(int x = 0; x < Items.Length; x++)
            {
                if(int.TryParse(Items[x], out int value)) {
                    if(value > -1) {
                        result[new Vector2(x, y)] = value;
                    }

                }
            }

            y++;
        }

        return result;

    }

    protected override void Initialize()
    {
        Settings.LoadSettings(settingsFilePath);
        base.Initialize();
        //TEST
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
        textureAtlas = Content.Load<Texture2D>("assets/atlas");
        hitboxAtlas = Content.Load<Texture2D>("assets/collision_atlas");

        Texture2D playerTexture = Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");

        /*switch(settings.Level)
            {
                case 1:
                levelStartPos.X = 50;
                levelStartPos.Y = 600;
                break;

                case 2:
                levelStartPos.X = 200;
                levelStartPos.Y = 400;
                break;
            }*/

        
        //var playerPosition = settings.GetPlayerPos(settings.PlayerPos.ToString.);
        //Rectangle playerDrect = new Rectangle(50, 600, playerSizeW, playerSizeH);
        player = new Player(playerTexture, new Rectangle(50, 600, Player.playerSizeW, Player.playerSizeH), new(0, 0, 20, 30), Globals.Graphics);
        player.LoadContent(this);
        sprites.Add(player);

        mainMenu = new MainMenuScreen(font, Globals.Graphics);
        paused = new PausedScreen(font, Globals.Graphics);
        quit = new QuitScreen(font, Globals.Graphics);
        options = new SettingsScreen(font, Globals.Graphics);
        poptions = new PausedSettingsScreen(font, Globals.Graphics);
        death = new DeathScreen(font, Globals.Graphics);
        win = new WinScreen(font, Globals.Graphics);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kstate = Keyboard.GetState();

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

        if (currentGameState == GameState.MainMenu)
        {
            mainMenu.Update(gameTime);
            if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
            {
                currentGameState = GameState.Quit;
            }
        }
        else if (currentGameState == GameState.Playing)
        {
        player.Update(gameTime);

            if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
            {
                currentGameState = GameState.Paused;
            }
        } else if (currentGameState == GameState.Paused) {
            if (IsKeyPressed(kstate, prevkstate, Keys.Escape))
            {
                currentGameState = GameState.MainMenu;
            }
            paused.Update(gameTime);
        } else if (currentGameState == GameState.Quit)
        {
            quit.Update(gameTime);
        } else if (currentGameState == GameState.Options)
        {
            options.Update(gameTime);
        } else if (currentGameState == GameState.POptions)
        {
            poptions.Update(gameTime);
        } else if (currentGameState == GameState.Death)
        {
            death.Update(gameTime);
        } else if (currentGameState == GameState.Win)
        {
            win.Update(gameTime);
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

        if (currentGameState == GameState.MainMenu)
        {
            mainMenu.Draw(Globals.SpriteBatch, Globals.Graphics);
        }
        else if (currentGameState == GameState.Playing)
        {
            if(hasF3On)
            {
                foreach(var rect in player.horizontalCollisions)
                {
                    DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize), 1, Color.DarkBlue);
                }
                foreach(var rect in player.verticalCollisions)
                {
                    DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize), 1, Color.DarkBlue);
                }
                DrawRectHollow(Globals.SpriteBatch, player.Drect, 4, Color.Blue);
            }

            int tpr = 8; //Tiles per row
            int p_tilesize = 16; //Pixel Tilesize
            foreach(var item in normal)
            {
                if(player.stamina < 500)
                {
                    if(item.Value == 14) continue;
                }

                Rectangle dest = new(
                    (int)item.Key.X * tilesize,
                    (int)item.Key.Y * tilesize,
                    tilesize,
                    tilesize
                );

                int x = item.Value % tpr;
                int y = item.Value / tpr;

                Rectangle src = new(
                    x * p_tilesize,
                    y * p_tilesize,
                    p_tilesize,
                    p_tilesize
                );

                Globals.SpriteBatch.Draw(textureAtlas, dest, src, Color.White);
            }

            foreach(var item in collision)
            {
                if(player.stamina < 500)
                {
                    if(item.Value == 5) continue;
                }

                Rectangle dest = new(
                    (int)item.Key.X * tilesize,
                    (int)item.Key.Y * tilesize,
                    tilesize,
                    tilesize
                );

                int x = item.Value % tpr;
                int y = item.Value / tpr;

                Rectangle src = new(
                    x * p_tilesize,
                    y * p_tilesize,
                    p_tilesize,
                    p_tilesize
                );

                if(hasF3On)
                {
                    //spriteBatch.Draw(hitboxAtlas, dest, src, Color.White);
                    DrawRectHollow(Globals.SpriteBatch, dest, 2, Color.Orange);
                }
            }

            player.Draw(Globals.SpriteBatch);
           
        }
        else if (currentGameState == GameState.Paused)
        {
            paused.Draw(Globals.SpriteBatch, Globals.Graphics);
        } else if (currentGameState == GameState.Quit)
        {
            quit.Draw(Globals.SpriteBatch, Globals.Graphics);
        } else if(currentGameState == GameState.Options)
        {
            options.Draw(Globals.SpriteBatch, Globals.Graphics);
        } else if(currentGameState == GameState.POptions)
        {
            poptions.Draw(Globals.SpriteBatch, Globals.Graphics);
        } else if(currentGameState == GameState.Death)
        {
            death.Draw(Globals.SpriteBatch, Globals.Graphics);
        } else if(currentGameState == GameState.Win)
        {
            win.Draw(Globals.SpriteBatch, Globals.Graphics);
        }

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
                    "FPS: " + FPS
                    //"Level: " + settings.Level
                };

                string[] otherDebugInfo = otherDebugInfoList.ToArray();

                string[] combinedDebugInfo = mainDebugInfo.Concat(otherDebugInfo).ToArray();

                Vector2 pos = Vector2.Zero;

                foreach(var info in combinedDebugInfo)
                {
                    Globals.SpriteBatch.DrawString(font, info, pos, Color.Black);
                    pos.Y += 32;
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