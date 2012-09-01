using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace WalkAndTalk
{

    public enum ScreenState
    {
        TransitionOn, Active, TransitionOff, Hidden,
    }

    public enum ScreenType 
    {
        PlayerFixed, WorldFixed, Menu,
    }

    public abstract class GameScreen
    {

        #region Properties

        public bool IsPopup
        {
            get { return mIsPopup; }
            protected set { mIsPopup = value; }
        }
        bool mIsPopup = false;

        public TimeSpan TransitionOnTime
        {
            get { return mTransitionOnTime; }
            protected set { mTransitionOnTime = value; }
        }
        TimeSpan mTransitionOnTime = TimeSpan.Zero;

        public TimeSpan TransitionOffTime
        {
            get { return mTransitionOffTime; }
            protected set { mTransitionOffTime = value; }
        }
        TimeSpan mTransitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Transition Progress 0-1, 0 being fully active, 1 being fully hidden.
        /// </summary>
        public float TransitionProgress
        {
            get { return mTransitionProgress; }
            protected set { mTransitionProgress = value; }
        }
        float mTransitionProgress = 1;

        public float TransitionAlpha 
        {
            get { return 1f - TransitionProgress; }
        }

        public ScreenState ScreenState 
        {
            get { return mScreenState; }
            protected set { mScreenState = value; }
        }
        ScreenState mScreenState = ScreenState.TransitionOn;

        public ScreenType ScreenType
        {
            get { return mScreenType; }
            protected set { mScreenType = value; }
        }
        ScreenType mScreenType = ScreenType.PlayerFixed;

        public bool IsExiting
        {
            get { return mIsExiting; }
            protected internal set { mIsExiting = value; }
        }
        bool mIsExiting = false;

        public bool IsActive
        {
            get
            {
                return !mOtherScreenHasFocus &&
                    (mScreenState == ScreenState.TransitionOn ||
                     mScreenState == ScreenState.Active);
            }
        }
        bool mOtherScreenHasFocus;

        public ScreenManager ScreenManager
        {
            get { return mScreenManager; }
            internal set { mScreenManager = value; }
        }
        ScreenManager mScreenManager;

        protected Song mMusic;
        protected bool mIsMusicPlaying = false;

        #endregion

        #region Initialization

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            mOtherScreenHasFocus = otherScreenHasFocus;

            if (mIsExiting)
            {
                mScreenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, mTransitionOffTime, 1)) 
                {
                    //TODO screen manager remove screen
                }
            }
            else if (coveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, mTransitionOffTime, 1))
                    mScreenState = ScreenState.TransitionOff;
                else
                    mScreenState = ScreenState.Hidden;
            }
            else
            {
                if (UpdateTransition(gameTime, mTransitionOnTime, -1))
                    mScreenState = ScreenState.TransitionOn;
                else
                    mScreenState = ScreenState.Active;
            }
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                    time.TotalMilliseconds);

            mTransitionProgress += transitionDelta * direction;

            if(((direction < 0) && (mTransitionProgress <= 0)) ||
                ((direction > 0) && (mTransitionProgress >= 1))) 
            {
                mTransitionProgress = MathHelper.Clamp(mTransitionProgress, 0, 1);
                return false;
            }
            else
                return true;
        }
        
        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region Public Methods

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                mIsExiting = true;
            }
        }

        #endregion
    }
}
