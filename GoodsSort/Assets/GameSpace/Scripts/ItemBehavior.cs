using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private Image _imageItem;
    public ItemTypeEnum _itemTypeEnum;
    private ItemData _itemData;
    private Vector3 _startPosition;
    private bool _dragging;
    private Vector3 _offset;
    private bool _isTrigger;

    public void Start()
    {
        var rand = Random.Range(0, 7);
        InitItem((ItemTypeEnum) rand);
    }

    public void InitItem(ItemTypeEnum itemTypeEnum, Vector3 startPosition = default)
    {
        _itemTypeEnum = itemTypeEnum;
        UIItem();
    }

    private void UIItem()
    {
        _imageItem.sprite = AtlasManager.Instance.GetSprite(_itemTypeEnum.ToString());
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

    private void ResetToBeginning()
    {  
        transform.localPosition = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// To new space
    /// </summary>
    public void UpdatePosition(Vector3 spacePosition)
    {
        transform.localPosition = spacePosition;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_dragging) return;
        if (!other.CompareTag("Space")) return;

        var spaceBehavior = other.gameObject.GetComponent<SpaceBehavior>();

        if (spaceBehavior.IsAvailable())
        {
            
        }
        else
        {
            if (spaceBehavior.AvailableSpaceInLayer())
            {
                _isTrigger = true;
                spaceBehavior.FillData(this);
            }
            else
            {
                ResetToBeginning();
            }
        }
    }
}
