using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetLibrary;

namespace PokemonGameServer
{
    public class NPC : Character
    {
        int mMovementRange = 0;
        Vector2 mDisplacement = Vector2.Zero;

        const float TimeBetweenStepsMin = 4;

        float mTimeSinceLastMove;

        #region Constructors

        public NPC(WorldMap worldMap, Vector2 coords, int range)
            :base(worldMap, coords)
        {
            mMovementRange = range;
        }

        //This should be in the player thing not NPC
        public static NPC CreateFromPrimitive(WorldMap map, PlayerPrimitive prim, int range) 
        {
            NPC npc = new NPC(map, new Vector2(prim.X, prim.Y), range);
            npc.Name = prim.Name;
            return npc;
        }

        #endregion

        public override void Update(TimeSpan time)
        {
            if (mMovementRange > 0)
            {
                //Console.WriteLine(mTimeSinceLastMove + "<- time since; timebetween ->" + TimeBetweenStepsMin);
                if (mTimeSinceLastMove >= TimeBetweenStepsMin)
                {
                    float moveProbability = (mTimeSinceLastMove - TimeBetweenStepsMin) * 10;
                    if (moveProbability >= GameServer.Random.Next(101))
                    {
                        AssignCharacterDirectionAndPosition();
                        mTimeSinceLastMove = 0;
                    }
                }
                mTimeSinceLastMove += (float)time.TotalSeconds;
            }
        }

        private void AssignCharacterDirectionAndPosition()
        {
            List<FacingDirection> moveOptions;
            SetMovementOptions(out moveOptions);
            FacingDirection direction = moveOptions[GameServer.Random.Next(moveOptions.Count)];
            HandleMovement(direction, false);
        }

        private void SetMovementOptions(out List<FacingDirection> moveOptions)
        {
            moveOptions = FacingDirection.GetValues(typeof(FacingDirection)).
                Cast<FacingDirection>().ToList<FacingDirection>();
            moveOptions.Remove(FacingDirection.Null);

            if (mDisplacement.X == mMovementRange)
            {
                moveOptions.Remove(FacingDirection.Right);
            }
            else if (mDisplacement.X == -mMovementRange)
            {
                moveOptions.Remove(FacingDirection.Left);
            }

            if (mDisplacement.Y == mMovementRange)
            {
                moveOptions.Remove(FacingDirection.Down);
            }
            else if (mDisplacement.Y == -mMovementRange)
            {
                moveOptions.Remove(FacingDirection.Up);
            }
        }

        private void HandleMovement(FacingDirection direction, bool isRunning)
        {
            Vector2 position = Coordinates;
            Direction = direction;
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

            if (/*CurrentWorld.CollisionCheck(position)*/true)
            {
                Coordinates = position;
                mDisplacement = displacement;
            }
        }

        //Converts NPC to Primitive--Passable through network
        public PlayerPrimitive toPrimitive()
        {
            PlayerPrimitive prim = new PlayerPrimitive();
            prim.X = (int)Coordinates.X;
            prim.Y = (int)Coordinates.Y;
            prim.IsRunning = false;
            prim.Name = "NPC";
            prim.UniqueId = 0;
            return prim;
        }

    }
}
