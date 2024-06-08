using RimWorld;
using Verse.AI;
using Verse;
using System.Linq;

namespace ArtificialBeings
{
    // Attack anything that isn't also under the influence of berserker directives.
    public class MentalState_DirectiveBerserker : MentalState
    {
        public override bool ForceHostileTo(Thing t)
        {
            if (t is Pawn pawn && ABF_Utils.IsProgrammableDrone(pawn) && pawn.GetComp<CompArtificialPawn>().ActiveDirectives.Contains(ABF_DirectiveDefOf.ABF_Directive_Synstruct_Berserker))
            {
                return false;
            }
            return true;
        }

        public override bool ForceHostileTo(Faction f)
        {
            return true;
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}
