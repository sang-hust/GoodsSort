using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    public void CheckWinAndNextLevel()
    {
        var isWin = GameManager.Instance.ListSelf.All(self => self.GetNumLayerCurrent() <= 0);
        if (!isWin) return;
        
        LevelManager.Instance.NextLevel();
    }
}
