using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class GradientEffect : BaseMeshEffect
{
    public enum GradientDirection
    {
        Vertical,
        Horizontal,
        DiagonalTopLeftToBottomRight,
        DiagonalTopRightToBottomLeft
    }

    [SerializeField] Gradient _gradient = null;
    [SerializeField] GradientDirection _gradientDirection = GradientDirection.Vertical;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0 || _gradient == null) return;

        UIVertex vertex = new UIVertex();
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        // Calculate min and max bounds
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            minX = Mathf.Min(minX, vertex.position.x);
            maxX = Mathf.Max(maxX, vertex.position.x);
            minY = Mathf.Min(minY, vertex.position.y);
            maxY = Mathf.Max(maxY, vertex.position.y);
        }

        float width = maxX - minX;
        float height = maxY - minY;

        // Apply the gradient
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            float t = 0f; // Normalized position
            switch (_gradientDirection)
            {
                case GradientDirection.Vertical:
                    t = (vertex.position.y - minY) / height;
                    break;
                case GradientDirection.Horizontal:
                    t = (vertex.position.x - minX) / width;
                    break;
                case GradientDirection.DiagonalTopLeftToBottomRight:
                    t = ((vertex.position.x - minX) + (vertex.position.y - minY)) / (width + height);
                    break;
                case GradientDirection.DiagonalTopRightToBottomLeft:
                    t = ((maxX - vertex.position.x) + (vertex.position.y - minY)) / (width + height);
                    break;
            }

            vertex.color = _gradient.Evaluate(t);
            vh.SetUIVertex(vertex, i);
        }
    }
}
