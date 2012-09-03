using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonGameServer
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

        public bool CollisionCheck(Vector2 position)
        {
            /*foreach (Layer layer in Layers)
            {
                if (layer.Collisions)
                {
                    return layer.Tiles[(int)(position.Y * layer.Width + position.X)].Texture == null;
                }
            }*/
            return true;
        }

    }
}
