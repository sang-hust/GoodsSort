using System;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // example spawn item in layer
        }
    }
#endif
}
