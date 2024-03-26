using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Tools;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UICollectEffect : MMSingleton<UICollectEffect>
{
    [SerializeField] private Image iconPrefab;
    public void DoCollect(Vector3 start, Image target, Action callback = null, float delay = 0, float scaleIcon = 1)
    {
        var transTarget = target.transform;
        var icon = GetIconFromPool(target.sprite, start, scaleIcon);
        DOTween.Sequence()
            .AppendCallback(() => icon.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 0.4f)).SetEase(Ease.InOutElastic))
            .AppendInterval(delay + 0.3f)
            .Append(icon.DOMove(transTarget.position, 0.7f).SetEase(Ease.InBack))
            .Join(icon.DOScale(Vector3.one, 0.7f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                callback?.Invoke();
                PoolManager.Pools["Icon"].Despawn(icon, PoolManager.Pools["Icon"].transform);
                transTarget.DOComplete();
                transTarget.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.15f, 3, 0).SetEase(Ease.InOutElastic).SetTarget(transTarget);
            });
    }
    public void DoCollect(Vector3 start, Sprite sprite, Transform target, Action callback = null)
    {
        var transTarget = target.transform;
        var icon = GetIconFromPool(sprite, start);
        DOTween.Sequence()
            .AppendCallback(() => icon.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 0.4f)).SetEase(Ease.InOutElastic))
            .AppendInterval(0.3f)
            .Append(icon.DOMove(transTarget.position, 0.7f).SetEase(Ease.InBack))
            .AppendCallback(() =>
            {
                callback?.Invoke();
                PoolManager.Pools["Icon"].Despawn(icon, PoolManager.Pools["Icon"].transform);
                transTarget.DOComplete();
                transTarget.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f, 3, 0).SetEase(Ease.InOutElastic).SetTarget(transTarget);
            });
    }
    public IEnumerator DoBonusOnStartGame(Image target, Action callback = null)
    {
        SoundManager.Instance.PlaySfx("AddMoveStart");
        var transTarget = target.transform;
        var icon = GetIconFromPool(target.sprite, new Vector3(-3, 0, 0.9f), 3);
        icon.SetLocalScale(3, 3, 3);
        var seq = DOTween.Sequence()
            .AppendInterval(0.7f)
            .Append(icon.DOLocalMoveX(0, 0.2f))
            .AppendInterval(0.35f)
            .Append(icon.DOMove(transTarget.position, 0.7f).SetEase(Ease.InBack))
            .Join(icon.DOScale(Vector3.one, 0.7f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                callback?.Invoke();
                PoolManager.Pools["Icon"].Despawn(icon, PoolManager.Pools["Icon"].transform);
                transTarget.DOComplete();
                transTarget.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f, 3, 0).SetEase(Ease.InOutElastic).SetTarget(transTarget);
            });
        yield return seq.WaitForCompletion();
    }
    private Transform GetIconFromPool(Sprite sprite, Vector3 start, float scale = 1)
    {
        var icon = PoolManager.Pools["Icon"].Spawn(iconPrefab.transform);
        var image = icon.GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        
        icon.SetParent(PopupUIManager.Instance.stackPopupTrans);
        icon.SetLocalScale(scale, scale, scale);
        icon.gameObject.SetActive(true);
        icon.position = new Vector3(start.x + Random.Range(-0.2f, 0.2f), start.y + Random.Range(-0.2f, 0.2f), start.z);
        icon.SetAsLastSibling();
        return icon;
    }
}
