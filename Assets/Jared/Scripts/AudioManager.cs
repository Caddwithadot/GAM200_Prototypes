using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public GameObject MasSliderAS;
    public GameObject MusSliderAS;
    public GameObject SfxSliderAS;
    public AudioClip MasSliderSFX;
    public AudioClip MusSliderSFX;
    public AudioClip SfxSliderSFX;

    public float MasVolumeValue;
    public Slider MasVolumeSlider;

    public float MusVolumeValue;
    public Slider MusVolumeSlider;

    public float SfxVolumeValue;
    public Slider SfxVolumeSlider;

    void Start()
    {
        MasSliderAS = transform.GetChild(0).gameObject;
        MusSliderAS = transform.GetChild(1).gameObject;
        SfxSliderAS = transform.GetChild(2).gameObject;

        MasVolumeSlider.value = PlayerPrefs.GetFloat("Master");
        MusVolumeSlider.value = PlayerPrefs.GetFloat("Music");
        SfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX");

        MasSliderAS.gameObject.SetActive(true);
        MusSliderAS.gameObject.SetActive(true);
        SfxSliderAS.gameObject.SetActive(true);
    }

    void Update()
    {
        Mixer.SetFloat("Master", MasVolumeValue);
        PlayerPrefs.SetFloat("Master", MasVolumeValue);

        Mixer.SetFloat("Music", MusVolumeValue);
        PlayerPrefs.SetFloat("Music", MusVolumeValue);

        Mixer.SetFloat("SFX", SfxVolumeValue);
        PlayerPrefs.SetFloat("SFX", SfxVolumeValue);
    }

    public void SetMasLvl(float masLvl)
    {
        MasVolumeValue = masLvl;
        MasSliderAS.gameObject.GetComponent<AudioSource>().PlayOneShot(MasSliderSFX);
    }

    public void SetMusLvl(float musLvl)
    {
        MusVolumeValue = musLvl;
        MusSliderAS.gameObject.GetComponent<AudioSource>().PlayOneShot(MusSliderSFX);
    }

    public void SetSfxLvl(float sfxLvl)
    {
        SfxVolumeValue = sfxLvl;
        SfxSliderAS.gameObject.GetComponent<AudioSource>().PlayOneShot(SfxSliderSFX);
    }
}
