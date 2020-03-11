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
    public class ThinkNode_ConditionalDirtyRoom : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            // Get the current room, excluding doors
            var pawnRoom = pawn.GetRoom(RegionType.Normal);
            // Get the first 25 filth, mimicking the logic in WorkGiver_CleanFilth for eligibility
            var filthToClean = pawnRoom.ContainedAndAdjacentThings.Where(x => x != null && x is Filth filth && filth.TicksSinceThickened > 600 && filth.Map.areaManager.Home[filth.Position]).Take(25).ToList();
            return filthToClean.Count != 0;
        }
    }
}
