using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private SpaceBehavior _spacePrefab;
    [SerializeField] private ItemBehavior _itemPrefab;

    public List<ItemBehavior> ListItem = new List<ItemBehavior>();
    private List<SpaceBehavior> _listSpace = new List<SpaceBehavior>();
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
            ItemBehavior itemBehavior = null;
            
            if (layerData.listItemData[i].itemType != ItemTypeEnum.None)
            {
                itemBehavior = Instantiate(_itemPrefab, space.transform);
                itemBehavior.InitItem(layerData.listItemData[i]).UpdateItemPosition(space);
            }
            ListItem.Add(itemBehavior);
            space.InitSpace(i, itemBehavior == null);
            _listSpace.Add(space);
        }
    }

    public bool AvailableEmptySpace()
    {
        return ListItem.Any(itemBehavior => itemBehavior == null);
    }

    public void FillItemToSpace(int indexSpace, ItemBehavior itemBehavior)
    {
        // fill this
        if (ListItem[indexSpace] == null)
        {
            ListItem[indexSpace] = itemBehavior;
            _listSpace[indexSpace].UpdateStatus(false);
            itemBehavior.UpdatePosition(_listSpace[indexSpace].transform);
            
            //_selfBehavior.UpdateSelf();
            
            Debug.LogError("Index Space Null");
        }
        // find fill
        else
        {
            Debug.LogError("Index Space Not Null");
            foreach (var space in _listSpace)
            {
                if (!space.IsAvailable()) continue;
                ListItem[indexSpace] = itemBehavior;
                space.UpdateStatus(false);
                itemBehavior.UpdatePosition(space.transform);

                //_selfBehavior.UpdateSelf();
                break;
            }
        }
    }

    public void RemoveItemInSpace(int indexSpace)
    {
        ListItem[indexSpace] = null;
        _listSpace[indexSpace].UpdateStatus(true);
    }
}
