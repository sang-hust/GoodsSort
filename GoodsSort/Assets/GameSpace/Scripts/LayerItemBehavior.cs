using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private SpaceBehavior _spacePrefab;
    [SerializeField] private ItemBehavior _itemPrefab;

    private List<ItemBehavior> _listItem = new List<ItemBehavior>();
    private List<SpaceBehavior> _listSpace = new List<SpaceBehavior>();
    private SelfBehavior _selfBehavior;

    private void Start()
    {
        _selfBehavior = GetComponentInParent<SelfBehavior>();
    }

    public void InitLayerItem(LayerData layerData)
    {
        var listItemData = layerData.listItemData;
        WinLoseManager.QuantityItemInLevel += listItemData.Count;
        for (var i = 0; i < listItemData.Count; i++)
        {
            var space = Instantiate(_spacePrefab, transform);
            ItemBehavior itemBehavior = null;
            
            if (listItemData[i].itemType != ItemTypeEnum.None)
            {
                itemBehavior = Instantiate(_itemPrefab, space.transform);
                itemBehavior.InitItem(listItemData[i]).UpdateItemPosition(space);
            }
            _listItem.Add(itemBehavior);
            space.InitSpace(i, itemBehavior == null);
            _listSpace.Add(space);
        }
    }
    
    public bool AvailableEmptySpace()
    {
        return _listItem.Any(itemBehavior => itemBehavior == null);
    }

    public void FillItemToSpace(int indexSpace, ItemBehavior itemBehavior)
    {
        if (_listItem[indexSpace] == null)
        {
            _listItem[indexSpace] = itemBehavior;
            _listSpace[indexSpace].UpdateStatus(false);
            itemBehavior.UpdatePosition(_listSpace[indexSpace].transform);
            itemBehavior.UpdateItemPosition(_listSpace[indexSpace]);
        }
        else
        {
            Debug.LogError("Index Space Not Null");
            foreach (var space in _listSpace)
            {
                if (!space.IsAvailable()) continue;
                _listItem[indexSpace] = itemBehavior;
                space.UpdateStatus(false);
                itemBehavior.UpdatePosition(space.transform);

                break;
            }
        }
        
        CheckLayerDone();
    }

    public void RemoveItemInSpace(int indexSpace)
    {
        _listItem[indexSpace] = null;
        _listSpace[indexSpace].UpdateStatus(true);
    }

    private void CheckLayerDone()
    {
        var itemTypeFirst = _listItem[0].ItemTypeEnum;
        if (_listItem.Any(item => item.ItemTypeEnum != itemTypeFirst)) return;
        
        _selfBehavior.UpdateCurrentLayer();
    }

    public void ChangeToEmptyLayer()
    {
        foreach (var item in _listItem)
        {
            item.gameObject.SetActive(false);
        }
        
        _listItem.Clear();
        for (var i = 0; i < _listSpace.Count; i++)
        {
            _listItem.Add(null);
        }
    }
}
