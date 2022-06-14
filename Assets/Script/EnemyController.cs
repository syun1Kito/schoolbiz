using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
  
    public Transform E_startpoint;//敵開始座標
    public Transform target;//自機当たり判定部分座標
   
    public GameObject bullet;//反射可能弾
    public GameObject bulletX;//反射不可能弾

    public AudioSource bulletse,damage;//弾発射音、被弾音

    public static float xE, yE;//敵現在座標
    private float stoptime;//敵停止時間    
    private float dx, dy;//自機当たり判定座標と弾発射座標の差
    private float x0, y0;//敵移動時始点座標

    private int count;//弾発射インターバル用カウント
　　private int Shootcount;//弾発射角度用カウント
    private float qua;//弾角度    
    private float radius;//円攻撃の半径
　　public static int fastdestroy;//弾消滅時間の種類

    private int moveswitch;//敵移動中ステージ
    private float dis0, dis;//敵移動距離
    private float v;//敵移動速度
    private float degE;//敵移動角度

　　private int AIstage, AIpattern,AIstagecheck;//敵の行動と弾幕パターン遷移,１フレーム前のパターン
    private int move,move2;//細分化した敵の行動と弾幕パターン遷移 

    public static int hp;//敵HP↓
    public static int maxhp;
    

    void Start()
    {
    
        hp = 100;
        AIstage = 0;
        AIpattern = 10;      
        xE = E_startpoint.position.x;
        yE = E_startpoint.position.y;
        stoptime = 0;
        moveswitch = 0;
        maxhp = hp;
        radius = 0;
        move = 0;
        move2 = 0;
        fastdestroy = 0;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        bulletse = audioSources[0];
        damage = audioSources[1];
    }

    void Shoot(Transform pos, float spe, float ang, int way, int inter, int homing, int timing)
    //敵座標からの弾幕　(発射位置,スピード,角度,方向数,インターバル,ホーミング,タイミング)
    {

        if (count % inter == timing)//弾幕のずれを生成
        {
            Shootcount += 1;
            bulletse.PlayOneShot(bulletse.clip);

            for (int i = 0; i < way; i++)
            {
                qua = -90f + Mathf.Atan2(Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way))) * Mathf.Rad2Deg;

                GameObject newbullet0 =
                Instantiate(bullet, pos.position, Quaternion.Euler(0f, 0f, qua)) as GameObject;
                newbullet0.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way))).normalized * spe);

                if (homing == 1)
                {
                    newbullet0.gameObject.tag = "P_homing";
                }

            }

        }

    }
    
    void Shoot2(float x, float y, float spe, float ang, int way, int inter,int homing,int timing)
    //敵座標以外からの弾幕　(発射位置x,発射位置y,スピード,角度,方向数,インターバル、ホーミング、タイミング)
    {
        
        if (count % inter == timing)
        {
            Shootcount += 1;
            bulletse.PlayOneShot(bulletse.clip);

            for (int i = 0; i < way; i++)
            {

                qua = -90f + Mathf.Atan2(Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way)) , Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way))) * Mathf.Rad2Deg;
                            
                GameObject newbullet0 =
              Instantiate(bullet, new Vector2(x,y), Quaternion.Euler(0f, 0f, qua)) as GameObject;
                newbullet0.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way))).normalized * spe);

                if (homing == 1)
                {
                    newbullet0.gameObject.tag = "P_homing";
                }

            }

        }

    }


    void ShootX(Transform pos, float spe, float ang, int way, int inter, int homing,int timing)
    //敵座標からの反射できない弾幕　(発射位置,スピード,角度,方向数,インターバル,ホーミング,タイミング)
    {
        
        if (count % inter == timing)
        {
            Shootcount += 1;
            bulletse.PlayOneShot(bulletse.clip);

            for (int i = 0; i < way; i++)
            {

                //qua = Mathf.Atan(Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way)) / Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way))) * Mathf.Rad2Deg;
                qua = -90f + Mathf.Atan2(Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way))) * Mathf.Rad2Deg;

                GameObject newbullet0 =
              Instantiate(bulletX, pos.position, Quaternion.Euler(0f, 0f, qua)) as GameObject;
                newbullet0.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way))).normalized * spe);

                if (homing == 1)
                {
                    newbullet0.gameObject.tag = "P_homing";
                }

            }

        }

    }

    void ShootX2(float x, float y, float spe, float ang, int way, int inter, int homing, int timing)
    //敵座標以外からの反射できない弾幕　(発射位置x,発射位置y,スピード,角度,方向数,インターバル,ホーミング,タイミング)
    {


        if (count % inter == timing)
        {
            Shootcount += 1;
            bulletse.PlayOneShot(bulletse.clip);

            for (int i = 0; i < way; i++)
            {

                qua = -90f + Mathf.Atan2(Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way))) * Mathf.Rad2Deg;
                GameObject newbullet0 = Instantiate(bulletX, new Vector2(x, y), Quaternion.Euler(0f, 0f, qua)) as GameObject;
                newbullet0.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * (ang + i * 360f / way)), Mathf.Sin(Mathf.Deg2Rad * (ang + i * 360f / way))).normalized * spe);

                if (homing == 1)
                {
                    newbullet0.gameObject.tag = "P_homing";
                }

            }
        
        }

    }
      
    void MoveStr0(float x1, float y1, float time)//直線移動　(終点x,終点y,所要時間)
    {
    
        if (moveswitch == 0)
        {
            x0 = transform.position.x;
            y0 = transform.position.y;
            dis0 = Mathf.Pow((Mathf.Pow((x1 - x0), 2) + Mathf.Pow((y1 - y0), 2)), 0.5f);
            v = dis0 / time;
            degE = Mathf.Atan2((y1 - y0), (x1 - x0));

            moveswitch = 1;
            
        }

        if (moveswitch == 1)
        {

            xE += v * Mathf.Cos(degE) * Time.deltaTime;
            yE += v * Mathf.Sin(degE) * Time.deltaTime;
            dis = Mathf.Pow((Mathf.Pow((x1 - xE), 2) + Mathf.Pow((y1 - yE), 2)), 0.5f);

            transform.position = new Vector2(xE, yE);

        }

        if (dis <= 0.15f * dis0)
        {
            move += 1;
            moveswitch = 0;
           
        }

    }

    void Stop(float time)//停止
    {
        stoptime += Time.deltaTime;

        if (stoptime > time)
        {
            move += 1;
            stoptime = 0;
        }
    }

    void Update()
    {

        if (Time.timeScale == 0)//ポーズ中行動停止
        {
            return;
        }
        
        if (GameController.battle == 1)
        {
            count += 1;
            if (target != null)
            {
                dx = target.position.x - transform.position.x;
                dy = target.position.y - transform.position.y;
                                               
                switch (Select.state)//難易度
                {

                    case 0:

                        if (hp > maxhp / AIpattern * 8)//下方向3way
                        {

                            if (AIstage != 1)
                            {
                                AIstage = 1;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }
                                                        
                            Shoot(transform, 40, -45, 1, 20, 0, 0);
                            Shoot(transform, 40, -90, 1, 20, 0, 10);
                            Shoot(transform, 40, -135, 1, 20, 0, 0);


                            if (move == 0)
                            {
                                MoveStr0(0, 2, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(-1, 1, 4);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(1, 1, 4);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(0, 2, 4);
                            }
                            else if (move == 4)
                            {
                                move = 1;

                            }

                                                   

                        }

                        if (hp > maxhp / AIpattern * 6 && hp <= maxhp / AIpattern * 8)//3way回転
                        {

                            if (AIstage != 2)
                            {
                                AIstage = 2;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 20, Shootcount * 17, 3, 20, 0, 0);
                           


                            if (move == 0)
                            {
                                MoveStr0(0, 0, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 0, 4);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(0, 1, 4);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(-1, 0, 4);
                            }
                            else if (move == 4)
                            {
                              move = 1;

                            }



                        }

                        if (hp > maxhp / AIpattern * 4 && hp <= maxhp / AIpattern * 6)//4way揺れ
                        {

                            if (AIstage != 3)
                            {
                                AIstage = 3;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }
                                                       
                            Shoot(transform, 50, -90 + (30 * Mathf.Sin(Mathf.Deg2Rad * 60 * Shootcount)), 4, 20, 0, 0);



                            if (move == 0)
                            {
                                MoveStr0(0, 0, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(-1, 0, 2);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(0, -1, 2);
                            }
                            else if (move == 4)
                            {
                                MoveStr0(1, 0, 2);
                            }
                            else if (move == 5)
                            {
                                move = 1;

                            }



                        }

                        if (hp > maxhp / AIpattern * 2 && hp <= maxhp / AIpattern * 4)//6way回転
                        {

                            if (AIstage != 4)
                            {
                                AIstage = 4;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 40, Shootcount * 8, 6, 10, 0, 0);



                            if (move == 0)
                            {
                                MoveStr0(0, 0, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 0.5f, 4);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(1, -0.5f, 2);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(-1, 0.5f, 4);
                            }
                            else if (move == 4)
                            {
                                MoveStr0(-1, -0.5f, 2);
                            }
                            else if (move == 5)

                            {
                                move = 1;

                            }



                        }

                        if (hp <= maxhp / AIpattern * 2)//下方向4連弾＋反射不可交じり
                        {

                            if (AIstage != 5)
                            {
                                AIstage = 5;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 40, -100, 1, 80, 0, 0);
                            Shoot(transform, 40, -105, 1, 80, 0, 0);
                            Shoot(transform, 40, -110, 1, 80, 0, 0);
                            Shoot(transform, 40, -115, 1, 80, 0, 0);

                            Shoot(transform, 40, -95, 1, 80, 0, 20);
                            ShootX(transform, 40, -90, 1, 80, 0, 20);
                            Shoot(transform, 40, -85, 1, 80, 0, 20);
                            ShootX(transform, 40, -80, 1, 80, 0, 20);

                            Shoot(transform, 40, -85, 1, 80, 0, 40);
                            ShootX(transform, 40, -90, 1, 80, 0, 40);
                            Shoot(transform, 40, -95, 1, 80, 0, 40);
                            ShootX(transform, 40, -100, 1, 80, 0, 40);

                            Shoot(transform, 40, -80, 1, 80, 0, 60);
                            Shoot(transform, 40, -75, 1, 80, 0, 60);
                            Shoot(transform, 40, -70, 1, 80, 0, 60);
                            Shoot(transform, 40, -65, 1, 80, 0, 60);


                            if (move == 0)
                            {
                                MoveStr0(0, 1.8f, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 1.8f, 2);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(-1, 1.8f, 2);
                            }
                            else if (move == 3)
                            { 
                                move = 1;

                            }



                        }


                        break;

                    case 1:


                        if (hp > maxhp / AIpattern * 9)//高速6way →　4way回転一部（左右反転）
                        {

                            if (AIstage != 1)
                            {
                                AIstage = 1;
                            }
                                                
                          
                            if (move2 == 0)
                            {

                               

                                if (move == 0)
                                {
                                    MoveStr0(0, 1, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }

                            }

                            if (move2 == 1)
                            {
                                Shoot(transform, 50, -90, 6, 5, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(0, 1, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 2;
                                }
                            }

                            if (move2 == 2)
                            {
                                
                                if (move == 0)
                                {
                                    MoveStr0(-1, 1, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 3;
                                }

                            }

                            if (move2 == 3)
                            {
                                Shoot(transform, 60f, Shootcount * -10, 4, 3, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(-1, 1, 0.5f);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 4;
                                }
                            }

                            if (move2 == 4)
                            {

                                if (move == 0)
                                {
                                    MoveStr0(1, 1, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 5;
                                }

                            }

                            if (move2 == 5)
                            {
                                Shoot(transform, 60f, Shootcount * 10, 4, 3, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(1, 1, 0.5f);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 2;
                                }
                            }
                        }

                        if (hp > maxhp / AIpattern * 8 && hp <= maxhp / AIpattern * 9)//4way螺旋(固定)
                        {

                            if (AIstage != 2)
                            {
                                AIstage = 2;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }
                       
                            if (move2 == 0)
                            {
                                
                                if (move == 0)
                                {
                                    MoveStr0(0,0,1);
                                    
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                                
                            }

                            if (move2 == 1)
                            {
                                if (fastdestroy != 1 )
                                {
                                    fastdestroy = 1;
                                }

                                radius += 0.04f;
                                Shoot2(radius * Mathf.Cos(Shootcount * 2 * Mathf.Deg2Rad), radius * Mathf.Sin(Shootcount * 2 * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * 2 + 90)* Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * 2 + 90) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * 2 + 180) * Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * 2 + 180) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * 2 + 270) * Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * 2 + 270) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                
                                if (move == 0)
                                {
                                    Stop(2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 2;
                                    radius = 0;
                                    fastdestroy = 0;
                                }
                            }

                            if (move2 == 2)
                            {

                                if (move == 0)
                                {
                                    Stop(2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 3;
                                }
                                
                            }

                            if (move2 == 3)
                            {
                                if (fastdestroy != 1)
                                {
                                    fastdestroy = 1;
                                }

                                radius += 0.04f;
                                Shoot2(radius * Mathf.Cos(Shootcount * -2 * Mathf.Deg2Rad), radius * Mathf.Sin(Shootcount * -2 * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * -2 + 90) * Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * -2 + 90) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * -2 + 180) * Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * -2 + 180) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                                Shoot2(radius * Mathf.Cos((Shootcount * -2 + 270) * Mathf.Deg2Rad), radius * Mathf.Sin((Shootcount * -2 + 270) * Mathf.Deg2Rad), 0, -90, 1, 8, 0, 0);
                           

                                if (move == 0)
                                {
                                    
                                    Stop(2);
                                }
                                else if (move == 1)
                                {
                               
                                    move = 0;
                                    move2 = 4;
                                    radius = 0;
                                    fastdestroy = 0;
                                }
                               
                            }

                            if (move2 == 4)
                            {
                                if (move == 0)
                                {
                                    Stop(2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                            }

                 
                        }

                        if (hp > maxhp / AIpattern * 7 && hp <= maxhp / AIpattern * 8)//下高速移動 ＋ 左右2way + 斜め下
                        {

                            if (AIstage != 3)
                            {
                                AIstage = 3;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;
                                fastdestroy = 0;

                            }
                                                                             
                         
                            if (move2 == 0)
                            {
                                                               
                                if (move == 0)
                                {
                                    MoveStr0(0, 2.3f, 2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }

                            }

                            if (move2 == 1)
                            {

                                Shoot(transform, 40, 0, 2, 5, 0, 0);
                                Shoot(transform, 40, -60, 1, 15, 0, 0);
                                Shoot(transform, 40, -120, 1, 15, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(0,-2.3f,1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 2;
                                }

                            }

                            if (move2 == 2)
                            {

                                if (move == 0)
                                {
                                    MoveStr0(-1, 2.3f, 2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 3;
                                }

                            }

                            if (move2 == 3)
                            {

                                Shoot(transform, 40, 0, 2, 5, 0, 0);
                                Shoot(transform, 40, -60, 1, 15, 0, 0);
                                Shoot(transform, 40, -120, 1, 15, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(-1,-2.3f,1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 4;
                                }

                            }

                            if (move2 == 4)
                            {

                                if (move == 0)
                                {
                                    MoveStr0(1, 2.3f, 2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 5;
                                }

                            }

                            if (move2 == 5)
                            {

                                Shoot(transform, 40, 0, 2, 5, 0, 0);
                                Shoot(transform, 40, -60, 1, 15, 0, 0);
                                Shoot(transform, 40, -120, 1, 15, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(1,-2.3f,1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 0;
                                }

                            }
                        }

                        if (hp > maxhp / AIpattern * 6 && hp <= maxhp / AIpattern * 7)//下から上クロス移動
                        {

                            if (AIstage != 4)
                            {
                                AIstage = 4;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot2(Mathf.PingPong(Time.time,3.8f) - 1.9f, -2.2f, 40, 90, 1, 20, 0, 0);
                            Shoot2(1.9f - Mathf.PingPong(Time.time, 3.8f), -2.2f, 40, 90, 1, 20, 0, 0);
                                                       
                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 0, 2);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(-1, 0, 2);
                            }
                            else if (move == 4)
                            {
                                move = 0;
                            }

                        }

                        if (hp > maxhp / AIpattern * 5 && hp <= maxhp / AIpattern * 6)//速度の違う4つの9way(花火)
                            {

                            if (AIstage != 5)
                            {
                                AIstage = 5;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                           


                            Shoot(transform, 20f, -90, 9, 80, 0, 0);
                            Shoot(transform, 40f, -95, 9, 80, 0, 0);
                            Shoot(transform, 60f, -100, 9, 80, 0, 0);
                            Shoot(transform, 80f, -105, 9, 80, 0, 0);


                            if (move == 0)
                            {

                                MoveStr0(0, 0.5f, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 1, 1);
                             
                            }
                            else if (move == 2)
                            {
                                Stop(2);
                                
                            }
                            else if (move == 3)
                            {
                                MoveStr0(0, 0.5f, 1);
                                
                            }
                            else if (move == 4)
                            {
                                Stop(2);
                                
                            }
                            else if (move == 5)
                            {
                                MoveStr0(-1, 1, 1);

                            }
                            else if (move == 6)
                            {
                                Stop(2);

                            }
                            else if (move == 7)
                            {
                                MoveStr0(0, 0.5f, 1);

                            }
                            else if (move == 8)
                            {
                                Stop(2);

                            }
                            else if (move == 9)
                            {
                                move = 1;
                            }




                        }

                        if (hp > maxhp / AIpattern * 4 && hp <= maxhp / AIpattern * 5)//画面中央左右から揺れ + 下反射不可弾
                        {

                            if (AIstage != 6)
                            {
                                AIstage = 6;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot2(1.9f, 0, 40, 180 + (70 * Mathf.Sin(5 * Shootcount)), 1, 15, 0, 0);//揺れ
                            Shoot2(-1.9f, 0, 40, (70 * Mathf.Sin(5 * Shootcount)), 1, 15, 0, 0);

                            ShootX(transform, 60,-90, 1, 40, 0, 0);//N way
                                                        
                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                Stop(1);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(1, 1.5f, 1);
                            }
                            else if (move == 3)
                            {
                                Stop(1);
                            }
                            else if (move == 4)
                            {
                                MoveStr0(0, 1, 1);
                            }
                            else if (move == 5)
                            {
                                Stop(1);
                            }
                            else if (move == 6)
                            {
                                MoveStr0(-1, 1.5f, 1);
                            }
                            else if (move == 7)
                            {
                                Stop(1);
                            }
                            else if (move == 8)
                            {
                                MoveStr0(0, 1, 1);
                            }
                            else if(move == 9)
                            {
                                move = 1;
                            }
                        }

                        if (hp > maxhp / AIpattern * 3 && hp <= maxhp / AIpattern * 4)//3way回転 + 1way回転反射不可
                        {

                            if (AIstage != 7)
                            {
                                AIstage = 7;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }
                                                        
                            Shoot(transform, 30, Shootcount * 23, 3, 6, 0, 0);

                            ShootX(transform, 30, Shootcount * 43, 1, 6, 0, 0);
                           

                            if (move == 0)
                            {
                                MoveStr0(0f, 1f, 2f);
                            }
                            else if (move == 1)
                            {
                                Stop(10);
                            }
                            else if (move == 2)
                            {
                                move = 1;
                            }

                        }

                        if (hp > maxhp / AIpattern * 2 && hp <= maxhp / AIpattern * 3)//下反射可、不可交互に直線高速打ち → 角度ずらし
                        {

                            if (AIstage != 8)
                            {
                                AIstage = 8;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            if (move2 == 0)
                            {
                                if (move == 0)
                                {
                                    MoveStr0(0, 2, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                            }

                            if (move2 == 1)
                            {

                                for (int i = 0; i < 4; i++)
                                {
                                    Shoot2(-1.75f + i, 2, 90, -90, 1, 10, 0, 0);
                                    ShootX2(-1.25f + i, 2, 90, -90, 1, 10, 0, 0);
                                }

                                if (move == 0)
                                {
                                   Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 2;
                                }
                            }

                            if (move2 == 2)
                            {
                                if (move == 0)
                                {
                                    MoveStr0(1.5f, 2, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 3;
                                }
                            }

                            if (move2 == 3)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Shoot2(-1.75f + i * 1.1f, 2.2f - 0.1f * i, 170 - i * 20, -95, 1, 10, 0, 0);
                                    ShootX2(-1.25f + i, 2.2f - 0.1f * i, 170 - i * 20, -95, 1, 10, 0, 0);
                                }

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 4;
                                }
                            }

                            if (move2 == 4)
                            {
                                if (move == 0)
                                {
                                    MoveStr0(0, 2, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 5;
                                }
                            }

                            if (move2 == 5)
                            {

                                for (int i = 0; i < 4; i++)
                                {
                                    Shoot2(-1.75f + i, 2, 90, -90, 1, 10, 0, 0);
                                    ShootX2(-1.25f + i, 2, 90, -90, 1, 10, 0, 0);
                                }

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 6;
                                }
                            }


                            if (move2 == 6)
                            {
                                if (move == 0)
                                {
                                    MoveStr0(-1.5f, 2, 1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 7;
                                }
                            }


                            if (move2 == 7)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Shoot2(-1.75f + i , 1.8f + 0.1f * i, 90 + i * 20, -85, 1, 10, 0, 0);
                                    ShootX2(-1.25f + i * 0.9f, 1.8f + 0.1f * i, 90 + i * 20, -85, 1, 10, 0, 0);
                                }

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 8;
                                }
                            }


                            if (move2 == 8)
                            {
                                move = 0;
                                move2 = 0;
                            }
                        }

                        if (hp > maxhp / AIpattern * 1 && hp <= maxhp / AIpattern * 2)//敵周囲4か所から位置固定ホーミング + 左右下から反射不可揺れ
                        {

                            if (AIstage != 9)
                            {
                                AIstage = 9;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            if (move2 == 0)
                            {
                                Shoot2(transform.position.x + 0.3f, transform.position.y, 70, Mathf.Atan2(dy,dx - 0.3f) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                Shoot2(transform.position.x, transform.position.y + 0.3f, 70, Mathf.Atan2(dy - 0.3f,dx) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                Shoot2(transform.position.x - 0.3f, transform.position.y, 70, Mathf.Atan2(dy,dx + 0.3f) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                Shoot2(transform.position.x, transform.position.y - 0.3f, 70, Mathf.Atan2(dy + 0.3f,dx) * Mathf.Rad2Deg, 8, 60, 0, 0);

                                Shoot2(-1.9f, -2.2f, 40, 45 + (10 * Mathf.Sin(Mathf.Deg2Rad * 30 * Shootcount)), 1, 40, 0, 0);
                                Shoot2(1.9f, -2.2f, 40, 135 + (10 * Mathf.Sin(Mathf.Deg2Rad * 30 * Shootcount)), 1, 40, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(-1.8f, 1.8f, 1);
                                }
                                else if(move == 1)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 2)
                                {
                                    MoveStr0(-0.6f, 1.8f, 1);
                                }
                                else if (move == 3)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 4)
                                {
                                    MoveStr0(0.6f, 1.8f, 1);
                                }
                                else if (move == 5)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 6)
                                {
                                    MoveStr0(1.8f, 1.8f, 1);
                                }
                                else if (move == 7)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 8)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                                
                            }

                            if (move2 == 1)
                            {
                                ShootX2(transform.position.x + 0.3f, transform.position.y, 70, Mathf.Atan2(dy, dx - 0.3f) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                ShootX2(transform.position.x, transform.position.y + 0.3f, 70, Mathf.Atan2(dy - 0.3f, dx) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                ShootX2(transform.position.x - 0.3f, transform.position.y, 70, Mathf.Atan2(dy, dx + 0.3f) * Mathf.Rad2Deg, 8, 60, 0, 0);
                                ShootX2(transform.position.x, transform.position.y - 0.3f, 70, Mathf.Atan2(dy + 0.3f, dx) * Mathf.Rad2Deg, 8, 60, 0, 0);

                                Shoot2(-1.9f, -2.2f, 40, 45 + (10 * Mathf.Sin(Mathf.Deg2Rad * 30 * Shootcount)), 1, 40, 0, 0);
                                Shoot2(1.9f, -2.2f, 40, 135 + (10 * Mathf.Sin(Mathf.Deg2Rad * 30 * Shootcount)), 1, 40, 0, 0);


                                if (move == 0)
                                {
                                    MoveStr0(-1.8f, 1, 1);
                                }
                                else if (move == 1)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 2)
                                {
                                    MoveStr0(-0.6f, 1, 1);
                                }
                                else if (move == 3)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 4)
                                {
                                    MoveStr0(0.6f, 1, 1);
                                }
                                else if (move == 5)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 6)
                                {
                                    MoveStr0(1.8f, 1, 1);
                                }
                                else if (move == 7)
                                {
                                    Stop(0.5f);
                                }
                                if (move == 8)
                                {
                                    move = 0;
                                    move2 = 0;
                                }

                            }
                        }

                        if (hp <= maxhp / AIpattern * 1)//4方向予告 → 矢印型弾幕
                        {

                            if (AIstage != 10)
                            {
                                AIstage = 10;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            if (move2 == 0)
                            {

                                if (move == 0)
                                {
                                    MoveStr0(0, 1, 2);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                                
                            }

                            if (move2 == 1)
                            {

                               
                                if (move == 0)
                                {
                                     Stop(1);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0, 0.5f, 0.5f);                                   
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(0, 1, 0.5f);
                                }
                                else if (move == 3)
                                {                                
                                    move = 0;
                                    move2 = 2;
                                }

                            }

                            if (move2 == 2)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    Shoot2(0.45f * i, 2.2f, 110 - 10 * i, -90, 1, 20, 0, 0);
                                    Shoot2(-0.45f * i, 2.2f, 110 - 10 * i, -90, 1, 20, 0, 0);
                                }

                                ShootX(transform, 30,Shootcount * 20, 2, 5, 0, 0);

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 3;
                                }
                            }

                            if (move2 == 3)
                            {

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(-0.5f,1, 0.5f);
                                }
                                else if (move == 2)
                                {
                                     MoveStr0(0, 1, 0.5f);
                                }
                                else if (move == 3)
                                {
                                    move = 0;
                                    move2 = 4;
                                }

                            }

                            if (move2 == 4)
                            {

                                for (int i = 0; i < 6; i++)
                                {
                                    Shoot2(1.9f, 0.44f * i, 110 - 10 * i, 180, 1, 20, 0, 0);
                                    Shoot2(1.9f, -0.44f * i, 110 - 10 * i, 180, 1, 20, 0, 0);
                                }
                                ShootX(transform, 30, Shootcount * 20, 2, 5, 0, 0);//回転

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 5;
                                }
                            }

                            if (move2 == 5)
                            {
                                                               
                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0, 1.5f, 0.5f);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(0, 1, 0.5f);
                                }
                                else if (move == 3)
                                {
                                    move = 0;
                                    move2 = 6;
                                }

                            }

                            if (move2 == 6)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    Shoot2(0.45f * i, -2.2f, 110 - 10 * i, 90, 1, 20, 0, 0);
                                    Shoot2(-0.45f * i, -2.2f, 110 - 10 * i, 90, 1, 20, 0, 0);
                                }
                                ShootX(transform, 30, Shootcount * 20, 2, 5, 0, 0);//回転

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 7;
                                }
                            }

                            if (move2 == 7)
                            {

                               
                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0.5f, 1, 0.5f);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(0, 1, 0.5f);
                                }
                                else if (move == 3)
                                {
                                    move = 0;
                                    move2 = 8;
                                }

                            }

                            if (move2 == 8)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    Shoot2(-1.9f, 0.44f * i, 110 - 10 * i, 0, 1, 20, 0, 0);
                                    Shoot2(-1.9f, -0.44f * i, 110 - 10 * i, 0, 1, 20, 0, 0);
                                }
                                ShootX(transform, 30, Shootcount * 20, 2, 5, 0, 0);//回転

                                if (move == 0)
                                {
                                    Stop(1);
                                }
                                else if (move == 1)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                            }
                        }

                        break;

                    case 2:

                        if (hp > maxhp / AIpattern * 9)//6way → 2way回転 + ホーミング
                        {

                            if (AIstage != 1)
                            {
                                AIstage = 1;
                            }

                            if (move2 == 0)
                            {

                                Shoot(transform, 40, -90, 6, 20, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(1, 1, 2);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(-1, 1, 4);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(1, 1, 4);
                                }
                                else if (move == 3)
                                {
                                    move = 1;
                                    move2 = 1;
                                }

                            }

                            if (move2 == 1)
                            {

                                Shoot(transform, 40,Shootcount * 13, 2, 5, 0, 0);
                                Shoot(transform, 20, -90, 1, 15, 1, 0);

                                if (move == 0)
                                {
                                    MoveStr0(1, 1, 2);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(-1, 1, 4);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(1, 1, 4);
                                }
                                else if (move == 3)
                                {
                                    move = 1;
                                    move2 = 0;
                                }

                            }

                        }

                        if (hp > maxhp / AIpattern * 8 && hp <= maxhp / AIpattern * 9)//3way回転 + 6way反射不可
                        {

                            if (AIstage != 2)
                            {
                                AIstage = 2;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                          
                            Shoot(transform, 50, -90 + Shootcount * 25, 3, 10, 0, 0);
                            ShootX(transform, 10, -90, 6, 60, 0, 0);




                            if (move == 0)
                            {
                                MoveStr0(1f, -0.5f, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(1, 1, 2);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(-1, 1, 2);
                            }
                            else if (move == 3)
                            {
                                MoveStr0(-1, -0.5f, 2);
                            }
                            else if (move == 4)
                            {
                                MoveStr0(1, -0.5f, 2);
                            }
                            else if (move == 5)
                            {
                                move = 1;
                            }

                        }
                        
                        if (hp > maxhp / AIpattern * 7 && hp <= maxhp / AIpattern * 8)//下方向揺れ + ×字反射不可
                        {

                            if (AIstage != 3)
                            {
                                AIstage = 3;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 70, -90 + (90 * Mathf.Sin(5 * Shootcount)), 1, 3, 0, 0);
                            ShootX(transform, 60, -45 + Shootcount * 5, 4, 50, 0, 0);

                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0.2f, 1, 2);
                            }
                            else if (move == 2)
                            {
                                MoveStr0(-0.2f,1, 2);
                            }
                            else if (move == 3)
                            {
                                move = 1;
                            }

                        }

                        if (hp > maxhp / AIpattern * 6 && hp <= maxhp / AIpattern * 7)//四隅から揺れ + ×字反射不可
                        {

                            if (AIstage != 4)
                            {
                                AIstage = 4;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                           
                            Shoot2(1.9f, 2.2f, 40, -135 + (45f * Mathf.Sin(5f * Shootcount)), 1, 20, 0, 0);
                            Shoot2(1.9f, -2.2f, 40, 135 + (45f * Mathf.Sin(5f * Shootcount)), 1, 20, 0, 0);
                            Shoot2(-1.9f, 2.2f, 40, -45 + (45f * Mathf.Sin(5f * Shootcount)), 1, 20, 0, 0);
                            Shoot2(-1.9f, -2.2f, 40, 45 + (45f * Mathf.Sin(5f * Shootcount)), 1, 20, 0, 0);
                            ShootX(transform, 60f, -45f, 4, 50, 0, 0);


                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 0, 2);
                            }
                            else if (move == 2)
                            {
                                move = 0;
                            }

                        }

                        if (hp > maxhp / AIpattern * 5 && hp <= maxhp / AIpattern * 6)//ホーミング + 星形弾幕反射可、不可
                        {
                           
                            if (AIstage != 5)
                            {
                                AIstage = 5;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }
                          

                            if (move2 == 0 )
                            {

                                Shoot(transform, 10, -90, 1, 30, 1, 0);

                                if (move == 0)
                                {
                                    MoveStr0(0, 0, 0.5f);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0, 0, 3);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(0, 2, 0.5f);
                                }
                                else if (move == 3)
                                {
                                    move = 0;
                                    move2 = 1;
                                }
                            }

                            if (move2 == 1)
                            {

                                ShootX(transform, 20, -90, 1, 2, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(-0.587f *2, -0.809f * 2 +0.7f, 0.2f);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0.951f * 2, 0.309f * 2+0.4f, 0.3f);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(-0.951f * 2, 0.309f * 2, 0.3f);
                                }
                                else if (move == 3)
                                {
                                    MoveStr0(0.587f * 2, -0.809f * 2+0.1f, 0.3f);
                                }
                                else if (move == 4)
                                {
                                    MoveStr0(0f, 1f * 2-0.4f, 0.2f);
                                }
                                else if (move == 5)
                                {
                                    move = 0;
                                    move2 = 2;
                                }

                            }


                            if (move2 == 2)
                            {
                                if (move == 0)
                                {
                                    MoveStr0(0f, 0f, 0.5f);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0f, 0f, 3f);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(0f, 2f, 0.5f);
                                }
                                else if (move == 3)
                                {
                                    move = 0;
                                    move2 = 3;
                                }
                            }

                            if (move2 ==3)
                            {

                                Shoot(transform, 20, -90, 1, 2, 0, 0);

                                if (move == 0)
                                {
                                    MoveStr0(-0.587f * 2, -0.809f * 2 + 0.7f, 0.2f);
                                }
                                else if (move == 1)
                                {
                                    MoveStr0(0.951f * 2, 0.309f * 2 + 0.4f, 0.3f);
                                }
                                else if (move == 2)
                                {
                                    MoveStr0(-0.951f * 2, 0.309f * 2, 0.3f);
                                }
                                else if (move == 3)
                                {
                                    MoveStr0(0.587f * 2, -0.809f * 2 + 0.1f, 0.3f);
                                }
                                else if (move == 4)
                                {
                                    MoveStr0(0f, 1f * 2 - 0.4f, 0.2f);
                                }
                                else if (move == 5)
                                {
                                    move = 0;
                                    move2 = 0;
                                }

                            }



                        }

                        if (hp > maxhp / AIpattern * 4 && hp <= maxhp / AIpattern * 5)//横一列下方向微揺れ + 8way反射不可
                        {

                            if (AIstage != 6)
                            {
                                AIstage = 6;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                              Shoot2(1.5f, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 20, 0, 0);
                              Shoot2(1, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 30, 0, 0);
                              Shoot2(0.5f, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 20, 0, 0);
                              Shoot2(0, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 30, 0, 0);
                              Shoot2(-0.5f, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 20, 0, 0);
                              Shoot2(-1, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 30, 0, 0);
                              Shoot2(-1.5f, 2.2f, 40, -90 + (3 * Mathf.Sin(1f * Shootcount)), 1, 20, 0, 0);
                          
                            ShootX(transform, 60, -45, 8, 50, 0, 0);

                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 2)
                            {
                                move = 1;
                            }

                        }

                        if (hp > maxhp / AIpattern * 3 && hp <= maxhp / AIpattern * 4)//高速3way回転 + 逆回転反射不可
                        {

                            if (AIstage != 7)
                            {
                                AIstage = 7;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                           
                            Shoot(transform, 70, Shootcount * 10, 3, 3, 0, 0);
                            ShootX(transform, 10, Shootcount * -5, 1, 10, 0, 0);


                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 2)
                            {
                                move = 1;
                            }

                        }

                        if (hp > maxhp / AIpattern * 2 && hp <= maxhp / AIpattern * 3)//左右から交互に横一列打ち + 位置固定ホーミング反射可、不可 
                        {

                            if (AIstage != 8)
                            {
                                AIstage = 8;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 70f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg, 1, 10, 0, 0);
                            ShootX2(1.9f, 2.2f, 50f, Mathf.Atan2(target.position.y - 2.2f, target.position.x - 1.9f) * Mathf.Rad2Deg, 1, 60, 0, 0);
                            ShootX2(-1.9f, 2.2f, 50f, Mathf.Atan2(target.position.y - 2.2f, target.position.x + 1.9f) * Mathf.Rad2Deg, 1, 60, 0, 0);

                            ShootX2(1.9f, 1, 20, 180, 1, 30, 0, 0);
                            ShootX2(-1.9f, 0.5f, 20, 0, 1, 30, 0, 0);
                            ShootX2(1.9f, 0, 20f, 180, 1, 30, 0, 15);
                            ShootX2(-1.9f, -0.5f, 20, 0, 1, 30, 0, 15);
                            ShootX2(1.9f, -1, 20, 180, 1, 30, 0, 0);
                            ShootX2(-1.9f, -1.5f, 20, 0, 1, 30, 0, 0);

                            if (move == 0)
                            {
                                MoveStr0(1, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(-1, 1, 1);
                            }

                            else if (move == 2)
                            {
                                move = 0;
                            }

                        }

                        if (hp > maxhp / AIpattern * 1 && hp <= maxhp / AIpattern * 2)//12way回転 + 4way回転反射不可
                        {

                            if (AIstage != 9)
                            {
                                AIstage = 9;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                            Shoot(transform, 30, Shootcount * 1, 12, 15, 0, 0);
                            ShootX(transform, 30, Shootcount * 5, 4, 10, 0, 5);                          

                            if (move == 0)
                            {
                                MoveStr0(0f, 1f, 2f);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 2)
                            {
                                move = 1;
                            }

                        }

                        if (hp <= maxhp / AIpattern * 1)//低速6way回転 + 低速50way回転反射不可
                        {

                            if (AIstage != 10)
                            {
                                AIstage = 10;
                            }

                            if (AIstagecheck != AIstage)
                            {
                                move = 0;
                                move2 = 0;
                                moveswitch = 0;

                            }

                          
                            Shoot(transform, 10, Shootcount * 17, 6, 10, 0, 0);                                                                              
                            ShootX(transform, 20, -90 + Shootcount *11, 50, 40, 0, 5);

                            if (move == 0)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 1)
                            {
                                MoveStr0(0, 1, 2);
                            }
                            else if (move == 2)
                            {
                                move = 1;
                            }

                        }

                        break;

                  

                }


                AIstagecheck = AIstage;//敵行動パターンの更新

            }

        }    
    }

    //-----------------------------弾への当たり判定
        void OnTriggerEnter2D(Collider2D op)
        {
            if (op.tag == "P_bullet" || op.tag == "E_homing")
            {
                Destroy(op.gameObject);
                damage.PlayOneShot(damage.clip);
                hp -= 1;
            }
        }
    

}
