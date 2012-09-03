using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonGameServer
{
    public class NPCController
    {
        public List<NPC> NPCs
        {
            get { return mNPCs; }
        }
        List<NPC> mNPCs;

        public NPCController() { }

    }
}
