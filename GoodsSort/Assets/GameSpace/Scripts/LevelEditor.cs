using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
    [SerializeField] private GameObject gridColumnPrefab;
    
    [SerializeField] private SelfSO _selfSo; 
    [SerializeField] private SelfBehavior _selfPrefab; 

    private int level;
    private List<SelfData> levelData;

    private void Start()
    {
        
    }

    
    public void GetAndLoadLevelData()
    {
        var allLevelData = _selfSo.LevelData;
        if (allLevelData.ContainsKey(level))
        {
            levelData = _selfSo.LevelData[level];
        }
        else
        {
            levelData = new List<SelfData>();
            allLevelData.Add(level, levelData);
        }
        
        /*for (var i = 0; i < levelData.Count; i++)
        {s
            var self = Instantiate(_selfPrefab, gridTransform);
            self.InitData(levelData[i]);
            _listSelf.Add(self);
        }*/
    }
    public void ClearAllSelf()
    {
        
    }

    public void ClearSelfSelect(int id)
    {
        
    }
    
    public void AddSelf()
    {
        
    }
}
