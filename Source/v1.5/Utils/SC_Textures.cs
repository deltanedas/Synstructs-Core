using Verse;
using UnityEngine;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public static class SC_Textures
    {
        static SC_Textures()
        {
        }
        public static readonly Texture2D VanillaBleedIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding");

        // Settings
        public static readonly Texture2D DrawPocket = ContentFinder<Texture2D>.Get("UI/Icons/Settings/DrawPocket");

        // Medicine
        public static readonly Texture2D NoCare = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare");
        public static readonly Texture2D NoMed = ContentFinder<Texture2D>.Get("Things/Pawns/Hatchling_east");
        public static readonly Texture2D RepairStimSimple = ContentFinder<Texture2D>.Get("Things/Items/Manufactured/ABF_RepairStimSimple/ABF_RepairStimSimple_a");
        public static readonly Texture2D RepairStimIntermediate = ContentFinder<Texture2D>.Get("Things/Items/Manufactured/ABF_RepairStimIntermediate/ABF_RepairStimIntermediate_a");
        public static readonly Texture2D RepairStimAdvanced = ContentFinder<Texture2D>.Get("Things/Items/Manufactured/ABF_RepairStimAdvanced/ABF_RepairStimAdvanced_a");

        // Gizmos
        public static readonly Texture2D RestrictionGizmoIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gizmos/ABF_RestrictionGizmo");

        // Race Exemplars
        public static readonly Texture2D MechDroneExemplar = ContentFinder<Texture2D>.Get("UI/Icons/Dialogs/ABF_MechDroneExemplar");
        public static readonly Texture2D MechSapientExemplar = ContentFinder<Texture2D>.Get("UI/Icons/Dialogs/ABF_MechSapientExemplar");
        public static readonly Texture2D BasicHumanExemplar = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists");
        public static readonly Texture2D MechDroneABF_PawnTypeRestricted = ContentFinder<Texture2D>.Get("UI/Icons/Dialogs/ABF_MechDronePawnTypeRestricted");
        public static readonly Texture2D MechSapientABF_PawnTypeRestricted = ContentFinder<Texture2D>.Get("UI/Icons/Dialogs/ABF_MechSapientPawnTypeRestricted");
        public static readonly Texture2D OrganicABF_PawnTypeRestricted = ContentFinder<Texture2D>.Get("UI/Icons/Dialogs/ABF_OrganicPawnTypeRestricted");
    }
}