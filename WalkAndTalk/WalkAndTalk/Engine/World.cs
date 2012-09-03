using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NetLibrary;
using Microsoft.Xna.Framework.Content;

namespace WalkAndTalk.Engine
{
    
    public class World : GameScreen
    {
        public GraphicsDevice GraphicsDevice
        {
            get { return ScreenManager.GraphicsDevice; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return ScreenManager.SpriteBatch; }
        }
        
        protected Player mPlayer;

        protected List<Keys> mMovementKeys;

        public Texture2D CollisionTexture
        {
            get { return mCollisionTexture; }
            set { mCollisionTexture = value; }
        }
        Texture2D mCollisionTexture;

        public WorldMap Map
        {
            get { return mMap; }
            set { mMap = value; }
        }
        private WorldMap mMap;

        protected List<NPC> mNPCs;
        protected Dictionary<int, PeerPlayer> mPeerPlayers;

        public Camera Camera
        {
            get { return mCamera; }
        }
        protected Camera mCamera;

        protected ContentManager mContentManager;

        public World() { }

        public override void LoadContent()
        {
            mContentManager = new ContentManager(ScreenManager.Game.Services, "Content");
            mNPCs = new List<NPC>();
            mPeerPlayers = new Dictionary<int, PeerPlayer>();

            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            if (mIsMusicPlaying)
                MediaPlayer.Stop();
        }

        public override void Draw(GameTime gameTime)
        {
            mCamera.Update(mPlayer.DrawnPosition);
            
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null);
            //SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null,null
            mMap.Draw(SpriteBatch, false, mPlayer.DrawnPosition, mCamera);
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null);
            mPlayer.Draw(SpriteBatch, this, gameTime);
            foreach (NPC npc in mNPCs)
            {
                npc.Draw(SpriteBatch, this, gameTime);
            }

            if (GameLauncher.IsOnline)
            {
                foreach (var peer in mPeerPlayers)
                {
                    if (peer.Key != GameLauncher.GameNetClient.ClientsUniqueId)
                        peer.Value.Draw(SpriteBatch, this, gameTime);
                }
            }
        
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null);
            mMap.Draw(SpriteBatch, true, mPlayer.DrawnPosition, mCamera);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (mMusic != null)
            {
                if (!mIsMusicPlaying)
                {
                    MediaPlayer.Play(mMusic);
                    mIsMusicPlaying = true;
                }
            }
            
            mPlayer.Update(gameTime);

            foreach (NPC npc in mNPCs)
            {
                npc.Update(gameTime);
            }

            if(GameLauncher.IsOnline)
                ManagePeerPlayers(gameTime);
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        
        public bool CollisionCheck(Vector2 position)
        {
            foreach (Layer layer in mMap.Layers)
            {
                if (layer.Collisions)
                {
                    return layer.Tiles[(int)(position.Y * layer.Width + position.X)].Texture == null;
                }
            }
            return true;
        }

        private void ManagePeerPlayers(GameTime gameTime)
        {
            lock (GameLauncher.GameNetClient.PlayerList)
            {
                foreach (var player in GameLauncher.GameNetClient.PlayerList)
                {
                    if (player.Key == GameLauncher.GameNetClient.ClientsUniqueId)
                        continue;

                    Vector2 updatedPosition = new Vector2(player.Value.X, player.Value.Y);
                    if (mPeerPlayers.ContainsKey(player.Key))
                    {
                        mPeerPlayers[player.Key].IsRunning = player.Value.IsRunning;
                        Vector2 previousPosition = mPeerPlayers[player.Key].Coordinates;
                        Vector2 result = updatedPosition - previousPosition;
                        bool isRunning = mPeerPlayers[player.Key].IsRunning;

                        if(result != Vector2.Zero)
                            mPeerPlayers[player.Key].Update(gameTime, result);
                        
                    }
                    else
                    {
                        PeerPlayer peer = new PeerPlayer(mContentManager.Load<Texture2D>("CharacterSheets/hikersheet"), updatedPosition, this, player.Key);
                        mPeerPlayers.Add(player.Key, peer);
                        Console.WriteLine("New player added @ " + peer.Coordinates);
                        //TODO: new player entered the realm
                    }

                    List<int> itemsToRemove = new List<int>();
                    foreach (var peer in mPeerPlayers)
                    {
                        if (!GameLauncher.GameNetClient.PlayerList.ContainsKey(peer.Key))
                        {
                            //TODO: player has left
                            itemsToRemove.Add(peer.Key);
                        }
                    }

                    foreach (int key in itemsToRemove)
                    {
                        Console.WriteLine("removed a player: " + key);
                        Console.WriteLine(GameLauncher.GameNetClient.PlayerList.Count);
                        mPeerPlayers.Remove(key);
                    }

                }
            }
            
            
        }

    }
}
