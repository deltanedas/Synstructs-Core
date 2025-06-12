using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ArtificialBeings
{
    public class Incidents_PersonalityShift : IncidentWorker
    {
        // All player non-cryptosleep synstructs with at least 70% mood are possible candidates.
        protected virtual List<Pawn> Candidates()
        {
            List<Pawn> possiblePawns = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep;
            for (int i = possiblePawns.Count - 1; i >= 0; i--)
            {
                Pawn possiblePawn = possiblePawns[i];
                if (!SC_Utils.IsSynstruct(possiblePawn))
                {
                    possiblePawns.RemoveAt(i);
                    continue;
                }

                if (possiblePawn.needs.mood == null || possiblePawn.needs.mood.CurInstantLevelPercentage < 0.7f)
                {
                    possiblePawns.RemoveAt(i);
                }
            }
            return possiblePawns;
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return ModLister.BiotechInstalled && Candidates().Count > 0;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (!Candidates().TryRandomElement(out Pawn pawn))
            {
                return false;
            }
            ChoiceLetter_PersonalityShiftRequest choiceLetter = (ChoiceLetter_PersonalityShiftRequest)LetterMaker.MakeLetter(ABF_LetterDefOf.ABF_Letter_Synstruct_PersonalityShiftRequest);
            choiceLetter.subject = pawn;
            choiceLetter.Label = "ABF_PersonalityShiftFreewilled".Translate();
            choiceLetter.Text = "ABF_PersonalityShiftFreewilledDesc".Translate(pawn);
            choiceLetter.lookTargets = pawn;
            Find.LetterStack.ReceiveLetter(choiceLetter);
            return true;
        }
    }
}
