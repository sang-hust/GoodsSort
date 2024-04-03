using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public Transform ParentLayerItemHighest;
    private WinLoseManager winLoseManager;

    protected override void Awake()
    {
        base.Awake();
        winLoseManager = GetComponent<WinLoseManager>();
    }
}
