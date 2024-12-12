using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class UISetVolume : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;

    public void SetLevel(float sliderValue)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
