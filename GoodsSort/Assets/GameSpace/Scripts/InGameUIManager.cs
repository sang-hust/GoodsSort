using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MMSingleton<InGameUIManager>
{
    public UIButtonSkill[] bBooster;
    public UIResource timeResource;
    private BoosterManager boosters;
    [HideInInspector] public UICombo UICombo;
    
    public AnimationCurve curve;
    [SerializeField] private TMP_Text textLevel;
    [SerializeField] private GameObject hudMove, hudTime, hudBoosterHarmer, hudFrozen;
    [SerializeField] private Image iconMove, iconTime;

    private WrapperData wrapperPlayer;
    
    protected override void Awake()
    {
        UICombo = GetComponentInChildren<UICombo>();
        LevelManager.Instance.OnLoadLevel += OnLoadLevel;
        boosters = GameManager.Instance.boosterManager;
        GameDataCenter.Instance.GetOrCreate(out wrapperPlayer);
        InitializeButtonSkill();
        timeResource.FormatNumber = time => time.ToTime("mm:ss");
    }
    
    private void OnLoadLevel()
    {
        SoundManager.Instance.ResumeMusic();
        UpdateNumberSkill();
        SetTextLevel();
        //hudMove.SetActive(false);
        hudTime.SetActive(true);
    }
    
    private void InitializeButtonSkill()
    {
        bBooster[0].onClick = () => ActionClick(boosters.RevertBooster);
        bBooster[1].onClick = () => ActionClick(boosters.ShuffleBooster);
        bBooster[2].onClick = () => ActionClick(boosters.FrozenBooster);
        bBooster[3].onClick = () => ActionClick(boosters.HammerBooster);
    }
    
    public void ActionClick(Action action)
    {
        StartCoroutine(IEActionClick());
        return;

        IEnumerator IEActionClick()
        {
            yield return null;
            action?.Invoke();
        }
    }
    
    public void UpdateNumberSkill()
    {
        for (int i = 0; i < bBooster.Length; i++)
        {
            bBooster[i].UpdateUI(wrapperPlayer.Client.booster[i]);
        }
    }
    private void SetTextLevel()
    {
        textLevel.text = $"Lvl. {wrapperPlayer.Client.level + 1}";
    }
    
    public IEnumerator BonusResourceOnStart(Action callback)
    {
        if (GameManager.Instance.timeManager.timeBonusStart > 0 && GameManager.modeGame == ModeGamePlay.Time)
        {
            yield return new WaitForSecondsRealtime(0.6f);
            yield return UICollectEffect.Instance.DoBonusOnStartGame(iconTime, callback);
        }
    }

    public void OnHudHammer()
    {
        hudBoosterHarmer.SetActive(true);
    }

    public void OnHudFrozen()
    {
        hudFrozen.SetActive(true);
    }
}
