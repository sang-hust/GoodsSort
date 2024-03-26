using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Tools;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AnimationState = Spine.AnimationState;
using Random = UnityEngine.Random;

public class NotifyManager : MMSingleton<NotifyManager>
{
    [SerializeField] private TMP_Text lMessage;
    [SerializeField] private Image[] imageBG;
    [SerializeField] private SkeletonAnimation spineClearStage;
    [SerializeField] private GameObject fade;

    private AnimationState animationState;
    private bool isShowing;
    private Action action;
    public void ShowWarning(string message, Action callback = null)
    {
        if (isShowing) return;
        imageBG[0].color = ColorExtension.FromHex("1D192F");
        imageBG[1].color = ColorExtension.FromHex("CC4333");
        lMessage.text = message;
        action = callback;
        PlayAnim(1.5f);
    }
    public void ShowSuccess(string message, Action callback = null, float timeShow = 1.5f)
    {
        if (isShowing) return;
        imageBG[0].color = ColorExtension.FromHex("283F93");
        imageBG[1].color = ColorExtension.FromHex("4D942B");
        lMessage.text = message;
        action = callback;
        PlayAnim(timeShow);
    }
    public IEnumerator CoShow(string message)
    {
        ShowWarning(message);
        yield return PlayAnim(1.5f);
    }
    public void ShowStageClear()
    {
        if (isShowing) return;
        spineClearStage.gameObject.SetActive(true);
        animationState = spineClearStage.AnimationState;
        animationState.SetAnimation(0, "animation", false);
        ParticleUIManager.Instance.PlayPaperFireworks();
        imageBG[0].color = ColorExtension.FromHex("283F93");
        imageBG[1].color = ColorExtension.FromHex("4D942B");

        lMessage.text = RandomStageClear();
        PlayAnim(1.5f);
    }

    private readonly string[] StageClearArr = new string[]
    {
        "Excellent",
        "Cool",
        "Awesome",
        "Perfect",
        "Great"
    };
    private string RandomStageClear()
    {
        var randIndex = Random.Range(0, StageClearArr.Length);
        return StageClearArr[randIndex];
    }
    private Sequence PlayAnim(float timeShow)
    {
        var sequence = DOTween.Sequence()
            .AppendCallback(() =>
            {
                fade.SetActive(true);
                isShowing = true;
                lMessage.enabled = false;
            })
            .Append(imageBG[0].rectTransform.DOSizeDelta(new Vector2(1280, 360), 0.2f)).Play()
            .Join(imageBG[0].DOFade(1, 0.2f))
            .Join(imageBG[1].rectTransform.DOSizeDelta(new Vector2(1280, 360), 0.2f))
            .AppendCallback(() => lMessage.enabled = true)
            .AppendInterval(timeShow)
            .AppendCallback(() => lMessage.enabled = false)
            .Append(imageBG[0].rectTransform.DOSizeDelta(new Vector2(0, 360), 0.15f))
            .Join(imageBG[0].DOFade(0, 0.15f))
            .Join(imageBG[1].rectTransform.DOSizeDelta(new Vector2(0, 360), 0.15f))
            .AppendCallback(() =>
            {
                spineClearStage.gameObject.SetActive(false);
                isShowing = false;
                fade.SetActive(false);
            })
            .AppendInterval(1)
            .AppendCallback(() =>
            {
                action?.Invoke();
                action = null;
            }).SetAutoKill(false);
        return sequence;
    }
}
