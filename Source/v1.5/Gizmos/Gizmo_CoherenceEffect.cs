using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public class Gizmo_CoherenceEffect : Gizmo
    {
        public CompCoherenceNeed coherenceNeed;

        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

        public Gizmo_CoherenceEffect()
        {
            Order = -95f;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect overRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Find.WindowStack.ImmediateWindow(137486536, overRect, WindowLayer.GameUI, delegate
            {
                Rect firstRect = overRect.AtZero().ContractedBy(6f);
                firstRect.height = overRect.height / 2f;
                Text.Font = GameFont.Tiny;
                Widgets.Label(firstRect, "ABF_CoherenceEffectGizmoLabel".Translate(coherenceNeed.coherenceEffectTicks < 0 ? "ABF_CoherenceEffectGizmoLabelNegative".Translate() : "ABF_CoherenceEffectGizmoLabelPositive".Translate(), Mathf.RoundToInt(Mathf.Abs(coherenceNeed.coherenceEffectTicks / 60000))));
                Rect secondRect = overRect.AtZero().ContractedBy(6f);
                secondRect.yMin = overRect.height / 2f;
                float fillPercent = Mathf.Clamp(Mathf.Abs(coherenceNeed.coherenceEffectTicks / 60000) / 15f, 0, 1);
                Widgets.FillableBar(secondRect, fillPercent, FullBarTex, EmptyBarTex, doBorder: true);
                TooltipHandler.TipRegion(secondRect, () => "ABF_CoherenceEffectDesc".Translate(), Gen.HashCombineInt(coherenceNeed.GetHashCode(), 283641));
            });
            return new GizmoResult(GizmoState.Clear);
        }
    }
}