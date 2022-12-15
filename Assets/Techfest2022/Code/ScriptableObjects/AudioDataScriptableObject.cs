using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio Data")]
public class AudioDataScriptableObject : ScriptableObject
{
    [Header("Audio Setup Data")]
    [Space]
    [field: SerializeField]
    public List<AudioData> AudioData;
}