using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSouls_Manager : MonoBehaviour
{

    public static CardSouls_Manager CardSoulsManager;
    
    // Player
    public GameObject player;
    public int playerHP = 100;
    public int currentPlayerHP;

    
    // Gundyr
    public GameObject gundyr;
    public int gundyrHP = 1000;
    public int currentGundyrHP;


    // Slot
    public static int playerX;
    public static int playerY;
    public GameObject startSlot;
    
    // 进度条
    public GameObject progress_bar_player_prefab;        // 进度条 prefab
    public GameObject progress_bar_gundyr_prefab;        // 进度条 prefab
    public GameObject progress_bar;               // 进度条 指代
    public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_player_position;      // 玩家进度条位置 空物体
    public GameObject progress_bar_gundyr_position;      // 古达进度条位置 空物体
    public TMP_Text countdown_text;               // 显示秒数文本

    public bool isPlayerDoingAction = false;
    

    private void Awake()
    {
        CardSoulsManager = this;
    }

    private void Start()
    {
        PlacePlayer();
        SetHP();
    }



    void PlacePlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Card_Soul_Player");
        }
        if (startSlot == null)
        {
            startSlot = GameObject.Find("Card_Slot_5");
        }

        player.transform.position = startSlot.transform.position;
        playerX = startSlot.GetComponent<Slot>().x;
        playerY = startSlot.GetComponent<Slot>().y;
    }

    void SetHP()
    {
        currentPlayerHP = playerHP;
        currentGundyrHP = gundyrHP;
    }

    public IEnumerator MoveToSlot(Slot slot)
    {
        isPlayerDoingAction = true;
        
        float totalTime = 3f;   // 设置 倒计时时长
        float remainingTime = totalTime;        // 设置倒计时参数
        float timeInterval = 0.05f;              // 设置进度条更新的时间间隔

        // 实例化 进度条 prefab
        progress_bar = Instantiate(progress_bar_player_prefab, transform.position, Quaternion.identity);
        progress_bar.transform.localPosition = progress_bar_player_position.transform.position; //  记得更改谁的position
        progress_bar.transform.parent = transform;
        progress_bar_root = progress_bar.transform.Find("Bar_Root").gameObject;
        countdown_text = progress_bar.GetComponentInChildren<TMP_Text>();
        
        // 初始化进度条和时间显示
        Vector3 originalScale = progress_bar_root.transform.localScale;      // 记录 original scale 设置为 1
        progress_bar_root.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);  // 进度条 scale 设置为 0
        countdown_text.text = $"{remainingTime:0.0} S";                   // 将 剩余时间 用 小数点后 1位 格式来显示

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);
            remainingTime -= timeInterval;

            // 更新进度条和时间显示
            float progress = (totalTime - remainingTime) / totalTime;
            progress_bar_root.transform.localScale = new Vector3(progress, originalScale.y, originalScale.z);
            countdown_text.text = $"{remainingTime:0.0} S";
        }
        
        // 销毁 进度条 prefab
        Destroy(progress_bar);

        // 触移动
        player.transform.position = slot.gameObject.transform.position;
        playerX = slot.x;
        playerY = slot.y;

        isPlayerDoingAction = false;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
}
