using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BlobGame;
using System.IO;

namespace BlobGame;

public class Main : Game
{
    private GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    Player player;

    public static List<Sprite> sprites;
    public static bool hasF3On = false;
    public static Texture2D pixelTexture;
    public static Texture2D rectangleTexture;
    public static SpriteFont font;
    public static GameState currentGameState = GameState.MainMenu;
    private MainMenuScreen mainMenu;
    private PausedScreen paused;
    private QuitScreen quit;
    public Dictionary<Vector2, int> normal;
    public Dictionary<Vector2, int> collision;
    public Texture2D textureAtlas;
    public Texture2D hitboxAtlas;
    private int tilesize = 30; //Display Tilesize
    public int playerSizeW = 60;
    public int playerSizeH = 90;
    private List<Rectangle> intersections;
    KeyboardState prevkstate;
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Options,
        Quit
    }
    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "content";
        IsMouseVisible = true;
        normal = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_normal.csv"));
        collision = LoadMap(Path.Combine(Content.RootDirectory, "..", "data", "testlevel_collision.csv"));

        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;

        sprites = new();
        intersections = new();
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.Red });

        //! Definition of a texture and a position for the sprite class is needed here.
        //! Apart from that, you can do whatever the fuck you want with all entities after this point.

        font = Content.Load<SpriteFont>("assets/fonts/font");
        textureAtlas = Content.Load<Texture2D>("assets/atlas");
        hitboxAtlas = Content.Load<Texture2D>("assets/collision_atlas");

        Texture2D playerTexture = Content.Load<Texture2D>("assets/sprites/player/PlayerIdle1");
        Rectangle playerDrect = new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 200, playerSizeW, playerSizeH);
        player = new Player(playerTexture, playerDrect, new(0, 0, 20, 30), graphics);
        player.LoadContent(this);
        sprites.Add(player);

        mainMenu = new MainMenuScreen(font, graphics);
        paused = new PausedScreen(font, graphics);
        quit = new QuitScreen(font, graphics);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kstate = Keyboard.GetState();

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

        foreach (var rect in intersections)
        {
            if(collision.TryGetValue(new Vector2(rect.X, rect.Y), out int value))
            {
                
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
        }

        prevkstate = kstate;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

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
                    DrawRectHollow(spriteBatch, new Rectangle(rect.X * tilesize, rect.Y * tilesize, tilesize, tilesize), 1);
                    spriteBatch.End();
                }
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                DrawRectHollow(spriteBatch, player.Drect, 4);
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
                    spriteBatch.Draw(hitboxAtlas, dest, src, Color.White);
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
        }

        if(hasF3On)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Current Game State: " + currentGameState, Vector2.Zero, Color.Black);
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

    public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness) {
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
    }

    public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target) {

        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % tilesize)) / tilesize;
        int heightInTiles = (target.Height - (target.Height % tilesize)) / tilesize;

        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {

                intersections.Add(new Rectangle(

                    (target.X + x*tilesize) / tilesize,
                    (target.Y + y*(tilesize-1)) / tilesize,
                    tilesize,
                    tilesize

                ));

            }
        }

        return intersections;
    }
    public List<Rectangle> getIntersectingTilesVertical(Rectangle target) {

        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % tilesize)) / tilesize;
        int heightInTiles = (target.Height - (target.Height % tilesize)) / tilesize;

        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {

                intersections.Add(new Rectangle(

                    (target.X + x*(tilesize-1)) / tilesize,
                    (target.Y + y*tilesize) / tilesize,
                    tilesize,
                    tilesize

                ));

            }
        }

        return intersections;
    }
}