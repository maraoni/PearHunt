using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public struct SoundInstance
{
    [SerializeField] private AudioSource source;
    public SoundEffects effects;

    public void PlaySoundEffect()
    {
        source.Play();
    }
}

public enum SoundEffects
{
   Taunt1,
   Taunt2,
   Taunt3,

}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<SoundInstance> soundInstances = new();

    public void PlaySoundEffect(SoundEffects anEffect)
    {
        for (var i = 0; i < soundInstances.Count; i++)
            if (soundInstances[i].effects == anEffect)
            {
                soundInstances[i].PlaySoundEffect();
                return;
            }
    }

    [ServerRpc]
    public void PlaySound_ServerRPC(SoundEffects anEffect)
    {
        Broadcast_ClientRPC(anEffect);
    }

    [ClientRpc]
    public void Broadcast_ClientRPC(SoundEffects anEffect)
    {
        PlaySoundEffect(anEffect);
    }

    #region Singleton

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("[SoundManager] Mutiple soundmanagers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion
}