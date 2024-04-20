using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public Transform ParentLayerItemHighest;
    [HideInInspector] public WinLoseManager winLoseManager;
    [HideInInspector] public BoosterManager boosterManager;

    protected override void Awake()
    {
        base.Awake();
        winLoseManager = GetComponent<WinLoseManager>();
        boosterManager = GetComponent<BoosterManager>();
    }

    public void CheckDoneLayer(SpaceBehavior spaceStart, SpaceBehavior spaceEnd)
    {
        var indexSelfStart = spaceStart.GetIndexSelf();
        var indexSelfEnd = spaceEnd.GetIndexSelf();
        if (indexSelfStart == indexSelfEnd) return;
        
        spaceStart.UpdateLayer();
        spaceEnd.UpdateLayer();
        
    }

    public void OnReplay()
    {
        LevelManager.Instance.OnReplay();
    }
}
