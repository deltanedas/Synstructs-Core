using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    // A CompAssignable subclass that allows pawns to be assigned an energy reservoir. There are no restrictions besides being able to charge (no 1-1 restrictions, etc).
    public class CompAssignableToPawn_EnergyReservoir : CompAssignableToPawn
    {
        private List<Pawn> cachedCandidates;

        // Override the base class so that we can cache the assigning candidates when the dialog is opened, so we don't recalculate it every frame.
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (ShouldShowAssignmentGizmo())
            {
                Command_Action command_Action = new Command_Action
                {
                    defaultLabel = GetAssignmentGizmoLabel(),
                    icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner"),
                    defaultDesc = GetAssignmentGizmoDesc(),
                    action = delegate
                    {
                        cachedCandidates?.Clear();
                        Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
                    },
                    hotKey = KeyBindingDefOf.Misc4
                };
                if (!Props.noAssignablePawnsDesc.NullOrEmpty() && !AssigningCandidates.Any())
                {
                    command_Action.Disable(Props.noAssignablePawnsDesc);
                }
                yield return command_Action;
            }
        }

        // All animals and colonists are possible candidates (with the exception of Dryads, which Core seems to think have a special use case).
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                if (cachedCandidates.NullOrEmpty())
                {
                    SC_Utils.PawnComparer comparer = new SC_Utils.PawnComparer();
                    cachedCandidates = new List<Pawn>();
                    foreach (Pawn pawn in parent.Map.mapPawns.SpawnedColonyAnimals)
                    {
                        if (SC_Utils.CanCharge(pawn))
                        {
                            cachedCandidates.InsertIntoSortedList(pawn, comparer);
                        }
                    }
                    foreach (Pawn pawn in parent.Map.mapPawns.FreeColonists)
                    {
                        if (SC_Utils.CanCharge(pawn))
                        {
                            cachedCandidates.InsertIntoSortedList(pawn, comparer);
                        }
                    }
                }
                return cachedCandidates;
            }
        }

        // Can be assigned if the pawn can charge.
        public override AcceptanceReport CanAssignTo(Pawn pawn)
        {
            if (!SC_Utils.CanCharge(pawn))
            {
                return "ABF_IncapableOfChargingShort".Translate();
            }
            return AcceptanceReport.WasAccepted;
        }
    }
}
