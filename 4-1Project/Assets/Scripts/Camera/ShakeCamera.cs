using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public float shakeDuration = 0.3f; // 진동 지속 시간
    public float shakeAmplitude = 1.2f; // 진폭
    public float shakeFrequency = 2.0f; // 빈도

    private float ShakeElapsedTime = 0;

    public int playerstateRoomnum = 1;

    public CinemachineVirtualCamera virtualcamera;
    private CinemachineBasicMultiChannelPerlin virtualcameraNoise;

    private void Awake()
    {
        if (virtualcamera != null)
            virtualcameraNoise = virtualcamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (virtualcamera != null || virtualcameraNoise != null)
        {
            if(ShakeElapsedTime > 0)
            {
                virtualcameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualcameraNoise.m_FrequencyGain = shakeFrequency;

                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualcameraNoise.m_AmplitudeGain = 0;
                ShakeElapsedTime = 0f;
            }
        }
    }

    public void PutShakeTime()
    {
        if (playerstateRoomnum == 9)
            ShakeElapsedTime = shakeDuration;
    }
}
