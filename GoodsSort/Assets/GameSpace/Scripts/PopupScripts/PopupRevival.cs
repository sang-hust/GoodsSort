using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupRevival : BasePopup<PopupRevival>
{
    protected override void OnShowPopup()
    {
        
    }

    protected override void OnHidePopup()
    {
        
    }

    public void OnClickReplay()
    {
        GameManager.Instance.OnReplay();
    }

    public void OnClickAds()
    {
        if (true)
        {
            GameManager.Instance.timeManager.BonusTimeRemain(120);
            base.OnClickBack();
        }
    }
}
