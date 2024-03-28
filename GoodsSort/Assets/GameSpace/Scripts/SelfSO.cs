using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfSO", menuName = "ScriptableObjects/SelfSO", order = 0)]
public class SelfSO : SerializedScriptableObject
{
    public List<SelfData> ListSelfData;
}