using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    private List<ItemBehavior> _listItem;
    [SerializeField] private List<SpaceBehavior> _listSpace;

    private void Start()
    {
        // for (var i = 0; i < _listItem.Count; i++)
        // {
        //     _listSpace[i].InitSpace(_listItem[i].);
        // }
    }

    public bool AvailableEmptySpace()
    {
        return _listSpace.Any(space => space.IsAvailable());
    }

    public void FillItemToSpace(ItemTypeEnum itemTypeEnum)
    {
        foreach (var space in _listSpace)
        {
            if (space.IsAvailable())
            {
                space.InitSpace(itemTypeEnum);
                break;
            }
        }
    }
}
