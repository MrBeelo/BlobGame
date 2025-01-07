using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace BlobGame
{
    public class CrystalEvent : Event
    {
    public static Texture2D crystal;
    public static Texture2D warning;
    public int crystalTimer;
    public Vector2 warningPos;
    public Vector2 crystalPos;
    public static Vector2 warningSize = new(128, 128);
    public static Vector2 crystalSize = new(120, 1440);
    public Rectangle crystalDrect;

    public override void LoadContent(Game game)
    {
        crystal = LoadTexture("sprites/events/Crystal.png");
        warning = LoadTexture("sprites/events/Warning.png");
    }

    public static void Start()
    {
        CrystalEvent crystalEvent = new();
        Game.events.Add(crystalEvent);
        crystalEvent.crystalTimer = 300;
        Random random = new();
        int posX = random.Next(0 + 30, Settings.SimulationSize.X - 30);
        crystalEvent.warningPos = new(posX, Settings.SimulationSize.Y / 2);
        crystalEvent.crystalPos = new(posX, Settings.SimulationSize.Y);
    }

    public override void Update()
    {
        crystalDrect = new Rectangle(crystalPos, crystalSize);

        if(crystalDrect.Intersects(Game.player.Drect))
        {
            Player.Damage(50);
        }

        if(crystalTimer > 0)
        {
            crystalTimer--;
        }

        if(crystalTimer > 180 && crystalTimer < 200)
        {
            crystalPos.Y -= 60;
        } else if(crystalTimer < 20)
        {
            crystalPos.Y += 60;
        }

        if(crystalTimer == 0)
        {
            Game.events.Remove(this);
        }
    }

    public override void Draw()
    {
        int sineOut = crystalTimer % 100;
        if(crystalTimer > 200)
        {
            DrawTexturePro(warning, new Rectangle(0, 0, 256, 256), new Rectangle(warningPos, warningSize), Vector2.Zero, 0f, new Color(255, 255, 255, sineOut));
        } else if(crystalTimer < 200)
        {
            DrawTexturePro(crystal, new Rectangle(0, 0, 20, 240), crystalDrect, Vector2.Zero, 0f, Color.White);
        }
    }
    }
}


