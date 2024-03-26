using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundData", fileName = "SoundData", order = 0)]
public class SoundData : SerializedScriptableObject
{
    public Dictionary<string, AudioClip> listAudioClip;
}
