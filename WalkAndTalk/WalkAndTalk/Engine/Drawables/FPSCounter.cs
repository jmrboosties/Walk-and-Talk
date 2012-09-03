using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WalkAndTalk.Engine.Drawables
{
    public class FPSCounter : DrawableGameComponent
    {
        ContentManager mContentManager;
        SpriteBatch mSpriteBatch;
        SpriteFont mFont;

        int mFrameRate;
        int mFrameCounter;
        TimeSpan elapsedTime;

        public FPSCounter(Game game)
            : base(game)
        {
            mContentManager = Game.Content;
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mFont = mContentManager.Load<SpriteFont>("WalkAndTalk");
        }

        protected override void UnloadContent()
        {
            mContentManager.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                mFrameRate = mFrameCounter;
                mFrameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            mFrameCounter++;

            string fps = string.Format("fps: {0}", mFrameRate);

            mSpriteBatch.Begin();

            mSpriteBatch.DrawString(mFont, fps, new Vector2(33, 33), Color.Black);
            mSpriteBatch.DrawString(mFont, fps, new Vector2(32, 32), Color.White);

            mSpriteBatch.End();
        }

    }
}
