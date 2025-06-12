using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public class Gizmo_EnergyReservoir : Gizmo
    {
        public CompEnergyReservoir compEnergyReservoir;

        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

        public Gizmo_EnergyReservoir()
        {
            Order = -100f;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect overRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Find.WindowStack.ImmediateWindow(251714293, overRect, WindowLayer.GameUI, delegate
            {
                Rect firstRect = overRect.AtZero().ContractedBy(6f);
                firstRect.height = overRect.height / 2f;
                Text.Font = GameFont.Tiny;
                Widgets.Label(firstRect, "ABF_ReservoirGizmoLabel".Translate());
                Rect secondRect = overRect.AtZero().ContractedBy(6f);
                secondRect.yMin = overRect.height / 2f;
                float fillPercent = compEnergyReservoir.reserve / compEnergyReservoir.Props.maximumReserve;
                Widgets.FillableBar(secondRect, fillPercent, FullBarTex, EmptyBarTex, doBorder: true);
                TooltipHandler.TipRegion(secondRect, () => "ABF_ReservoirLevel".Translate(compEnergyReservoir.reserve.ToString(format: "F2")), Gen.HashCombineInt(compEnergyReservoir.GetHashCode(), 213523));
            });
            return new GizmoResult(GizmoState.Clear);
        }
    }
}