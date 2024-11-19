using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame;

public class Canvas
{
    private GraphicsDevice GraphicsDevice;
    private readonly RenderTarget2D target;
    private Rectangle destinationRectangle;

    public Canvas(GraphicsDevice graphicsDevice, int width, int height)
    {
        GraphicsDevice = graphicsDevice;
        target = new(GraphicsDevice, width, height);
    }

    public void SetDestinationRectangle()
    {
        var screenSize = GraphicsDevice.PresentationParameters.Bounds;

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
        GraphicsDevice.SetRenderTarget(target);
        GraphicsDevice.Clear(Color.CornflowerBlue);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        spriteBatch.Draw(target, destinationRectangle, Color.White);
        spriteBatch.End();
    }
}