using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class PopupUIManager : MMSingleton<PopupUIManager>
{
    public PopupListPrefab ListPopupPrefab;
    public PopupFadeManager fadeManager;
    [FormerlySerializedAs("poolPopup")] public Transform poolPopupTrans;
    [FormerlySerializedAs("stackPopup")] public Transform stackPopupTrans;

    public readonly PopupStack popupStack = new PopupStack();
    public T GetPopup<T>() where T : IPopup
    {
        var type = typeof(T);
        if (popupStack.Popups.TryGetValue(type, out var popup))
        {
            var p = popup.GetGameObject();
            if (popupStack.StackPopupShowing.Contains(type) && popupStack.CurrentPopupType != type)
            {
                HidePopup(type);
            }
            return p.GetComponent<T>();
        }

        var newPopup = Instantiate(ListPopupPrefab.dictionaryPopup[type], poolPopupTrans);
        var IPopup = newPopup.GetComponent<T>();
        //newPopup.transform.ResetTransform();
        newPopup.SetActive(false);
        popupStack.Popups.Add(type, IPopup);
        return IPopup;
    }
    public void ShowPopup(Type type)
    {
        if (!popupStack.Popups.ContainsKey(type))
        {
            return;
        }
        var fade = fadeManager.GetFade(type);
        var p = popupStack.Popups[type].GetGameObject().transform;
        fade.name = "-----Fade-----" + type.Name;
        fade.transform.SetParent(stackPopupTrans);
        fade.transform.ResetTransform();
        p.SetParent(stackPopupTrans);
        p.GetComponent<RectTransform>().ResetRectTransform();
        p.gameObject.SetActive(true);
        popupStack.StackPopupShowing.Push(type);
    }
    public void HidePopup(Type type)
    {
        if (fadeManager == null) return;
        fadeManager.ReturnFade(type);
        
        var popup = popupStack.Popups[type].GetGameObject().transform;
        popup.SetParent(poolPopupTrans);
        popup.ResetTransform();
        popup.gameObject.SetActive(false);
        
        popupStack.RemoveFromStackShowing(type);
    }
}

public class PopupStack
{
    public readonly Dictionary<Type, IPopup> Popups = new Dictionary<Type, IPopup>();
    public readonly Stack<Type> StackPopupShowing = new Stack<Type>();
    public readonly Stack<Type> StackPopupNext = new Stack<Type>();
    public static bool IsWaiting;
    public Type CurrentPopupType => AmountPopupShowing == 0 ? null : StackPopupShowing.Peek();
    public IPopup CurrentPopup => AmountPopupShowing == 0 ? null : Popups[CurrentPopupType];
    public int AmountPopupShowing => StackPopupShowing.Count;
    public int AmountPopupNext => StackPopupNext.Count;

    public void RemoveFromStackShowing(Type type)
    {
        if (!StackPopupShowing.Contains(type))
        {
            return;
        }
        var tempStack = new Stack<Type>();

        while (StackPopupShowing.Count > 0 && StackPopupShowing.Peek() != type)
        {
            tempStack.Push(StackPopupShowing.Pop());
        }
        // Xóa phần tử cần tìm
        if (StackPopupShowing.Count > 0)
        {
            StackPopupShowing.Pop();
        }

        // Đưa các phần tử từ tempStack trở lại StackPopupShowing
        while (tempStack.Count > 0)
        {
            StackPopupShowing.Push(tempStack.Pop());
        }
    }

}