using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM 등록")]
    public Dictionary<string, AudioClip> bgmSound = new Dictionary<string, AudioClip>();
    [Header("SFX 등록")]
    public Dictionary<string, AudioClip> sfxSound = new Dictionary<string, AudioClip>();
    [Header("BGM 플레이어")]
    public AudioSource bgmPlayer;
    [Header("SFX 플레이어")]
    public AudioSource[] sfxPlayer;

    string temp;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        AudioClip[] obj_bgm = Resources.LoadAll<AudioClip>("Sound/BGM");
        AudioClip[] obj_sfx = Resources.LoadAll<AudioClip>("Sound/SFX");

        for (int i = 0; i < obj_sfx.Length; i++)
        {
            // 리소스 폴더에서 사운드 등록
            sfxSound.Add(obj_sfx[i].name, obj_sfx[i] as AudioClip);
            if (i < obj_bgm.Length)
                bgmSound.Add(obj_bgm[i].name, obj_bgm[i] as AudioClip);
        }
    }

    public void PlayBGM(string _bgmName)
    {
        if (bgmSound.ContainsKey(_bgmName)) // 내가 재생하려는 BGM이 있으면
        {
            bgmPlayer.clip = bgmSound[_bgmName];
            bgmPlayer.Play();
        }
    }

    public void PlaySFX(string _sfxName, bool _isOverlapSound = true)
    {
        if (!_isOverlapSound) // 효과음이 중복으로 재생되면 안 됩니다.
        {
            for (int i = 0; i < sfxPlayer.Length; i++) // 현재 실행중인 사운드 검색
            {
                if (sfxPlayer[i].clip != null) // AudioSource의 Clip에 사운드가 있는지 체크
                {
                    if (sfxPlayer[i].clip.name == _sfxName) // AudioClip에 이미 내가 실행하려는 사운드가 등록되어 있을 경우
                    {
                        if (sfxPlayer[i].isPlaying) // 그 AudioClip이 실행중이면
                            return; // 효과음을 재생하지 않음.
                    }
                }
            }
        }

        if (sfxSound.ContainsKey(_sfxName)) // 내가 재생하려는 사운드 이름과 효과음 배열의 사운드 이름과 똑같으면
        {
            for (int i = 0; i < sfxPlayer.Length; i++) // 재생되지 않는 AudioSource 검색
            {
                if (!sfxPlayer[i].isPlaying) // 재생되지 않는 AudioSource를 찾으면
                {
                    sfxPlayer[i].clip = sfxSound[_sfxName]; // 효과음 플레이어에 사운드를 넣어주고
                    sfxPlayer[i].Play(); // 실행한다
                    return;
                }
            }
        }
    }

    public void StopSFX(string _sfxName)
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].clip != null) // AudioSource의 Clip에 사운드가 있는지 체크
            {
                if (sfxPlayer[i].clip.name == _sfxName) // 정지하고 싶은 효과음을 발견하면
                {
                    if (sfxPlayer[i].isPlaying) // 재생 중이면
                    {
                        sfxPlayer[i].Stop(); // 멈춘다.
                        return;
                    }
                }
            }
        }
    }
}
