using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private int _comboCount;
    private int _timeRemain;
    private int TIME_REMAIN_COMBO = 10;
    private WrapperData _wrapperPlayer;
    private UICombo UICombo;

    public static int highestCombo;
    private void Start()
    {
        GameDataCenter.Instance.Get(out _wrapperPlayer);
        UICombo = InGameUIManager.Instance.UICombo;
        _timeRemain = TIME_REMAIN_COMBO;
    }
    public void UpdateCombo()
    {
        _comboCount += 1;
        SoundManager.Instance.PlaySfx($"DoneBolt_{Mathf.Min(_comboCount, 6)}");
        highestCombo = Mathf.Max(_comboCount, highestCombo);
        switch (_comboCount)
        {
            case 2:
                Action2Combo();
                break;
            case 3:
                Action3Combo();
                SoundManager.Instance.PlaySfx("Combo");
                break;
            case 4:
                Action4Combo();
                break;
            case 5:
                SoundManager.Instance.PlaySfx("Combo");
                break;
            case 6:
                Action6Combo();
                break;
            case 7:
                Action7Combo();
                SoundManager.Instance.PlaySfx("Big_Combo");
                break;
            case 8:
                break;
            case 9:
                Action9Combo();
                SoundManager.Instance.PlaySfx("Big_Combo");
                break;
            case 10:
                break;
            case 11:
                Action11Combo();
                SoundManager.Instance.PlaySfx("Big_Combo");
                break;
            case 13:
                Action13Combo();
                break;
            default:
                _timeRemain = TIME_REMAIN_COMBO;
                break;
        }
        UICombo.DoCombo(_comboCount, _timeRemain, () => _comboCount = 0);
    }
    public void ResetToNextStage()
    {
        _comboCount = 0;
        _timeRemain = TIME_REMAIN_COMBO;
        UICombo.ResetCombo();
    }

    private void Action2Combo()
    {
        _timeRemain = 10;
        GameManager.Instance.timeManager.BonusTimeRemain(10);
    }

    private void Action3Combo()
    {
        _timeRemain = 10;
        GameManager.Instance.timeManager.BonusTimeRemain(10);
    }
    
    private void Action4Combo()
    {
        _timeRemain = 10;
        GameManager.Instance.timeManager.BonusTimeRemain(15);
    }
    
    private void Action6Combo()
    {
        _timeRemain = 10;
        GameManager.Instance.timeManager.BonusTimeRemain(10);
    }
    
    private void Action7Combo()
    {
        _timeRemain = 10;
        var randTypeBooster = Random.Range(0, 4);
        _wrapperPlayer.ModifyBooster(randTypeBooster, 1);
        InGameUIManager.Instance.bBooster[randTypeBooster]
            .GetComponent<UIResource>()
            .MoreValue(1)
            .DoCollectImmediately(UICombo.transform.position, InGameUIManager.Instance.UpdateNumberSkill);
    }
    
    private void Action9Combo()
    {
        _timeRemain = 10;
        var randTypeBooster = Random.Range(0, 4);
        _wrapperPlayer.ModifyBooster(randTypeBooster, 1);
        InGameUIManager.Instance.bBooster[randTypeBooster]
            .GetComponent<UIResource>()
            .MoreValue(2)
            .DoCollectImmediately(UICombo.transform.position, InGameUIManager.Instance.UpdateNumberSkill);
    }
    
    private void Action11Combo()
    {
        _timeRemain = 10;
        var randTypeBooster = Random.Range(0, 4);
        InGameUIManager.Instance.bBooster[randTypeBooster]
            .GetComponent<UIResource>()
            .MoreValue(5)
            .DoCollectImmediately(UICombo.transform.position, InGameUIManager.Instance.UpdateNumberSkill);
    }
    
    private void Action13Combo()
    {
        _timeRemain = 10;
        var randTypeBooster = Random.Range(0, 4);
        InGameUIManager.Instance.bBooster[randTypeBooster]
            .GetComponent<UIResource>()
            .MoreValue(7)
            .DoCollectImmediately(UICombo.transform.position, InGameUIManager.Instance.UpdateNumberSkill);
    }
}
