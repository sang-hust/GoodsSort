using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    private ItemTypeEnum _itemTypeEnum = ItemTypeEnum.None;
    private LayerItemBehavior _layerItemBehavior;

    /// <summary>
    /// Parent always Layer Item
    /// </summary>
    private void Start()
    {
        _layerItemBehavior = GetComponentInParent<LayerItemBehavior>();
    }

    public void InitSpace(ItemTypeEnum itemTypeEnum)
    {
        _itemTypeEnum = itemTypeEnum;
    }
    
    public bool IsAvailable()
    {
        return _itemTypeEnum == ItemTypeEnum.None;
    }

    public bool AvailableSpaceInLayer()
    {
        return _layerItemBehavior.AvailableEmptySpace();
    }

    public void FillData(ItemBehavior itemBehavior)
    {
        _layerItemBehavior.FillItemToSpace(itemBehavior._itemTypeEnum);
        itemBehavior.transform.position = transform.position;
    }
}
