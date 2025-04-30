using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    public abstract class CompEnergyReservoir : ThingComp
    {
        public float reserve = 0f;

        // Food Need is measured in single digit units without a name. Treat 125 watts as one unit until someone ideates a better solution.
        public const int wattsToNeedUnits = 125;

        public CompProperties_EnergyReservoir Props
        {
            get
            {
                return props as CompProperties_EnergyReservoir;
            }
        }

        public virtual bool Usable => reserve > (Props.minimumReserveBeforeReady * Props.maximumReserve);

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref reserve, "ABF_reservoirReserve", 0f);
        }

        // Show a gizmo for controlling the target level of the reserve (and which shows the current reserve without using an inspect string).
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Find.Selector.SingleSelectedThing == parent)
            {
                yield return new Gizmo_EnergyReservoir
                {
                    compEnergyReservoir = this
                };
            }
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            if (Props.reactsIfDamagedAboveMinimumReserve && Usable)
            {
                ReactToDamage(dinfo);
            }
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption option in base.CompFloatMenuOptions(selPawn))
            {
                yield return option;
            }
            if (!Usable)
            {
                yield return new FloatMenuOption("ABF_InsufficientReserve".Translate(), null);
            }
            // Check if the pawn can reach the building.
            else if (!selPawn.CanReach(parent, PathEndMode.Touch, Danger.Deadly))
            {
                yield return new FloatMenuOption("CannotUseNoPath".Translate(), null);
            }
            // Check if the pawn can charge.
            else if (!SC_Utils.CanCharge(selPawn) || selPawn.needs?.food == null)
            {
                yield return new FloatMenuOption("ABF_IncapableOfCharging".Translate(selPawn), null);
            }
            else
            {
                yield return new FloatMenuOption("ABF_ForceCharge".Translate(), delegate () {
                    Job job = new Job(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, new LocalTargetInfo(parent));
                    selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                });
            }
        }

        public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
        {
            foreach (FloatMenuOption option in base.CompMultiSelectFloatMenuOptions(selPawns))
            {
                yield return option;
            }
            if (!Usable)
            {
                yield return new FloatMenuOption("ABF_InsufficientReserve".Translate(), null);
                yield break;
            }

            List<Pawn> usableFor = new List<Pawn>();
            foreach (Pawn pawn in selPawns)
            {
                if (pawn.CanReach(parent, PathEndMode.Touch, Danger.Deadly) && SC_Utils.CanCharge(pawn) && pawn.needs?.food != null)
                {
                    usableFor.Add(pawn);
                }
            }
            if (!usableFor.NullOrEmpty())
            {
                yield return new FloatMenuOption("ABF_ForceCharge".Translate(), delegate () {
                    foreach (Pawn pawn in usableFor)
                    {
                        Job job = new Job(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, new LocalTargetInfo(parent));
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                });
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            // Add this charger to the map set so each charger can find the others easily and force a recache.
            SC_MapComponent mapComponent = parent.Map.GetComponent<SC_MapComponent>();
            mapComponent.reservoirs.Add(parent);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            SC_MapComponent mapComponent = map.GetComponent<SC_MapComponent>();
            mapComponent.reservoirs.Remove(parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            SC_MapComponent mapComponent = previousMap.GetComponent<SC_MapComponent>();
            mapComponent.reservoirs.Remove(parent);
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            SC_MapComponent mapComponent = prevMap.GetComponent<SC_MapComponent>();
            mapComponent.reservoirs.Remove(parent);
        }

        public override void Notify_MapRemoved()
        {
            base.Notify_MapRemoved();
            SC_MapComponent mapComponent = parent.Map.GetComponent<SC_MapComponent>();
            mapComponent.reservoirs.Remove(parent);
        }

        public override string CompInspectStringExtra()
        {
            if (!Usable)
            {
                return "ABF_InsufficientReserve".Translate();
            }
            return base.CompInspectStringExtra();
        }

        public abstract void ReactToDamage(DamageInfo dinfo);

        // There is a difference between pawns automatically using the reservoir and players ordering pawns to use the reservoir.
        public virtual bool AutomaticallyUsableFor(Pawn pawn)
        {
            if (!Usable)
            {
                return false;
            }

            // If the parent is a building with an assignable comp, pawns can only automatically use it if they own it or no one owns it.
            Building building = parent as Building;
            if (building?.GetAssignedPawns()?.Contains(pawn) != false)
            {
                return true;
            }
            return false;
        }
    }
}