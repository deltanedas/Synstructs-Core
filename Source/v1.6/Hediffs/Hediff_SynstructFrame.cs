using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    // Frames are destroyed if the total health of the pawn falls below 20% or upon death.
    public class Hediff_SynstructFrame : HediffWithComps
    {
        public override bool ShouldRemove => base.ShouldRemove || pawn.health.summaryHealth.SummaryHealthPercent <= 0.2f;

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            pawn.health.RemoveHediff(this);
            base.Notify_PawnDied(dinfo, culprit);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (pawn.Faction == Faction.OfPlayer && pawn.PositionHeld != IntVec3.Invalid)
            {
                MoteMaker.ThrowText(pawn.PositionHeld.ToVector3(), pawn.MapHeld, "ABF_ComponentDestroyed".Translate(LabelCap), Color.red, 10f);
            }
        }
    }
}
