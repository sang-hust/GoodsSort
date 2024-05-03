using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerItemBehavior : MonoBehaviour
{
    [SerializeField] private SpaceBehavior _spacePrefab;
    [SerializeField] private ItemBehavior _itemPrefab;

    [SerializeField] private List<ItemBehavior> _listItem = new List<ItemBehavior>();
    private List<SpaceBehavior> _listSpace = new List<SpaceBehavior>();
    private SelfBehavior _selfBehavior;
    private int _indexLayer;
    public int IndexLayer => _indexLayer;
    public List<ItemBehavior> ListItem => _listItem;

    private void Start()
    {
        _selfBehavior = GetComponentInParent<SelfBehavior>();
    }

    public void InitLayerItem(int indexLayer, LayerData layerData)
    {
        _indexLayer = indexLayer;
        var listItemData = layerData.listItemData;
        for (var i = 0; i < listItemData.Count; i++)
        {
            var space = Instantiate(_spacePrefab, transform);
            ItemBehavior itemBehavior = null;
            
            if (listItemData[i].itemType != ItemTypeEnum.None)
            {
                itemBehavior = Instantiate(_itemPrefab, space.transform);
                itemBehavior.InitItem(listItemData[i]).UpdateItemPosition(space);
                GameManager.Instance.winLoseManager.UpdateQuantityItem(1);
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
            
            GameManager.Instance.boosterManager
                .LoadToStackTurnMove(itemBehavior.uniqueId);
        }
        else
        {
            Debug.LogError("Index Space Not Null");
            for (var i = 0; i < _listSpace.Count; i++)
            {
                if (!_listSpace[i].IsAvailable()) continue;
                _listItem[i] = itemBehavior;
                _listSpace[i].UpdateStatus(false);
                itemBehavior.UpdatePosition(_listSpace[i].transform);

                break;
            }
        }
    }

    public void RemoveItemInSpace(int indexSpace)
    {
        _listItem[indexSpace] = null;
        _listSpace[indexSpace].UpdateStatus(true);
    }

    public void UpdateLayerAfterAction()
    {
        var haveEmptySpace = true;
        foreach (var item in _listItem)
        {
            if (item != null)
            {
                haveEmptySpace = false;
                break;
            }
        }

        if (haveEmptySpace)
        {
            var isEmpty = CheckLayerEmpty();
            if (!isEmpty) return;
            ExecuteEmptyLayer();
        }
        else
        {
            var isDone = CheckLayerDone();
            if (!isDone) return;
            
            GameManager.Instance.comboManager.UpdateCombo();
            var winLoseManager = GameManager.Instance.winLoseManager;
            winLoseManager.UpdateQuantityItem(-_listSpace.Count);
            
            for (var i = 0; i < _listItem.Count; i++)
            {
                GameManager.Instance.RemoveItemDict(_listItem[i].uniqueId);
                 _listItem[i].gameObject.SetActive(false);
                 _listItem[i] = null;
            }
            
            GameManager.Instance.boosterManager.ClearStackTurnMove();
            winLoseManager.CheckWinAndNextLevel();
            ExecuteEmptyLayer();
        }
    }
    
    /// <summary>
    /// done -> all null -> exe
    /// </summary>
    /// <returns></returns>
    private bool CheckLayerDone()
    {
        var firstItem = _listItem[0];

        if (firstItem == null)
        {
            return false;
        }
        
        foreach (var item in _listItem)
        {
            if (item == null)
            {
                return false;
            }

            if (item.ItemTypeEnum != firstItem.ItemTypeEnum)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// all null -> exe
    /// </summary>
    /// <returns></returns>
    private bool CheckLayerEmpty()
    {
        foreach (var item in _listItem)
        {
            if (item != null)
            {
                return false;
            }
        }

        return true;
    }

    private void ExecuteEmptyLayer()
    {
        foreach (var space in _listSpace)
        {
            space.UpdateStatus(true);
        }
        _selfBehavior.UpdateCurrentLayer();
    }

    public int GetIndexSelf()
    {
        return _selfBehavior.IndexSelf;
    }

    public void ClearFadeItem()
    {
        foreach (var item in _listItem.Where(item => item != null))
        {
            item.ClearFade();
        }
    }
}
