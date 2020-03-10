using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RobotVacuum
{
    public class CompVacuumConstructor : ThingComp

    {
        public CompProperties_VacuumConstructor Props => (CompProperties_VacuumConstructor)props;

        public override void CompTick()
        {
            var faction = this.parent.Faction ?? Faction.OfPlayer;
            try
            {
                var request = new PawnGenerationRequest(this.Props.hatcherPawn, faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
                for (int i = 0; i < this.parent.stackCount; i++)
                {
                    var pawn = PawnGenerator.GeneratePawn(request);
                    if (!PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent))
                    {
                        Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                    }
                }
            }
            finally
            {
                this.parent.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
