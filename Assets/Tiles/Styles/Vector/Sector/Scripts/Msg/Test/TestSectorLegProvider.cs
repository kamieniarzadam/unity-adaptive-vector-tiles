using UnityEngine;

[ExecuteAlways]
public class TestSectorLegProvider : MonoBehaviour, HierarchyMsg<TileSectorLeg>.IProvider
{
    TileSector sector = new TileSector(6, 50);

    TileSectorLeg HierarchyMsg<TileSectorLeg>.IProvider.Provide()
    {
        return new TileSectorLeg { tileSectorLeg = sector.GetLegEnd() };
    }
}
