using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using NetLibrary;
using Microsoft.Xna.Framework;
using Lidgren.Network.Xna;
using System.Diagnostics;

namespace WalkAndTalk.Engine.Net
{
    public class GameNetClient
    {
        NetClient mClient;

        public Dictionary<int, PlayerPrimitive> PlayerList
        {
            get { return mPlayerList; }
        }
        Dictionary<int, PlayerPrimitive> mPlayerList;
                
        //string mHostIP = "localhost";
        string mHostIP = "192.168.1.107";

        static System.Timers.Timer mUpdater;

        public bool IsRunning
        {
            get { return mIsRunning; }
        }
        static bool mIsRunning = false;

        public int ClientsUniqueId
        {
            get { return mClientsId; }
        }
        int mClientsId;

        public GameNetClient()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("pokemon");
            mClient = new NetClient(config);
        }

        public void Start(Player player)
        {
            NetOutgoingMessage outmsg = mClient.CreateMessage();

            mClient.Start();

            PlayerPrimitive playerPrim = new PlayerPrimitive();
            mClientsId = playerPrim.UniqueId;

            playerPrim.X = (int)player.Coordinates.X;
            playerPrim.Y = (int)player.Coordinates.Y;
            playerPrim.Name = "Quas";

            outmsg.Write((byte)PacketTypes.Login);
            DataReadWrite.Write(outmsg, playerPrim);

            mClient.Connect(mHostIP, 7377, outmsg);

            Console.WriteLine("Client started");
            Console.WriteLine("epic");

            mPlayerList = new Dictionary<int, PlayerPrimitive>();

            mUpdater = new System.Timers.Timer(50);
            mUpdater.Elapsed += new System.Timers.ElapsedEventHandler(UpdateElapsed);
            mUpdater.Enabled = true;

            WaitForStartingInfo();
        }

        private void UpdateElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckServerMessages();
        }

        private void CheckServerMessages()
        {
            NetIncomingMessage nic;
            while ((nic = mClient.ReadMessage()) != null)
            {
                if (nic.MessageType == NetIncomingMessageType.Data)
                {
                    if (nic.ReadByte() == (byte)PacketTypes.WorldState)
                    {
                        UpdatePlayerList(nic);
                    }
                }
            }
        }

        public void SendToServer(Vector2 coords, bool isRunning)
        {
            NetOutgoingMessage outmsg = mClient.CreateMessage();
            
            outmsg.Write((byte)PacketTypes.Move);
            XNAExtensions.Write(outmsg, coords);
            outmsg.Write(isRunning);

            mClient.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }

        private void WaitForStartingInfo()
        {
            bool canStart = false;

            NetIncomingMessage nic;

            while (!canStart)
            {
                if ((nic = mClient.ReadMessage()) != null)
                {
                    switch (nic.MessageType)
                    {
                        case NetIncomingMessageType.Data :
                            if ((nic.ReadByte() == (byte)PacketTypes.WorldState))
                            {
                                UpdatePlayerList(nic);
                                canStart = true;
                            }

                            break;
                    }
                }
            }
        }

        private void UpdatePlayerList(NetIncomingMessage nim)
        {
            lock (mPlayerList)
            {
                mPlayerList.Clear();
                
                int count = 0;
                count = nim.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    PlayerPrimitive player = new PlayerPrimitive();
                    nim.ReadAllProperties(player);
                    if(player.UniqueId != ClientsUniqueId)
                        Console.WriteLine("player: " + player.Name + " x: " + player.X + "; y: " + player.Y);

                    mPlayerList.Add(player.UniqueId, player);
                }
            }
        }

        public void Disconnect()
        {
            //NetOutgoingMessage nom = mClient.CreateMessage();

            //nom.Write((byte)PacketTypes.);
        }

    }
}
