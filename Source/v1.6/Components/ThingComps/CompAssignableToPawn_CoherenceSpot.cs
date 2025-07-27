using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    // A simple CompAssignable subclass that allows mechanical pawns to be assigned to a spot. There are no restrictions besides having a coherence need (no 1-1 restrictions, etc).
    public class CompAssignableToPawn_CoherenceSpot : CompAssignableToPawn
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
                        if (pawn.HasComp<CompCoherenceNeed>())
                        {
                            cachedCandidates.InsertIntoSortedList(pawn, comparer);
                        }
                    }
                    foreach (Pawn pawn in parent.Map.mapPawns.FreeColonists)
                    {
                        if (pawn.HasComp<CompCoherenceNeed>())
                        {
                            cachedCandidates.InsertIntoSortedList(pawn, comparer);
                        }
                    }
                }
                return cachedCandidates;
            }
        }

        // The pawn being in the cached list implies it has a coherence need.
        public override AcceptanceReport CanAssignTo(Pawn pawn)
        {
            return AcceptanceReport.WasAccepted;
        }
    }
}
