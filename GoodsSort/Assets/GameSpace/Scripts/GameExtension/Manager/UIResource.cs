using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{
    [SerializeField] private Image iconResource;
    [SerializeField] private TMP_Text textResource;
    [SerializeField] private string keySFX;
    [SerializeField] private float scaleIcon = 1;

    private int currentAmount, cacheAmount, delta;

    public Func<int,string> FormatNumber = ContextExtension.FormatNumber;
    public UIResource MoreValue(int more)
    {
        delta = Mathf.Min(Mathf.Abs(more), 10);
        currentAmount += more;
        return this;
    }
    public UIResource MultiValue(float coEff)
    {
        delta = (int)Mathf.Min(currentAmount * coEff - currentAmount, 10);
        currentAmount = (int)(currentAmount * coEff);
        return this;
    }
    public UIResource ReplaceValue(int newValue)
    {
        delta = Mathf.Min(newValue - currentAmount, 10);
        currentAmount = newValue;
        return this;
    }
    public UIResource DoCollectAnim(Vector3 start, Action callback = null)
    {
        var current = currentAmount;
        durationDOText = 0.15f * delta + 0.3f;
        UICollectEffect.Instance.DoCollect(start, iconResource, () =>
        {
            DoAnim(current, callback);
            PlaySFX();
        }, 0, scaleIcon);
        for (var i = 1; i < delta; i++)
        {
            UICollectEffect.Instance.DoCollect(start, iconResource, PlaySFX, i * 0.1f, scaleIcon);
        }
        return this;
    }
    public UIResource DoCollectImmediately(Vector3 start, Action callback = null)
    {
        durationDOText = 0.15f * delta + 0.3f;
        UICollectEffect.Instance.DoCollect(start, iconResource, () =>
        {
            DoImmediately();
            callback?.Invoke();
            PlaySFX();
        }, 0, scaleIcon);
        for (var i = 1; i < delta; i++)
        {
            UICollectEffect.Instance.DoCollect(start, iconResource, PlaySFX, i * 0.1f, scaleIcon);
        }

        return this;
    }
    public void DoFadeAmount(Vector3 start, int amount, Sprite sprite = null) => UICollectEffect.Instance.DoFloatAmount(start, amount, sprite);
    public void DoFadeAmountAndCollect(Vector3 start, int amount, Action callback = null) => UICollectEffect.Instance.DoFloatAmountAndCollect(start, amount, iconResource, callback);
    private float durationDOText;
    private void DoAnim(int current, Action callback)
    {
        if (textResource == null) return;
        DOTween.Sequence()
            .Append(textResource.transform.DOScale(1.2f, 0.3f))
            .Join(textResource.DOTextInt(cacheAmount, current, durationDOText, FormatNumber).SetTarget(cacheAmount))
            .AppendCallback(() => callback?.Invoke())
            .Append(textResource.transform.DOScale(1f, 0.3f));
        cacheAmount = current;
    }
    private void PlaySFX()
    {
        if (!string.IsNullOrEmpty(keySFX)) SoundManager.Instance.PlaySfx(keySFX);
    }
    public void DoAnim(float timeDuration, Action callback = null)
    {
        DOTween.Sequence()
            .Append(textResource.DOTextInt(cacheAmount, currentAmount, timeDuration, FormatNumber))
            .AppendCallback(() => cacheAmount = currentAmount)
            .AppendInterval(0.5f)
            .AppendCallback(() => callback?.Invoke());
    }
    public UIResource DoImmediately()
    {
        textResource.text = FormatNumber(currentAmount);
        cacheAmount = currentAmount;
        return this;
    }
    public void DoPunchText()
    {
        textResource.DOComplete();
        textResource.transform.DOPunchScale(Vector3.one * 0.2f, 0.15f, 3, 0).SetEase(Ease.InOutElastic).SetTarget(textResource);
    }
    public void DoColorText(string hexColor)
    {
        textResource.color = ColorExtension.FromHex(hexColor);
    }
}
