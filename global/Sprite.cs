
using BlobGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame
{
    public class Sprite
    {
        public Texture2D Texture;
        public Rectangle Drect;
        public Rectangle Srect;
        public GraphicsDeviceManager Graphics {get; set;}

        public Sprite(Texture2D texture, Rectangle drect, Rectangle srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
        }
        public virtual void LoadContent(Game game) {}
        public virtual void Update(GameTime gameTime) {}

        public virtual void Draw(SpriteBatch spriteBatch) {}
    }
}