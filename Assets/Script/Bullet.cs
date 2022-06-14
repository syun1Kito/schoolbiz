using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {



    public Sprite sprite1;//反射後の弾
    private SpriteRenderer spriteRenderer;

    private int argchange;
    private int reversecount;//反射後の角度取得のための遅延生成
    private float xB, yB;//反射後の角度取得の始点座標
    private float deg, degE, degP, deltadeg, limitdegE, limitdegP;//ホーミングの角度について

    private int destroytype;//弾消滅時間の切り替え
    private float destroytime;//弾消滅までの時間蓄積

    
    
    private Rigidbody2D forward;//弾正面方向への加速

    // Use this for initialization
    void Start()
    {

        forward = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        limitdegE = 5;//敵→自機のホーミング限界角度：5度
        limitdegP = 10;//自機→敵のホーミング限界角度：10度


        reversecount = 0;

        destroytype = EnemyController.fastdestroy;//弾生成時に消滅時間を取得
        destroytime = 0;

        if (destroytype == 0)
        {
            Destroy(gameObject, 20);//20秒で弾消滅
        }
        
    }
	
	// Update is called once per frame
	void Update()
    {

        
        //-----------------------------------配置型の弾幕の消滅
        if (destroytype == 1)
        {
            destroytime += Time.deltaTime;
        }

        if (destroytime > 2)
        {
            Destroy(gameObject);
        }

        //-----------------------------------敵が撃つ自機ホーミング
        if (this.tag == "P_homing")
        {


            degP = Mathf.Atan2(PlayerController.yP - 0.08f - transform.position.y,PlayerController.xP - transform.position.x) * Mathf.Rad2Deg;
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

        //-----------------------------------反射による弾の角度反転
        if (argchange == 1)
        {

                       
            if (reversecount == 2)
            {
                xB = transform.position.x;
                yB = transform.position.y;
            }

            if (reversecount == 3)
            {
                deg = Mathf.Atan2(transform.position.y - yB, transform.position.x - xB) * Mathf.Rad2Deg -90f;
                //1フレーム後の座標から進行方向角度を算出

                transform.rotation = Quaternion.Euler(0, 0, deg);

                argchange = 0;//初期化↓
                reversecount = 0;
                
                this.tag = "E_homing";//反射後の弾は5秒で消滅↓
                Destroy(gameObject, 5);
            }

            reversecount += 1;
           
        }

        //-----------------------------------自機が撃つ敵ホーミング
        if (this.tag == "E_homing")
        {

            if (destroytype != 0)//消滅時間蓄積の停止↓
            {
                destroytype = 0;
            }

            degE = Mathf.Atan2(EnemyController.yE - transform.position.y, EnemyController.xE - transform.position.x) * Mathf.Rad2Deg;
            deltadeg = degE - (transform.eulerAngles.z + 90);

            //敵のホーミングと同様↓
            if (deltadeg > 180)
            {
                deltadeg -= 360;
            }
            else if (deltadeg < -180)
            {
                deltadeg += 360;
            }

            if (deltadeg > limitdegP)
            {
                deltadeg = limitdegP;
            }
            else if (deltadeg < -limitdegP)
            {
                deltadeg = -limitdegP;
            }

            transform.Rotate(0, 0, deltadeg);
            forward.AddForce(transform.up * 4);

        }

    }
    //---------------------------------------うきわへの当たり判定
    void OnTriggerEnter2D(Collider2D op)
    {
        if (op.tag == "Reflector")
        {
            argchange = 1;

            spriteRenderer.sprite = sprite1;//弾の色を赤に
          
        }

    }
}
