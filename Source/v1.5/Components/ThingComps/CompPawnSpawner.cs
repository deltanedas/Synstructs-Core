using Verse;
using RimWorld;

namespace ArtificialBeings
{
    public class CompPawnSpawner : ThingComp
    {
        public CompProperties_PawnSpawner Props
        {
            get
            {
                return props as CompProperties_PawnSpawner;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            SpawnPawn();
            parent.Destroy();
        }

        // Fail-safe if an error should happen to occur at spawn, attempt to do it again repeatedly somewhat infrequently until success.
        public override void CompTick()
        {
            if (parent.IsHashIntervalTick(339))
            {
                SpawnPawn();
                parent.Destroy();
            }
        }

        // Generate and spawn the created pawn.
        public virtual Pawn SpawnPawn()
        {
            PawnGenerationRequest request = new PawnGenerationRequest(Props.pawnKind, Find.FactionManager.FirstFactionOfDef(Props.pawnKind.defaultFactionType), PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            // Pawns may sometimes spawn with apparel somewhere in the generation process. Ensure they don't actually spawn with any - if they even can have apparel.
            pawn.apparel?.DestroyAll();

            if (Props.hediffOnSpawn != null)
            {
                pawn.health.AddHediff(Props.hediffOnSpawn);
            }
            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
            if (pawn.Faction == Faction.OfPlayer)
            {
                Find.LetterStack.ReceiveLetter("ABF_NewbootSynstructCreatedLabel".Translate(), "ABF_NewbootSynstructCreatedText".Translate(pawn.def.label), LetterDefOf.PositiveEvent, pawn, hyperlinkThingDefs: new System.Collections.Generic.List<ThingDef> { pawn.def });
            }
            if (Props.mentalStateOnSpawn != null)
            {
                pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateOnSpawn, transitionSilently: true);
            }


            return pawn;
        }
    }
}