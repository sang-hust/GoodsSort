using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSkill : MonoBehaviour
{
    [SerializeField] private TMP_Text lSkill;
    [SerializeField] private GameObject bgNum, iconPlus;
    [SerializeField] private Image bgBtnBlock, bgIcon;
    public Action onClick { get; set; }

    public void UpdateUI(int number)
    {
        if (number > 0)
        {
            GetComponent<UIResource>().ReplaceValue(number).DoImmediately();
            bgNum.SetActive(true);
            iconPlus.SetActive(false);
        }
        else
        {
            bgNum.SetActive(false);
            iconPlus.SetActive(true);
        }
    }
    
    public void OnClick()
    {
        onClick?.Invoke();
    }
}
