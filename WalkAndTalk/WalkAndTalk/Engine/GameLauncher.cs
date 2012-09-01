using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WalkAndTalk.Engine;
using WalkAndTalk.Engine.Net;

namespace WalkAndTalk
{
    public class GameLauncher : Game
    {

        #region Fields

        GraphicsDeviceManager mGraphicsManager;
        ScreenManager mScreenManager;

        public static Random Random
        {
            get { return mRandom; }
        }

        static Random mRandom = new Random();


        public static GameNetClient GameNetClient
        {
            get { return mGameNetClient; }
        }
        static GameNetClient mGameNetClient = new GameNetClient();

        public int ScreenHeight
        {
            get { return mScreenHeight; }
            set { mScreenHeight = value; }
        }
        int mScreenHeight = 0;

        public int ScreenWidth
        {
            get { return mScreenWidth; }
            set { mScreenWidth = value; }
        }
        int mScreenWidth = 0;

        #endregion

        #region Initialization

        public GameLauncher()
        {
            Content.RootDirectory = "Content";
            mGraphicsManager = new GraphicsDeviceManager(this);
            mGraphicsManager.PreferredBackBufferWidth = 864;
            mGraphicsManager.PreferredBackBufferHeight = 480;

            mGraphicsManager.ApplyChanges();

            mScreenManager = new ScreenManager(this);

            Components.Add(mScreenManager);

            mScreenManager.AddScreen(new Menu());
        }

        #endregion

        #region Methods

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            
        }

        #endregion
    }
}
