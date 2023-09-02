using UnityEngine;
using Unity.VectorGraphics;

[ExecuteAlways]
[RequireComponent(typeof(VectorShapeController))]
public class SectorGradientFillHandler
    : MonoBehaviour
    , HierarchyMsg<TileScale>.IHandler
    , HierarchyMsg<TileColor>.IHandler
    , HierarchyMsg<TileNeighborColor>.IHandler
{
    private readonly GradientFill fill = new GradientFill
    {
        Type = GradientFillType.Linear,
        Mode = FillMode.NonZero,
        Addressing = AddressMode.Mirror,
        Stops = new GradientStop[] {
                new GradientStop() { StopPercentage = 0f },
                new GradientStop() { StopPercentage = 1.0f },
                new GradientStop() { StopPercentage = 1.0f },
            }
    };

    void OnEnable()
    {
        HierarchyMsg<TileScale>.Request(this);
        HierarchyMsg<TileColor>.Request(this);
        HierarchyMsg<TileNeighborColor>.Request(this);
        GetComponent<VectorShapeController>().Fill = fill;
    }

    void HierarchyMsg<TileScale>.IHandler.Handle(TileScale payload)
    {
        fill.Stops[1].StopPercentage = payload.scale;
        fill.Stops[2].StopPercentage = 2 - payload.scale;
        GetComponent<VectorShapeController>().SetFillDirty();
    }

    void HierarchyMsg<TileColor>.IHandler.Handle(TileColor payload)
    {
        fill.Stops[0].Color = payload.color;
        fill.Stops[1].Color = payload.color;
        GetComponent<VectorShapeController>().SetFillDirty();
    }

    void HierarchyMsg<TileNeighborColor>.IHandler.Handle(TileNeighborColor payload)
    {
        fill.Stops[2].Color = payload.color;
        GetComponent<VectorShapeController>().SetFillDirty();
    }
}
