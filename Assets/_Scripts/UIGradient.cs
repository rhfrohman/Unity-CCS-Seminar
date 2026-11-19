using UnityEngine;
using UnityEngine.UI;

// Vertical vertex-colour gradient for a uGUI Graphic (works with sliced images).
[RequireComponent(typeof(Graphic))]
public class UIGradient : BaseMeshEffect
{
    public Color top = Color.white;
    public Color bottom = Color.white;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        Rect r = graphic.rectTransform.rect;
        var v = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref v, i);
            float t = Mathf.InverseLerp(r.yMin, r.yMax, v.position.y);
            v.color = (Color)v.color * Color.Lerp(bottom, top, t);
            vh.SetUIVertex(v, i);
        }
    }
}
