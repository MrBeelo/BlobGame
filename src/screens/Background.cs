using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame;

public class Background 
{
    public Texture2D background;
    private List<Texture2D> frames = new();
    private int currentFrame = 0;
    private double timeElapsed = 0;
    private double frameDuration = 50;
    private int bgcolorDelay = 0;
    private float transitionProgress = 0f; // Tracks progress from 0 to 1
    private const float transitionSpeed = 2f; // Adjust for faster/slower transitions
    public void LoadContent()
    {
        background = LoadTexture("assets/backgrounds/mainbg");
        for (int i = 1; i < 28; i++)
        {
            frames.Add(LoadTexture("assets/backgrounds/menubg" + i));
        }
    }

    public void Update()
    {
        // Update animation frame
        timeElapsed += GetFrameTime();

        if (timeElapsed >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % frames.Count; // Loop through frames
            timeElapsed = 0;
        }

        if(bgcolorDelay > 0)
        {
            bgcolorDelay--;
        }

        if (Game.inputManager.PConfirm)
        {
            bgcolorDelay = 20;
        }
    }
    public void DrawBG()
    {
        BeginDrawing();
        if (Game.currentGameState == Game.GameState.Playing)
        {
            DrawTexture(background, 0, 0, Color.White);
        } else if (Game.currentGameState != Game.GameState.Playing)
        {
            DrawTexture(frames[currentFrame], 0, 0, BGColor());
        }
        EndDrawing();
    }

    public Color BGColor()
    {
        float elapsedSeconds = GetFrameTime();
        if (Game.currentGameState == Game.GameState.Quit)
        {
            transitionProgress = MathF.Min(transitionProgress + elapsedSeconds * transitionSpeed, 1f);
        }
        else
        {
            transitionProgress = MathF.Max(transitionProgress - elapsedSeconds * transitionSpeed, 0f);
        }
        return ColorLerp(Color.White, new Color(255, 153, 153), transitionProgress);
    }
}

