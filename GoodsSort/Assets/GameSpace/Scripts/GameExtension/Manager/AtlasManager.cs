using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MMPersistentSingleton<AtlasManager>
{
    public SpriteAtlas ui;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public Sprite GetSprite(string nameSprite)
    {
        return ui.GetSprite(nameSprite);
    }
}
