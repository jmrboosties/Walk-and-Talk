using System;
using Lidgren.Network;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NetLibrary;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

namespace PokemonGameServer
{
    public static class GameServer
    {
        public static Random Random = new Random();
        static NetServer Server;
        static NetPeerConfiguration Config;
        static NPCController ServerNPCController;

        static void Main(string[] args)
        {
            Config = new NetPeerConfiguration("pokemon");
            Config.Port = 7377;
            Config.MaximumConnections = 20;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Server = new NetServer(Config);

            Server.Start();

            Console.WriteLine("time to own");

            /*/Creating a ContentManager to load maps from XML
            Form form = new Form();
            GraphicsDeviceService gds = GraphicsDeviceService.AddRef(form.Handle,
                    form.ClientSize.Width, form.ClientSize.Height);

            ServiceContainer services = new ServiceContainer();
            services.AddService<IGraphicsDeviceService>(gds);
            content = new ContentManager(services, "Content");*/

            //Initialize all the maps here

            List<PlayerPrimitive> Players = new List<PlayerPrimitive>();

            ServerNPCController = new NPCController();
            NPC npc = new NPC(null, new Vector2(15, 15), 2);
            ServerNPCController.addNPC(npc);

            NetIncomingMessage nic;

            DateTime time = DateTime.Now;

            TimeSpan updateSpan = new TimeSpan(0, 0, 0, 0, 30);

            Console.WriteLine("Waiting for players to join...");

            while (true)
            {
                if ((nic = Server.ReadMessage()) != null)
                {
                    switch (nic.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval :
                            if (nic.ReadByte() == (byte)PacketTypes.Login)
                            {
                                Console.WriteLine("New Player joining.");

                                nic.SenderConnection.Approve();

                                PlayerPrimitive prim = DataReadWrite.ReadPlayer(nic);
                                prim.Connection = nic.SenderConnection;

                                Players.Add(prim);

                                Console.WriteLine("The following players are in:");
                                foreach (PlayerPrimitive player in Players)
                                {
                                    Console.WriteLine(player.Name + " joined " + System.DateTime.Now);
                                    Console.WriteLine(player.UniqueId);
                                }

                                NetOutgoingMessage outmsg = Server.CreateMessage();

                                outmsg.Write((byte)PacketTypes.WorldState);

                                outmsg.Write(Players.Count);

                                foreach (PlayerPrimitive player in Players)
                                {
                                    outmsg.WriteAllProperties(player);
                                }

                                Server.SendMessage(outmsg, nic.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                                Console.WriteLine("New player has joined, server updated and shit.");

                            }
                            break;

                        case NetIncomingMessageType.Data :

                            if (nic.ReadByte() == (byte)PacketTypes.Move)
                            {
                                foreach (PlayerPrimitive player in Players)
                                {
                                    if (player.Connection != nic.SenderConnection)
                                        continue;

                                    Vector2 position = XNAExtensions.ReadVector2(nic);
                                    bool running = nic.ReadBoolean();

                                    player.X = (int)position.X;
                                    player.Y = (int)position.Y;
                                    player.IsRunning = running;

                                    Console.WriteLine(player.Name + " position: " + player.X + ", " + player.Y + ": running = " + running);

                                    /*NetOutgoingMessage outmsg = Server.CreateMessage();

                                    outmsg.Write((byte)PacketTypes.WorldState);
                                    outmsg.Write(Players.Count);

                                    foreach (PlayerPrimitive playa in Players)
                                        outmsg.WriteAllProperties(playa);

                                    Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);*/
                                    break;
                                }

                            }

                            break;

                        case NetIncomingMessageType.StatusChanged :

                            Console.WriteLine(nic.SenderConnection.ToString() + " status changed." + (NetConnectionStatus)nic.SenderConnection.Status);

                            if (nic.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                                nic.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                            {
                                foreach (PlayerPrimitive player in Players)
                                {
                                    if (player.Connection == nic.SenderConnection)
                                    {
                                        Players.Remove(player);
                                        Console.WriteLine(player.Name + " left the game " + System.DateTime.Now);
                                        break;
                                    }
                                }
                            }
                            break;

                        default :

                            Console.WriteLine("Message type is: " + nic.MessageType);

                            break;
                    }
                }

                if ((time + updateSpan) < DateTime.Now)
                {
                    ServerNPCController.UpdateNPCs(updateSpan);

                    if (Server.ConnectionsCount > 0)
                    {
                        NetOutgoingMessage outmsg = Server.CreateMessage();
                        outmsg.Write((byte)PacketTypes.WorldState);
                        outmsg.Write(Players.Count);
                        outmsg.Write(ServerNPCController.NPCs.Count);

                        foreach (PlayerPrimitive player in Players)
                            outmsg.WriteAllProperties(player);

                        foreach (NPC nerdo in ServerNPCController.NPCs)
                            outmsg.WriteAllProperties(nerdo.toPrimitive());

                        Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                    }

                    time = DateTime.Now;
                }

                System.Threading.Thread.Sleep(1);
            }

        }
    }
}

