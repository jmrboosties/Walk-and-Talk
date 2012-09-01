using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WalkAndTalk.Engine
{
    public class Camera
    {

        World mWorld;

        int TilesCountX;
        int TilesCountY;
        
        Vector2 mCameraOffset;

        const float Multiplier = 2;

        public Camera(World world)
        {
            mWorld = world;
            TilesCountX = mWorld.Map.TilesCountX;
            TilesCountY = mWorld.Map.TilesCountY;
        }

        public void Update(Vector2 playerPosition)
        {
            float cameraAdjustX = 0;
            float cameraAdjustY = 0;
            if (playerPosition.X > 13)
                cameraAdjustX = playerPosition.X - 13;
            if (playerPosition.X + 13 > TilesCountX - 1)
                cameraAdjustX += -(playerPosition.X + 13 - (TilesCountX - 1));

            if (playerPosition.Y > 7)
                cameraAdjustY = playerPosition.Y - 7;
            if (playerPosition.Y + 7 > TilesCountY - 1)
                cameraAdjustY += -(playerPosition.Y + 7 - (TilesCountY - 1));

            mCameraOffset.X = cameraAdjustX;
            mCameraOffset.Y = cameraAdjustY;
        }
 
        public void SetVisibleMap(Vector2 playerPosition, out int xFactor, out int yFactor, int x, int y)
        {
            xFactor = (int)((x - mCameraOffset.X) * mWorld.Map.TileWidth * Multiplier);
            yFactor = (int)((y - mCameraOffset.Y) * mWorld.Map.TileHeight * Multiplier);
        }

        public void setNPCPosition(Vector2 npcPosition, out float positionX, out float positionY)
        {
            positionX = (npcPosition.X - mCameraOffset.X);
            positionY = (npcPosition.Y - mCameraOffset.Y);
        }

        public void SetPlayerPosition(Vector2 preadjustedPosition, out float positionX, out float positionY)
        {
            positionX = 13;
            positionY = 7;
            
            if (TilesCountX < 27 || preadjustedPosition.X < 13)
                positionX = preadjustedPosition.X;
            if (preadjustedPosition.X > TilesCountX - 14)
                positionX = 27 - (TilesCountX - preadjustedPosition.X);

            if (TilesCountY < 15 || preadjustedPosition.Y < 7)
                positionY = preadjustedPosition.Y;
            if (preadjustedPosition.Y > TilesCountY - 8)
                positionY = 15 - (TilesCountY - preadjustedPosition.Y);
        }
 
    }
}
