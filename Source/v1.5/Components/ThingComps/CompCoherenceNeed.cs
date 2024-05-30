using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public class CompCoherenceNeed : ThingComp
    {
        Pawn Pawn => (Pawn)parent;

        private static float ThresholdCritical => 0.1f;

        private static float ThresholdPoor => 0.3f;

        private static float ThresholdSatisfactory => 0.7f;

        public static readonly List<float> CoherenceThresholdBandPercentages = new List<float> { 0f, ThresholdCritical, ThresholdPoor, ThresholdSatisfactory, 1f };

        private static readonly float TicksPerDay = 60000;

        private static readonly float TicksPerLong = 2000;

        public ABF_CoherenceStage Stage
        {
            get
            {
                if (coherenceLevel < ThresholdCritical)
                    return ABF_CoherenceStage.Critical;
                else if (coherenceLevel < ThresholdPoor)
                    return ABF_CoherenceStage.Poor;
                else if (coherenceLevel < ThresholdSatisfactory)
                    return ABF_CoherenceStage.Sufficient;
                return ABF_CoherenceStage.Satisfactory;
            }
        }

        public float CoherenceLevel
        {
            get
            {
                return coherenceLevel;
            }
        }

        public float TargetCoherenceLevel
        {
            get
            {
                return targetLevel;
            }
            set
            {
                targetLevel = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private float DailyFallPerStage(ABF_CoherenceStage stage)
        {
            switch (stage)
            {
                case ABF_CoherenceStage.Critical:
                    return 0.05f; // 5% per day base (10 -> 0 should take 2 day with 1 efficiency)
                case ABF_CoherenceStage.Poor:
                    return 0.10f; // 10% per day base (30 -> 10 should take 2 days with 1 efficiency)
                case ABF_CoherenceStage.Sufficient:
                    return 0.20f; // 20% per day base (70 -> 30 should take 2 days with 1 efficiency)
                default:
                    return 0.30f; // 30% per day base (100 -> 70 should take 1 day with 1 efficiency)
            }
        }

        public float CoherenceFallPerDay()
        {
            return Mathf.Clamp(DailyFallPerStage(Stage) / Pawn.GetStatValue(ABF_StatDefOf.ABF_CoherenceRetention, cacheStaleAfterTicks: 2000), 0.005f, 2f);
        }

        public override void PostPostMake()
        {
            base.PostPostMake();
            if (coherenceLevel < 0)
            {
                coherenceLevel = 0.6f;
            }
            if (targetLevel < 0)
            {
                targetLevel = 0.5f;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref coherenceLevel, "ABF_coherenceLevel", -1);
            Scribe_Values.Look(ref targetLevel, "ABF_targetLevel", -1);
            Scribe_Values.Look(ref cachedFallRatePerDay, "ABF_cachedFallRatePerDay", -1);
            Scribe_Values.Look(ref coherenceEffectTicks, "ABF_coherenceEffectTicks", TicksPerDay);
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (!Pawn.Spawned || Find.TickManager.TicksGame % 2000 != 0)
            {
                return;
            }

            // Recache fall rate every in game day.
            if (Find.TickManager.TicksGame % 60000 == 0)
            {
                cachedFallRatePerDay = CoherenceFallPerDay();
            }

            TryCoherenceCheck();

            ChangeCoherenceEffectTicks();
            ChangeCoherenceLevel(-cachedFallRatePerDay * TicksPerLong / TicksPerDay);
        }

        // Alter the coherence level by the provided amount (decreases are assumed to be negative). Ensure the level never falls outside 0 - 1 range and handle stage changes appropriately.
        public void ChangeCoherenceLevel(float baseChange)
        {
            ABF_CoherenceStage currentStage = Stage;
            coherenceLevel = Mathf.Clamp(coherenceLevel + baseChange, 0f, 1f);

            // If we changed stages, make sure we initialize an appropriate stage effect hediff if there is one. They remove themselves automatically when appropriate.
            if (Stage != currentStage)
            {
                // Check all coherence stage effects for this race and apply them if the new stage matches their required stage.
                foreach (HediffDef hediffDef in SC_Utils.GetCoherenceEffects(Pawn.def))
                {
                    ABF_CoherenceEffectExtension effectExtension = hediffDef.GetModExtension<ABF_CoherenceEffectExtension>();
                    if (effectExtension.isCoherenceStageEffect && effectExtension.requiredCoherenceStageToOccur == Stage)
                    {
                        Pawn.health.AddHediff(hediffDef);
                    }
                }
            }
        }

        /// <summary>
        /// Alter the coherence effect ticks based on the provided tick rate. Actual effect is changed based on the pawn's coherence level.
        /// If less than 0.3 (poor coherence), effect trends negatively, and if higher than 0.7, effect trends positively.
        /// Between 0.3 and 0.7, effect trends toward 0 days, and does not change if between -60000 and 60000 already.
        /// </summary>
        public void ChangeCoherenceEffectTicks()
        {
            if (coherenceLevel < 0.3f)
            {
                coherenceEffectTicks -= TicksPerLong;

                if (coherenceEffectTicks > 0)
                {
                    coherenceEffectTicks -= coherenceEffectTicks * 0.001f;
                }
            }
            else if (coherenceLevel > 0.7f)
            {
                coherenceEffectTicks += TicksPerLong;

                if (coherenceEffectTicks < 0)
                {
                    coherenceEffectTicks -= coherenceEffectTicks * 0.001f;
                }
            }
            else
            {
                coherenceEffectTicks = Mathf.MoveTowards(coherenceEffectTicks, 0, (Mathf.Log((Mathf.Abs(coherenceEffectTicks) / TicksPerDay) + 2, 2) - 1) * TicksPerLong);
            }
            // Prevent the ticks from going outside a 60 day value positively or negatively.
            coherenceEffectTicks = Mathf.Clamp(coherenceEffectTicks, -3600000, 3600000);
        }

        // Coherence need has associated gizmos for displaying and controlling the coherence level of pawns.
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Find.Selector.SingleSelectedThing == parent)
            {
                Gizmo_CoherenceStatus coherenceStatusGizmo = new Gizmo_CoherenceStatus
                {
                    coherenceNeed = this
                };
                yield return coherenceStatusGizmo;

                Gizmo_CoherenceEffect coherenceEffectGizmo = new Gizmo_CoherenceEffect
                {
                    coherenceNeed = this
                };
                yield return coherenceEffectGizmo;
            }
        }

        public string CoherenceTipString()
        {
            if (coherenceLevelInfoCached == null)
            {
                for (int stageInt = 0; stageInt < CoherenceThresholdBandPercentages.Count - 1; stageInt++)
                {
                    coherenceLevelInfoCached += "ABF_CoherenceLevelInfoRange".Translate((CoherenceThresholdBandPercentages[stageInt] * 100f).ToStringDecimalIfSmall(), (CoherenceThresholdBandPercentages[stageInt + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "ABF_CoherenceLevelInfoFallRate".Translate(DailyFallPerStage((ABF_CoherenceStage)stageInt).ToStringPercent()) + "\n";
                }
            }
            return (("ABF_CoherenceGizmoLabel".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor) + CoherenceLevel.ToStringPercent("0.#") + "\n" + "ABF_CoherenceTargetLabel".Translate() + ": " + TargetCoherenceLevel.ToStringPercent("0.#") + "\n\n" + "ABF_CoherenceTargetLabelDesc".Translate() + "\n\n" + "ABF_CoherenceDesc".Translate() + ":\n\n" + coherenceLevelInfoCached).Resolve();
        }

        // Randomly applies coherence effects based on random chances from the ticksSincePoorCoherence level.
        public void TryCoherenceCheck()
        {
            // Check all valid coherence effects for this race and test whether they are applied now.
            foreach (HediffDef hediffDef in SC_Utils.GetCoherenceEffects(Pawn.def))
            {
                ABF_CoherenceEffectExtension effectExtension = hediffDef.GetModExtension<ABF_CoherenceEffectExtension>();
                // Coherence stage effects are applied/removed automatically on stage changes.
                if (effectExtension.isCoherenceStageEffect)
                {
                    continue;
                }

                // If there is a custom coherence worker that blocks this effect on the pawn, it can not occur.
                if (!effectExtension.CoherenceWorkers.NullOrEmpty() && effectExtension.CoherenceWorkers.Any(coherenceWorker => !coherenceWorker.CanApplyTo(Pawn)))
                {
                    continue;
                }

                // Effects requiring negative coherence
                if (effectExtension.daysBeforeCanOccur < 0)
                {
                    // Stage is too high to occur now.
                    if (Stage > effectExtension.requiredCoherenceStageToOccur)
                    {
                        continue;
                    }

                    // If coherence effect ticks is higher than days before can occur, it can not occur (both are negative here).
                    if (coherenceEffectTicks > effectExtension.daysBeforeCanOccur * 60000)
                    {
                        continue;
                    }

                    TryCoherenceEffect(effectExtension, coherenceEffectTicks, Pawn, hediffDef);
                }
                // Effects requiring positive coherence
                else
                {
                    // Stage is too low to occur now.
                    if (Stage < effectExtension.requiredCoherenceStageToOccur)
                    {
                        continue;
                    }

                    // If coherence effect ticks is lower than days before can occur, it can not occur (both are positive here).
                    if (coherenceEffectTicks < effectExtension.daysBeforeCanOccur * 60000)
                    {
                        continue;
                    }

                    TryCoherenceEffect(effectExtension, coherenceEffectTicks, Pawn, hediffDef);
                }
            }
        }

        public static void TryCoherenceEffect(ABF_CoherenceEffectExtension effectExtension, float effectTicks, Pawn pawn, HediffDef hediffDef)
        {
            // Check the chance to occur based on the extensions curve, with 0 corresponding to the days before it can occur. If it should, apply the appropriate effects.
            // For example, if 4 average coherence days must pass before an effect is applied, and 5 days have passed, the curve will evaluate at 1.
            if (Rand.MTBEventOccurs(effectExtension.meanDaysToOccur.Evaluate(Math.Abs(effectTicks) - Math.Abs(effectExtension.daysBeforeCanOccur)), TicksPerDay, 60f))
            {
                HashSet<BodyPartRecord> validParts = ValidBodyPartsForEffect(effectExtension, pawn);
                // If there are no legal parts identified, this effect can not occur.
                if (validParts == null)
                {
                    return;
                }

                // If the HashSet is empty, that means the effect should be applied to the whole body.
                if (validParts.Count == 0)
                {
                    pawn.health.AddHediff(hediffDef);
                    Hediff chosenHediff = HediffMaker.MakeHediff(hediffDef, pawn);
                    if (effectExtension.CoherenceWorkers.NullOrEmpty())
                    {
                        foreach (CoherenceWorker worker in effectExtension.CoherenceWorkers)
                        {
                            worker.OnApplied(pawn, null);
                        }
                    }
                    SendCoherenceEffectLetter(pawn, chosenHediff);
                }
                else
                {
                    BodyPartRecord chosenPart = validParts.RandomElement();
                    Hediff chosenHediff = HediffMaker.MakeHediff(hediffDef, pawn, chosenPart);
                    pawn.health.AddHediff(chosenHediff, chosenPart);
                    if (!effectExtension.CoherenceWorkers.NullOrEmpty())
                    {
                        foreach (CoherenceWorker worker in effectExtension.CoherenceWorkers)
                        {
                            worker.OnApplied(pawn, chosenPart);
                        }
                    }
                    SendCoherenceEffectLetter(pawn, chosenHediff);
                }
            }
        }

        public static HashSet<BodyPartRecord> ValidBodyPartsForEffect(ABF_CoherenceEffectExtension effectExtension, Pawn pawn)
        {
            // If parts to affect, tags to affect, and depth are all unspecified and thus no parts have been identified yet, the effect may be applied to the whole body, signified by an empty hash set.
            if (effectExtension.partsToAffect == null && effectExtension.bodyPartTagsToAffect == null && effectExtension.partDepthToAffect == BodyPartDepth.Undefined)
            {
                return new HashSet<BodyPartRecord>();
            }

            HashSet<BodyPartRecord> validParts = new HashSet<BodyPartRecord>();
            List<BodyPartRecord> pawnParts = pawn.RaceProps.body.AllParts;
            foreach (BodyPartRecord part in pawnParts)
            {
                // Missing parts are skipped entirely and are always illegal.
                if (pawn.health.hediffSet.PartIsMissing(part))
                {
                    continue;
                }

                // If the coherence worker indicates this part is illegal now, skip it.
                if (!effectExtension.CoherenceWorkers.NullOrEmpty() && effectExtension.CoherenceWorkers.Any(coherenceWorker => coherenceWorker.CanApplyOnPart(pawn, part) == false))
                {
                    continue;
                }

                // If this specific part is legal, add it.
                if (effectExtension.partsToAffect?.Contains(part.def) == true)
                {
                    validParts.Add(part);
                    continue;
                }

                // If this part has the correct tag, add it.
                if (effectExtension.bodyPartTagsToAffect != null && part.def.tags != null && effectExtension.bodyPartTagsToAffect.Any(tag => part.def.tags.Contains(tag)))
                {
                    validParts.Add(part);
                    continue;
                }

                // If this part has the correct depth, add it.
                if (effectExtension.partDepthToAffect == part.depth)
                {
                    validParts.Add(part);
                }
            }
            if (validParts.Count > 0)
            {
                return validParts;
            }
            // If no parts were identified and the effect can't be applied to the whole body, return null to signify there are no valid ways to use this effect.
            return null;
        }

        // Send a letter to the player about an effect being applied to the pawn if appropriate.
        public static void SendCoherenceEffectLetter(Pawn pawn, Hediff cause)
        {
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                Find.LetterStack.ReceiveLetter("ABF_CoherenceEffectOccurredLetterLabel".Translate(pawn.LabelShort, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "ABF_CoherenceEffectOccurredLetter".Translate(pawn.LabelShortCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, pawn);
            }
        }

        private float coherenceLevel = -1;
        public float targetLevel = -1;
        private float cachedFallRatePerDay = 0.1f;
        public float coherenceEffectTicks = TicksPerDay;
        public static string coherenceLevelInfoCached;
    }
}