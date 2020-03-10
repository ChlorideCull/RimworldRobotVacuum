using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RobotVacuum
{
    public class CompProperties_VacuumConstructor : CompProperties
    {
        public PawnKindDef hatcherPawn;

        public CompProperties_VacuumConstructor()
        {
            this.compClass = typeof(CompVacuumConstructor);
        }
    }
}
