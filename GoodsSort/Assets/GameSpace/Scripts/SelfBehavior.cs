using System.Collections.Generic;
using UnityEngine;

public class SelfBehavior : MonoBehaviour
{
    [SerializeField] private LayerItemBehavior _layerItemPrefab;
    private List<LayerItemBehavior> _listLayerItem;
    private SelfData _selfData;
    private int _maxSizeInLayer;
    public void InitData(SelfData selfData)
    {
        _selfData = selfData;
        _maxSizeInLayer = selfData.listLayerData[0].listItemData.Count;
        InitLayerItem();
    }

    private void InitLayerItem()
    {
        _listLayerItem = new List<LayerItemBehavior>();

        for (var i = _selfData.listLayerData.Count - 1; i >= 0; i--)
        {
            var layerData = _selfData.listLayerData[i];
            var layerItem = Instantiate(_layerItemPrefab, transform);
            _listLayerItem.Add(layerItem);
            layerItem.InitLayerItem(layerData);
        }
    }

    public void UpdateCurrentLayer()
    {
        WinLoseManager.QuantityItemInLevel -= _maxSizeInLayer;
        if (_listLayerItem.Count > 1)
        {
            // effect item onDestroy here 

            DestroyImmediate(_listLayerItem[0]);
            _listLayerItem.RemoveAt(0);
        }
        else
        {
            _listLayerItem[0].ChangeToEmptyLayer();
        }
    }
}
