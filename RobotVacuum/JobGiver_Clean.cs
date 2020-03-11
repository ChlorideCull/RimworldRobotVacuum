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
    // ReSharper disable once InconsistentNaming
    public class JobGiver_VacuumCleanRoom : ThinkNode_JobGiver
    {
        public JobGiver_VacuumCleanRoom()
        {

        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            // Get the current room, excluding doors
            var pawnRoom = pawn.GetRoom(RegionType.Normal);
            // Get the first 100 filth, mimicking the logic in WorkGiver_CleanFilth for eligibility
            var filthToClean = pawnRoom.ContainedAndAdjacentThings.Where(x => x != null && x is Filth filth && filth.TicksSinceThickened > 600 && filth.Map.areaManager.Home[filth.Position]).Take(100).ToList();
            if (filthToClean.Count == 0)
                return null;

            var job = JobMaker.MakeJob(JobDefOf.Clean);
            foreach (var filth in filthToClean)
            {
                if (filth != null && pawn.CanReserve(filth))
                {
                    job.AddQueuedTarget(TargetIndex.A, filth);
                }
            }
            return job;
        }
    }
}
