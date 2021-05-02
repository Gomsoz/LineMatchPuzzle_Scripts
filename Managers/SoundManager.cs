using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Defines.SoundType.SoundCnt];

    public void Init()
    {
        GameObject findSoundGo = GameObject.Find("@Sounds");
        if (findSoundGo == null)
        {
            findSoundGo = new GameObject { name = "@Sounds" };
            GameObject.DontDestroyOnLoad(findSoundGo);
        }

        for (int i = 0; i < (int)Defines.SoundType.SoundCnt; i++)
        {
            string[] typeNames = System.Enum.GetNames(typeof(Defines.SoundType));

            GameObject soundGo = new GameObject { name = typeNames[i] };
            soundGo.AddComponent<AudioSource>();
            _audioSources[i] = soundGo.GetComponent<AudioSource>();
        }
    }

    public void Play(string path, float pitch, Defines.SoundType soundType)
    {
        if (!path.Contains("Sounds/"))
            path = $"Sounds/{path}";

        AudioClip _audioClip = Managers.Resources.Load<AudioClip>(path);

        if (_audioClip == null)
        {
            Debug.Log($"Not Found Sound Clip : {path}");
            return;
        }

        if (soundType == Defines.SoundType.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Defines.SoundType.Bgm];

            audioSource.pitch = pitch;
            audioSource.loop = true;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)soundType];
            audioSource.clip = _audioClip;
            audioSource.PlayOneShot(_audioClip);
        }
    }

    public void Pause(Defines.SoundType Soundtype)
    {
        AudioSource audioSource = _audioSources[(int)Soundtype];

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            return;
        }
    }

    public void UnPause(Defines.SoundType Soundtype)
    {
        AudioSource audioSource = _audioSources[(int)Soundtype];

        if (audioSource.isPlaying == false)
        {
            audioSource.UnPause();
            return;
        }
    }
}
