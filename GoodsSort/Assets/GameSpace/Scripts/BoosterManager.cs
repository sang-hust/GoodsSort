using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoosterManager : MonoBehaviour
{
    private WrapperData _wrapperData;

    private void Awake()
    {
        GameDataCenter.Instance.GetOrCreate(out _wrapperData);
    }

    public void HammerBooster()
    {
        
    }

    public void ShuffleBooster()
    {
        if (_wrapperData.Client.booster[2] <= 0)
        {
            NotifyManager.Instance.ShowWarning("Not Enough Booster");
            return;
        }

        _wrapperData.ModifyBooster(2, -1);

        var quantityTypeItem = new Dictionary<ItemTypeEnum, int>();
        var listItemTypeCurrent = new List<ItemTypeEnum>();

        var dictItem = GameManager.Instance.DictItem;
        for (var i = 0; i < dictItem.Count; i++)
        {
            var item = dictItem[i];

            if (quantityTypeItem.ContainsKey(item.ItemTypeEnum))
            {
                quantityTypeItem[item.ItemTypeEnum]++;
            }
            else
            {
                quantityTypeItem.Add(item.ItemTypeEnum, 1);
                listItemTypeCurrent.Add(item.ItemTypeEnum);
            }
        }

        var dictSelf = GameManager.Instance.DictSelf;

        for (var i = 0; i < dictSelf.Count; i++)
        {
            var self = dictSelf[i];
            var listLayer = self.ListLayerItem;

            for (var iLayer = 0; iLayer < listLayer.Count; iLayer++)
            {
                var layer = listLayer[iLayer].ListItem;

                for (var iItem = 0; iItem < layer.Count; iItem++)
                {
                    var type = GetItemTypeRandom();
                    layer[iItem].ChangeTypeItem(type);
                }
            }
        }

        ItemTypeEnum GetItemTypeRandom()
        {
            var type = ItemTypeEnum.None;

            while (true)
            {
                listItemTypeCurrent.MMShuffle();
                var typeCache = listItemTypeCurrent[0];

                if (quantityTypeItem[typeCache] > 1)
                {
                    type = typeCache;
                    quantityTypeItem[typeCache]--;
                    break;
                }
                
                if (quantityTypeItem.All(count => count.Value <= 0))
                {
                    break;
                }
            }
            
            return type;
        }
    }

    public void HintBooster()
    {
        
    }
    
    public void RevertBooster()
    {
        
    }

    public void FrozenBooster()
    {
        
    }
}
