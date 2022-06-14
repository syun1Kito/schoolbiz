using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorGauge : MonoBehaviour
{

   public  UnityEngine.UI.Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();        
        slider.maxValue = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.reflectorstock < 3)
        {
            slider.value = PlayerController.reflectorcount;
            
        }
        else
        {
            slider.value = 5;//カウントストップ時はゲージ最大
        }
    }
}
