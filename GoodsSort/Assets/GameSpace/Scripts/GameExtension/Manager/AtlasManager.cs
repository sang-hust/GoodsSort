using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MMPersistentSingleton<AtlasManager>
{
    public SpriteAtlas ui;

    public Sprite GetSprite(string nameSprite)
    {
#if UNITY_EDITOR
        if (ui == null)
        {
            ui = Resources.Load<AtlasManager>("[AtlasManager]").ui;
        }
#endif
        return ui.GetSprite(nameSprite);
    }
}
