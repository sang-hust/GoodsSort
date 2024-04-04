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
}
