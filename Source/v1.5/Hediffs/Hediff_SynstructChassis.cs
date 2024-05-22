using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    // Chassis upgrades are destroyed when the pawn is killed.
    public class Hediff_SynstructChassis : Hediff
    {
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
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
