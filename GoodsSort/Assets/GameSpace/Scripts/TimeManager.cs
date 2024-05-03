using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] [DisplayAsString] private int numberTimeRemain;
    private InGameUIManager uiManager;
    private WrapperData wrapperPlayer;
    public bool pause = false;

    private string hexColorTimeNormal = "FFD029", hexColorTimeOut = "FF413B";

    private void Awake()
    {
        // LevelManager.Instance.OnRevivalLevel += PlusNumberTimeRevival;
        LevelManager.Instance.OnBonusStart += PlusNumberTimeStart;
        LevelManager.Instance.OnStartLevel += StartCountTime;
        LevelManager.Instance.OnLoadLevel += LoadLimitResource;
        uiManager = InGameUIManager.Instance;
        GameDataCenter.Instance.Get(out wrapperPlayer);
        IECount = CountTimeGame();
    }

    public void StartCountTime()
    {
        if (GameManager.modeGame != ModeGamePlay.Time) return;
        StopCountTime();
        IECount = CountTimeGame();
        StartCoroutine(IECount);
    }

    private void PlusNumberTimeRevival()
    {
        if (GameManager.modeGame != ModeGamePlay.Time) return;
        // numberTimeRemain += wrapperPlayer.Client.timeBonus;
        // wrapperPlayer.ModifyResourceAmount(ItemTypeEnum.Time, -wrapperPlayer.Client.timeBonus);
        // uiManager.timeResource.ReplaceValue(numberTimeRemain).DoAnim(0.5f);
        // StartCountTime();
    }

    private void PlusNumberTimeStart()
    {
        if (GameManager.modeGame != ModeGamePlay.Time) return;
        numberTimeRemain += timeBonusStart;
        uiManager.timeResource.ReplaceValue(numberTimeRemain).DoAnim(0.5f);
    }

    private void LoadLimitResource()
    {
        if (GameManager.modeGame != ModeGamePlay.Time) return;
        // numberTimeRemain = LevelManager.Instance.LevelDataNow.timeRemain;
        numberTimeRemain = 300;
        // numberTimeRemain = 3600;
        // uiManager.timeResource.ReplaceValue(numberTimeRemain).DoImmediately();
    }

    public void BonusTimeRemain(int timeBonus)
    {
        var position = uiManager.UICombo.transform.position - new Vector3(0.5f, 0, 0);
        uiManager.timeResource.MoreValue(0).DoFadeAmountAndCollect(position, timeBonus, () =>
        {
            numberTimeRemain += timeBonus;
            uiManager.timeResource.ReplaceValue(numberTimeRemain).DoImmediately();
         });
    }

    private IEnumerator IECount;

    private IEnumerator CountTimeGame()
    {
        while (numberTimeRemain > 0 && !pause)
        {
            yield return new WaitForSecondsRealtime(1);
            numberTimeRemain--;
            uiManager.timeResource.ReplaceValue(numberTimeRemain).DoImmediately();
            if (numberTimeRemain <= 10)
            {
                uiManager.timeResource.DoPunchText();
                uiManager.timeResource.DoColorText(hexColorTimeOut);
            }
            else
            {
                uiManager.timeResource.DoColorText(hexColorTimeNormal);
            }
        }

        if (pause)
        {
            yield break;
        }
        
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.Instance.winLoseManager.OutOfTime();
    }

    public int timeBonusStart;
    public void StopCountTime() => StopCoroutine(IECount);
    public bool isOutOfTime() => numberTimeRemain <= 0;

    private void OnEnable()
    {
        // if (wrapperPlayer.Client.timeBonus > 0)
        // {
            // timeBonusStart = wrapperPlayer.Client.timeBonus;
            // wrapperPlayer.ModifyResourceAmount(ItemTypeEnum.Time, -wrapperPlayer.Client.timeBonus);
        // }
    }
}
