using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        background = Main.main.Content.Load<Texture2D>("assets/backgrounds/mainbg");
        for (int i = 1; i < 28; i++)
        {
            frames.Add(Main.main.Content.Load<Texture2D>("assets/backgrounds/menubg" + i));
        }
    }

    public void Update(GameTime gameTime)
    {
        // Update animation frame
        timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (timeElapsed >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % frames.Count; // Loop through frames
            timeElapsed = 0;
        }

        if(bgcolorDelay > 0)
        {
            bgcolorDelay--;
        }

        if (Main.inputManager.PConfirm)
        {
            bgcolorDelay = 20;
        }
    }
    public void DrawBG(GameTime gameTime)
    {
        Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if (Main.currentGameState == Main.GameState.Playing)
        {
            Globals.SpriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);
        } else if (Main.currentGameState != Main.GameState.Playing)
        {
            Globals.SpriteBatch.Draw(frames[currentFrame], Vector2.Zero, BGColor(gameTime));
        }
        Globals.SpriteBatch.End();
    }

    public Color BGColor(GameTime gameTime)
    {
        float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Main.currentGameState == Main.GameState.Quit)
        {
            transitionProgress = MathF.Min(transitionProgress + elapsedSeconds * transitionSpeed, 1f);
        }
        else
        {
            transitionProgress = MathF.Max(transitionProgress - elapsedSeconds * transitionSpeed, 0f);
        }
        return Color.Lerp(Color.White, Color.LightSalmon, transitionProgress);
    }
}

