using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlasterBarController : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    public void SetMaxPoint(float point)
    {
        slider.maxValue = point;
        slider.value = point;
    }
    public void SetPoint(float point)
    {
        slider.value = point;
    }
}
