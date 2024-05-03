using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemBehavior : MonoBehaviour
{
    public int uniqueId;
    [SerializeField] private Image _imageItem;
    [SerializeField] private Image _imageItemFade;
    private ItemTypeEnum _itemTypeEnum;
    private SpaceBehavior spaceCache;
    private Transform cacheParent;
    public ItemTypeEnum ItemTypeEnum => _itemTypeEnum;
    private ItemData _itemData;
    private bool _dragging;
    private Vector3 _offset;
    private bool _isTrigger;
    private SpaceBehavior _spaceBehavior;
    
    private SpaceBehavior spaceStart;
    private SpaceBehavior spaceEnd;

    private const float heightDefault = 154f;
    
    public ItemBehavior InitItem(ItemData itemData)
    {
        _itemData = itemData;
        _itemTypeEnum = _itemData.itemType;
        
        SetUIItem();
        return this;
    }

    private void SetUIItem()
    {
        _imageItem.sprite = AtlasManager.Instance.GetSprite(_itemTypeEnum.ToString());
        _imageItemFade.sprite = AtlasManager.Instance.GetSprite(_itemTypeEnum.ToString());
        _imageItem.SetNativeSize();
        _imageItemFade.SetNativeSize();

        var height = _imageItem.rectTransform.rect.height;
        ChangePosition(height);
    }

    private void ChangePosition(float height)
    {
        if (height > heightDefault)
        {
            var pivot = (height - heightDefault) / 2;
            var itemLocalPosition = transform.localPosition;
            itemLocalPosition = new Vector3(itemLocalPosition.x, itemLocalPosition.y + pivot,
                itemLocalPosition.z);
            _imageItem.transform.localPosition = itemLocalPosition;
            _imageItemFade.transform.localPosition = itemLocalPosition;
        }
    }

    public void UpdateItemPosition(SpaceBehavior spaceBehavior)
    {
        _spaceBehavior = spaceBehavior;
    }

    #region Dragging Mode

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

        var itemTransform = transform;
        cacheParent = itemTransform.parent;
        itemTransform.parent = GameManager.Instance.ParentLayerItemHighest;
        _dragging = true;
    }

    public void OnUp()
    {
        _dragging = false;
        transform.parent = cacheParent;
        if (spaceCache != null)
        {
            if (spaceCache.ItemCanFillThisLayer())
            {
                spaceEnd = spaceCache;
                spaceStart = _spaceBehavior;
                
                GameManager.Instance.boosterManager.SetFinishData(spaceEnd.IndexSpace, spaceEnd.GetIndexSelf());
                GameManager.Instance.boosterManager.SetStartData(spaceStart.IndexSpace, spaceStart.GetIndexSelf());
                
                _spaceBehavior.RemoveData();
                spaceCache.FillData(this);
                
                GameManager.Instance.CheckDoneLayer(spaceStart, spaceEnd);
            }
            else
            {
                ResetPosition();
            }
        }
        else
        {
            ResetPosition();
        }
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_dragging) return;
        if (!other.CompareTag("Space")) return;
        spaceCache = other.gameObject.GetComponent<SpaceBehavior>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        spaceCache = null;
    }
    #endregion

    public void EffectDestroy()
    {
        
    }

    public void ClearFade()
    {
        _imageItemFade.gameObject.SetActive(false);
    }

    public void ChangeTypeItem(ItemTypeEnum itemType)
    {
        _itemTypeEnum = itemType;
        
        SetUIItem();
    }
}
