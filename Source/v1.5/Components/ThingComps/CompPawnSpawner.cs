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
            Faction faction;
            if (Props.pawnKind.defaultFactionType.isPlayer)
            {
                faction = Faction.OfPlayer;
            }
            else
            {
                faction = Find.FactionManager.FirstFactionOfDef(Props.pawnKind.defaultFactionType);
            }
            PawnGenerationRequest request = new PawnGenerationRequest(Props.pawnKind, faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 20, fixedChronologicalAge: 0, fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            // Pawns may sometimes spawn with apparel somewhere in the generation process. Ensure they don't actually spawn with any - if they even can have apparel.
            pawn.apparel?.DestroyAll();

            if (Props.hediffOnSpawn != null)
            {
                pawn.health.AddHediff(Props.hediffOnSpawn);
            }
            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
            if (pawn.Faction == Faction.OfPlayer && Props.pawnKind.GetModExtension<ABF_ArtificialPawnKindExtension>() is ABF_ArtificialPawnKindExtension ext)
            {
                if (ext.pawnState == ABF_ArtificialState.Blank)
                {
                    Find.LetterStack.ReceiveLetter("ABF_NewbootSynstructCreatedLabel".Translate(), "ABF_NewbootSynstructCreatedText".Translate(pawn.def.label), LetterDefOf.PositiveEvent, pawn, hyperlinkThingDefs: new System.Collections.Generic.List<ThingDef> { pawn.def });
                }
                else
                {
                    Find.LetterStack.ReceiveLetter("ABF_NewbootSynstructCreatedLabel".Translate(), "ABF_NewbootSynstructCreatedTextSimple".Translate(pawn.def.label), LetterDefOf.PositiveEvent, pawn, hyperlinkThingDefs: new System.Collections.Generic.List<ThingDef> { pawn.def });
                }
                if (Props.mentalStateOnSpawn != null)
                {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateOnSpawn, transitionSilently: true);
                }
            }

            return pawn;
        }
    }
}