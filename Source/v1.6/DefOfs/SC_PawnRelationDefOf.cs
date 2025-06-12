using RimWorld;

namespace ArtificialBeings
{
    [DefOf]
    public class SC_PawnRelationDefOf
    {
        static SC_PawnRelationDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SC_PawnRelationDefOf));
        }

        public static PawnRelationDef ABF_PawnRelation_Synstruct_Creator;

        public static PawnRelationDef ABF_PawnRelation_Synstruct_Creation;
    }
}
