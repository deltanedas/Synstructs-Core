using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public class Gizmo_CoherenceStatus : Gizmo
    {
        public CompCoherenceNeed coherenceNeed;

        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

        private static readonly Texture2D FullHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

        private static readonly Texture2D TargetLevelTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));

        private static bool draggingBar;

        public Gizmo_CoherenceStatus()
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
                Widgets.Label(firstRect, "ABF_CoherenceGizmoLabel".Translate());
                Rect secondRect = overRect.AtZero().ContractedBy(6f);
                secondRect.yMin = overRect.height / 2f;
                float fillPercent = coherenceNeed.CoherenceLevel;
                if (Find.Selector.SingleSelectedThing is Pawn pawn && pawn.IsColonistPlayerControlled)
                {
                    Widgets.DraggableBar(secondRect, FullBarTex, FullHighlightTex, EmptyBarTex, TargetLevelTex, ref draggingBar, fillPercent, ref coherenceNeed.targetLevel, CompCoherenceNeed.CoherenceThresholdBandPercentages);
                }
                else
                {
                    Widgets.FillableBar(secondRect, fillPercent, FullBarTex, EmptyBarTex, doBorder: true);
                }
                TooltipHandler.TipRegion(secondRect, () => coherenceNeed.CoherenceTipString(), Gen.HashCombineInt(coherenceNeed.GetHashCode(), 171495));
            });
            return new GizmoResult(GizmoState.Clear);
        }
    }
}