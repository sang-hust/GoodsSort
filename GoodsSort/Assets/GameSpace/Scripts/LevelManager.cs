using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SelfData selfPrefab;
    private WrapperData playerData;
    private void Start()
    {
        GameDataCenter.Instance.GetOrCreate(out playerData);
    }

    private void InitLevel()
    {
        
    }
}
