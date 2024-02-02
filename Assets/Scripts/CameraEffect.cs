using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField]
    GameObject VirtualCamera;
    [SerializeField]
    float ShakeIntensity;
    [SerializeField]
    float totalShakeTime;
    float shakeTimer;

    CinemachineVirtualCamera CMVcam;

    private void Start()
    {
        CMVcam = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        UpdateShakeTimer();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin noise = CMVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = ShakeIntensity;
        noise.m_FrequencyGain = 5;
        shakeTimer = totalShakeTime;
    }

    public void StopCameraShake()
    {
        CinemachineBasicMultiChannelPerlin noise = CMVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    void UpdateShakeTimer()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopCameraShake();
            }
        }
    }
}
