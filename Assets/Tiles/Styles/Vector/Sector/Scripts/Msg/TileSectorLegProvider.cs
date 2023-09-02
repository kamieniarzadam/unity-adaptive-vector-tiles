using UnityEngine;

[ExecuteAlways]
public class TileSectorLegProvider : MonoBehaviour
    , HierarchyMsg<TileSectorLeg>.IProvider
{
    private TileSector sector;

    TileSectorLeg HierarchyMsg<TileSectorLeg>.IProvider.Provide() => new TileSectorLeg { tileSectorLeg = sector.GetLegEnd() };

    private void Awake()
    {
        sector = new TileSector(GetComponentInParent<LocationOnTilemapHelper>().NumberOfCellSides, 50);
    }
}
