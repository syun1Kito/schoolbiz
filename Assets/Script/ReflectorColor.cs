using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorColor : MonoBehaviour
{
 
    public static SpriteRenderer transparency;//うきわ透明度
   
    // Use this for initialization
    void Start()
    {
       transparency = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerController.limitTime <= 3)//うきわ残り時間3秒以下で点滅
        {

            transparency.color = new Color(1f,1f,1f, Mathf.Pow(Mathf.Cos(Mathf.PI * PlayerController.limitTime), 2));

        }
    }
}
