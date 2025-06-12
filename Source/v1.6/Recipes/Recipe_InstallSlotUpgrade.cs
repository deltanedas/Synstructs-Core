using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ArtificialBeings
{
    // Slot upgrades occupy the slot they are in, and are mutually exclusive with all other Hediffs of the same class on that part.
    public class Recipe_InstallSlotUpgrade : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!base.AvailableOnNow(thing, part)) return false;

            return base.AvailableOnNow(thing, part) && thing is Pawn pawn && !pawn.health.hediffSet.hediffs.Any(hediff => hediff.def.hediffClass == recipe.addsHediff?.hediffClass);
        }

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            if (recipe.targetsBodyPart)
            {
                foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts())
                {
                    if (recipe.appliedOnFixedBodyParts.Contains(bodyPartRecord.def))
                    {
                        yield return bodyPartRecord;
                    }
                }
            }
            else
            {
                foreach (BodyPartRecord bodyPartRecord in base.GetPartsToApplyOn(pawn, recipe))
                {
                    yield return bodyPartRecord;
                }
            }
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            pawn.health.AddHediff(bill.recipe.addsHediff, part);
        }
    }
}

