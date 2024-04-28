using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombo : MonoBehaviour
{
    [SerializeField] private TMP_Text textNumCombo;
    [SerializeField] private Image circleTime;
    // [SerializeField] private ParticleSystem effect;
    private Transform content;
    private CanvasGroup canvasGroup;
    private float factorPunch;
    private void Awake()
    {
        content = transform;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }
    public void DoCombo(int numCombo, float duration, Action callBackWhenOutTime)
    {
        textNumCombo.text = numCombo.ToString();
        factorPunch = Mathf.Clamp(numCombo, 1, 5);
        content.DOComplete();
        content.DOPunchScale(Vector3.one * 0.1f * factorPunch, 0.3f, 3, 0).SetEase(Ease.InOutElastic).SetTarget(content);
        circleTime.DOKill();    
        canvasGroup.alpha = 1;
        // effect?.Stop();
        // effect?.Play();
        DOTween.Sequence()
            .Append(DOTween.To(() => 1f, value => circleTime.fillAmount = value, 0, duration).SetEase(Ease.Linear))
            .AppendCallback(() => callBackWhenOutTime?.Invoke())
            .Append(DOTween.To(() => 1f, value => canvasGroup.alpha = value, 0, 0.2f).SetEase(Ease.Linear))
            .SetTarget(circleTime);
    }
    /* public void DoBonusGold(int amount)
    {
        GameManager.NumGoldBonusPerBase += amount;
        UICollectEffect.Instance.DoFloatAmount(transform.position - new Vector3(0.5f, 0, 0), amount, AtlasManager.Instance.GetSprite("IconGold"));
    } */
    public void ResetCombo() => circleTime.DOComplete();
}