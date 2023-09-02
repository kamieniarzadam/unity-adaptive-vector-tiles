using UnityEngine;
using Unity.VectorGraphics;

public class SectorEdgeConnectedRoundContourBuilder : SectorContourBuilder
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

    float baseScale;

    public bool SetBaseScale(float baseScale)
    {
        if (this.baseScale != baseScale)
        {
            this.baseScale = baseScale;
            return true;
        }
        return false;
    }

    public override BezierContour Build()
    {
        var rightLegEnd = tileSectorLeg * new Vector2(scale, scale);
        var leftLegEnd = rightLegEnd * new Vector2(1, -1);

        var controlPointDistance = 4;
        var rightLegEndControl = new Vector2(rightLegEnd.y, -rightLegEnd.x);
        rightLegEndControl.Normalize();
        rightLegEndControl *= controlPointDistance;
        rightLegEndControl += rightLegEnd;
        var leftLegEndControl = rightLegEndControl * new Vector2(1, -1);

        var baseRightEnd = tileSectorLeg * new Vector2(1, baseScale);
        var baseLeftEnd = baseRightEnd * new Vector2(1, -1);

        var baseRightEndControl = baseRightEnd + new Vector2(-controlPointDistance, 0);
        var baseLeftEndControl = baseRightEndControl * new Vector2(1, -1);

        return new BezierContour()
        {
            Segments = new BezierPathSegment[] {
                new BezierPathSegment() { P0 = leftLegEnd, P1 = leftLegEndControl, P2 = baseLeftEndControl },
                new BezierPathSegment() { P0 = baseLeftEnd, P1 = baseRightEnd, P2 = baseLeftEnd },
                new BezierPathSegment() { P0 = baseRightEnd, P1 = baseRightEndControl, P2 = rightLegEndControl },
                new BezierPathSegment() { P0 = rightLegEnd, P1 = leftLegEnd, P2 = rightLegEnd },
            },
            Closed = true
        };
    }
}

public class SectorEdgeConnectedRoundContourHandler
    : SectorContourHandler<SectorEdgeConnectedRoundContourBuilder>
    , HierarchyMsg<TileScale>.IHandler
    , HierarchyMsg<SectorBaseContactWidth>.IHandler
{
    protected override void OnEnable()
    {
        contourBuilder.SetScale(HierarchyMsg<TileScale>.Get(gameObject).scale);
        contourBuilder.SetBaseScale(HierarchyMsg<SectorBaseContactWidth>.Get(gameObject).scale);
        base.OnEnable();
    }

    void HierarchyMsg<TileScale>.IHandler.Handle(TileScale payload)
    {
        if (contourBuilder.SetScale(payload.scale))
        {
            SetContour();
        }
    }

    void HierarchyMsg<SectorBaseContactWidth>.IHandler.Handle(SectorBaseContactWidth payload)
    {
        if (contourBuilder.SetBaseScale(payload.scale))
        {
            SetContour();
        }
    }
}
