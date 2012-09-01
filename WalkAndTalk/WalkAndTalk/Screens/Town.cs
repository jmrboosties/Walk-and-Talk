using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkAndTalk.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace WalkAndTalk.Screens
{
    public class Town : World
    {
        public Town() { }

        public override void LoadContent()
        {
            base.LoadContent();
            Map = mContentManager.Load<WorldMap>("Maps/littletown");
            mPlayer = new Player(mContentManager.Load<Texture2D>("CharacterSheets/hikersheet"), new Vector2(5, 15), this);
            mNPCs.Add(new NPC(mContentManager.Load<Texture2D>("karatemansheet"), new Vector2(22, 11), this, 4));
            mNPCs.Add(new NPC(mContentManager.Load<Texture2D>("CharacterSheets/captainsheet"), new Vector2(14, 4), this, 4));
            mMusic = mContentManager.Load<Song>("Music/town3");
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
