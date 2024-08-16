using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{

    public int maxLives = 3; // 最大ライフ数
    public GameObject heartPrefab; // ハートののプレハブ
    public Transform heartsContainer; // ハートを配置するコンテナ
    private int currentLives;

    private List<Image> hearts = new List<Image>(); // 動的に生成するハート

    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
        InitailzeHearts();
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitailzeHearts()
    {
        // 既存のハート(もしあれば)を削除
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        hearts.Clear();

        // ハートの生成
        for (int i = 0; i < maxLives; i++)
        {
            GameObject heart = Instantiate(heartPrefab,
                                           heartsContainer);
            Image heartImage = heart.GetComponent<Image>();
            if (heartImage != null)
            {
                hearts.Add(heartImage);
            }
            else
            {
                Debug.Log("ハート作成できない");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives < 0)
        {
            currentLives = 0;
        }

        Debug.Log("ライフ：" + currentLives + "ダメージ:" + damage);

        UpdateHearts();

        if (currentLives == 0)
        {
            // ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // プレイヤーコントロールを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();

            // ゲームオーバーにする
            playerCnt.GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        UpdateHearts();
    }

    public void IncreaseMaxLives(int amount)
    {
        maxLives += amount;
        InitailzeHearts();
        UpdateHearts();
    }

    void UpdateHearts() {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentLives)
            {
                Debug.Log("ダメージ：" + hearts[i].enabled);
                hearts[i].enabled = true;
            }
            else
            {
                Debug.Log("ダメージなし：" + hearts[i].enabled);
                hearts[i].enabled = false;
                
            }
        }
    }

}
