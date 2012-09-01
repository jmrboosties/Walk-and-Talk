using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WalkAndTalk.Engine
{
    public class NPCBrain
    {
        int mMovementRange = 0;
        Vector2 mDisplacement = Vector2.Zero;

        const float TimeBetweenStepsMin = 4;

        float mTimeSinceLastMove;

        Character mCharacter;
        World mCurrentWorld;

        public NPCBrain(Character npc, World world, int range)
        {
            mCharacter = npc;
            mCurrentWorld = world;
            mMovementRange = range;
        }

        public void Update(GameTime gameTime)
        {
            if (mMovementRange > 0)
            {
                if (mTimeSinceLastMove >= TimeBetweenStepsMin)
                {
                    float moveProbability = (mTimeSinceLastMove - TimeBetweenStepsMin) / 10;
                    if (moveProbability >= GameLauncher.Random.Next(101))
                    {
                        AssignCharacterDirectionAndPosition();
                        mTimeSinceLastMove = 0;
                    }
                }
                mTimeSinceLastMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void AssignCharacterDirectionAndPosition()
        {
            List<Character.FacingDirection> moveOptions;
            SetMovementOptions(out moveOptions);
            Character.FacingDirection direction = moveOptions[GameLauncher.Random.Next(moveOptions.Count)];
            HandleMovement(direction, false);
        }

        private void HandleMovement(Character.FacingDirection direction, bool isRunning)
        {
            if (!mCharacter.IsIdle())
                return;

            Vector2 position = mCharacter.Coordinates;
            mCharacter.Direction = direction;
            Vector2 displacement = mDisplacement;
            switch (direction)
            {
                case Character.FacingDirection.Up:
                    position.Y -= 1;
                    displacement.Y -= 1;
                    break;
                case Character.FacingDirection.Down:
                    position.Y += 1;
                    displacement.Y += 1;
                    break;
                case Character.FacingDirection.Left:
                    position.X -= 1;
                    displacement.X -= 1;
                    break;
                case Character.FacingDirection.Right:
                    position.X += 1;
                    displacement.X += 1;
                    break;
            }

            if (mCurrentWorld.CollisionCheck(position))
            {
                Character.ActionState action = Character.ActionState.Walk;
                if (isRunning)
                    action = Character.ActionState.Run;

                mCharacter.Coordinates = position;
                mDisplacement = displacement;
                mCharacter.SetMovement(action);
            }
        }

        private void SetMovementOptions(out List<Character.FacingDirection> moveOptions)
        {
            moveOptions = Character.FacingDirection.GetValues(typeof(Character.FacingDirection)).
                Cast<Character.FacingDirection>().ToList<Character.FacingDirection>();
            moveOptions.Remove(Character.FacingDirection.Null);

            if (mDisplacement.X == mMovementRange)
            {
                moveOptions.Remove(Character.FacingDirection.Right);
            }
            else if (mDisplacement.X == -mMovementRange)
            {
                moveOptions.Remove(Character.FacingDirection.Left);
            }

            if (mDisplacement.Y == mMovementRange)
            {
                moveOptions.Remove(Character.FacingDirection.Down);
            }
            else if (mDisplacement.Y == -mMovementRange)
            {
                moveOptions.Remove(Character.FacingDirection.Up);
            }
        }

        public bool IsStillMoving()
        {
            return false;
        }

    }
}
