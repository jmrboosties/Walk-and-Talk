using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WalkAndTalk.Engine
{
    public class NPC : Character
    {

        NPCBrain mBrain;

        public NPC(Texture2D spriteSheet, Vector2 coordinates, World world, int range) : base(spriteSheet, coordinates) 
        {
            mBrain = new NPCBrain(this, world, range);
        }

        public void Draw(SpriteBatch spriteBatch, World world, GameTime gameTime)
        {
            float positionX;
            float positionY;

            world.Camera.setNPCPosition(DrawnPosition, out positionX, out positionY);

            base.Draw(spriteBatch, positionX, positionY, world.Map, gameTime);
        }

        protected override bool IsStillMoving()
        {
            return mBrain.IsStillMoving();
        }

        public override void Update(GameTime gameTime)
        {
            mBrain.Update(gameTime);
        }
    }
}
