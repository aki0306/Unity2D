using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitualPad : MonoBehaviour
{
    // タブが動く距離
    public float maxLength = 70;
    // 上下左右に動くフラグ
    public bool is4DPad = false;
    // 操作するプレイヤーのGameObject
    GameObject player;
    // タブの初期座標
    Vector2 defPos;
    // タッチ位置
    Vector2 downPos;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーキャラクターを取得
        player = GameObject.FindGameObjectWithTag("Player");
        // タブの初期座標
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ダウンイベント
    public void PadDown()
    {
        // マウスポイントのスクリーン座標
        downPos = Input.mousePosition;
    }

    // ドラッグイベント
    public void PadDrag()
    {
        // マウスポイントのスクリーン座標
        Vector2 mousePosition = Input.mousePosition;
        // 新しいタブの位置を求める
        // マウスダウン位置からの移動差分
        Vector2 newTabPos = mousePosition - downPos;
        if (!is4DPad)
        {
            // 横スクロールの場合は、Y軸を0にする
            newTabPos.y = 0;
        }

        // 移動ベクトルを計算する
        // ベクトルを正規化する
        Vector2 axis = newTabPos.normalized;
        // 2点の距離を求める
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > maxLength)
        {
            // 限界距離を超えたので限界座標を設定する
            newTabPos.x = axis.x * maxLength;
            newTabPos.y = axis.y * maxLength;
        }

        // タブを移動させる
        GetComponent<RectTransform>().localPosition = newTabPos;
        // プレイヤーキャラクターを移動させる
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
    }

    // アップイベント
    public void PadUp()
    {
        // タブの位置の初期化
        GetComponent<RectTransform>().localPosition = defPos;
        // プレイヤーキャラクターを停止させる
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0, 0);
    }
}
