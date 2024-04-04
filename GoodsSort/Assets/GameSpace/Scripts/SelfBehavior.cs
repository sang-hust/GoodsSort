using System.Collections.Generic;
using UnityEngine;

public class SelfBehavior : MonoBehaviour
{
    [SerializeField] private LayerItemBehavior _layerItemPrefab;
    [SerializeField] private List<LayerItemBehavior> _listLayerItem = new List<LayerItemBehavior>();
    private SelfData _selfData;
    public void InitData(SelfData selfData)
    {
        _selfData = selfData;
        InitLayerItem();
    }

    private void InitLayerItem()
    {
        var listLayerItemCache = new List<LayerItemBehavior>();

        for (var i = _selfData.listLayerData.Count - 1; i >= 0; i--)
        {
            var layerData = _selfData.listLayerData[i];
            var layerItem = Instantiate(_layerItemPrefab, transform);
            listLayerItemCache.Add(layerItem);
            layerItem.InitLayerItem(_selfData.listLayerData.Count - i, layerData);
        }

        for (var i = listLayerItemCache.Count - 1; i >= 0; i--)
        {
            _listLayerItem.Add(listLayerItemCache[i]);
        }
    }

    public void UpdateCurrentLayer()
    {
        var numberLayer = _listLayerItem.Count;
        if (numberLayer <= 1) return; 
        
        _listLayerItem[0].gameObject.SetActive(false);
        _listLayerItem.RemoveAt(0);
    }
}
