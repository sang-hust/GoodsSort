using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    [DisplayAsString] public ItemTypeEnum _itemTypeEnum = ItemTypeEnum.None;
    [SerializeField] private ItemBehavior _itemPrefab;
    private int _indexSpace;
    private LayerItemBehavior _layerItemBehavior;
    private ItemBehavior _itemBehavior;

    /// <summary>
    /// Parent always Layer Item
    /// </summary>
    private void Start()
    {
        _layerItemBehavior = GetComponentInParent<LayerItemBehavior>();
    }

    public void InitSpace(int indexSpace, ItemData itemData)
    {
        _indexSpace = indexSpace;
        if (itemData.itemType == ItemTypeEnum.None)
        {
            _itemBehavior = null;
        }
        else
        {
            _itemBehavior = Instantiate(_itemPrefab, transform);
            _itemBehavior.InitItem(itemData).UpdateItemPosition(this);
        }
    }

    public void UpdateData(ItemBehavior itemBehavior)
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
        _itemBehavior = null;
    }
}
