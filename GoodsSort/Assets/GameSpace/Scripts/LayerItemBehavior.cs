using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private SpaceBehavior _spacePrefab;
    [SerializeField] private ItemBehavior _itemPrefab;

    public List<ItemBehavior> ListItem;
    private SelfBehavior _selfBehavior;

    private void Start()
    {
        _selfBehavior = GetComponentInParent<SelfBehavior>();
    }

    public void InitLayerItem(LayerData layerData)
    {
        ListItem = new List<ItemBehavior>();

        for (var i = 0; i < layerData.listItemData.Count; i++)
        {
            var space = Instantiate(_spacePrefab, transform);
            ItemBehavior itemBehavior = null;
            
            if (layerData.listItemData[i].itemType != ItemTypeEnum.None)
            {
                itemBehavior = Instantiate(_itemPrefab, transform);
                itemBehavior.InitItem(layerData.listItemData[i]).UpdateItemPosition(space);
            }
            
            space.InitSpace(i, itemBehavior);
            _listSpace.Add(space);
            ListItem.Add(itemBehavior);
        }
    }

    public bool AvailableEmptySpace()
    {
        return _listSpace.Any(space => space.IsAvailable());
    }

    public void FillItemToSpace(int indexSpace, ItemBehavior itemBehavior)
    {
        var itemType = itemBehavior.ItemTypeEnum;
        if (_listSpace[indexSpace].IsAvailable())
        {
            ListItem[indexSpace].ItemTypeEnum = itemType;
            _listSpace[indexSpace].UpdateData(itemBehavior);
            itemBehavior.UpdatePosition(_listSpace[indexSpace].transform);

            _selfBehavior.UpdateSelf();
            return;
        }

        for (var i = 0; i < _listSpace.Count; i++)
        {
            if (!_listSpace[i].IsAvailable()) continue;
            ListItem[i].ItemTypeEnum = itemType;
            _listSpace[i].UpdateData(itemBehavior);
            itemBehavior.UpdatePosition(_listSpace[i].transform);

            _selfBehavior.UpdateSelf();
            break;
        }
    }

    public void RemoveItemInSpace(int indexSpace)
    {
        
    }
}
