using System.Numerics;
using Raylib_cs;
using Sys = System.Drawing;

public static class Extensions
{
    public static float Left(this Rectangle rect) => rect.X;
    public static float Right(this Rectangle rect) => rect.X + rect.Width;
    public static float Top(this Rectangle rect) => rect.Y;
    public static float Bottom(this Rectangle rect) => rect.Y + rect.Height;
    public static Vector2 Center(this Rectangle rect)
    {
        float centerX = rect.X + rect.Width / 2f;
        float centerY = rect.Y + rect.Height / 2f;
        return new Vector2(centerX, centerY);
    }
    public static bool Intersects(this Rectangle a, Rectangle b)
    {
        return a.X < b.X + b.Width && 
           a.X + a.Width > b.X && 
           a.Y < b.Y + b.Height && 
           a.Y + a.Height > b.Y;
    }

    public static Sys.Point Clamp(this Sys.Point point, Sys.Point min, Sys.Point max)
    {
        int clampedX = Math.Clamp(point.X, min.X, max.X);
        int clampedY = Math.Clamp(point.Y, min.Y, max.Y);
        return new Sys.Point(clampedX, clampedY);
    }

    public static Vector2 ToVector2(this Sys.Point point)
    {
        return new Vector2(point.X, point.Y);
    }

    public static Sys.Point ToPoint(this Vector2 vector2)
    {
        return new Sys.Point((int)vector2.X, (int)vector2.Y);
    }

}
