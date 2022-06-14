using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{


    public AudioSource BGM1, BGM2;

    // Use this for initialization
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        BGM1 = audioSources[0];
        BGM2 = audioSources[1];

        BGM1.Play();
        BGM2.Play();
        BGM1.volume = 1;
        BGM2.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeScale == 0)//強制停止
        {
            BGM1.volume = 0;
            BGM2.volume = 0;
        }
        else
        {
            //フェードでBGM切り替え
            if (GameController.BGMselect == 0)
            {
                if (BGM1.volume <= 1)
                {
                    BGM1.volume += Time.deltaTime;
                }

                if (BGM2.volume >= 0)
                {
                    BGM2.volume -= Time.deltaTime;
                }

            }

            if (GameController.BGMselect == 1)
            {
                if (BGM2.volume <= 1)
                {
                    BGM2.volume += Time.deltaTime;
                }

                if (BGM1.volume >= 0)
                {
                    BGM1.volume -= Time.deltaTime;
                }

            }

            if (GameController.BGMselect == 2)
            {

                if (BGM1.volume >= 0)
                {
                    BGM1.volume -= Time.deltaTime;
                }

                if (BGM2.volume >= 0)
                {
                    BGM2.volume -= Time.deltaTime;
                }

            }

        }

    }

}
