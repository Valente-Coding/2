using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private void Start() {
        float volume = PlayerPrefs.GetFloat("volume");
        GetComponent<Slider>().value = volume;
    }
}
