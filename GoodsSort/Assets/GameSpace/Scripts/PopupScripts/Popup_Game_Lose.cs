using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Game_Lose : BasePopup<Popup_Game_Lose>
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
}
