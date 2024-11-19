using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame;

public class Canvas
{
    private readonly RenderTarget2D target;
    private Rectangle destinationRectangle;

    public Canvas(int width, int height)
    {
        target = new(Globals.GraphicsDevice, width, height);
    }

    public void SetDestinationRectangle()
    {
        var screenSize = Globals.GraphicsDevice.PresentationParameters.Bounds;

        float scaleX = (float)screenSize.Width / target.Width;
        float scaleY = (float)screenSize.Height / target.Height;
        float scale = Math.Min(scaleX, scaleY);

        int newWidth = (int)(target.Width * scale);
        int newHeight = (int)(target.Height * scale);

        int posX = (screenSize.Width - newWidth) / 2;
        int posY = (screenSize.Height - newHeight) / 2;

        destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
    }

    public void Activate()
    {
        Globals.GraphicsDevice.SetRenderTarget(target);
        Globals.GraphicsDevice.Clear(Color.DarkGray);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Globals.GraphicsDevice.SetRenderTarget(null);
        Globals.GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        spriteBatch.Draw(target, destinationRectangle, Color.White);
        spriteBatch.End();
    }
}