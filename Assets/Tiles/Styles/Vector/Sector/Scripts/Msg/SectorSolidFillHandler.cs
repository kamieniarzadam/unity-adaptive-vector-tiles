using UnityEngine;
using Unity.VectorGraphics;

[ExecuteAlways]
[RequireComponent(typeof(VectorShapeController))]
public class SectorSolidFillHandler : MonoBehaviour
    , HierarchyMsg<TileColor>.IHandler
{
    private readonly SolidFill fill = new SolidFill
    {
        Mode = FillMode.NonZero
    };

    void OnEnable()
    {
        HierarchyMsg<TileColor>.Request(this);
        GetComponent<VectorShapeController>().Fill = fill;
    }

    void HierarchyMsg<TileColor>.IHandler.Handle(TileColor payload)
    {
        fill.Color = payload.color;
        GetComponent<VectorShapeController>().SetGeometryDirty();
    }
}
