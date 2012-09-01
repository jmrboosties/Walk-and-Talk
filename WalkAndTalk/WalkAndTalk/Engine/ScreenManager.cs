using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using WalkAndTalk.Engine;
using WalkAndTalk.Engine.Net;

namespace WalkAndTalk
{
    public class ScreenManager : DrawableGameComponent
    {

        #region Fields

        List<GameScreen> mScreens = new List<GameScreen>();
        List<GameScreen> mLiveScreens = new List<GameScreen>();

        InputHandler mInputHandler = new InputHandler();
        
        Texture2D mBlankTexture;

        bool mIsInitialized;
        bool mIsTraceEnabled;

        #endregion

        #region Properties

        public SpriteBatch SpriteBatch
        {
            get { return mSpriteBatch; }
        }
        SpriteBatch mSpriteBatch;

        public static SpriteFont MenuFont
        {
            get { return mMenuFont; }
        }
        static SpriteFont mMenuFont;

        public static SpriteFont SmallFont
        {
            get { return mSmallFont; }
        }
        static SpriteFont mSmallFont;

        public bool TraceEnabled
        {
            get { return mIsTraceEnabled; }
            set { mIsTraceEnabled = value; }
        }

        public Vector2 TilesPerScreen
        {
            get { return mTilesPerScreen; }
        }
        Vector2 mTilesPerScreen;

        #endregion

        #region Initialization

        public ScreenManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            mIsInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            mTilesPerScreen.X = GraphicsDevice.PresentationParameters.BackBufferWidth / 32;
            mTilesPerScreen.Y = GraphicsDevice.PresentationParameters.BackBufferHeight / 32;

            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mMenuFont = content.Load<SpriteFont>("WalkAndTalk");
            mSmallFont = content.Load<SpriteFont>("SmallText");
            mBlankTexture = content.Load<Texture2D>("blank"); 

            foreach (GameScreen screen in mScreens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen screen in mScreens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            mInputHandler.Update();

            mLiveScreens.Clear();
            foreach (GameScreen screen in mScreens)
                mLiveScreens.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive; //Do we want this?
            bool coveredByOtherScreen = false;

            while (mLiveScreens.Count > 0)
            {
                GameScreen screen = mLiveScreens[mLiveScreens.Count - 1];

                mLiveScreens.RemoveAt(mLiveScreens.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        //screen.HandleInput(mInputHandler);
                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            if (mIsTraceEnabled)
                TraceScreens();

        }

        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in mScreens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in mScreens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            
            screen.IsExiting = false;

            if (mIsInitialized)
                screen.LoadContent();

            mScreens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (mIsInitialized)
                screen.UnloadContent();

            mScreens.Remove(screen);
            mLiveScreens.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return mScreens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mBlankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height),
                Color.Black * alpha);
            mSpriteBatch.End();
        }

        #endregion
    }
}
