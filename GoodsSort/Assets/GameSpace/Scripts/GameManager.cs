using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public WinLoseManager winLoseManager;
    [HideInInspector] public List<SelfBehavior> ListSelf = new List<SelfBehavior>();
    public bool CheckDoneLayer(List<ItemBehavior> listItem)
    {
        return listItem.All(item => item.ItemTypeEnum == listItem[0].ItemTypeEnum);
    }
}
