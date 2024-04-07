using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public Transform ParentLayerItemHighest;
    [HideInInspector] public WinLoseManager winLoseManager;

    protected override void Awake()
    {
        base.Awake();
        winLoseManager = GetComponent<WinLoseManager>();
    }

    public void CheckDoneLayer(SpaceBehavior spaceStart, SpaceBehavior spaceEnd)
    {
        var indexSelfStart = spaceStart.GetIndexSelf();
        var indexSelfEnd = spaceEnd.GetIndexSelf();
        Debug.LogError("Self Start: " + indexSelfStart);
        Debug.LogError("Self End: " + indexSelfEnd);
        if (indexSelfStart == indexSelfEnd) return;
        
        spaceStart.UpdateLayer();
        spaceEnd.UpdateLayer();
        
    }
}
