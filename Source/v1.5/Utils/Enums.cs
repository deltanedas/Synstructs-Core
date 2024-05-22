using System;

namespace ArtificialBeings
{
    [Flags]
    public enum ABF_PawnType
    {
        None = 0b_0000_0000,  // 0
        Drone = 0b_0000_0001,  // 1
        Sapient = 0b_0000_0010,  // 2
        Mechanical = Drone | Sapient, // 3
        Organic = 0b_0000_0100,  // 4
        NonAI = Drone | Organic, // 5
        Autonomous = Sapient | Organic, // 6
        All = Drone | Sapient | Organic // 7
    }

    public enum ABF_CoherenceStage
    {
        Critical,
        Poor,
        Sufficient,
        Satisfactory
    }
}
