using UnityEngine;
using Unity.VectorGraphics;

public class SectorCenterContourBuilder : SectorContourBuilder
{
    float scale;

    public bool SetScale(float scale)
    {
        if (this.scale != scale)
        {
            this.scale = scale;
            return true;
        }
        return false;
    }

    public override BezierContour Build()
    {
        var rightLegEnd = tileSectorLeg * scale;
        var leftLegEnd = rightLegEnd * new Vector2(1, -1);
        return new BezierContour()
        {
            Segments = new BezierPathSegment[] {
                new BezierPathSegment() { P0 = Vector2.zero, P1 = rightLegEnd },
                new BezierPathSegment() { P0 = rightLegEnd, P1 = leftLegEnd, P2 = rightLegEnd },
                new BezierPathSegment() { P0 = leftLegEnd, P2 = leftLegEnd },
            },
            Closed = true
        };
    }
}

public class SectorCenterContourHandler : SectorContourHandler<SectorCenterContourBuilder>, HierarchyMsg<TileScale>.IHandler
{
    protected override void OnEnable()
    {
        contourBuilder.SetScale(HierarchyMsg<TileScale>.Get(gameObject).scale);
        base.OnEnable();
    }

    void HierarchyMsg<TileScale>.IHandler.Handle(TileScale payload)
    {
        if (contourBuilder.SetScale(payload.scale))
        {
            SetContour();
        }
    }
}
