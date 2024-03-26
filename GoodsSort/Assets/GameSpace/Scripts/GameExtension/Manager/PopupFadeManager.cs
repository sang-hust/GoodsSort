using System;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class PopupFadeManager : MonoBehaviour
{
    [SerializeField] private GameObject fadePrefab;
    
    private Dictionary<Type, GameObject> activeFade = new Dictionary<Type, GameObject>();
    public GameObject GetFade(Type popup)
    {
        if (IsActiveFadePopup(popup)) return activeFade[popup];
        var fade = PoolManager.Pools["PoolFade"].Spawn(fadePrefab).gameObject;
        activeFade.Add(popup, fade);
        return fade;
    }
    public void ReturnFade(Type popup)
    {
        if (!IsActiveFadePopup(popup)) return;
        var fade = activeFade[popup];
        activeFade.Remove(popup);
        PoolManager.Pools["PoolFade"].Despawn(fade.transform, transform);
    }
    private bool IsActiveFadePopup(Type popup)
    {
        return popup != null && activeFade.ContainsKey(popup);
    }
}