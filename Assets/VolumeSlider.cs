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
        AkSoundEngine.SetRTPCValue("Master_Volume", slider.value);

    }

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 100f;
        slider.minValue = 0f;
        slider.value = 0f;

        AkSoundEngine.SetRTPCValue("Master_Volume", slider.value);
    }
}
