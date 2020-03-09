using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace RobotVacuum
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            Log.Message("Applying patches");
            var harmony = new Harmony("se.chcl.robotvacuum");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("Applied all patches");
        }

    }
}
