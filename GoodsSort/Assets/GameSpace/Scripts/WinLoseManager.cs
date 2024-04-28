using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    [SerializeField] private TMP_Text quantityItem;
    private int quantityItemInLevel;
    public int QuantityItemInLevel => quantityItemInLevel;
    
    public void CheckWinAndNextLevel()
    {
        if (quantityItemInLevel > 0)
        {
            // Chua Win
            return;
        }
        
        LevelManager.Instance.NextLevel();
    }

    public void UpdateQuantityItem(int value)
    {
        quantityItemInLevel += value;
        quantityItem.text = quantityItemInLevel.ToString();
    }
    
    public void OutOfTime()
    {
        PopupUIManager.Instance.GetPopup<PopupRevival>().ShowPopup();
    }
}
