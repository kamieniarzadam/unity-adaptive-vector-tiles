using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(VectorShapeController))]
public class SectorContourHandler<T> : MonoBehaviour, HierarchyMsg<TileSectorLeg>.IHandler
    where T : SectorContourBuilder, new()
{
    protected T contourBuilder;

    void Awake()
    {
        contourBuilder = new T();
        HierarchyMsg<TileSectorLeg>.Request(this);
    }

    protected void SetContour()
    {
        GetComponent<VectorShapeController>().SetContour(GetInstanceID(), contourBuilder.Build());
    }

    protected void UnsetContour()
    {
        GetComponent<VectorShapeController>().UnsetContour(GetInstanceID());
    }

    protected virtual void OnEnable()
    {
        SetContour();
    }

    void OnDisable()
    {
        UnsetContour();
    }

    void HierarchyMsg<TileSectorLeg>.IHandler.Handle(TileSectorLeg payload)
    {
        contourBuilder.SetTileSectorLeg(payload.tileSectorLeg);
    }
}
