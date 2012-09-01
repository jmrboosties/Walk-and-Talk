using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkAndTalk.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WalkAndTalk.Screens
{
    public class Field : World
    {
        public Field() { }

        public override void LoadContent() 
        {
            base.LoadContent();
            Map = mContentManager.Load<WorldMap>("Maps/field");
            mPlayer = new Player(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(5, 5), this);
            mCamera = new Camera(this);
            GameLauncher.GameNetClient.Start(mPlayer);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            mContentManager.Unload();
        }

    }
}
