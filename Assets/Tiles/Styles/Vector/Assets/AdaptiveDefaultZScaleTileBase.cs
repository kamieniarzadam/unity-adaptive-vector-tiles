using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class AdaptiveDefaultZScaleTileBase : LocationAwareTileBase
{
    public float defaultScaleZ = 0.7f;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.transform *= Matrix4x4.Scale(new Vector3(1f, 1f, defaultScaleZ));
        base.GetTileData(position, tilemap, ref tileData);
    }
}
