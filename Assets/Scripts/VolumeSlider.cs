using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VolumeSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;

    public void onSliderChanged()
    {
        AudioListener.volume = slider.value;

    }

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 1f;
        slider.minValue = 0f;
        slider.value = 0.75f;

        AudioListener.volume = slider.value;
    }
}
