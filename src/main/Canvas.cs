using Raylib_cs;
using static Raylib_cs.Raylib;
namespace BlobGame;

public class Canvas
{
    private readonly RenderTexture2D target;
    private Rectangle destinationRectangle;

    public Canvas(int width, int height)
    {
        target = LoadRenderTexture(width, height);
    }

    public void SetDestinationRectangle()
    {
    // Get the screen width and height
    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Calculate the scale factor for both width and height
    float scaleX = (float)screenWidth / target.Texture.Width;
    float scaleY = (float)screenHeight / target.Texture.Height;
    float scale = Math.Min(scaleX, scaleY); // Choose the smaller scale factor to maintain aspect ratio

    // Calculate the new width and height after scaling
    int newWidth = (int)(target.Texture.Width * scale);
    int newHeight = (int)(target.Texture.Height * scale);

    // Calculate the position to center the canvas
    int posX = (screenWidth - newWidth) / 2;
    int posY = (screenHeight - newHeight) / 2;

    // Set the destination rectangle
    destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
    }


    public void Activate()
    {
        BeginTextureMode(target);
        ClearBackground(Color.SkyBlue);
    }

    public void Draw()
    {
        EndTextureMode();
        ClearBackground(Color.SkyBlue);
        BeginDrawing();
        DrawTexture(target.Texture, (int)destinationRectangle.X, (int)destinationRectangle.Y, Color.White);
        EndDrawing();
    }
}