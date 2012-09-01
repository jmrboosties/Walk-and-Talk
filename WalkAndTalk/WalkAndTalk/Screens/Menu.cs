using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WalkAndTalk.Screens;

namespace WalkAndTalk
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Menu : GameScreen
    {
        SpriteBatch mSpriteBatch;
        GraphicsDevice mDevice;

        SpriteFont mFont;

        ContentManager mContent;

        private struct TextView
        {
            public Vector2 Position;
            public Vector2 Dimensions;
            public string Text;
        }

        TextView[] mMenuOptions;
        int mSelectedIndex = 0;

        Texture2D mBackground;
        Song mUhMusic;

        bool mMusicPlaying = false;

        public Menu() { }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mContent = new ContentManager(ScreenManager.Game.Services, "Content");
            mSpriteBatch = ScreenManager.SpriteBatch;
            mDevice = ScreenManager.GraphicsDevice;

            int height = (int)(mDevice.PresentationParameters.BackBufferHeight);

            mBackground = mContent.Load<Texture2D>("mainpage");
            mFont = ScreenManager.MenuFont;
            mUhMusic = mContent.Load<Song>("Music/Track18");

            mMenuOptions = new TextView[2];
            PopulateMenuOptions("Join Game", 0);
            PopulateMenuOptions("Quit", 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public override void UnloadContent()
        {
            if (mMusicPlaying)
                MediaPlayer.Stop();

            mContent.Unload();
        }

        private void PopulateMenuOptions(string text, int index)
        {
            mMenuOptions[index].Text = text;
            mMenuOptions[index].Dimensions = mFont.MeasureString(text);
            float verticalAdjust = 20f;
            if (index > 0)
            {
                verticalAdjust += mMenuOptions[index - 1].Dimensions.Y + 20f;
            }

            mMenuOptions[index].Position = new Vector2((mDevice.PresentationParameters.BackBufferWidth / 2) - (mMenuOptions[index].Dimensions.X / 2), (mDevice.PresentationParameters.BackBufferHeight / 2) + verticalAdjust);            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                ScreenManager.Game.Exit();

            // TODO: Add your update logic here
            MediaPlayer.IsRepeating = false;
            if (!mMusicPlaying)
            {
                MediaPlayer.Play(mUhMusic);
                mMusicPlaying = true;
            }

            HandleArrowMovement();
            HandleEnter();
        }

        private void HandleArrowMovement()
        {
            KeyboardState keys = Keyboard.GetState();
            switch (mSelectedIndex)
            {
                case 0 :
                    if(keys.IsKeyDown(Keys.Down))
                    {
                        mSelectedIndex = 1;
                    }
                    break;
                case 1 :
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        mSelectedIndex = 0;
                    }
                    break;
            }
        }

        private void HandleEnter()
        {
            KeyboardState keys = Keyboard.GetState();
            switch (mSelectedIndex)
            {
                case 0 :
                    if(keys.IsKeyDown(Keys.Enter)) 
                    {
                        ScreenManager.AddScreen(new Town());
                        ScreenManager.RemoveScreen(this);
                    }
                    break;
                case 1 :
                    if (keys.IsKeyDown(Keys.Enter))
                    {
                        ScreenManager.Game.Exit();
                    }
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
            DrawBackground();
            DrawOptions(gameTime);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            Rectangle screen = new Rectangle(0, 0, mDevice.PresentationParameters.BackBufferWidth, mDevice.PresentationParameters.BackBufferHeight);
            mSpriteBatch.Draw(mBackground, screen, Color.White);
        }

        private void DrawOptions(GameTime gameTime)
        {
            for (int i = 0; i < mMenuOptions.Length; i++)
            {
                Color color = Color.White;
                float scale = 1;
                if (i == mSelectedIndex)
                {
                    double time = gameTime.TotalGameTime.TotalSeconds;
                    float pulsate = (float)Math.Sin(time * 6) + 1;

                    scale = 1 + pulsate * 0.05f;
                    color = Color.SkyBlue;
                }
                Vector2 origin = new Vector2(0, mFont.LineSpacing / 2);
                mSpriteBatch.DrawString(mFont, mMenuOptions[i].Text, mMenuOptions[i].Position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
                //mSpriteBatch.DrawString(mFont, mMenuOptions[i].Text, mMenuOptions[i].Position, color);
            }
        }


    }
}
