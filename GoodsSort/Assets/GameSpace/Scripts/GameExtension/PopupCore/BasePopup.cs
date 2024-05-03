using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public interface IPopup
{
    IEnumerator CoShowPopup();
    IEnumerator CoHidePopup(Action callback);
    void ShowPopup();
    void ShowPopupReplace(bool canBack);
    void OnClickBack();
    GameObject GetGameObject();
}

public abstract class BasePopup<T> : MonoBehaviour, IPopup where T : BasePopup<T>
{
    [SerializeField] protected Transform content;
    protected float durationShow = 0.2f, durationHide = 0.3f;
    protected float ratioSize;
    /// <summary>
    /// Để thực hiện animation hiện popup, không nên dùng trực tiếp
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoShowPopup()
    {
        PopupUIManager.Instance.ShowPopup(typeof(T));
        if (content)
        {
            ratioSize = ContextExtension.ScalePopupByRatioScreen();
            content.localScale = Vector3.one * (ratioSize - 0.2f);
            content.DOScale(Vector3.one * ratioSize, durationShow).SetEase(Ease.OutBack).SetUpdate(true);
        }
        OnShowPopup();
        yield return new WaitForSecondsRealtime(durationShow);
    }
    /// <summary>
    /// Ẩn popup hiện tại để mở popup này lên
    /// </summary>
    /// <param name="canBack">Có thể trở về popup cũ đã ẩn đi không, nếu có thì sẽ stack
    /// popup cũ lại</param>
    /// <returns></returns>
    private IEnumerator CoShowPopupReplace(bool canBack)
    {
        var pAtTop = PopupUIManager.Instance.popupStack.CurrentPopup;
        if (pAtTop != null)
        {
            var t = PopupUIManager.Instance.popupStack.CurrentPopupType;
            yield return pAtTop.CoHidePopup(() => StackToNextPopup(canBack ? t : null));
        }
        yield return CoShowPopup();
    }
    /// <summary>
    /// Clear stack next (để tránh lỗi) và thêm popup này vào stack next để mở trong lần kế tiếp
    /// </summary>
    /// <param name="t">type của Popup</param>
    private static void StackToNextPopup(Type t)
    {
        if (t == null) return;
        var popupStack = PopupUIManager.Instance.popupStack;
        popupStack.StackPopupNext.Clear();
        popupStack.StackPopupNext.Push(t);
    }
    /// <summary>
    /// Hiện lần lượt tất cả các popup đã lưu trong stack next
    /// </summary>
    /// <returns></returns>
    private static IEnumerator CoShowNextPopup()
    {
        var popupStack = PopupUIManager.Instance.popupStack;
        while (popupStack.AmountPopupNext > 0)
        {
            var typeP = popupStack.StackPopupNext.Pop();
            yield return popupStack.Popups[typeP].CoShowPopup();
        }
    }
    /// <summary>
    /// Thực hiện Animation ẩn popup, không nên gọi trực tiếp
    /// </summary>
    /// <param name="callback">call back sau khi ẩn xong, thường là dùng để lưu popup này vào stack next</param>
    /// <returns></returns>
    public virtual IEnumerator CoHidePopup(Action callback = null)
    {
        if (PopupUIManager.Instance.popupStack.AmountPopupShowing <= 0) yield break;
        OnHidePopup();
        // if (content)
        // {
        //     content.DOScale(new Vector3(0.9f, 0.9f), durationHide).SetEase(Ease.InBack).SetUpdate(true);
        //     yield return new WaitForSecondsRealtime(durationHide);
        // }
        PopupUIManager.Instance.HidePopup(typeof(T));
        callback?.Invoke();
    }
    /// <summary>
    /// Hiện popup
    /// </summary>
    public virtual void ShowPopup()
    {
        if (PopupStack.IsWaiting || PopupUIManager.Instance.popupStack.CurrentPopupType == typeof(T))
        {
            return;
        }
        PopupStack.IsWaiting = true;
        CoroutineChain.Start.Play(CoShowPopup()).Call(() => PopupStack.IsWaiting = false);
    }
    /// <summary>
    /// Hiện và thay thế popup hiện tại
    /// </summary>
    /// <param name="canBack">Có thể trở về popup cũ đã ẩn đi không</param>
    public void ShowPopupReplace(bool canBack)
    {
        if (PopupStack.IsWaiting || PopupUIManager.Instance.popupStack.CurrentPopupType == typeof(T))
        {
            return;
        }
        PopupStack.IsWaiting = true;
        CoroutineChain.Start.Play(CoShowPopupReplace(canBack)).Call(() => PopupStack.IsWaiting = false);
    }
    /// <summary>
    /// Ẩn popup
    /// </summary>
    public virtual void OnClickBack()
    {
        if (PopupStack.IsWaiting || PopupUIManager.Instance.popupStack.AmountPopupShowing == 0)
        {
            return;
        }
        SoundManager.Instance.PlaySfx("Touch1");
        PopupStack.IsWaiting = true;
        CoroutineChain.Start.Play(CoHidePopup()).Play(CoShowNextPopup()).Call(() => PopupStack.IsWaiting = false);
    }
    
    public virtual void OnClickBackNotSound()
    {
        if (PopupStack.IsWaiting || PopupUIManager.Instance.popupStack.AmountPopupShowing == 0)
        {
            return;
        }
        PopupStack.IsWaiting = true;
        CoroutineChain.Start.Play(CoHidePopup()).Play(CoShowNextPopup()).Call(() => PopupStack.IsWaiting = false);
    }
    public GameObject GetGameObject() => gameObject;
    /// <summary>
    /// Call back khi hiện, nên dùng thay cho OnEnable
    /// </summary>
    protected abstract IEnumerator OnShowPopup();
    /// <summary>
    /// Call back khi ẩn, nên dùng thay cho OnDisable
    /// </summary>
    protected abstract void OnHidePopup();
}