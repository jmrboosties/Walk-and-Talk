using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WalkAndTalk.Engine
{
    public class Tile
    {
        public Texture2D Texture;
        public Rectangle SourceRectangle;
        public SpriteEffects SpriteEffects;
    }

    public class Layer
    {
        public int Width;
        public int Height;
        public Tile[] Tiles;
        public bool Collisions;
        public bool Foreground;
    }

    public class WorldMap
    {
        public int TileWidth;
        public int TileHeight;
        public int TilesCountX;
        public int TilesCountY;
        public List<Layer> Layers = new List<Layer>();

        const float Multiplier = 2f;

        public enum MovementDirection
        {
            Horizontal, Verical
        }

        public void Draw(SpriteBatch spriteBatch, bool foreground, Vector2 position, Camera camera)
        {
            foreach (var l in Layers)
            {
                if (l.Foreground == foreground)
                {

                    for (int y = 0; y < TilesCountY; y++)
                    {
                        for (int x = 0; x < TilesCountX; x++)
                        {                            
                            Tile t = l.Tiles[y * l.Width + x];

                            int xFactor;
                            int yFactor;

                            camera.SetVisibleMap(position, out xFactor, out yFactor, x, y);

                            if (t.Texture != null)
                            {
                                spriteBatch.Draw(t.Texture, new Rectangle((int)(xFactor), (int)(yFactor), (int)(TileWidth * Multiplier), (int)(TileHeight * Multiplier)),
                                t.SourceRectangle, Color.White);
                            }
                        }
                    }
                }
            }
        }

        public bool FreeMovement(Vector2 position, MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Horizontal :
                    if (TilesCountX < 27 || position.X < 13)
                        return true;
                    break;
                case MovementDirection.Verical :
                    if (TilesCountY < 15 || position.Y < 7)
                        return true;
                    break;
            }

            return false;
        }

    }
}
