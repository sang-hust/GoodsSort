using System.Collections.Generic;
using UnityEngine;

public class SelfBehavior : MonoBehaviour
{
    [SerializeField] private LayerItemBehavior _layerItemPrefab;
    private List<LayerItemBehavior> _listLayerItem;
    private SelfData selfData;
    private LayerItemBehavior _currentLayerItem;

    public void InitData(SelfData selfData)
    {
        this.selfData = selfData;
        
        InitLayerItem();
    }

    private void InitLayerItem()
    {
        _listLayerItem = new List<LayerItemBehavior>();

        for (var i = selfData.listLayerData.Count - 1; i >= 0; i--)
        {
            var layerData = selfData.listLayerData[i];
            var layerItem = Instantiate(_layerItemPrefab, transform);
            _listLayerItem.Add(layerItem);
            layerItem.InitLayerItem(layerData);
        }

        _currentLayerItem = _listLayerItem[_listLayerItem.Count - 1];
    }

    public int GetNumLayerCurrent()
    {
        return _listLayerItem.Count;
    }

    public void UpdateSelf()
    {
        var checkDone = GameManager.Instance.CheckDoneLayer(_currentLayerItem.ListItem);
        if (!checkDone) return;

        if (_listLayerItem.Count > 1)
        {
            DestroyImmediate(_listLayerItem[_listLayerItem.Count]);
            _currentLayerItem = _listLayerItem[_listLayerItem.Count - 1];
        }
        if (_listLayerItem.Count <= 0)
        {
            GameManager.Instance.winLoseManager.CheckWinAndNextLevel();
            return;
        } 
        
        // Update UI
    }
}
