using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SelfSO", menuName = "ScriptableObjects/SelfSO", order = 0)]
public class SelfSO : SerializedScriptableObject
{
    public Dictionary<int, List<SelfData>> LevelData = new Dictionary<int, List<SelfData>>();
}
