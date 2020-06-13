﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayLocalSound() // 하나의 고정된 사운드를 계속해서 반복 실행할 때 사용함.
    {
        _audioSource.Stop();
        _audioSource.Play();
    }

    public void PlayLocalSound(string _soundName, bool _isLoop = false) // 여러 개의 사운드를 변경하면서 실행할 때 사용함.
    {
        if(SoundManager.instance.sfxSound.ContainsKey(_soundName))
        {
            _audioSource.Stop();
            _audioSource.clip = SoundManager.instance.sfxSound[_soundName];
            if (_isLoop)
                _audioSource.loop = true;
            else
                _audioSource.loop = false;
            _audioSource.Play();
        }
    }
}
