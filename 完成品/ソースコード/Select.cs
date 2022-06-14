using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{

    public static int state;//難易度　0=easy 1=normal 2=hard
    private int scene;//画面変更 0=タイトル　1=遷移中　2=難易度選択画面


    private int pauseswitch, pauseselect;//ポーズ切り替え、選択
    private float pausetime;//ポーズ内時間
    public GameObject pauseUI;//ポーズ画面↓
    public GameObject resume, end;

    public Animator ukiwaanimator;//うきわ回転
    public Animator flashanimator;//テキスト点滅
    public Animator arrowanimator;//矢印フェード

    public Image fade, title;//暗転、タイトル
    private int fadeswitch;//フェード
    private float timer;//フェード時間
    private float a;//フェード透明度

    public AudioSource piko;//セレクト音


    // Use this for initialization
    void Start()
    {

        pauseUI.SetActive(false);
        pauseswitch = 0;
        pauseselect = 1;
        pausetime = 0;

        scene = 0;
        state = 0;

        timer = 2f;//フェード時間　2秒
        fadeswitch = 0;
        a = 0;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        piko = audioSources[0];

    }



    // Update is called once per frame
    void Update()
    {
      
        //-----------------------------ゲームスタート
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z)) && Time.timeScale == 1 && scene == 0)
        {
            piko.PlayOneShot(piko.clip);
            scene = 1;
            flashanimator.SetBool("push", true);
        }

        if (scene == 1 && timer > 0)
        {
            timer -= Time.deltaTime;
            title.color = new Color(1f, 1f, 1f, timer / 2f);

        }
        else if (scene == 1 && timer < 0)
        {
            ukiwaanimator.SetBool("IsSelect", true);
            arrowanimator.SetBool("IsSelect", true);
            scene = 2;
        }

        //-------------------------------難易度選択（うきわ回転）
        else if (scene == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && Time.timeScale == 1)
            {
                piko.PlayOneShot(piko.clip);
                state++;
                if (state >= 3)
                {
                    state = 0;
                }
                ukiwaanimator.SetInteger("State", state);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Time.timeScale == 1)
            {
                piko.PlayOneShot(piko.clip);
                state--;
                if (state < 0)
                {
                    state = 2;
                }
                ukiwaanimator.SetInteger("State", state);
            }

            //--------------------------------難易度決定＆シーンチェンジ
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z)) && Time.timeScale == 1)
            {
                piko.PlayOneShot(piko.clip);
                fadeswitch = 1;
                
            }
        }

        if (fadeswitch == 1)
        {
           
            a += Time.deltaTime;

            fade.color = new Color(0, 0, 0, a);

            if (a > 1)
            {
                fadeswitch = 0;
                SceneManager.LoadScene("main");
            }
        }

        //--------------------------------ポーズ画面
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            piko.PlayOneShot(piko.clip);

            pauseUI.SetActive(!pauseUI.activeSelf); //　ポーズUIのアクティブ、非アクティブを切り替え


            if (pauseUI.activeSelf)
            {
                Time.timeScale = 0f;//　ポーズUIが表示されてる時は停止
                pauseswitch = 1;
            }
            else
            {
                Time.timeScale = 1f;//　ポーズUIが表示されてなければ通常通り進行
                pauseswitch = 0;
            }
        }

        if (pauseswitch == 1)
        {
            pausetime += 0.017f;//時間停止中のタイムカウント

            //ポーズ内ボタン選択
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                pauseselect += 1;
                piko.PlayOneShot(piko.clip);

                if (pauseselect > 2)
                {
                    pauseselect = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pauseselect -= 1;
                piko.PlayOneShot(piko.clip);

                if (pauseselect < 1)
                {
                    pauseselect = 2;
                }
            }


            if (pauseselect == 1)//再開ボタン
            {
                resume.transform.localScale = new Vector3(1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1);
                end.transform.localScale = new Vector3(1, 1, 1);

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
                {
                    piko.PlayOneShot(piko.clip);
                    pauseUI.SetActive(false);
                    Time.timeScale = 1f;
                    pauseswitch = 0;
                }
            }
            else if (pauseselect == 2)//終了ボタン
            {
                resume.transform.localScale = new Vector3(1, 1, 1);
                end.transform.localScale = new Vector3(1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1);


                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
                {
                    piko.PlayOneShot(piko.clip);
                    Application.Quit();

                }

            }

        }

    }

}
