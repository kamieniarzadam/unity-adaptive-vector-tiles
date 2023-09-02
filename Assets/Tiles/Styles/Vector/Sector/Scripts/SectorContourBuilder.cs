using UnityEngine;
using Unity.VectorGraphics;

[ExecuteAlways]
public abstract class SectorContourBuilder
{
    protected Vector2 tileSectorLeg;

    public void SetTileSectorLeg(Vector2 tileSectorLeg)
    {
        this.tileSectorLeg = tileSectorLeg;
    }

    public abstract BezierContour Build();
}
