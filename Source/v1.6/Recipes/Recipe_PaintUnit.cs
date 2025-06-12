using System.Collections.Generic;
using AlienRace;
using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class Recipe_PaintUnit : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            return base.AvailableOnNow(thing, part) && thing.def is ThingDef_AlienRace;
        }

        // On completion, open a dialog menu to select the new paint color.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            Find.WindowStack.Add(new Dialog_Repaint(pawn));
        }
    }
}
