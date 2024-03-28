using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MMPersistentSingleton<AtlasManager>
{
    public SpriteAtlas ui;

    public Sprite GetSprite(string nameSprite)
    {
        return ui.GetSprite(nameSprite);
    }
}
