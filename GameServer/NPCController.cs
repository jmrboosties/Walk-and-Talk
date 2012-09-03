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
            set { mNPCs = value; }
        }
        List<NPC> mNPCs = new List<NPC>();

        public NPCController() { }

        public NPCController(List<NPC> npcs)
        {
            mNPCs = npcs;
        }

        public void addNPC(NPC npc)
        {
            mNPCs.Add(npc);
        }

        public void UpdateNPCs(TimeSpan time)
        {
            foreach (NPC npc in mNPCs)
            {
                npc.Update(time);
            }
        }

    }
}
