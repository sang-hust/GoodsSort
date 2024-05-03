using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoosterManager : MonoBehaviour
{
    public readonly Stack<(int idItem, int indexStartInLayer, int startSelf, int indexFinishInLayer, 
        int finishSelf)> StackTurnMove = new Stack<(int, int, int, int, int)>();

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
        if (_wrapperData.Client.booster[1] <= 0)
        {
            NotifyManager.Instance.ShowWarning("Not Enough Booster");
            return;
        }

        _wrapperData.ModifyBooster(1, -1);
        InGameUIManager.Instance.UpdateNumberSkill();

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
        if (_wrapperData.Client.booster[0] <= 0)
        {
            NotifyManager.Instance.ShowWarning("Not Enough Booster");
            return;
        }

        PopupStack.IsWaiting = true;
        if (StackTurnMove.Count <= 0) 
        {
            NotifyManager.Instance.ShowWarning("Can not do this!!", () => { PopupStack.IsWaiting = false; });
            return;
        }

        SoundManager.Instance.PlaySfx("Undo");
        _wrapperData.ModifyBooster(0, -1);
        InGameUIManager.Instance.UpdateNumberSkill();
        
        var turnMove = StackTurnMove.Pop();
        var startBaseLayer = GameManager.Instance.DictSelf[turnMove.startSelf].GetCurrentLayer();
        var indexInStartLayer = turnMove.indexStartInLayer;
        var indexInFinishLayer = turnMove.indexFinishInLayer;
        var finishBaseLayer = GameManager.Instance.DictSelf[turnMove.finishSelf].GetCurrentLayer();
        
        finishBaseLayer.RemoveItemInSpace(indexInFinishLayer);
        startBaseLayer.FillItemToSpace(indexInStartLayer, GameManager.Instance.DictItem[turnMove.idItem]);
        
        PopupStack.IsWaiting = false;
    }

    private (int indexStart, int startSelf) startData;
    private (int indexFinish, int finishSelf) finishData;

    public void SetFinishData(int indexFinish, int finishSelf)
    {
        finishData.indexFinish = indexFinish;
        finishData.finishSelf = finishSelf;
    }
    
    public void SetStartData(int indexStart, int startSelf)
    {
        startData.indexStart = indexStart;
        startData.startSelf = startSelf;
    }
    
    public void LoadToStackTurnMove(int idItem)
    {
        StackTurnMove.Push((idItem, startData.indexStart, startData.startSelf
            , finishData.indexFinish, finishData.finishSelf));
    }
    public void ClearStackTurnMove() => StackTurnMove.Clear();

    public void FrozenBooster()
    {
        if (_wrapperData.Client.booster[2] <= 0)
        {
            NotifyManager.Instance.ShowWarning("Not Enough Booster");
            return;
        }

        _wrapperData.ModifyBooster(2, -1);
        InGameUIManager.Instance.UpdateNumberSkill();

        StartCoroutine(IEFrozen());
    }


    private IEnumerator IEFrozen()
    {
        InGameUIManager.Instance.OnHudFrozen();
        GameManager.Instance.timeManager.pause = true;
        GameManager.Instance.timeManager.StopCountTime();

        yield return new WaitForSeconds(10f);
        GameManager.Instance.timeManager.pause = false;
        GameManager.Instance.timeManager.StartCountTime();
    }

    public void AddBlankBooster()
    {
        
    }
}
