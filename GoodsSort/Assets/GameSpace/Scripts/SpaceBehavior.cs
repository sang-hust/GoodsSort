using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    [SerializeField] private int _indexSpace;
    [SerializeField] private bool _available;
    [SerializeField] private LayerItemBehavior _layerItemBehavior;
    [SerializeField] private BoxCollider2D _collider2D;

    public int IndexSpace => _indexSpace;
    
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
        UpdateStatus(available);
    }

    public void UpdateStatus(bool isAvailable)
    {
        _available = isAvailable;
        _collider2D.enabled = _available;
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

    public int GetIndexSelf()
    {
        return _layerItemBehavior.GetIndexSelf();
    }
    
    public void UpdateLayer()
    {
        _layerItemBehavior.UpdateLayerAfterAction();
    }
}
