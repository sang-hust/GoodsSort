using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>
{
    public bool CameraToMouseRay(out RaycastHit raycastHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out raycastHit);
    }
}
