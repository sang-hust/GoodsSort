using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public Transform ParentLayerItemHighest;
    [HideInInspector] public WinLoseManager winLoseManager;
    [HideInInspector] public BoosterManager boosterManager;
    
    public readonly Dictionary<int, SelfBehavior> DictSelf = new Dictionary<int, SelfBehavior>();
    public readonly Dictionary<int, ItemBehavior> DictItem = new Dictionary<int, ItemBehavior>();

    protected override void Awake()
    {
        base.Awake();
        winLoseManager = GetComponent<WinLoseManager>();
        boosterManager = GetComponent<BoosterManager>();
    }

    public void SetUniqueIDSelf(SelfBehavior selfData)
    {
        var id = selfData._indexSelf;
        if (!DictSelf.ContainsKey(id))
        {
            DictSelf.Add(id, selfData);
            return;
        }

        DictSelf[id] = selfData;
    }
    
    public void SetUniqueIDItem()
    {
        var listItem = GetComponentsInChildren<ItemBehavior>();
        DictItem.Clear();
        for (var i = 0; i < listItem.Length; i++)
        {
            DictItem.Add(i, listItem[i]);
            listItem[i].uniqueId = i;
        }

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
