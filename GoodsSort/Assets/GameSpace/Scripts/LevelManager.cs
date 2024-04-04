using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class LevelManager : MMSingleton<LevelManager>
{
    [SerializeField] private SelfBehavior selfPrefab;
    [SerializeField] private SelfSO selfSo;
    [SerializeField] private Transform gridTransform;
    private WrapperData playerData;
    private List<SelfBehavior> _listSelf;
    private void Start()
    {
        GameDataCenter.Instance.GetOrCreate(out playerData);
        InitLevel();
    }

    private void InitLevel()
    {
        var level = playerData.Client.level;
        var levelData = selfSo.LevelData[level];

        _listSelf = new List<SelfBehavior>();
        for (var i = 0; i < levelData.Count; i++)
        {
            var self = Instantiate(selfPrefab, gridTransform);
            self.InitData(levelData[i]);
            _listSelf.Add(self);
        }
    }

    public void NextLevel()
    {
        //playerData.ModifyLevel(1);
        DestroyAllSelf();
        LoadSceneManager.Instance.LoadScene("InGameScene");
        InitLevel();
    }

    private void DestroyAllSelf()
    {
        foreach (var self in _listSelf)
        {
            DestroyImmediate(self);
        }
    }
}
