using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;

namespace RobotVacuum
{
    public class JobGiver_WanderFilth : JobGiver_Wander
    {
        protected override IntVec3 GetWanderRoot(Pawn pawn)
        {
            // Start by finding filth to wander around
            var eligibleFilth = pawn.Map.listerFilthInHomeArea.FilthInHomeArea
                .Where(x => x is Filth filth && filth.TicksSinceThickened > 600).ToList();
            var closestFilth = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map,
                eligibleFilth, PathEndMode.Touch, TraverseParms.For(pawn, maxDanger));
            if (closestFilth != null)
                return closestFilth.Position;

            // If not, find some place to hang out
            var randomHangoutSpots = pawn.Map.listerBuildings.allBuildingsColonist
                .Where(x => (x.def == ThingDefOf.Wall || x.def.building.ai_chillDestination) && pawn.Map.areaManager.Home[x.Position])
                .Select(x => GenAdjFast.AdjacentCells8Way(x).RandomElement())
                .Where(x => !x.IsForbidden(pawn) && pawn.CanReach(x, PathEndMode.Touch, maxDanger)).ToList();
            if (randomHangoutSpots.Count > 0)
                return randomHangoutSpots.RandomElement();

            // If we have neither, wander around your current position.
            return pawn.Position;
        }
    }
}
