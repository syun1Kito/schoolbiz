using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
       
    private int fadeswitch;//フェード　0=なし 1=フェードイン 2=フェードアウト　
　　private float a;//フェードα値

    private int textswitch;//会話　0=なし 1=遊び方 2=開始時 3=GAMEOVER時 4=GAMECLEAR時
    public Image GO;//GAMEOVER
    public Image GC;//GAMECLEAR
    public Image how1, how2, how3;//遊び方

    private int pauseswitch,pauseselect;//ポーズ切り替え、選択
    private float pausetime;//ポーズ内時間
    public GameObject pauseUI;//ポーズ画面↓
    public GameObject resume, toTitle;
    public Image panel,fade;
    private float fadetime;

    public Text spnametext, spmsgtext;//名前、会話文テキスト
    public GameObject window;//メッセージウィンドウ
    private int msg;//テキスト番号

    public static int battle;//戦闘モード切替　0=非戦闘中 1=戦闘中
    
    public static int BGMselect;//BGM変更
    public AudioSource piko,pausein,pauseout,GCse,GOse;//SE
    
    void Start()
    {
        pauseUI.SetActive(false);
        window.SetActive(false);
        fadeswitch = 1;
        fadetime = 0f;
        a = 1;
        textswitch = 0;
        msg = 0;
        pauseswitch = 0;
        pauseselect = 1;
        battle = 0;            
              
        panel.color = new Color(1, 1, 1, 0);
        GO.color = new Color(1, 1, 1, 0);
        GC.color = new Color(1, 1, 1, 0);
        how1.color = new Color(1, 1, 1, 0);
        how2.color = new Color(1, 1, 1, 0);
        how3.color = new Color(1, 1, 1, 0);      

        AudioSource[] audioSources = GetComponents<AudioSource>();
        piko = audioSources[0];
        pausein = audioSources[1];
        pauseout = audioSources[2];
        GCse = audioSources[3];
        GOse = audioSources[4];
        BGMselect = 0;
    }

    // Update is called once per frame
    void Update () {
          
        //---------------------------------フェードイン、アウト
        if (fadeswitch == 1)
        {
            fade.color = new Color(0, 0, 0, a);
            if (a >= 0)
            {
                a -= 0.5f * Time.deltaTime;
            }
            else if (a < 0)
            {
                fadeswitch = 0;
                textswitch = 1;
                a = 0;
            }
        }
        else if (fadeswitch == 2)
        {
            fade.color = new Color(0, 0, 0, a);
            if (a <= 1)
            {
                a += Time.deltaTime;
            }
            else if (a > 1)
            {
                fadeswitch = 0;
                a = 1;
                SceneManager.LoadScene("start");
            }
        }

        //----------------------------------GAMEOVER,GAMECLEAR判定
        if (PlayerController.hp <= 0)
        {
            if (textswitch != 4)
            {
                textswitch = 3;
                battle = 0;
                PlayerController.judge = 3;
            }

        }

        if (EnemyController.hp <= 0)
        {
            if (textswitch != 3)
            {
                textswitch = 4;
                battle = 0;
                PlayerController.judge = 3;
            }

        }

        //------------------------------------テキスト関連
        if (textswitch == 1)//遊び方
        {

            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
            {
                if (Time.timeScale == 1)
                {

                    msg += 1;
                    piko.PlayOneShot(piko.clip);
                    if (fadetime != 0)
                    {
                        fadetime = 0;
                    }
                }
            }

            if (msg == 0)
            {
                if (how1.color.a != 1)
                {
                    how1.color = new Color(1, 1, 1, 1);
                }
                               
                if (fadetime < 0.5f)
                {
                    fadetime += Time.deltaTime;
                    how1.transform.localScale = new Vector3(fadetime/0.5f * 20, fadetime/0.5f * 20, 1);
                    
                }
                else if(fadetime >= 0.5)
                {
                    how1.transform.localScale = new Vector3(20,20,1);
                }

            }

            if (msg == 1)
            {
                if (fadetime < 1) {
                    fadetime += Time.deltaTime;
                    how1.color = new Color(1, 1, 1, 1 - fadetime);
                    how2.color = new Color(1, 1, 1, fadetime);
                }
            }

            if (msg == 2)
            {
                if (how1.color.a != 0)
                {
                    how1.color = new Color(1, 1, 1, 0);
                }

                if (fadetime < 1)
                {
                    fadetime += Time.deltaTime;
                    how2.color = new Color(1, 1, 1, 1 - fadetime);
                    how3.color = new Color(1, 1, 1, fadetime);
                }
            }

            if (msg == 3)
            {
                if (how2.color.a != 0)
                {
                    how2.color = new Color(1, 1, 1, 0);
                }

                if (fadetime < 0.5f)
                {
                    fadetime += Time.deltaTime;
                    how3.transform.localScale = new Vector3((1 - fadetime / 0.5f) * 20, (1 - fadetime / 0.5f) * 20, 1);

                }
                else if (fadetime >= 0.5f)
                {
                    msg = 4;
                }
            }

            if (msg == 4)
            {

                how3.color = new Color(1, 1, 1, 0);
                how3.transform.localScale = new Vector3(20, 20, 1);
                textswitch = 2;
                msg = 0;
            }
                     
        }

        if (textswitch == 2)//開始時
        {
    
            if (msg == 0)
            {

                window.SetActive(true);
                msg = 1;
            }

            if (msg == 1) { 
            spnametext.text = "生徒会長　:　冬香";
            spmsgtext.text = "ちょっとそこのあんた！　なんで夏服なのよ！\n" + "制服移行期間はまだでしょっ";
            }

            if (msg == 2)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "（えぇ...　そんなのあったっけ...）\n" + "だって暑いんだからしょうがないじゃん";
            }

            if (msg == 3)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "暑くて勉強に集中できないほうがよっぽど問題でしょっ！\n" + "これだから頭のおかたい人たちはまったく...";
            }

            if (msg == 4)
            {
                spnametext.text = "生徒会長　:　冬香";
                spmsgtext.text = "生徒会長に歯向かうとはいい度胸だ\n" + "そっちがその気ならここで決着をつけようじゃないか";
            }

            if (msg == 5)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "（ただでさえ暑いってのに戦うのか...\nめんどくさいなぁ...\n海でのんびりしたいなぁ）";
            }

            if (msg == 6)
            {
                msg = 0;
                battle = 1;
                textswitch = 0;
                window.SetActive(false);
                if (BGMselect != 1)
                {
                    BGMselect = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
            {
                if (Time.timeScale == 1)
                {
                    msg += 1;
                    piko.PlayOneShot(piko.clip);
                }
            }

        }

        if (textswitch == 3)//GAMEOVER時
        {


            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
            {
                if (Time.timeScale == 1)
                {
                    msg += 1;
                    piko.PlayOneShot(piko.clip);
                }
            }

            if (msg == 0)
            {
                window.SetActive(true);
                msg = 1;
            }

            if (msg == 1)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "ぐぬぬぅ...\n" + "やはり生徒会には逆らえないのかぁ...";
            }

            if (msg == 2)
            {
                spnametext.text = "生徒会長　:　冬香";
                spmsgtext.text = "所詮この程度か\n" + "この私に挑んできた度胸だけは認めてやろう";
            }
                       
            if (msg == 3)
            {
                spnametext.text = "生徒会長　:　冬香";
                spmsgtext.text = "しかしあんたにはまだまだ早すぎたようだねぇ\n" + "おとなしく学校の規則に従ってもらうからねっ！";
            }

            if (msg == 4)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "（......　　　　　　）";
            }
            
            if (msg == 5)
            {
                
                window.SetActive(false);

                if (BGMselect != 2)
                {
                    BGMselect = 2;
                    GOse.PlayOneShot(GOse.clip);
                }

                if (fadetime <= 1)
                {
                    fadetime += Time.deltaTime;

                    panel.color = new Color(0, 0, 0, 95f / 255f * fadetime);
                    GO.color = new Color(1, 1, 1, fadetime);
                }
                
            }

            if (msg == 6)
            {
                fadetime = 0;
                textswitch = 0;
                fadeswitch = 2;
            }

        }

        if (textswitch == 4)//GAMECLEAR時
        {


            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
            {
                if (Time.timeScale == 1)
                {
                    msg += 1;
                    piko.PlayOneShot(piko.clip);
                }
            }

            if (msg == 0)
            {
                window.SetActive(true);
                msg = 1;
            }

            if (msg == 1)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "どうだ！　まいったかっ！！\n";
            }

            if (msg == 2)
            {
                spnametext.text = "生徒会長　:　冬香";
                spmsgtext.text = "くっ...　　まさかあんたに負けるとはね...\n" + "";
            }

            if (msg == 3)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "生徒が快適に勉強できる環境を創るのが生徒会の役割でしょ？" + "最近は地球温暖化も進んでどんどん気温が高くなってるわけだし";
            }

            if (msg == 4)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "それで熱中症の人が出たらどう責任取るつもりなのよっ！"+"第一なんでこの学校にはクーラーが１つもないのよ！あんたたちはバカなの？死ぬの？";
            }

            if (msg == 5)
            {
                spnametext.text = "  生徒　　:　小夏";
                spmsgtext.text = "生徒会なら世の中がどうやって暑さ対策をしているかくらい把(ry  　だーかーら" + "　\n”すくーるびず”は大切なのっ！　わかったぁ？";
            }

            if (msg == 6)
            {
                spnametext.text = "生徒会長　:　冬香";
                spmsgtext.text = "すくーるびず...　　いい響きね...\n" + "わかったわ、今から夏服を解禁するわ\n" + "（この子生徒会に向いてるんじゃないかしら...）";
            }

            if (msg == 7)
            {

                window.SetActive(false);

                if (BGMselect != 2)
                {
                    BGMselect = 2;
                    GCse.PlayOneShot(GCse.clip);
                }

                if (fadetime <= 1)
                {
                    fadetime += Time.deltaTime;

                    panel.color = new Color(0, 0, 0, 95f / 255f * fadetime);
                    GC.color = new Color(1,1, 1, fadetime);
                }

            }

            if (msg == 8)
            {
                fadetime = 0;                
                textswitch = 0;
                fadeswitch = 2;
            }

        }
        
        //--------------------------ポーズ画面
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {

            pauseUI.SetActive(!pauseUI.activeSelf); //　ポーズUIのアクティブ、非アクティブを切り替え


            if (pauseUI.activeSelf)
            {
                Time.timeScale = 0f;//　ポーズUIが表示されてる時は停止
                pauseswitch = 1;
                pausein.PlayOneShot(pausein.clip);
            }
            else
            {
                Time.timeScale = 1f;//　ポーズUIが表示されてなければ通常通り進行
                pauseswitch = 0;
                pauseout.PlayOneShot(pauseout.clip);
            }
        }

        if (pauseswitch == 1)
        {

            pausetime += 0.017f;

            if (PlayerController.playertimescale != 0)
            {
                PlayerController.playertimescale = 0;
            }

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


            if (pauseselect == 1)
            {
                resume.transform.localScale = new Vector3(1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1);
                toTitle.transform.localScale = new Vector3(1, 1, 1);

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
                {
                    piko.PlayOneShot(piko.clip);
                    pauseUI.SetActive(false);
                    Time.timeScale = 1f;
                    pauseswitch = 0;
                }
            }
            else if (pauseselect == 2)
            {
                resume.transform.localScale = new Vector3(1, 1, 1);
                toTitle.transform.localScale = new Vector3(1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1 + 0.4f * Mathf.Pow(Mathf.Cos(Mathf.PI * pausetime), 2), 1);


                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Z))
                {
                    piko.PlayOneShot(piko.clip);
                    pauseUI.SetActive(false);
                    Time.timeScale = 1f;
                    pauseswitch = 0;
                    fadeswitch = 2;

                }
            }
        }

        //----------------------------即時リスタート
        if (Input.GetKeyDown(KeyCode.R))
        {
            int restart = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(restart);
            Time.timeScale = 1;
            
        }
    }
}
