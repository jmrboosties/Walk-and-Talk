using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Lidgren.Network.Xna;

namespace NetLibrary
{
    public static class DataReadWrite
    {
        ///<summary>
        /// Write a Player Primitive (just data, no graphics) to outgoing server message
        ///</summary>
        public static void Write(this NetOutgoingMessage message, PlayerPrimitive playerPrim)
        {
            message.Write(playerPrim.Name);
            //XNAExtensions.Write(message, playerPrim.Position);
            message.Write(playerPrim.X);
            message.Write(playerPrim.Y);
            message.Write(playerPrim.UniqueId);
        }

        ///<summary>
        /// Read a Player Primitive
        ///</summary>
        public static PlayerPrimitive ReadPlayer(this NetIncomingMessage message)
        {
            PlayerPrimitive playerPrim = new PlayerPrimitive();
            playerPrim.Name = message.ReadString();
            //playerPrim.Position = XNAExtensions.ReadVector2(message);
            playerPrim.X = message.ReadInt32();
            playerPrim.Y = message.ReadInt32();
            playerPrim.UniqueId = message.ReadInt32();
            return playerPrim;
        }

    }
}
