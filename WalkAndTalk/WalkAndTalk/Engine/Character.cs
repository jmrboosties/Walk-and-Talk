using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WalkAndTalk.Engine
{

    public abstract class Character
    {
        const int Multiplier = 2;

        Texture2D mCharacterSheet;

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

        public Vector2 DrawnPosition
        {
            get { return mDrawnPosition; }
        }
        Vector2 mDrawnPosition;

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

        enum StepState
        {
            StepOne, Idle, StepTwo, Null
        }
        StepState mStepState;

        float mMoveProgress = -1;
        const float WalkAnimationTime = .5f;
        const float RunAnimationTime = .25f;

        const int CharacterHeight = 21;
        const int CharacterWidth = 16;

        public Character(Texture2D spriteSheet, Vector2 coordinates)
        {
            mCharacterSheet = spriteSheet;
            mCoordinates = coordinates;
            SetDrawnPosition(0, 0);
            mState = ActionState.Idle;
            mDirection = FacingDirection.Down;
            mStepState = StepState.Idle;
        }

        protected void Draw(SpriteBatch spriteBatch, float positionX, float positionY, WorldMap map, GameTime gameTime)
        {
            switch (mState)
            {
                case ActionState.Walk:
                case ActionState.Run:

                    float time = 0;
                    switch (mState)
                    {
                        case ActionState.Run:
                            time = RunAnimationTime;
                            break;
                        case ActionState.Walk:
                            time = WalkAnimationTime;
                            break;
                    }

                    mMoveProgress += (float)(gameTime.ElapsedGameTime.TotalSeconds);
                    if (mMoveProgress <= time / 3)
                    {
                        mStepState = StepState.StepOne;
                    }
                    else if (mMoveProgress <= 2 * time / 3)
                    {
                        mStepState = StepState.Idle;
                    }
                    else if (mMoveProgress < time)
                    {
                        mStepState = StepState.StepTwo;
                    }
                    if (mMoveProgress >= time)
                    {
                        mMoveProgress = -1;
                        mState = ActionState.Idle;
                        if (!IsStillMoving())
                        {
                            mStepState = StepState.Idle;
                        }
                    }

                    float movementAdjustX = 0;
                    float movementAdjustY = 0;
                    if (mMoveProgress >= 0)
                    {
                        float animationTime = 0;
                        switch (mState)
                        {
                            case ActionState.Run:
                                animationTime = RunAnimationTime;
                                break;
                            case ActionState.Walk:
                                animationTime = WalkAnimationTime;
                                break;
                        }

                        switch (mDirection)
                        {
                            case FacingDirection.Left:
                                movementAdjustX = ((1 - (mMoveProgress / animationTime)));
                                break;
                            case FacingDirection.Right:
                                movementAdjustX = -((1 - (mMoveProgress / animationTime)));
                                break;
                            case FacingDirection.Up:
                                movementAdjustY = ((1 - (mMoveProgress / animationTime)));
                                break;
                            case FacingDirection.Down:
                                movementAdjustY = -((1 - (mMoveProgress / animationTime)));
                                break;
                        }
                    }

                    SetDrawnPosition(movementAdjustX, movementAdjustY);

                    break;
            }

            spriteBatch.Draw(mCharacterSheet,
                new Rectangle((int)(positionX * map.TileWidth * Multiplier),
                    (int)(positionY * map.TileHeight * Multiplier - 5 * Multiplier),
                    (int)(CharacterWidth * Multiplier),
                    (int)((CharacterHeight) * Multiplier)),
                new Rectangle((int)(mStepState) * CharacterWidth, (int)(mDirection) * CharacterHeight, CharacterWidth, CharacterHeight),
                Color.White, 0, Vector2.Zero, SpriteEffects.None, (float)(mCoordinates.Y / map.TilesCountY));
            
            if (mName != null)
            {
                Vector2 textPos = new Vector2((int)(positionX * 2 * map.TileWidth + (16 - (ScreenManager.SmallFont.MeasureString(Name).X / 2))), (int)(positionY * 2 * map.TileWidth - 32));

                spriteBatch.DrawString(ScreenManager.SmallFont, Name, textPos, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

        }

        public virtual void Update(GameTime gameTime) { }

        protected abstract bool IsStillMoving();
        
        private void SetDrawnPosition(float movementAdjustX, float movementAdjustY)
        {
            mDrawnPosition.X = (mCoordinates.X + movementAdjustX);
            mDrawnPosition.Y = (mCoordinates.Y + movementAdjustY);
        }

        public bool IsIdle()
        {
            return mState == ActionState.Idle;
        }

        public void SetMovement(ActionState actionState)
        {
            mState = actionState;

            if (mState != ActionState.Idle)
            {
                mMoveProgress = 0;
            }
        }

    }
}
