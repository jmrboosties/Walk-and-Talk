using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace WalkAndTalk
{
    public class Loading : GameScreen
    {
        SpriteBatch mSpriteBatch;
        GraphicsDevice mDevice;

        SpriteFont mFont;

        private struct TextView
        {
            public Vector2 Position;
            public Vector2 Dimensions;
            public string Text;
        }

        public Loading() { }

        public override void LoadContent()
        {
            mFont = ScreenManager.MenuFont;
            mDevice = ScreenManager.GraphicsDevice;
            mSpriteBatch = ScreenManager.SpriteBatch;
        }

        public override void UnloadContent()
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
            DrawBackground();
            DrawText(gameTime);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            mDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
        }

        private void DrawText(GameTime gameTime)
        {
            float scale = 1;
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulse = (float)Math.Sin(time * 6) + 1;

            scale = 1 + pulse * 0.05f;

            TextView tView = new TextView();
            tView.Text = "Loading";
            tView.Dimensions = mFont.MeasureString(tView.Text);
            tView.Position = new Vector2(mDevice.PresentationParameters.BackBufferWidth - (tView.Dimensions.X * 1.5f),
                mDevice.PresentationParameters.BackBufferHeight - (tView.Dimensions.Y * 2));

            Vector2 origin = new Vector2(0, mFont.LineSpacing / 2);

            mSpriteBatch.DrawString(mFont, tView.Text, tView.Position, Color.White, 0,
                origin, scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
