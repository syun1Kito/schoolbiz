using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 1.92f || Mathf.Abs(transform.position.y) > 2.24)//画面外に出た弾を削除
        {
            Destroy(this.gameObject);
        }

    }
}
