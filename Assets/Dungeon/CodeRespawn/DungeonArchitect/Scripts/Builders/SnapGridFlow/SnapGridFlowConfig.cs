//$ Copyright 2015-21, Code Respawn Technologies Pvt Ltd - All Rights Reserved $//
using DungeonArchitect.Flow.Impl.SnapGridFlow;
using UnityEngine;

namespace DungeonArchitect.Builders.SnapGridFlow
{
    public class SnapGridFlowConfig : DungeonConfig
    {
        public SnapGridFlowAsset flowGraph;
        public SnapGridFlowModuleDatabase moduleDatabase;
        
        [Tooltip(@"If the flow graph cannot converge to a solution, retry again this many times.  Usually a dungeon converges within 1-10 tries, depending on how you've designed the flow graph")]
        public int numGraphRetries = 100;
        
        public override bool HasValidConfig(ref string errorMessage)
        {
            if (flowGraph == null)
            {
                errorMessage = "Flow Graph asset is not assigned";
                return false;
            }
            
            if (moduleDatabase == null)
            {
                errorMessage = "Module Database asset is not assigned";
                return false;
            }

            return true;
        }
        
    }
}