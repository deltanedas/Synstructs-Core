using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    // A simple ThingComp designed to add the necessary trackers and gizmo's for synstruct animals to allow drafting.
    public class CompDraftableSynstructAnimal : ThingComp
    {
        private Pawn_DraftController drafter;

        private Pawn Pawn => parent as Pawn;

        public override void PostPostMake()
        {
            if (Pawn.drafter == null)
            {
                Pawn.drafter = new Pawn_DraftController(Pawn);
            }
            // The game expects there to be an equipment tracker for anything with a draft tracker.
            if (Pawn.equipment == null)
            {
                Pawn.equipment = new Pawn_EquipmentTracker(Pawn);
            }
            base.PostPostMake();
        }

        // The extra selection overlay for drawing lines for paths only happens for Colonist pawns (ie. humanlikes).
        public override void PostDrawExtraSelectionOverlays()
        {
            Pawn.pather.curPath?.DrawPath(Pawn);
            Pawn.jobs.DrawLinesBetweenTargets();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (drafter == null)
            {
                drafter = Pawn.drafter;
            }
            if (drafter.ShowDraftGizmo)
            {
                Command_Toggle command_Toggle = new Command_Toggle
                {
                    hotKey = KeyBindingDefOf.Command_ColonistDraft,
                    isActive = () => drafter.Drafted,
                    toggleAction = delegate
                    {
                        drafter.Drafted = !drafter.Drafted;
                    },
                    defaultDesc = "CommandToggleDraftDesc".Translate(),
                    icon = TexCommand.Draft,
                    turnOnSound = SoundDefOf.DraftOn,
                    turnOffSound = SoundDefOf.DraftOff,
                    groupKeyIgnoreContent = 81729172,
                    defaultLabel = (drafter.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate()
                };
                if (Pawn.Downed)
                {
                    command_Toggle.Disable("IsIncapped".Translate(Pawn.LabelShort, Pawn));
                }
                yield return command_Toggle;
            }
            foreach (Gizmo attackGizmo in PawnAttackGizmoUtility.GetAttackGizmos(Pawn))
            {
                yield return attackGizmo;
            }
        }
    }
}
