using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private Image _imageItem;
    private ItemTypeEnum _itemTypeEnum;
    public ItemTypeEnum ItemTypeEnum
    {
        get => _itemTypeEnum;
        set => _itemTypeEnum = value;
    }
    
    private ItemData _itemData;
    private bool _dragging;
    private Vector3 _offset;
    private bool _isTrigger;
    private SpaceBehavior _spaceBehavior;

    public ItemBehavior InitItem(ItemData itemData)
    {
        _itemData = itemData;
        _imageItem.sprite = AtlasManager.Instance.GetSprite(itemData.itemType.ToString());

        return this;
    }

    public void UpdateItemPosition(SpaceBehavior spaceBehavior)
    {
        _spaceBehavior = spaceBehavior;
    }

    private void Update()
    {
        if (_dragging)
        {
            if (Camera.main != null) transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }
    }

    public void OnDown()
    {
        if (Camera.main != null)
        {
            _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        _dragging = true;
    }

    public void OnUp()
    {
        _dragging = false;
    }

    /// <summary>
    /// Not match any space
    /// </summary>
    private void ResetPosition()
    {  
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// To new space
    /// </summary>
    public void UpdatePosition(Transform spaceTransform)
    {
        transform.parent = spaceTransform;
        transform.localPosition = Vector3.zero;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_dragging) return;
        if (!other.CompareTag("Space")) return;

        var spaceBehavior = other.gameObject.GetComponent<SpaceBehavior>();

        if (spaceBehavior.ItemCanFillThisLayer())
        {
            _spaceBehavior.RemoveData();
            spaceBehavior.FillData(this);
        }
        else
        {
            ResetPosition();
        }
    }
}
