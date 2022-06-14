using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletX : MonoBehaviour
{
    
    private int argchange;
    private float xB, yB;//反射後の角度取得の始点座標
    private float deg, degE, degP, deltadeg, limitdegE;//ホーミングの角度について

    private int destroytype;//弾消滅時間の切り替え

    private Rigidbody2D forward;//弾正面方向への加速

    // Use this for initialization
    void Start()
    {

        forward = GetComponent<Rigidbody2D>();
    
        limitdegE = 5;//敵→自機のホーミング限界角度：5度
                         
        destroytype = EnemyController.fastdestroy;//弾生成時に消滅時間を取得
    
        if (destroytype == 0)
        {
            Destroy(gameObject, 20);//20秒で弾消滅
        }

    }

    // Update is called once per frame
    void Update()
    {


        //-----------------------------------敵が撃つ自機ホーミング
        if (this.tag == "P_homing")
        {


            degP = Mathf.Atan2(PlayerController.yP - transform.position.y,PlayerController.xP - transform.position.x) * Mathf.Rad2Deg;
            deltadeg = degP - (transform.eulerAngles.z + 90);//x軸正を基準とした弾正面と自機の角度の差

            if (deltadeg > 180)//角度差を-180°～180°に統一↓
            {
                deltadeg -= 360;
            }
            else if (deltadeg < -180)
            {
                deltadeg += 360;
            }

            if (deltadeg > limitdegE)//ホーミング限界角度に制限↓
            {
                deltadeg = limitdegE;
            }
            else if (deltadeg < -limitdegE)
            {
                deltadeg = -limitdegE;
            }

            transform.Rotate(0, 0, deltadeg);//角度を更新して加速↓
            forward.AddForce(transform.up);

        }



    }

   
}
