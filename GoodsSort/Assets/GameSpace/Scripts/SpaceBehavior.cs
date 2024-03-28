using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    public ItemTypeEnum _itemTypeEnum = ItemTypeEnum.None;
    private int _indexSpace;
    private bool _available;
    private LayerItemBehavior _layerItemBehavior;

    /// <summary>
    /// Parent always Layer Item
    /// </summary>
    private void Start()
    {
        _layerItemBehavior = GetComponentInParent<LayerItemBehavior>();
    }

    public void InitSpace(int indexSpace, ItemTypeEnum itemTypeEnum)
    {
        _indexSpace = indexSpace;
        _itemTypeEnum = itemTypeEnum;
        _available = _itemTypeEnum == ItemTypeEnum.None;
    }
    
    public bool IsAvailable()
    {
        return _available;
    }

    public bool AvailableSpaceInLayer()
    {
        return _layerItemBehavior.AvailableEmptySpace();
    }

    public void FillData(ItemBehavior itemBehavior)
    {
        _itemTypeEnum = itemBehavior._itemTypeEnum;
        //_layerItemBehavior.FillItemToSpace(itemBehavior._itemTypeEnum);
        itemBehavior.UpdatePosition(transform.position);
    }
}
