using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NetLibrary;

namespace WalkAndTalk.Engine
{
    public class PeerPlayer : Character
    {   
        private int mUniqueId;

        public bool WillContinueMoving
        {
            get { return mWillContinueMoving; }
            set { mWillContinueMoving = value; }
        }
        bool mWillContinueMoving = false;

        public bool IsRunning
        {
            get { return mIsRunning; }
            set { mIsRunning = value; }
        }
        bool mIsRunning = false;

        public PeerPlayer(Texture2D spriteSheet, Vector2 coordinates, World world, int unique) : base(spriteSheet, coordinates)
        {
            Name = "Buddy"; //change to get from online
            mUniqueId = unique;
        }

        public void Draw(SpriteBatch spriteBatch, World world, GameTime gameTime)
        {
            float positionX;
            float positionY;

            world.Camera.setNPCPosition(DrawnPosition, out positionX, out positionY);

            base.Draw(spriteBatch, positionX, positionY, world.Map, gameTime);
        }

        public void Update(GameTime gameTime, Vector2 movement)
        {            
            HandleMovement(movement);
        }

        private void HandleMovement(Vector2 movement)
        {
            Vector2 position = Coordinates;
            if (movement.X == 1)
            {
                position.X += 1;
                Direction = Character.FacingDirection.Right;
            }
            else if (movement.X == -1)
            {
                position.X -= 1;
                Direction = Character.FacingDirection.Left;
            }
            else if (movement.Y == 1)
            {
                position.Y += 1;
                Direction = Character.FacingDirection.Down;
            }
            else if (movement.Y == -1)
            {
                position.Y -= 1;
                Direction = Character.FacingDirection.Up;
            }

            Character.ActionState action = Character.ActionState.Walk;
            if (mIsRunning)
                action = Character.ActionState.Run;

            Coordinates = position;
            SetMovement(action);
        }

        protected override bool IsStillMoving()
        {
            return mWillContinueMoving;
        }
    }
}
