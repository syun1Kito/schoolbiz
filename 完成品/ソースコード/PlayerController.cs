using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed, lowspeed, defspeed;//スピード　現在、低速、通常
    private float rangex,rangey;//移動可能範囲

    public static int hp,maxhp;//敵現在体力、最大値
    public static int playertimescale;//0=ポーズ中 1=ゲーム進行中

    public static float xP, yP;//現在のプレイヤー座標
    public GameObject reflectorAct;//うきわアクティブ状況
    private bool preAct;//1フレーム前のうきわアクティブ状況
    public GameObject playerdownhalf;//自機下半身
    private SpriteRenderer transparencyup, transparencydown;//自機透明度   
    
    public GameObject ukiwa1Act, ukiwa2Act, ukiwa3Act;//うきわストック状況
    public GameObject life1, life2, life3;//残機
    public Transform reflector;//うきわ本体       
    public static float reflectorcount;//うきわ取得までのカウントアップ
    public static int reflectorgettime;//うきわ取得時間
    public static int reflectorstock;//うきわ所持数    
    public static float limitTime;//うきわ使用可能時間

    public static int judge;//当たり判定　0=当たり判定あり 1=被弾時 2=復活時
    private int stop;//被弾時の移動停止
    private float disapx, disapy;//被弾時自機サイズ変更
    private float regenetime;//復活時無敵時間
    private float disappausetime;//被弾から復活までの時間

 　 public AudioSource stockup, defeat, stockuse, reflectoroff;//SE

    void Start()
    {
        hp = 4;
        maxhp = hp;

        speed = 2.5f;
        defspeed = speed;
        lowspeed = speed * 0.4f;

        rangex = 1.92f;
        rangey = 2.24f;
        
        AudioSource[] audioSources = GetComponents<AudioSource>();
        stockup = audioSources[0];
        defeat = audioSources[1];
        stockuse = audioSources[2];
        reflectoroff = audioSources[3];


        transparencyup = GetComponent<SpriteRenderer>();
        transparencydown = playerdownhalf.GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector2(0, -1.75f);//スタート位置

        reflectorAct.SetActive(false);
        reflectorcount = 0;
        reflectorstock = 0;
        reflectorgettime = 5;

        ukiwa1Act.SetActive(false);
        ukiwa2Act.SetActive(false);
        ukiwa3Act.SetActive(false);

        judge = 0;
        disapx = 1;
        disapy = 1;
        regenetime = 3;
        stop = 0;
        disappausetime = 1;
        playertimescale = 1;
               
    }

    //---------------------------------被弾or敵への衝突
    void OnTriggerEnter2D(Collider2D op)
    {

        if (judge == 0)
        {

            if (op.tag == "E_bullet" || op.tag == "P_homing" || op.tag == "Enemy")
            {

                judge = 1;
                hp -= 1;
                limitTime = 0;
            }
        }
    }



    void Update()
    {


        xP = transform.position.x;
        yP = transform.position.y;
        preAct = reflectorAct.activeSelf;
       
        //-----------------------------復活
        if (judge == 1)
        {
            if (disapx > 0)
            {
                disapx -= 6f * Time.deltaTime;
                disapy += 6f * Time.deltaTime;
                transform.localScale = new Vector3(0.8f * disapx, 0.8f * disapy, 1);
            }

            if (stop != 1)
            {
                stop = 1;
                defeat.PlayOneShot(defeat.clip);
            }


            if (disapx <= 0)
            {
                if (disapx != 0)
                {
                    transform.localScale = new Vector3(0, 0, 1);
                }

                disappausetime -= Time.deltaTime;

                if (disappausetime < 0)
                {
                    disapx = 1;
                    disapy = 1;
                    transform.position = new Vector2(0, -1.75f);
                    disappausetime = 1;
                    judge = 2;


                }

            }

        }

        if (judge == 2)
        {

            if (transform.localScale.x != 0.8f)
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 1);
                stop = 0;
                speed = defspeed;
            }

            if (hp > 0)
            {
                regenetime -= Time.deltaTime;

                transparencyup.color = new Color(1, 1, 1, Mathf.Pow(Mathf.Cos(Mathf.PI * regenetime), 2));
                transparencydown.color = new Color(1, 1, 1, Mathf.Pow(Mathf.Cos(Mathf.PI * regenetime), 2));

                if (regenetime < 0)
                {
                    transparencyup.color = new Color(1, 1, 1, 1);
                    transparencydown.color = new Color(1, 1, 1, 1);
                    GameController.battle = 1;
                    regenetime = 3;
                    judge = 0;

                }
            }

            if (hp <= 0)
            {
                transparencyup.color = new Color(1, 1, 1, 1);
                transparencydown.color = new Color(1, 1, 1, 1);

            }

        }

        //----------------------------戦闘中
        if (GameController.battle == 1)
        {
            if (stop == 0)
            {
                if (Time.timeScale == 1)
                {
                reflector.Rotate(0, 0, -10);//うきわ回転
                }
                                
                if (reflectorstock < 3 && limitTime <= 0)//うきわ取得までのタイムカウント
                {
                    reflectorcount += Time.deltaTime;
                }

                if (limitTime > 0f)//うきわ使用時の制限時間
                {
                    limitTime -= Time.deltaTime;
                }
                           
                if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z)) && playertimescale == 1)//うきわ使用
                {

                    if (reflectorstock >= 1)
                    {
                        limitTime += 8;
                        reflectorstock -= 1;
                        ReflectorColor.transparency.color = new Color(1, 1, 1, 1);
                        stockuse.PlayOneShot(stockuse.clip);

                    }
                    
                    if (limitTime > 0)
                    {
                        reflectorAct.SetActive(true);
                        
                    }

                }

                if (limitTime <= 0f)
                {

                    reflectorAct.SetActive(false);

                    if (preAct != reflectorAct.activeSelf)//うきわが消えたときのみSE
                    {
                        reflectoroff.PlayOneShot(reflectoroff.clip);
                    }

                }


                if (reflectorcount >= reflectorgettime && reflectorstock < 3)//うきわ加算
                {
                    reflectorstock += 1;
                    stockup.PlayOneShot(stockup.clip);
                    reflectorcount = 0f;
                }
                else if (reflectorcount >= reflectorgettime && reflectorstock == 3)//うきわカウントストップ
                {
                    reflectorcount = reflectorgettime;
                }

                switch (reflectorstock)//うきわ所持数表示
                {
                    case 1:
                        ukiwa1Act.SetActive(true);
                        ukiwa2Act.SetActive(false);
                        ukiwa3Act.SetActive(false);
                        break;

                    case 2:
                        ukiwa1Act.SetActive(true);
                        ukiwa2Act.SetActive(true);
                        ukiwa3Act.SetActive(false);
                        break;

                    case 3:
                        ukiwa1Act.SetActive(true);
                        ukiwa2Act.SetActive(true);
                        ukiwa3Act.SetActive(true);
                        break;

                    default:
                        ukiwa1Act.SetActive(false);
                        ukiwa2Act.SetActive(false);
                        ukiwa3Act.SetActive(false);
                        break;
                }
                

                if (Input.GetKey(KeyCode.LeftShift))//低速移動↓
                {
                    speed = lowspeed;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speed = defspeed;
                }

                            
                transform.position = new Vector2(Mathf.Clamp(transform.position.x + Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, -rangex, rangex),Mathf.Clamp(transform.position.y + Input.GetAxisRaw("Vertical") * speed * Time.deltaTime, -rangey, rangey));
                //自機8方向移動


                switch (hp)//残機表示
                {
                    case 1:
                        life1.SetActive(false);
                        life2.SetActive(false);
                        life3.SetActive(false);
                        break;

                    case 2:
                        life1.SetActive(true);
                        life2.SetActive(false);
                        life3.SetActive(false);
                        break;

                    case 3:
                        life1.SetActive(true);
                        life2.SetActive(true);
                        life3.SetActive(false);
                        break;

                    case 4:
                        life1.SetActive(true);
                        life2.SetActive(true);
                        life3.SetActive(true);
                        break;

                    default:
                        break;
                        
                }

            }

        }

        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && playertimescale == 0)//ポーズからの復帰
        {
            playertimescale = 1;
        }
        
    }

}


