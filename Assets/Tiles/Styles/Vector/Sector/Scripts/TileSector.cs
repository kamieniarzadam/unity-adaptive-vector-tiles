using UnityEngine;

public class TileSector
{
    private readonly float angleBetweenLegs;
    protected readonly float baseLength;

    protected readonly float altitude;
    protected readonly float legLength;

    protected readonly Vector2 legEnd;

    public TileSector(int numberOfSides, float apothemLength)
    {
        angleBetweenLegs = 360f / numberOfSides;
        altitude = apothemLength / 2;
        baseLength = altitude * 2 * Mathf.Tan(Mathf.PI / numberOfSides);

        //tileSideLength / 2 / Mathf.Tan(Mathf.PI / numberOfTileSides);
        legLength = baseLength / 2 / Mathf.Sin(Mathf.PI / numberOfSides);

        legEnd = new Vector2(altitude, baseLength / 2);
    }

    public virtual Vector2 GetLegEnd()
    {
        return legEnd;
    }
}