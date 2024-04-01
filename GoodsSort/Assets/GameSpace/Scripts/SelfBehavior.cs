using System.Collections;
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
        foreach (var layerData in selfData.listLayerData)
        {
            var layerItem = Instantiate(_layerItemPrefab, transform);
            _listLayerItem.Add(layerItem);
            layerItem.InitLayerItem(layerData);    
        }

        _currentLayerItem = _listLayerItem[0];
    }

    public int GetNumLayerCurrent()
    {
        return _listLayerItem.Count;
    }

    public void UpdateSelf()
    {
        var checkDone = GameManager.Instance.CheckDoneLayer(_currentLayerItem.ListItem);
        if (!checkDone) return;
        
        DestroyImmediate(_listLayerItem[0]);
        if (_listLayerItem.Count <= 0)
        {
            GameManager.Instance.winLoseManager.CheckWinAndNextLevel();
            return;
        } 
        _currentLayerItem = _listLayerItem[0];
        // Update UI
    }
}
