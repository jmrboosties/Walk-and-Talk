using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using WalkAndTalk.Engine;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace WalkAndTalk.Screens
{
    
    public class Lobby : World
    {
        public Lobby() { }

        public override void LoadContent()
        {
            base.LoadContent();
            Map = mContentManager.Load<WorldMap>("pokemonnewmap");
            mPlayer = new Player(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(11, 8), this);

            //mNPCs.Add(new Character(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(6, 9), this, 4));

            mMusic = mContentManager.Load<Song>("Music/village");

            mCamera = new Camera(this);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            mContentManager.Unload();
        }

    }
}
