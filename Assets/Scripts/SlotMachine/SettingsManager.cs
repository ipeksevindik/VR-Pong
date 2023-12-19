using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Transform Head;
    public float SpawnDistance = 2;
    public GameObject Settings;
    public GameObject RightHand;
    public GameObject LeftHand;
    public InputActionProperty ShowButton;

    [SerializeField] private AudioMixer MyMixer;
    [SerializeField] private Slider SfxSlider;
    [SerializeField] private Slider MusicSlider;

    private void Start()
    {
        Settings.SetActive(false);
    }

    void Update()
    {
        if (ShowButton.action.WasPressedThisFrame())
        {
            LeftHand.SetActive(!Settings.activeSelf);
            RightHand.SetActive(!Settings.activeSelf);
            Settings.SetActive(!Settings.activeSelf);
            Settings.transform.position = Head.position + new Vector3(Head.forward.x, 0, Head.forward.z).normalized * SpawnDistance;
        }

        Settings.transform.LookAt(new Vector3(Head.position.x, Settings.transform.position.y, Head.position.z));
        Settings.transform.forward *= -1;
    }

    public void SetMusicVolume()
    { 
        float volume = MusicSlider.value;
        MyMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = SfxSlider.value;
        MyMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }


}
