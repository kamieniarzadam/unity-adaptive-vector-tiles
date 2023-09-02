using UnityEngine;

[ExecuteAlways]
public class SectorColorBasedConnectionCombiner
    : MonoBehaviour
    , HierarchyMsg<TileColor>.IHandler
    , HierarchyMsg<TileNeighborColor>.IHandler
    , HierarchyMsg<SectorBaseContactWidth>.IProvider
{
    private Color tileColor;
    private Color neighborColor;

    void HierarchyMsg<TileColor>.IHandler.Handle(TileColor payload)
    {
        tileColor = payload.color;
        HierarchyMsg<SectorBaseContactWidth>.Publish(this);
    }

    void HierarchyMsg<TileNeighborColor>.IHandler.Handle(TileNeighborColor payload)
    {
        neighborColor = payload.color;
        HierarchyMsg<SectorBaseContactWidth>.Publish(this);
    }

    SectorBaseContactWidth HierarchyMsg<SectorBaseContactWidth>.IProvider.Provide()
    {
        tileColor = HierarchyMsg<TileColor>.Get(gameObject).color;
        neighborColor = HierarchyMsg<TileNeighborColor>.Get(gameObject).color;
        float tileColorH, tileColorS, tileColorV;
        Color.RGBToHSV(tileColor, out tileColorH, out tileColorS, out tileColorV);
        float neighborColorH, neighborColorS, neighborColorV;
        Color.RGBToHSV(neighborColor, out neighborColorH, out neighborColorS, out neighborColorV);

        var hueDiff = Mathf.Abs(tileColorH - neighborColorH);
        var hueDist = hueDiff > 0.5f ? 1.0f - hueDiff : hueDiff;

        return new SectorBaseContactWidth { scale = 0.75f - hueDist };
    }
}
