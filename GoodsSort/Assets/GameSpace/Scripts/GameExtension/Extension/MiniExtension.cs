using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;

public static class TimeExtension
{
    private static readonly string[] NumToStr = Enumerable.Range(0, 60).Select(i => i.ToString("00")).ToArray();
    public static string ToTime(this long shit, string dayFormat, string timeFormat)
    {
        var day = shit / 86400;
        var hour = (shit % 86400) / 3600;
        var min = ((shit % 86400) % 3600) / 60;
        var second = ((shit % 86400) % 3600) % 60;

        var timeString = "";
        if (day > 0) timeString = dayFormat.Replace("DD", day.ToString("00"));
        timeString += timeFormat.Replace("hh", NumToStr[hour]).Replace("mm", NumToStr[min]).Replace("ss", NumToStr[second]);
        return timeString;
    }

    public static string ToTime(this long shit, string timeFormat)
    {
        var hour = shit / 3600;
        var min = (shit % 3600) / 60;
        var second = (shit % 3600) % 60;

        var timeString = "";
        timeString += timeFormat.Replace("hh", hour.ToString("00")).Replace("mm", NumToStr[min]).Replace("ss", NumToStr[second]);
        return timeString;
    }

    public static string ToTime(this int shit, string timeFormat)
    {
        var hour = shit / 3600;
        var min = (shit % 3600) / 60;
        var second = (shit % 3600) % 60;

        var timeString = "";
        timeString += timeFormat.Replace("hh", hour.ToString("00")).Replace("mm", NumToStr[min]).Replace("ss", NumToStr[second]);
        return timeString;
    }
}
public static class DoAllTheSameThingOneTime
{
    private static readonly Dictionary<string, DoAllTheSameThing> _dictionary = new Dictionary<string, DoAllTheSameThing>();
    public static void DoAction(string key, Action _actionDoManyTime, Action _actionSameDoOneTime)
    {
        if (!_dictionary.ContainsKey(key))
        {
            _dictionary.Add(key, new DoAllTheSameThing());
        }
        _dictionary[key].DoAction(_actionDoManyTime, _actionSameDoOneTime);
    }

    private class DoAllTheSameThing
    {
        private Action actionMain;
        private bool isSendRequested;
        public void DoAction(Action _actionDoManyTime, Action _actionSameDoOneTime)
        {
            _actionDoManyTime?.Invoke();
            actionMain = _actionSameDoOneTime;
            if (isSendRequested) return;
            isSendRequested = true;
            CoroutineChain.Start.Play(CoDo());
        }
        private IEnumerator CoDo()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            isSendRequested = false;
            actionMain?.Invoke();
        }
    }
}
public static class ColorExtension
{
    public static Color FromHex(string hex)
    {
        if (hex.Length < 6)
        {
            throw new System.FormatException("Needs a string with a length of at least 6");
        }

        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);
        string alpha;
        if (hex.Length >= 8)
            alpha = hex.Substring(6, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
            (int.Parse(g, NumberStyles.HexNumber) / 255f),
            (int.Parse(b, NumberStyles.HexNumber) / 255f),
            (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }

    public static void SetAlpha(this TMP_Text text, float value)
    {
        var colorNow = text.color;
        text.color = new Color(colorNow.r, colorNow.g, colorNow.b, value);
    }
}
public static class EnumExtension
{
    public static T Parse<T>(this string value, T defaultValue) where T : struct
    {
        if (null == value || value.Equals("")) return defaultValue;
        return (Enum.TryParse<T>(value, true, out var result) ? result : defaultValue);
    }
}

public static class ContextExtension
{
    public static string FormatNumber(int number)
    {
        var nfi = CultureInfo.CurrentCulture.NumberFormat;
        return number.ToString("N0", nfi);
    }
    public static string FormatNumber(long number)
    {
        var nfi = CultureInfo.CurrentCulture.NumberFormat;
        return number.ToString("N0", nfi);
    }
    public static bool IsCanClick()
    {
        SoundManager.Instance.PlaySfx("Touch1");
        if (PopupStack.IsWaiting) return false;
        return true;
    }
    public static void PlayCharacter(SkeletonGraphic character)
    {
        var animState = character.AnimationState;
        animState.SetEmptyAnimation(0, 0);
        animState.SetAnimation(0, "appear", false).Complete += _ =>
        {
            animState.SetAnimation(0, "idle", true);
        };
    }
    public static float ScalePopupByRatioScreen()
    {
        var ratio = (float)Screen.height / Screen.width;
        return Mathf.Clamp(ratio / 2, 0.7f, 1f);
    }
}
public static class DOTweenExtension
{
    public static Tweener DOTextInt(this TMP_Text text, int initialValue, int finalValue, float duration, Func<int, string> convertor)
    {
        return DOTween.To(
            () => initialValue,
            it => text.text = convertor(it),
            finalValue,
            duration
        );
    }
    public static Tweener DOTextInt(this TMP_Text text, int initialValue, int finalValue, float duration)
    {
        return text.DOTextInt(initialValue, finalValue, duration, ContextExtension.FormatNumber);
    }
}
// public static class AdsExtension
// {
//     public static void ShowVideoRewardAds(Action callbackSuccess, Action callbackFailed, string where)
//     {
//         IronSourceManager.Instance.ShowRewardedVideo(where, () => { callbackSuccess?.Invoke(); }, () => { callbackFailed?.Invoke(); });
//     }
//}