using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RobotVacuum
{
    /*
    [HarmonyPatch(typeof(StunHandler))]
    static class StunHandlerPatches
    {
        [HarmonyPostfix, HarmonyPatch("EMPAdaptationTicksDuration", MethodType.Getter)]
        // ReSharper disable once InconsistentNaming
        static void EMPAdaptationTicksDurationPostfix(StunHandler __instance, ref int __result)
        {
            Log.Message(__instance.parent.def.defName + " EMPAdaptationTicksDuration");
            if (__instance.parent is Pawn pawn && pawn.def.defName == "Vacuum")
            {
                __result = 2200;
            }
        }
    }
    */

    [HarmonyPatch]
    static class PawnWoundDrawerPatches
    {
        private static Type TargetType => typeof(Ability).Assembly.GetType("RimWorld.PawnWoundDrawer+Wound");

        [HarmonyTargetMethod]
        static MethodBase TargetMethod()
        {
            return AccessTools.Constructor(TargetType, new [] {typeof(Pawn)});
        }

        [HarmonyPostfix]
        static void PawnWoundDrawerConstructorPostfix(object __instance, ref Material ___mat, Pawn __0)
        {
            if (__0.def.defName == "Vacuum")
            {
                ___mat = FleshTypeDefOf.Mechanoid.ChooseWoundOverlay();
            }
        }
    }

    [HarmonyPatch(typeof(ArmorUtility))]
    static class ArmorUtilityPatches
    {
        [HarmonyPostfix, HarmonyPatch("ApplyArmor")]
        static void ApplyArmorPostfix(Pawn pawn, ref bool metalArmor)
        {
            if (pawn.def.defName == "Vacuum")
            {
                metalArmor = true;
            }
        }
    }

    [HarmonyPatch(typeof(FoodUtility))]
    static class FoodUtilityPatches
    {
        [HarmonyPostfix, HarmonyPatch("IsAcceptablePreyFor")]
        static void IsAcceptablePreyForPostfix(Pawn prey, ref bool __result)
        {
            if (prey.def.defName == "Vacuum")
            {
                __result = false;
            }
        }
    }

    /*
    [HarmonyPatch(typeof(PawnColumnWorker_Hunt))]
    // ReSharper disable once InconsistentNaming
    static class PawnColumnWorker_HuntPatches
    {
        [HarmonyPostfix, HarmonyPatch("HasCheckbox")]
        static void HasCheckboxPostfix(Pawn pawn, ref bool __result)
        {
            Log.Message(pawn.def.defName + " HasCheckbox");
            if (pawn.def.defName == "Vacuum")
            {
                __result = false;
            }
        }
    }
    */

    [HarmonyPatch(typeof(Corpse))]
    // ReSharper disable once InconsistentNaming
    static class CorpsePatches
    {
        [HarmonyPostfix, HarmonyPatch("IngestibleNow", MethodType.Getter)]
        static void IngestibleNowPostfix(Corpse __instance, ref bool __result)
        {
            if (__instance.InnerPawn != null && __instance.InnerPawn.def.defName == "Vacuum")
            {
                __result = false;
            }
        }
    }

    // IsMechanoid has to be false, and IsFlesh has to be true, thus we need to do some changes.
    //
    // Easy ones:
    // RimWorld.Recipe_RemoveBodyPart, GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part), return RecipeDefOf.RemoveBodyPart.label if vacuum
    // RimWorld.PawnColumnWorker_Hunt, HasCheckbox(Pawn pawn), return false if vacuum
    // 
    //
    // How do I prevent the vacuum from showing up as wild animals?
}
