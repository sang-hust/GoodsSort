using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private Image _imageItem;
    public ItemTypeEnum _itemTypeEnum;
    private Vector3 _startPosition = Vector3.zero;
    private bool _dragging;
    private Vector3 _offset;
    private bool _isTrigger;

    public void Start()
    {
        
    }

    public void InitItem(ItemTypeEnum itemTypeEnum, Vector3 startPosition)
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
        if (!_isTrigger)
        {
            ResetToBeginning();
        }
    }

    private void ResetToBeginning()
    {  
        transform.localPosition = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Space")) return;
        var spaceBehavior = col.gameObject.GetComponent<SpaceBehavior>();

        if (!spaceBehavior.IsAvailable()) return;
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
