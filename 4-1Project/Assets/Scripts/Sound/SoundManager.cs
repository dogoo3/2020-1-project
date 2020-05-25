using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip sound;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM 등록")]
    public Sound[] bgmSound;
    [Header("SFX 등록")]
    public Sound[] sfxSound;
    [Header("BGM 플레이어")]
    public AudioSource bgmPlayer;
    [Header("SFX 플레이어")]
    public AudioSource[] sfxPlayer;

    private void Awake()
    {
        instance = this;
    }

    public void PlayBGM(string _bgmName)
    {
        for(int i=0;i<bgmSound.Length;i++)
        {
            if (bgmSound[i].name == _bgmName)
            {
                bgmPlayer.clip = bgmSound[i].sound;
                bgmPlayer.Play();
            }
        }
    }

    public void PlaySFX(string _sfxName)
    {
        for(int i=0;i<sfxSound.Length;i++) // 사운드 검색
        {
            if(sfxSound[i].name ==_sfxName) 
            {
                for(int j=0;j<sfxPlayer.Length;j++) // 재생되지 않는 AudioSource 검색
                {
                    if(!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfxSound[i].sound;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
            }
        }
    }
}
