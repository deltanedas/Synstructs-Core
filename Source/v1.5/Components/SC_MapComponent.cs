using Verse;

namespace ArtificialBeings
{
    public class SC_MapComponent : MapComponent
    {
        public SC_MapComponent(Map map) : base(map)
        {
        }

        public override void MapComponentTick()
        {
            if (map.IsHashIntervalTick(GenTicks.TickLongInterval))
            {
                SC_Utils.UpdateAmicableDroneCount(map);
                SC_Utils.UpdatePlayerOrganicPawnCount(map);
            }
        }
    }
}
