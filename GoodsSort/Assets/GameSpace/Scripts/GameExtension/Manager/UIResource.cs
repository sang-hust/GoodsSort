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
    public void DoCollectAnim(Vector3 start, Action callback = null)
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
    }
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
    public void DoImmediately()
    {
        textResource.text = FormatNumber(currentAmount);
        cacheAmount = currentAmount;
    }
}
