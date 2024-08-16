using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// UIを使うのに必要
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // 画像を持つGameObject
    public Sprite gameOverSpr;          // GAME OVER画像
    public Sprite gameClearSpr;         // GAME CLEAR画像
    public GameObject panel;            // パネル
    public GameObject restartButton;    // RESTARTボタン
    public GameObject nextButton;       // ネクストボタン
    Image titleImage;                   // 画像を表示している Image コンポーネント

    // 時間制限追加
    // 時間表示イメージ
    public GameObject timeBar;
    // 時間テキスト
    public GameObject timeText;
    // タイムコントロール
    TimeController timeCnt;

    // スコア追加
    // スコアテキスト
    public GameObject scoreText;
    // 合計スコア
    public static int totalScore;
    // ステージスコア
    public int stageScore = 0;

    // プレイヤー操作
    // 操作UIパネル
    public GameObject inputUI;

    // サウンド再生追加
    public AudioClip meGameOver; // ゲームオーバー
    public AudioClip meGameClear; // ゲームクリア


    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタン（パネル）を非表示にする
        panel.SetActive(false);

        // 時間制限を追加
        // タイムコントロールを取得
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                // 制限時間なしなら隠す
                timeBar.SetActive(false);
            }
        }

        // スコア追加
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            // ゲームクリア
            mainImage.SetActive(true);      // 画像を表示する
            panel.SetActive(true);          // ボタン(パネル)を表示する
            // RESTART ボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            // 時間制限追加
            if (timeCnt != null)
            {
                // 時間カウント停止
                timeCnt.isTimeOver = true;

                // スコア追加
                // 整数に代入することで少数を切り捨てる
                int time = (int)timeCnt.displayTime;
                // 残り時間をスコアに加える
                totalScore += time * 10;
            }

            // スコアを追加
            totalScore += stageScore;
            stageScore = 0;
            // スコア更新
            UpdateScore();

            // プレイヤー操作
            // 操作UI隠す
            inputUI.SetActive(false);

            // サウンド再生追加
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                // BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            // ゲームオーバー
            mainImage.SetActive(true);      // 画像を表示する
            panel.SetActive(true);          // ボタン(パネル)を表示する
            // NEXT ボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            // 時間制限追加
            if (timeCnt != null)
            {
                // 時間カウント停止
                timeCnt.isTimeOver = true;
            }

            // プレイヤー操作
            // 操作UI隠す
            inputUI.SetActive(false);

            // サウンド再生追加
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                // BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            // ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // プレイヤーコントロールを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // 時間制限追加
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    // 整数に代入することで少数を切り捨てる
                    int time = (int)timeCnt.displayTime;
                    // タイム更新
                    timeText.GetComponent<Text>().text = time.ToString();
                    // タイムオーバー
                    if (time == 0)
                    {
                        // ゲームオーバーにする
                        playerCnt.GameOver();
                    } 
                }
            }

            // スコアを追加
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }

        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // スコアを追加
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    // プレイヤー操作
    // ジャンプ
    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerCnt = player.GetComponent<PlayerController>();
        playerCnt.Jump();
    }
}
