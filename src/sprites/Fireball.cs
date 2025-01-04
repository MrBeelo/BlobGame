using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace BlobGame
{
    public class Fireball : CollMoveableSprite
    {
        int fireCounter;
        int fireActiveFrame;
        public static Texture2D[] fireTextures;
        private static Sound explosionSound;
        private static Sound laserShootSound;
        public bool alive = true;
        public bool bad = false;

        public Fireball(Texture2D texture, Rectangle drect, Rectangle srect, bool fireIsLeft, bool fireBad) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
            isLeft = fireIsLeft;
            bad = fireBad;
        }

        public override void LoadContent(Game game)
        {
            explosionSound = LoadSound("assets/sounds/explosion.wav");
            laserShootSound = LoadSound("assets/sounds/laserShoot.wav");

            fireTextures = new Texture2D[3];

            fireTextures[0] = LoadTexture("assets/sprites/fireball/Fireball1.png");
            fireTextures[1] = LoadTexture("assets/sprites/fireball/Fireball2.png");
            fireTextures[2] = LoadTexture("assets/sprites/fireball/Fireball3.png");
        }

        public override void Update()
        {
            base.Update();

            fireCounter++;
            if(fireCounter > 14)
            {
                fireCounter = 0;
                fireActiveFrame++;

                if(fireActiveFrame > fireTextures.Length - 1)
                {
                    fireActiveFrame = 0;
                }
            }

            if(isLeft)
            {
                Velocity.X = -5;
            } else if(!isLeft)
            {
                Velocity.X = 5;
            }

            Drect.X += (int)Velocity.X;
            horizontalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 1 || value == 4 || value == 5 || value >= 8)
                    {
                        //! Do nothing
                    } else if(value == 6)
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.excludedNormalTiles.Add(new Vector3(29, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            alive = false;
                            PlaySound(explosionSound);
                            SetSoundVolume(explosionSound, (float)Game.LoweredVolume);
                        }
                    } else {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left() - Drect.Width;
                            Velocity.X = 0;
                            alive = false;
                            PlaySound(explosionSound);
                            SetSoundVolume(explosionSound, (float)Game.LoweredVolume);
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right();
                            Velocity.X = 0;
                            alive = false;
                            PlaySound(explosionSound);
                            SetSoundVolume(explosionSound, (float)Game.LoweredVolume);
                        }      
                    }
                }
            }
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in verticalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 1 || value == 4 || value == 5 || value >= 8)
                    {
                        //! Do nothing
                    } else if(value == 6)
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.excludedNormalTiles.Add(new Vector3(29, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            Die();
                        }
                    } else {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            Drect.Y = collision.Top() - Drect.Height;
                            Velocity.Y = 0;
                            Die();
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom();
                            Velocity.Y = 0;
                            Die();
                        }
                    }
                }
            }

            if (Drect.Intersects(Game.player.Drect) && Game.player.Immunity == 0 && !Game.player.Immune && bad)
            {
                Player.Damage(10);
                alive = false;
            }

            if(Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                alive = false;
            }

            if(!alive)
            {
                Game.fireballs.Remove(this);
                Game.sprites.Remove(this);
            }
        }

        public override void Draw()
        {
            DrawTexturePro(fireTextures[fireActiveFrame], Srect, Drect, Vector2.Zero, 0f, Color.White);

            if (Game.hasF3On)
            {
                foreach (var rect in horizontalCollisions)
                {
                    DrawRectangleLinesEx(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                foreach (var rect in verticalCollisions)
                {
                    DrawRectangleLinesEx(new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                DrawRectangleLinesEx(Drect, 4, Color.Blue);
            }
        }

        public static void Fire(Rectangle drect, bool isLeft)
        {
            Fireball fireball = new Fireball(fireTextures[1], new Rectangle(drect.Center().X, new Random().Next((int)(drect.Top() + 1), (int)(drect.Bottom() - 32 - 1)), 32, 32), new Rectangle(0, 0, 16, 16), isLeft, false);
            Game.fireballs.Add(fireball);
            PlaySound(laserShootSound);
            SetSoundVolume(laserShootSound, (float)Game.LoweredVolume);
        }

        public static void FireBad(Rectangle drect, bool isLeft)
        {
            Fireball fireball = new Fireball(fireTextures[1], new Rectangle(drect.Center().X, new Random().Next((int)(drect.Top() + 1), (int)(drect.Bottom() - 48 - 1)), 48, 48), new Rectangle(0, 0, 16, 16), isLeft, true);
            Game.fireballs.Add(fireball);
            PlaySound(laserShootSound);
            SetSoundVolume(laserShootSound, (float)Game.LoweredVolume);
        }

        public void Die()
        {
            alive = false;
            PlaySound(explosionSound);
            SetSoundVolume(explosionSound, (float)Game.LoweredVolume);
        }

        public static void ClearAll()
        {
            Game.fireballs.Clear();
        }

        public override string[] GetDebugInfo()
        {
            return new string[] 
            {
                "---FIREBALL---",
                "Velocity: " + Velocity,
                "Is Alive: " + alive,
            };
        }
    }
}