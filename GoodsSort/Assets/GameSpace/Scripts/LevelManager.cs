using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class LevelManager : MMSingleton<LevelManager>
{
    [SerializeField] private SelfBehavior selfPrefab;
    [SerializeField] private SelfSO selfSo;
    [SerializeField] private Transform gridTransform;

    private WrapperData playerData;
    private List<SelfBehavior> _listSelf;
    public Action OnStartLevel, OnLoadLevel, OnBonusStart;
    
    private void Start()
    {
        GameDataCenter.Instance.GetOrCreate(out playerData);
        StartCoroutine(InitLevel());
    }

    private IEnumerator InitLevel()
    {
        var level = playerData.Client.level;

        if (level <= 1)
        {
            PopupUIManager.Instance.GetPopup<Popup_Tutorial>().ShowPopup();
        }
        
        var levelData = new List<SelfData>();
        if (level >= 19)
        {
            var indexRandom = UnityEngine.Random.Range(1, 8);
            levelData = selfSo.LevelData[indexRandom];
        }
        else
        {
            levelData = selfSo.LevelData[level];
        }
        
        _listSelf = new List<SelfBehavior>();
        for (var i = 0; i < levelData.Count; i++)
        {
            var self = Instantiate(selfPrefab, gridTransform);
            self.InitData(levelData[i]);
            _listSelf.Add(self);
            
            GameManager.Instance.SetUniqueIDSelf(self);
        }
        
        GameManager.Instance.SetUniqueIDItem();
        OnLoadLevel?.Invoke();
        yield return InGameUIManager.Instance.BonusResourceOnStart(OnBonusStart);

        OnStartLevel?.Invoke();
    }

    public void NextLevel()
    {
        playerData.ModifyLevel(1);
        DestroyAllSelf();
        LoadSceneManager.Instance.LoadScene("InGameScene");
        StartCoroutine(InitLevel());  
    }

    public void OnReplay()
    {
        DestroyAllSelf();
        LoadSceneManager.Instance.LoadScene("InGameScene");
        StartCoroutine(InitLevel());  
    }

    private void DestroyAllSelf()
    {
        foreach (var self in _listSelf)
        {
            DestroyImmediate(self);
        }
    }


    private int idFake = 999;
    public void AddSelfToList()
    {
        if (_listSelf.Count > 9)
        {
            NotifyManager.Instance.ShowWarning("Max Size");
            return;
        }
        var selfData = new SelfData(idFake, new List<LayerData>()
        {
            new LayerData(new List<ItemData>()
            {
                new ItemData(ItemTypeEnum.None),
                new ItemData(ItemTypeEnum.None),
                new ItemData(ItemTypeEnum.None),
            })
        });
        
        idFake++;
        var self = Instantiate(selfPrefab, gridTransform);
        self.InitData(selfData);
        _listSelf.Add(self);
        //gridTransform.GetComponent<GridLayoutGroup>().ex
    }
}
