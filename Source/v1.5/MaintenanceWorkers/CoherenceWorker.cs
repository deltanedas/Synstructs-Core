using Verse;

namespace ArtificialBeings
{
    public abstract class CoherenceWorker
    {
        public HediffDef def;

        public ABF_CoherenceEffectExtension effecter;

        // Method for identifying what coherence effects are applicable to a race. CompCoherenceNeed caches valid Hediffs for pawns based on this.
        public virtual bool CanEverApplyTo(ThingDef thingDef)
        {
            return true;
        }

        // Method for identifying whether the coherence effect may be applied to a particular pawn right now.
        public virtual bool CanApplyTo(Pawn pawn)
        {
            return CanEverApplyTo(pawn.def);
        }

        // Method for identifying whether the coherence effect may be applied to a particular pawn's part right now.
        public virtual bool CanApplyOnPart(Pawn pawn, BodyPartRecord part)
        {
            return true;
        }

        // Method for specifying special code to be done when the coherence effect is first applied. It is called after the Hediff is applied. Part can be null if applied to the whole body.
        public virtual void OnApplied(Pawn pawn, BodyPartRecord part)
        {
        }
    }
}
