using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WalkAndTalk.Engine.Net;
using NetLibrary;

namespace WalkAndTalk.Engine
{
    public class PlayerControls
    {
        Player mPlayer;

        World mWorld;

        protected List<Keys> mMovementKeys;

        public PlayerControls(Player player, World world)
        {
            mPlayer = player;
            mWorld = world;
            mMovementKeys = new List<Keys>();
            mMovementKeys.Add(Keys.Up);
            mMovementKeys.Add(Keys.Down);
            mMovementKeys.Add(Keys.Left);
            mMovementKeys.Add(Keys.Right);
        }

        public void Update(GameTime gameTime)
        {
            HandleUserInput(gameTime);
        }


        private void HandleUserInput(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            bool running = keyState.IsKeyDown(Keys.LeftShift);

            foreach (Keys key in mMovementKeys)
            {
                HandleMovement(key, gameTime, running);
            }
            //DEBUG BELOW
            if (keyState.IsKeyDown(Keys.E) && GameLauncher.IsOnline)
            {
                Console.WriteLine(GameLauncher.GameNetClient.PlayerList.Count);
                foreach (var player in GameLauncher.GameNetClient.PlayerList)
                {
                    Console.WriteLine(player.Value.Name + " with unique id " + player.Value.UniqueId + " at position: " + player.Value.X + ", " + player.Value.Y);
                }
            }
        }

        private void HandleMovement(Keys key, GameTime gameTime, bool isRunning)
        {
            if (!mPlayer.IsIdle())
            {
                return;
            }
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(key))
            {
                Vector2 position = mPlayer.Coordinates;
                switch (key)
                {
                    case Keys.Up:
                        position.Y -= 1;
                        mPlayer.Direction = Character.FacingDirection.Up;
                        break;
                    case Keys.Down:
                        position.Y += 1;
                        mPlayer.Direction = Character.FacingDirection.Down;
                        break;
                    case Keys.Left:
                        position.X -= 1;
                        mPlayer.Direction = Character.FacingDirection.Left;
                        break;
                    case Keys.Right:
                        mPlayer.Direction = Character.FacingDirection.Right;
                        position.X += 1;
                        break;
                }

                if (mWorld.CollisionCheck(position))
                {
                    Character.ActionState action = Character.ActionState.Walk;
                    if (isRunning)
                        action = Character.ActionState.Run;

                    mPlayer.Coordinates = position;
                    mPlayer.SetMovement(action);

                    if(GameLauncher.IsOnline)
                        GameLauncher.GameNetClient.SendToServer(mPlayer.Coordinates, isRunning);
                }
            }
        }

        public bool IsMovementKeyDown()
        {
            foreach (Keys key in mMovementKeys)
            {
                bool b = Keyboard.GetState().IsKeyDown(key);
                if (b) return b;
            }
            return false;
        }

        private NetLibrary.PlayerPrimitive.MoveDirection GetNetDirection(Character.FacingDirection direction) 
        {
            switch (direction)
            {
                case Character.FacingDirection.Down :
                    return NetLibrary.PlayerPrimitive.MoveDirection.Down;
                case Character.FacingDirection.Left :
                    return NetLibrary.PlayerPrimitive.MoveDirection.Left;
                case Character.FacingDirection.Right :
                    return NetLibrary.PlayerPrimitive.MoveDirection.Right;
                case Character.FacingDirection.Up :
                    return NetLibrary.PlayerPrimitive.MoveDirection.Up;
            }

            return NetLibrary.PlayerPrimitive.MoveDirection.Null;
        }

    }
}
