using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager {
    public AudioManager(GameFacade facade) : base(facade) { }

    public const string Sound_Prefix = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";
    public const string Sound_ButtonClick = "ButtonClick";


    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate), 0.3f, true);
    }

    private AudioClip LoadSound(string soundname)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundname);
    }


    public void PlayBgSound(string soundname)
    {
        PlaySound(bgAudioSource, LoadSound(soundname), 0.3f, true);
    }

    public void PlayNormalSound(string soundname)
    {
        PlaySound(normalAudioSource, LoadSound(soundname), 1f);
    }


    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
