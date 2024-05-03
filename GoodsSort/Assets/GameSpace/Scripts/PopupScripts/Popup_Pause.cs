using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Pause : BasePopup<Popup_Pause>
{
    protected override IEnumerator OnShowPopup()
    {
        yield break;

    }

    protected override void OnHidePopup()
    {
    }

    public void OnClickReplay()
    {
        SoundManager.Instance.PlaySfx("Touch");
        LevelManager.Instance.OnReplay();
    }
    
    public void OnClickResume()
    {
        SoundManager.Instance.PlaySfx("Touch");
        GameManager.Instance.timeManager.pause = false;
        GameManager.Instance.timeManager.StartCountTime();
        base.OnClickBack();
    }
}
