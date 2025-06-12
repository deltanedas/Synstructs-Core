using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class IngestionOutcomeDoer_SplitEffects : IngestionOutcomeDoer
    {
        public HediffDef organicEffect = new HediffDef();
        public ChemicalDef organicTolerance;
        public float organicSeverity = -1f;
        public bool useOrganicGeneToleranceFactors;

        public HediffDef artificialEffect = new HediffDef();
        public ChemicalDef artificialTolerance;
        public float artificialSeverity = -1;

        public bool divideByBodySize;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Hediff hediff;
            if (!ABF_Utils.IsArtificial(pawn))
            {
                hediff = HediffMaker.MakeHediff(organicEffect, pawn);
                float severity = (organicSeverity > 0) ? organicSeverity : organicEffect.initialSeverity;
                AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, organicTolerance, ref severity, useOrganicGeneToleranceFactors, divideByBodySize);
                hediff.Severity = severity;
                pawn.health.AddHediff(hediff);
            }
            else
            {
                hediff = HediffMaker.MakeHediff(artificialEffect, pawn);
                float severity = (artificialSeverity > 0) ? artificialSeverity : artificialEffect.initialSeverity;
                AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, artificialTolerance, ref severity, false, divideByBodySize);
                hediff.Severity = severity;
                pawn.health.AddHediff(hediff);
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && chance >= 1f)
            {
                foreach (StatDrawEntry item in organicEffect.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return item;
                }
                foreach (StatDrawEntry item in artificialEffect.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return item;
                }
            }
        }
    }
}


