using UnityEngine;

[ExecuteAlways]
public class TestManualSectorBaseContactWidthProvider : Poller<float>
    , HierarchyMsg<SectorBaseContactWidth>.IProvider
{
    [Range(0.0f, 1.0f)]
    public float tileSectorBaseContactWidth = 0.7f;

    protected override float CurrentValue
    {
        get => tileSectorBaseContactWidth;
    }

    protected override float LastValue
    {
        set => HierarchyMsg<SectorBaseContactWidth>.Publish(this);
    }

    SectorBaseContactWidth HierarchyMsg<SectorBaseContactWidth>.IProvider.Provide()
    {
        return new SectorBaseContactWidth { scale = LastValue };
    }
}
