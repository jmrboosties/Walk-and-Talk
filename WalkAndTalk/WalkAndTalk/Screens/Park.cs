using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkAndTalk.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace WalkAndTalk.Screens
{
    public class Park : World
    {
        public Park() { }

        public override void LoadContent()
        {
            base.LoadContent();
            Map = mContentManager.Load<WorldMap>("Maps/pokemonbigmap");
            mPlayer = new Player(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(17, 16), this);
            mNPCs.Add(new NPC(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(39, 12), this, 4));
            mNPCs.Add(new NPC(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(4, 19), this, 4));
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
