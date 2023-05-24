using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Base UI Audio Data", order = 1)]
public class UISettings : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public List<AudioClip> uICanvasClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> titleClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> loadingClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> gameNormalClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> gameCombatClips = new List<AudioClip>();

    public void Init()
    {

    }
    public void OnBeforeSerialize()
    {
        Init();
    }

    public void OnAfterDeserialize()
    {

    }
}
