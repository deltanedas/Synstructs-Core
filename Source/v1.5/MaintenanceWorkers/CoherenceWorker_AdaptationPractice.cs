using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple coherence worker that gives the pawn 4000 xp in crafting and intellectual on application.
    public class CoherenceWorker_AdaptationPractice : CoherenceWorker
    {
        public override void OnApplied(Pawn pawn, BodyPartRecord part)
        {
            if (pawn.skills is Pawn_SkillTracker skillTracker)
            {
                SkillRecord skill = skillTracker.GetSkill(SkillDefOf.Crafting);
                if (!skill.TotallyDisabled)
                {
                    skill.Learn(4000, true);
                }

                skill = skillTracker.GetSkill(SkillDefOf.Intellectual);
                if (!skill.TotallyDisabled)
                {
                    skill.Learn(4000, true);
                }
            }
        }
    }
}
