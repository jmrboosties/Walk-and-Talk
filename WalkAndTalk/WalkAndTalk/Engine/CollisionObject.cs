using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WalkAndTalk.Engine
{
    public class CollisionObject
    {
        Texture2D mTexture;
        Vector2 mDimensions;

        public Vector2 Position;

        public Rectangle BoundingBox
        {
            get
            {
                if (mTexture != null)
                {
                    return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    mTexture.Width,
                    mTexture.Height);
                }
                else if (mDimensions != null)
                {
                    return new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        (int)mDimensions.X,
                        (int)mDimensions.Y
                        );
                }
                else
                    return new Rectangle(-1, -1, -1, -1);
            }
        }

        public Texture2D Texture
        {
            get { return mTexture; }
        }

        public CollisionObject(Vector2 dimensions, Vector2 position)
        {
            Position = position;
            mDimensions = dimensions;
        }

        public CollisionObject(Texture2D texture, Vector2 position)
        {
            mTexture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mTexture != null)
                spriteBatch.Draw(mTexture, Position, Color.White);             
        }
    }
}
