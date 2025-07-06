using Verse.AI;
using Verse;
using System.Collections.Generic;

namespace ArtificialBeings
{
    public class Directive_Minuteman : Directive
    {
        //public override IEnumerable<Gizmo> GetGizmos()
        //{

        //}

        //public virtual void WakeUp()
        //{
        //    if (Awake)
        //    {
        //        return;
        //    }
        //    wokeUpTick = GenTicks.TicksGame;
        //    wakeUpOnTick = int.MinValue;
        //    Pawn obj = parent as Pawn;
        //    Building building = parent as Building;
        //    (obj?.GetLord() ?? building?.GetLord())?.Notify_DormancyWakeup();
        //    if (parent.Spawned)
        //    {
        //        if (parent is IAttackTarget t)
        //        {
        //            parent.Map.attackTargetsCache.UpdateTarget(t);
        //        }
        //        if (Props.jobDormancy && parent is Pawn pawn && pawn.CurJobDef == SleepJob)
        //        {
        //            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
        //        }
        //    }
        //}

        //public virtual void ToSleep()
        //{
        //    if (!Awake)
        //    {
        //        return;
        //    }
        //    wokeUpTick = int.MinValue;
        //    if (parent.Spawned)
        //    {
        //        if (parent is IAttackTarget t)
        //        {
        //            parent.Map.attackTargetsCache.UpdateTarget(t);
        //        }
        //        if (Props.jobDormancy && parent is Pawn pawn)
        //        {
        //            Job job = JobMaker.MakeJob(SleepJob, pawn.Position);
        //            job.forceSleep = true;
        //            pawn.jobs.StartJob(job, JobCondition.InterruptForced);
        //        }
        //    }
        //}
    }
}
