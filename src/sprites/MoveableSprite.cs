using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame
{
    public class MoveableSprite : Sprite
    {
        public Vector2 Velocity =  Vector2.Zero;

        public MoveableSprite(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
        }
    }
}