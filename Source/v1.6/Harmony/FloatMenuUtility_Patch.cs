using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace ArtificialBeings
{
    public class FloatMenuUtility_Patch
    {
        // The float menu option for ordering melee attacks has its own colonist checks, which Synstruct animals need to get around to make drafting useful.
        [HarmonyPatch(typeof(FloatMenuUtility), nameof(FloatMenuUtility.GetMeleeAttackAction))]
        [HarmonyPatch(new Type[] { typeof(Pawn), typeof(LocalTargetInfo), typeof(string), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal })]
        public class FloatMenuUtility_GetMeleeAttackAction_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(Pawn pawn, LocalTargetInfo target, ref string failStr, ref bool ignoreControlled)
            {
                if (SC_Utils.IsSynstruct(pawn) && pawn.RaceProps.intelligence == Intelligence.Animal)
                {
                    ignoreControlled = true;
                }
                return true;
            }
        }
    }
}
