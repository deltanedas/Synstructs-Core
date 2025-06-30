using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class CompTargetable_SynstructCorpse : CompTargetable_SingleCorpse
    {
        public new CompTargetProperties_SynstructCorpse Props
        {
            get
            {
                return (CompTargetProperties_SynstructCorpse)props;
            }
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            return base.ValidateTarget(target, showMessages) && target.Thing is Corpse corpse && SC_Utils.IsSynstruct(corpse.InnerPawn) && Props.validStates.Contains(ABF_Utils.PawnStateFor(corpse.InnerPawn));
        }
    }
}
