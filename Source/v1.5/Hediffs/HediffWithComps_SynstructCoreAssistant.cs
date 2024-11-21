using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    // Core Assistant upgrades are destroyed if the pawn is dead and has no consciousness source. This variant allows for HediffComps.
    public class HediffWithComps_CoreAssistant : HediffWithComps
    {
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            if (pawn.health.hediffSet.GetNotMissingParts(tag: BodyPartTagDefOf.ConsciousnessSource).EnumerableNullOrEmpty())
            {
                if (pawn.Faction == Faction.OfPlayer && pawn.PositionHeld != null)
                {
                    MoteMaker.ThrowText(pawn.PositionHeld.ToVector3(), pawn.MapHeld, "ABF_ComponentDestroyed".Translate(LabelCap), Color.red, 10f);
                }
                pawn.health.RemoveHediff(this);
                base.Notify_PawnDied(dinfo, culprit);
            }
        }
    }
}
