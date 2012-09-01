using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WalkAndTalk.Engine
{
    public class Player : Character
    {
        PlayerControls mPlayerControls;

        public Player(Texture2D spriteSheet, Vector2 coordinates, World world) : base(spriteSheet, coordinates)
        {
            mPlayerControls = new PlayerControls(this, world);
            Name = "JMRboosties";
        }
        
        public void Draw(SpriteBatch spriteBatch, World world, GameTime gameTime)
        {
            float positionX = 13;
            float positionY = 7;

            world.Camera.SetPlayerPosition(DrawnPosition, out positionX, out positionY);

            base.Draw(spriteBatch, positionX, positionY, world.Map, gameTime);
        }

        protected override bool IsStillMoving()
        {
            return mPlayerControls.IsMovementKeyDown();
        }

        public override void Update(GameTime gameTime)
        {
            mPlayerControls.Update(gameTime);
        }

    }
}
