using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private List<ItemBehavior> _listItem; // get from SO
    [SerializeField] private List<SpaceBehavior> _listSpace;

    private void Start()
    {
        _listItem = new List<ItemBehavior>();
        
        for (var i = 0; i < _listItem.Count; i++)
        {
            _listSpace[i].InitSpace(i, _listItem[i]._itemTypeEnum);
        }

    }

    public bool AvailableEmptySpace()
    {
        return _listSpace.Any(space => space.IsAvailable());
    }

    public void FillItemToSpace(int indexSpace, ItemTypeEnum itemTypeEnum)
    {
        //_listSpace[]
    }
}
