using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame
{
    public class MoveableSprite : Sprite
    {
        public Vector2 Velocity =  Vector2.Zero;
        public bool isMoving = false;
        public bool isLeft = false;
        public DirectionEnum Direction = DirectionEnum.NA;
        public enum DirectionEnum 
        {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NA}

        public MoveableSprite(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
        }

        public new virtual void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(Velocity.Y > 0.5 && Velocity.X > 0)
            {
                Direction = DirectionEnum.DownRight;
            }
            else if(Velocity.Y > 0.5 && Velocity.X < 0)
            {
                Direction = DirectionEnum.DownLeft;
            }
            else if(Velocity.Y < 0 && Velocity.X > 0)
            {
                Direction = DirectionEnum.UpRight;
            }
            else if(Velocity.Y < 0 && Velocity.X < 0)
            {
                Direction = DirectionEnum.UpLeft;
            }
            else if(Velocity.X > 0)
            {
                Direction = DirectionEnum.Right;
            }
            else if(Velocity.X < 0)
            {
                Direction = DirectionEnum.Left;
            }
            else if(Velocity.Y > 0.5)
            {
                Direction = DirectionEnum.Down;
            }
            else if(Velocity.Y < 0)
            {
                Direction = DirectionEnum.Up;
            }
            else if(Velocity.X == 0 && Velocity.Y == 0.5)
            {
                Direction = DirectionEnum.NA;
            }

            isMoving = Velocity.X != 0;

            if(Velocity.X < 0)
            {
                isLeft = true;
            } else if(Velocity.X > 0)
            {
                isLeft = false;
            }

        }
    }
}