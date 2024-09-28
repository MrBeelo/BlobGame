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
    private GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public Settings settings = new Settings();
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
    public Dictionary<Vector2, int> normal;
    public Dictionary<Vector2, int> collision;
    public Texture2D textureAtlas;
    public Texture2D hitboxAtlas;
    public int tilesize = 30; //Display Tilesize
    public static int playerSizeW = 60;
    public static int playerSizeH = 90;
    //public static Vector2 levelStartPos = new Vector2(0, 0);
    private List<Rectangle> intersections;
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
        POptions
    }
    public Main()
    {
        //IsFixedTimeStep = false;
        graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "content";
        IsMouseVisible = true;
        normal = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_normal.csv"));
        collision = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_collision.csv"));

        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;

        sprites = new();
        intersections = new();

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
    }

    protected override void LoadContent()
    {
        Settings.LoadSettings(settingsFilePath);
        spriteBatch = new SpriteBatch(GraphicsDevice);

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
        player = new Player(playerTexture, new Rectangle(50, 600, playerSizeW, playerSizeH), new(0, 0, 20, 30), graphics);
        player.LoadContent(this);
        sprites.Add(player);

        mainMenu = new MainMenuScreen(font, graphics);
        paused = new PausedScreen(font, graphics);
        quit = new QuitScreen(font, graphics);
        options = new SettingsScreen(font, graphics);
        poptions = new PausedSettingsScreen(font, graphics);
    }

    protected override void Update(GameTime gameTime)
    {
        //Settings.LoadSettings(settingsFilePath);
        KeyboardState kstate = Keyboard.GetState();

        /*if(IsKeyPressed(kstate, prevkstate, Keys.F))
        {
            settings.Level += 1;
            settings.SaveSettings(settingsFilePath);
        }*/
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
            graphics.IsFullScreen = true;
            IsMouseVisible = false;
            graphics.ApplyChanges();
        } else {
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
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

        player.Drect.X += (int)player.velocity.X;
        intersections = getIntersectingTilesHorizontal(player.Drect);

        foreach (var rect in intersections)
        {
            if(collision.TryGetValue(new Vector2(rect.X, rect.Y), out int value))
            {
                Rectangle collision = new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize);

                if(player.velocity.X > 0.0f)
                {
                    player.Drect.X = collision.Left - player.Drect.Width;
                } else if(player.velocity.X < 0.0f)
                {
                    player.Drect.X = collision.Right;
                }
            }
        }

        player.Drect.Y += (int)player.velocity.Y;
        intersections = getIntersectingTilesVertical(player.Drect);

        player.isInAir = true;
        foreach (var rect in intersections)
        {
            if(collision.TryGetValue(new Vector2(rect.X, rect.Y), out int value))
            {
                Rectangle collision = new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize);

                if(player.velocity.Y > 0.0f)
                {
                    player.Drect.Y = collision.Top - player.Drect.Height;
                    player.velocity.Y = 0.5f;
                    player.isInAir = false;
                } else if(player.velocity.Y < 0.0f)
                {
                    player.Drect.Y = collision.Bottom;
                } else {
                }

            }
        }

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
        }

        prevkstate = kstate;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        //Settings.LoadSettings(settingsFilePath);

        if (currentGameState == GameState.MainMenu)
        {
            mainMenu.Draw(spriteBatch, graphics);
        }
        else if (currentGameState == GameState.Playing)
        {
            if(hasF3On)
            {
                foreach(var rect in intersections)
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    DrawRectHollow(spriteBatch, new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize), 1, Color.DarkBlue);
                    spriteBatch.End();
                }
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                DrawRectHollow(spriteBatch, player.Drect, 4, Color.Blue);
                spriteBatch.End();
            }

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            int tpr = 8; //Tiles per row
            int p_tilesize = 16; //Pixel Tilesize
            foreach(var item in normal)
            {
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

                spriteBatch.Draw(textureAtlas, dest, src, Color.White);
            }

            foreach(var item in collision)
            {
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
                    DrawRectHollow(spriteBatch, dest, 2, Color.Orange);
                }
            }

            spriteBatch.End();

            player.Draw(spriteBatch);
           
        }
        else if (currentGameState == GameState.Paused)
        {
            paused.Draw(spriteBatch, graphics);
        } else if (currentGameState == GameState.Quit)
        {
            quit.Draw(spriteBatch, graphics);
        } else if(currentGameState == GameState.Options)
        {
            options.Draw(spriteBatch, graphics);
        } else if(currentGameState == GameState.POptions)
        {
            poptions.Draw(spriteBatch, graphics);
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

                spriteBatch.Begin();
                foreach(var info in combinedDebugInfo)
                {
                    spriteBatch.DrawString(font, info, pos, Color.Black);
                    pos.Y += 32;
                }
                spriteBatch.End();
            }

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

    public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target) {

        List<Rectangle> intersections = new();

        for (int x = 0; x <= playerSizeW; x += tilesize) {
            for (int y = 0; y <= playerSizeH; y += tilesize) {

                intersections.Add(new Rectangle(

                    (target.X + x) / tilesize,
                    (target.Y + y / tilesize *(tilesize-1)) / tilesize,
                    tilesize,
                    tilesize

                ));

            }
        }

        if (playerSizeW % tilesize != 0) {
        intersections.Add(new Rectangle(
            (target.X + playerSizeW) / tilesize,
            (target.Y) / tilesize,
            tilesize,
            tilesize
        ));
        }

        return intersections;
    }
    public List<Rectangle> getIntersectingTilesVertical(Rectangle target) {

        List<Rectangle> intersections = new();

        for (int x = 0; x <= playerSizeW; x += tilesize) {
            for (int y = 0; y <= playerSizeH; y += tilesize) {

                intersections.Add(new Rectangle(

                    (target.X + x / tilesize *(tilesize-1)) / tilesize,
                    (target.Y + y) / tilesize,
                    tilesize,
                    tilesize

                ));

            }
        }

        if (playerSizeH % tilesize != 0) {
        intersections.Add(new Rectangle(
            (target.X) / tilesize,
            (target.Y + playerSizeH) / tilesize,
            tilesize,
            tilesize
        ));
        }

        return intersections;
    }
}