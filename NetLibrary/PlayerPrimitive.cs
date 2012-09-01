using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace NetLibrary
{
    public enum PacketTypes
    {
        Login, Move, WorldState
    }

    public class PlayerPrimitive
    {

        public enum MoveDirection
        {
            Down, Right, Up, Left, Null
        }

        public int X
        {
            get { return mPositionX; }
            set { mPositionX = value; }
        }
        int mPositionX;

        public int Y
        {
            get { return mPositionY; }
            set { mPositionY = value; }
        }
        int mPositionY;

        public bool IsRunning
        {
            get { return mIsRunning; }
            set { mIsRunning = value; }
        }
        bool mIsRunning = false;

        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }
        String mName;

        public NetConnection Connection { get; set; }

        public int UniqueId 
        {
            get { return mUniqueId; }
            set { mUniqueId = value; }
        }
        int mUniqueId;

        public PlayerPrimitive(Vector2 position, String name, NetConnection connection)
        {
            mName = name;
            mPositionX = (int)position.X;
            mPositionY = (int)position.Y;
            Connection = connection;
            UniqueId = new Random().Next(20000);
        }

        public PlayerPrimitive(Vector2 position, String name)
        {
            mName = name;
            mPositionX = (int)position.X;
            mPositionY = (int)position.Y;
            UniqueId = new Random().Next(20000);
        }

        public PlayerPrimitive() 
        {
            UniqueId = new Random().Next(20000);
        }
    }
}
