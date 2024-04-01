using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    private int _indexSpace;
    private bool _available;
    private LayerItemBehavior _layerItemBehavior;
    private ItemBehavior _itemBehavior;

    /// <summary>
    /// Parent always Layer Item
    /// </summary>
    private void Start()
    {
        _layerItemBehavior = GetComponentInParent<LayerItemBehavior>();
    }

    public void InitSpace(int indexSpace)
    {
        _indexSpace = indexSpace;
    }

    public void UpdateData(bool itemBehavior)
    {
        _itemBehavior = itemBehavior;
    }
    
    public bool IsAvailable()
    {
        return _itemBehavior == null;
    }

    public bool ItemCanFillThisLayer()
    {
        if (_itemBehavior == null)
        {
            return true;
        }

        if (AvailableSpaceInLayer())
        {
            return true;
        }
        
        return false;
    }

    private bool AvailableSpaceInLayer()
    {
        return _layerItemBehavior.AvailableEmptySpace();
    }

    public void FillData(ItemBehavior itemBehavior)
    {
        _layerItemBehavior.FillItemToSpace(_indexSpace, itemBehavior);
    }

    public void RemoveData()
    {
        _layerItemBehavior.RemoveItemInSpace(_indexSpace);
    }
}
