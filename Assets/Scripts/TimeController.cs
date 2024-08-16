using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // true = 時間をカウントダウン計測する
    public bool isCountDown = true;
    // ゲームの最大時間
    public float gameTime = 0;
    // true = タイマー停止
    public bool isTimeOver = false;
    // 表示時間
    public float displayTime = 0;

    // 現在時間
    float times = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isCountDown)
        {
            // カウントダウン
            displayTime = gameTime;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimeOver)
        {
            times += Time.deltaTime;
            if (isCountDown)
            {
                // カウントダウン
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
        }
        else
        {
            // カウントアップ
            displayTime = times;
            if (displayTime >= gameTime)
            {
                displayTime = gameTime;
                isTimeOver = true;
            }
        }
    }
}
