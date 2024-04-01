using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
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

    public void InitSpace(int indexSpace, bool available)
    {
        _indexSpace = indexSpace;
        _available = available;
    }

    public void UpdateStatus(bool isAvailable)
    {
        _available = isAvailable;
    }
    
    public bool IsAvailable()
    {
        return _available;
    }

    public bool ItemCanFillThisLayer()
    {
        if (_available)
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
