using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetLibrary
{
    public class WorldPrimitive
    {
        private WorldMapPrimitive mMap;

        private Dictionary<Vector2, bool> mCollisionSet;

        public WorldPrimitive(WorldMapPrimitive map) 
        {
            mMap = map;
            PopulateCollisionSet();
        }

        private void PopulateCollisionSet()
        {
            mCollisionSet = new Dictionary<Vector2, bool>();
        }

        public bool CanPlayerMoveHere(Vector2 coords)
        {
            return true;
            //bool b = false;
            //mCollisionSet.TryGetValue(coords, out b);
            //return b;
        }
    }
}
