using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 移動速度
    public float speed = 3.0f;
    // true = 右向き false = 左向き
    public bool isToRight = false;
    public float revTime = 0; //反転時間
    public LayerMask groundLayer; //地面レイヤー
    float time = 0;
    






    // Start is called before the first frame update
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);//向きの変更

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (revTime > 0)
        {
            {
                time += Time.deltaTime;
                if (time >= revTime)
                {
                    isToRight = !isToRight; // フラグを反転させる
                    time = 0; // タイマーを初期化
                    if (isToRight)
                    {
                        transform.localScale = new Vector2(-1, 1); // 向きの変更
                    }
                    else
                    {
                        transform.localScale = new Vector2(1, 1); // 向きの変更
                    }

                }
            }
        }
    }

    void FixedUpdate()
    {
        // 地上反転
        bool onGround = Physics2D.CircleCast(transform.position, // 発射位置
                                             0.5f, // 円の半券
                                             Vector2.down, // 発射報告
                                             0.5f, // 発射距離
                                             groundLayer); // 検出するレイヤー
        if (onGround)
        {
            // 速度を更新する
            // Rigidbody2Dを取ってくる
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.velocity = new Vector2(speed, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(-speed, rbody.velocity.y);
            }
        }
    }

    // 接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight; // フラグを判定させる
        time = 0; // タイマーを初期化
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1); // 向きの変更
        }
        else
        {
            transform.localScale = new Vector2(1, 1); // 向きの変更
        }
    }
}
