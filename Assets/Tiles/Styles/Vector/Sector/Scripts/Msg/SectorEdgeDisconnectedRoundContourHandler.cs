using UnityEngine;
using Unity.VectorGraphics;

public class SectorEdgeDisconnectedRoundContourBuilder : SectorContourBuilder
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
        var rightLegEnd = tileSectorLeg * new Vector2(scale, scale);
        var leftLegEnd = rightLegEnd * new Vector2(1, -1);

        var controlPointDistance = 2;
        var rightLegEndControl = new Vector2(rightLegEnd.y, -rightLegEnd.x);
        rightLegEndControl.Normalize();
        rightLegEndControl *= controlPointDistance;
        rightLegEndControl += rightLegEnd;
        var leftLegEndControl = rightLegEndControl * new Vector2(1, -1);

        var baseRightEnd = rightLegEndControl + new Vector2(0, -controlPointDistance);
        var baseLeftEnd = baseRightEnd * new Vector2(1, -1);

        var baseRightEndControl = rightLegEndControl;
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

public class SectorEdgeDisconnectedRoundContourHandler
    : SectorContourHandler<SectorEdgeDisconnectedRoundContourBuilder>
    , HierarchyMsg<TileScale>.IHandler
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
