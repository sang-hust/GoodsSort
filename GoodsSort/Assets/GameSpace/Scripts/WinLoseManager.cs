using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    public static int QuantityItemInLevel;
    public void CheckWinAndNextLevel()
    {
        if (QuantityItemInLevel > 0)
        {
            // Chua Win
            return;
        }
        
        
        LevelManager.Instance.NextLevel();
    }
}
