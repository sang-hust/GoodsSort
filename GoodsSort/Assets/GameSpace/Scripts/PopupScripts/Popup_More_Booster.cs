using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_More_Booster : BasePopup<Popup_More_Booster> 
{
    [DisplayAsString] private BoosterType _boosterType;
    [SerializeField] private TMP_Text titlePopup, textPriceGold, textBoosterAdd;
    [SerializeField] private Image imageBooster;
    private int numBoosterAdd, priceGold;
    private WrapperData wrapperPlayer;

    private readonly string imageRevert = "BT_goiy";
    private readonly string imageFrozen = "BT_Dongbang";
    private readonly string imageShuffle = "BT_reroll";
    private readonly string imageHarmer = "BT_pha";

    private readonly Dictionary<BoosterType, int> dictNumBooster = new Dictionary<BoosterType, int>()
    {
        { BoosterType.Frozen ,1},
        { BoosterType.Revert ,1},
        { BoosterType.Shuffle ,1},
        { BoosterType.Hammer ,1},
    };

    protected override IEnumerator OnShowPopup()
    {
        yield break;
    }

    private void OnEnable()
    {
        GetData();        
        UpdateUI();
    }

    public Popup_More_Booster SetUpPopup(BoosterType boosterType)
    {
        _boosterType = boosterType;
        titlePopup.text = "More " + boosterType;
        return this;
    }
    
    private void GetData()
    {
        priceGold = 900;
        numBoosterAdd = dictNumBooster[_boosterType];
        GameDataCenter.Instance.Get(out wrapperPlayer);
    }
    private void UpdateUI()
    {
        textBoosterAdd.text = "+" + ContextExtension.FormatNumber(numBoosterAdd);
        // gold.ReplaceValue(wrapperPlayer.Client.golds).DoImmediately();
        textPriceGold.text = ContextExtension.FormatNumber(priceGold);
        imageBooster.sprite = _boosterType switch
        {
            BoosterType.Revert => AtlasManager.Instance.GetSprite(imageRevert),
            BoosterType.Frozen => AtlasManager.Instance.GetSprite(imageFrozen),
            BoosterType.Shuffle => AtlasManager.Instance.GetSprite(imageShuffle),
            BoosterType.Hammer => AtlasManager.Instance.GetSprite(imageHarmer),
            _ => throw new ArgumentOutOfRangeException()
        };
        imageBooster.SetNativeSize();
    }
    public void BuyWithAds()
    {
        if (!ContextExtension.IsCanClick()) return;
        
        OnSuccess();
        // AdsExtension.ShowVideoRewardAds(OnSuccess, () => NotifyManager.Instance.ShowWarning("Ads is not available"), $"Buy Booster {_boosterType.ToString()}");
    }
    public void BuyWithGold()
    {
        if (!ContextExtension.IsCanClick()) return;
        // if (wrapperPlayer.Client.golds < priceGold)
        // {
        //     PopupUIManager.Instance.GetPopup<Popup_More_Gold>().InitPopupInGame(true, gold).ShowPopup();
        //     return;
        // }
        //
        // wrapperPlayer.ModifyResourceAmount(ItemTypeEnum.Gold, -priceGold);
        // gold.MoreValue(-priceGold).DoAnim(0.5f, OnSuccess);
        OnSuccess();

    }
    
    private void OnSuccess()
    {
        switch (_boosterType)
        {
            case BoosterType.Revert:
                wrapperPlayer.ModifyBooster(0, 1);
                break;
            case BoosterType.Frozen:
                wrapperPlayer.ModifyBooster(2, 1);
                break;
            case BoosterType.Shuffle:
                wrapperPlayer.ModifyBooster(1, 1);
                break;
            case BoosterType.Hammer:
                wrapperPlayer.ModifyBooster(3, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        InGameUIManager.Instance.UpdateNumberSkill();

        // BaseCollector.Claim(rewards);
        //
        // if (wrapperPlayer.Client.booster[(int)_boosterType] - numBoosterAdd == 0)
        // {
        //     InGameUIManager.Instance.bBooster[(int)_boosterType].UpdateUIManual();
        // }
        //
        // InGameUIManager.Instance.bBooster[(int)_boosterType].GetComponent<UIResource>()
        //     .ReplaceValue(wrapperPlayer.Client.booster[(int)_boosterType]).DoCollectAnim(imageBooster.transform.position);
        //
        OnClickBack();
    }

    public override void OnClickBack()
    {
        base.OnClickBack();
    }

    public void OnClickAddMoreGold()
    {
        if (!ContextExtension.IsCanClick()) return;
    }
    
    protected override void OnHidePopup() { }
}
