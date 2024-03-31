using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private SpaceBehavior _spacePrefab;
    public List<ItemBehavior> ListItem;
    private List<SpaceBehavior> _listSpace;
    private SelfBehavior _selfBehavior;

    private void Start()
    {
        _selfBehavior = GetComponentInParent<SelfBehavior>();
    }

    public void InitLayerItem(LayerData layerData)
    {
        for (var i = 0; i < layerData.listItemData.Count; i++)
        {
            var space = Instantiate(_spacePrefab, transform);
            space.InitSpace(i, layerData.listItemData[i]);
            _listSpace.Add(space);
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


}
