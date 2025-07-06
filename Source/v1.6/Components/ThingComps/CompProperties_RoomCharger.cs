using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class CompProperties_RoomCharger : CompProperties
    {
        public CompProperties_RoomCharger()
        {
            compClass = typeof(CompRoomCharger);
        }

        public bool inoperableInLargeRooms = false;

        public bool inoperableOutdoors = false;

        public float maxChargeRatePerPawnPerDay = 0f;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (maxChargeRatePerPawnPerDay == 0f)
            {
                yield return $"{parentDef.defName} did not specify a maximum charge rate per pawn for its room charger comp. It will not charge pawns!";
            }
        }
    }
}