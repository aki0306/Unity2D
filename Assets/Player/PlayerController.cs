using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody2D型の変数
    Rigidbody2D rbody;
    // 入力
    float axisH = 0.0f;
    // 移動速度
    public float speed = 3.0f;
    // ジャンプ力
    public float jump = 9.0f;
    // 着地できるレイヤー
    public LayerMask groundLayer;
    // ジャンプ開始フラグ
    bool goJump = false;

    // アニメーション対応
    // アニメーター
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    // ゲームの状態
    public static string gameState = "playing";

    // スコア
    public int score = 0;

    // タッチスクリーン対応追加
    bool isMoving = false;

    // LifeManagerを参照
    public LifeManager lifeManager;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dから取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
        // Animatorを取ってくる
        animator = this.GetComponent<Animator>();
        // 停止から開始する
        this.nowAnime = stopAnime;
        // 停止から開始する
        this.oldAnime = stopAnime;
        // ゲーム中にする
        gameState = "playing";

    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        // 移動
        if (isMoving == false)
        {
            // 水平方向の入力をチェック
            axisH = Input.GetAxisRaw("Horizontal");
        } 

        // 向きの調整
        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        } else if (axisH < 0.0f)
        {
            // 左に移動
            transform.localScale = new Vector2(-1,1);
        }

        // キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    { 
        // 地上判定
        bool onGround = Physics2D.CircleCast(
            // 発射位置
            transform.position,
            // 円の半径
            0.2f,
            // 発射方向
            Vector2.down,
            // 発射距離
            0.0f,
            // 検出するレイヤー
            groundLayer);
        if (onGround || axisH != 0)
        {
            // 地上の上 or 速度が0ではない
            // 速度を更新する
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            // 地上の上でジャンプキーが押された
            // ジャンプさせる

            // ジャンプさせるベクトルを作る
            Vector2 jumpPw = new Vector2(0, jump);
            // 瞬間的な力を加える
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }

        // アニメーション更新
        if (onGround)
        {
            // 地上の上
            if (axisH == 0)
            {
                // 停止中
                nowAnime = stopAnime;
            }
            else
            {
                // 移動
                nowAnime = moveAnime;
            }
        }
        else
        {
            // 空中
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }

    }

    // ジャンプ
    public void Jump()
    {
        // ジャンプフラグを立てる
        goJump = true;
    }

    // 接触開始
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            // ゴール！！
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            // ゲームオーバー
            GameOver();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            // ItemDataを設定
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // スコアを設定
            score = item.value;

            // アイテム削除する
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            // ダメージを受ける
            lifeManager.TakeDamage(1);
        }
    }

    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        // ゲーム停止
        GameStop();
    }

    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();
        // =====================
        // ゲームオーバー演出
        // =====================
        // プレイヤー当たりを消す
        GetComponent<CapsuleCollider2D>().enabled = false;
        // プレイヤーを上に跳ね上げる演出
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    // ゲーム停止
    void GameStop()
    {
        // Rigidbody2Dを取ってくる
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // 速度を0にして強制停止
        rbody.velocity = new Vector2(0, 0);
    }

    // タッチスクリーン対応追加
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
}