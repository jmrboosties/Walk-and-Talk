using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonGameServer
{
    public abstract class Character
    {
        const int Multiplier = 2;

        protected String Name
        {
            get { return mName; }
            set { mName = value; }
        }
        String mName;

        public Vector2 Coordinates
        {
            get { return mCoordinates; }
            set { mCoordinates = value; }
        }
        Vector2 mCoordinates;

        public enum ActionState
        {
            Idle, Walk, Run
        }

        public ActionState State
        {
            get { return mState; }
        }
        ActionState mState;

        public enum FacingDirection
        {
            Down, Right, Left, Up, Null
        }

        public FacingDirection Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }
        FacingDirection mDirection;

        public WorldMap CurrentWorld
        {
            get { return mWorldMap; }
        }
        WorldMap mWorldMap;

        #region Constructors

        public Character(WorldMap worldMap, Vector2 coordinates)
        {
            mCoordinates = coordinates;
            mState = ActionState.Idle;
            mDirection = FacingDirection.Down;
            mWorldMap = worldMap;
        }

        #endregion

        public virtual void Update(TimeSpan time) { }
    }
}
