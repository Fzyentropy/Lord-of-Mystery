using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using Random = UnityEngine.Random;

public class CardSouls_Manager : MonoBehaviour
{

    public static CardSouls_Manager CardSoulsManager;
    
    // Player
    [Header("Player")]
    public GameObject player;
    public int playerHP = 100;
    public int currentPlayerHP;

    // Gundyr
    [Header("Gundyr")]
    public GameObject gundyr;
    public int gundyrHP = 1000;
    public int currentGundyrHP;

    // Slot
    [Header("Slot")]
    public static int playerX;
    public static int playerY;
    public GameObject startSlot;
    public List<GameObject> slotList;

    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_player_prefab;        // 进度条 prefab
    public GameObject progress_bar_gundyr_prefab;        // 进度条 prefab
    public GameObject progress_bar_player_position;      // 玩家进度条位置 空物体
    public GameObject progress_bar_gundyr_position;      // 古达进度条位置 空物体

    public List<GameObject> allProgressBar;

    public bool isPlayerDoingAction = false;
    
    // UI
    [Header("UI")]
    public GameObject playerHealthBar;
    public GameObject gundyrHealthBar;

    public GameObject BlackScreen;
    public TMP_Text dieText;
    public GameObject blackScreenPrefab;
    
    // Attack
    [Header("Attack")]
    public List<Vector2Int> attackSlots;
    public int GundyrAttackDamage = 40;
    public int PlayerAttackDamage = 110;
    public GameObject red_prefab;
    public List<GameObject> all_reds;
    public bool isGundyrAttacking;




    private void Awake()
    {
        CardSoulsManager = this;
    }

    private void Start()
    {
        PlacePlayer();
        SetHP();

        StartCoroutine(GundyrAttackRoutine());
    }

    private void Update()
    {
        UpdateUI();
        GundyrAttack();
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

    void UpdateUI()
    {
        float playerScale;
        float gundyrScale;
        
        if (currentPlayerHP <= 0)
        {
            playerScale = 0;
        }
        else
        {
            playerScale = (float)currentPlayerHP / playerHP;
        }

        if (currentGundyrHP <= 0)
        {
            gundyrScale = 0;
        }
        else
        {
            gundyrScale = (float)currentGundyrHP / gundyrHP;
        }

        playerHealthBar.transform.localScale = 
            new Vector3(playerScale, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
        gundyrHealthBar.transform.localScale =
            new Vector3(gundyrScale, gundyrHealthBar.transform.localScale.y, gundyrHealthBar.transform.localScale.z);
        Debug.Log("player scale : "+ playerScale);
        Debug.Log("playerHP = " + playerHP);
    }


    // 调用 玩家进度条
    
    public IEnumerator CallPlayerProgressBar(float progressTime)
    {
        GameObject progress_bar_player;               // 进度条 指代
        GameObject progress_bar_root_player;          // 进度条方块的根 指代
        TMP_Text countdown_text;               // 显示秒数文本
        
        float totalTime = progressTime;   // 设置 倒计时时长
        float remainingTime = totalTime;        // 设置倒计时参数
        float timeInterval = 0.05f;              // 设置进度条更新的时间间隔

        // 实例化 进度条 prefab
        progress_bar_player = Instantiate(progress_bar_player_prefab, transform.position, Quaternion.identity);
        allProgressBar.Add(progress_bar_player);    // 放入 进度条 list
        progress_bar_player.transform.localPosition = progress_bar_player_position.transform.position; //  记得更改谁的position
        progress_bar_player.transform.parent = transform;
        progress_bar_root_player = progress_bar_player.transform.Find("Bar_Root").gameObject;
        countdown_text = progress_bar_player.GetComponentInChildren<TMP_Text>();
        
        // 初始化进度条和时间显示
        Vector3 originalScale = progress_bar_root_player.transform.localScale;      // 记录 original scale 设置为 1
        progress_bar_root_player.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);  // 进度条 scale 设置为 0
        countdown_text.text = $"{remainingTime:0.0} S";                   // 将 剩余时间 用 小数点后 1位 格式来显示

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);
            remainingTime -= timeInterval;

            // 更新进度条和时间显示
            float progress = (totalTime - remainingTime) / totalTime;
            progress_bar_root_player.transform.localScale = new Vector3(progress, originalScale.y, originalScale.z);
            countdown_text.text = $"{remainingTime:0.0} S";
        }
        
        // 销毁 进度条 prefab
        Destroy(progress_bar_player);
    }
    
    // 调用 
    IEnumerator CallGundyrProgressBar(float progressTime)
    { 
        GameObject progress_bar_gundyr;               // 进度条 指代
        GameObject progress_bar_root_gundyr;          // 进度条方块的根 指代
        TMP_Text countdown_text;               // 显示秒数文本
    
        float totalTime = progressTime;   // 设置 倒计时时长
        float remainingTime = totalTime;        // 设置倒计时参数
        float timeInterval = 0.05f;              // 设置进度条更新的时间间隔

        // 实例化 进度条 prefab
        progress_bar_gundyr = Instantiate(progress_bar_gundyr_prefab, transform.position, Quaternion.identity);
        allProgressBar.Add(progress_bar_gundyr);    // 放入 进度条 list
        progress_bar_gundyr.transform.localPosition = progress_bar_gundyr_position.transform.position; //  记得更改谁的position
        progress_bar_gundyr.transform.parent = transform;
        progress_bar_root_gundyr = progress_bar_gundyr.transform.Find("Bar_Root").gameObject;
        countdown_text = progress_bar_gundyr.GetComponentInChildren<TMP_Text>();
        
        // 初始化进度条和时间显示
        Vector3 originalScale = progress_bar_root_gundyr.transform.localScale;      // 记录 original scale 设置为 1
        progress_bar_root_gundyr.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);  // 进度条 scale 设置为 0
        countdown_text.text = $"{remainingTime:0.0} S";                   // 将 剩余时间 用 小数点后 1位 格式来显示

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);
            remainingTime -= timeInterval;

            // 更新进度条和时间显示
            float progress = (totalTime - remainingTime) / totalTime;
            progress_bar_root_gundyr.transform.localScale = new Vector3(progress, originalScale.y, originalScale.z);
            countdown_text.text = $"{remainingTime:0.0} S";
        }
        
        // 销毁 进度条 prefab
        
        Destroy(progress_bar_gundyr);
    }
    

    public IEnumerator MoveToSlot(Slot slot)            // 移动
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(2));


        // 触移动
        player.transform.position = slot.gameObject.transform.position;
        playerX = slot.x;
        playerY = slot.y;

        isPlayerDoingAction = false;
    }



    public IEnumerator Attack()             // 攻击
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(2));

        
        // 攻击逻辑
        
        if (playerX == 1)
        {
            if (currentGundyrHP - PlayerAttackDamage <= 0)
            {
                StartCoroutine(BeatGundyr());
            }
            else
            {
                currentGundyrHP -= PlayerAttackDamage;
            }
            
        }
        
        isPlayerDoingAction = false;
       
    }


    public IEnumerator Heal()
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(3));

        
        // 回血逻辑
        StartCoroutine(HealOverTime(45));
        isPlayerDoingAction = false;
    }

    private IEnumerator HealOverTime(int totalHealAmount) 
    {
        int amountHealed = 0; // 已经回复的血量
        float healInterval = 0.025f;
        int healEachTime = 1;

        while (amountHealed < totalHealAmount) {
            
            currentPlayerHP += healEachTime;
            amountHealed += healEachTime;

            if (currentPlayerHP > playerHP) {
                currentPlayerHP = playerHP;
                break;
            }
            
            Debug.Log("current player HP = "+ currentPlayerHP);
            yield return new WaitForSeconds(healInterval);
        }
        
    }
    
    
    
    //// Gundyr Attack

    void GundyrAttack()
    {
        if (!isGundyrAttacking)
        {
            isGundyrAttacking = true;
            StartCoroutine(GundyrAttackRoutine());
        }
    }
    
    IEnumerator GundyrAttackRoutine()
    {
        isGundyrAttacking = true;
        float attackDelay = Random.Range(1, 4);
            yield return new WaitForSeconds(attackDelay); // 等待攻击倒计时
            
            ChooseAttackSlots(); // 选择攻击的slot

            yield return StartCoroutine(CallGundyrProgressBar(3));
            
            // 检查是否对玩家造成伤害
            DamagePlayer();
            
            isGundyrAttacking = false;

    }

    void ChooseAttackSlots()
    {
        int attackMoveIndicator = Random.Range(1, 6);
        
        // ■ ■ ■ ■
        // □ □ □ □
        if (attackMoveIndicator == 1)                       
        {                                                   
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(1, 1), 
                new Vector2Int(1, 2),
                new Vector2Int(1, 3),
                new Vector2Int(1, 4)
            };
        }
        
        // ■ ■ ■ □
        // □ ■ □ □
        if (attackMoveIndicator == 2)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(1, 1), 
                new Vector2Int(1, 2),
                new Vector2Int(1, 3),
                new Vector2Int(2, 2)
            };
        }
        
        // □ ■ ■ ■
        // □ □ ■ □
        if (attackMoveIndicator == 3)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(2, 3), 
                new Vector2Int(1, 2),
                new Vector2Int(1, 3),
                new Vector2Int(1, 4)
            };
        }
        
        // ■ ■ □ □
        // ■ ■ □ □
        if (attackMoveIndicator == 4)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(1, 1), 
                new Vector2Int(1, 2),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2)
            };
        }
        
        // □ ■ ■ □
        // □ ■ ■ □
        if (attackMoveIndicator == 5)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(2, 2), 
                new Vector2Int(1, 2),
                new Vector2Int(1, 3),
                new Vector2Int(2, 3)
            };
        }
        
        // □ □ ■ ■
        // □ □ ■ ■
        if (attackMoveIndicator == 6)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(2, 3), 
                new Vector2Int(2, 4),
                new Vector2Int(1, 3),
                new Vector2Int(1, 4)
            };
        }
        
        // □ ■ □ □
        // ■ ■ ■ □
        if (attackMoveIndicator == 7)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(2, 1), 
                new Vector2Int(2, 2),
                new Vector2Int(2, 3),
                new Vector2Int(1, 2)
            };
        }
        
        // □ □ ■ □
        // □ ■ ■ ■
        if (attackMoveIndicator == 8)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(1, 3), 
                new Vector2Int(2, 2),
                new Vector2Int(2, 3),
                new Vector2Int(2, 4)
            };
        }

        foreach (var vector2Int in attackSlots)
        {
            foreach (GameObject slot in slotList)
            {
                if (slot.GetComponent<Slot>().x == vector2Int.x && slot.GetComponent<Slot>().y == vector2Int.y)
                {
                    GameObject redReference = Instantiate(red_prefab, slot.transform.position,Quaternion.identity);
                    all_reds.Add(redReference);
                }
            }
        }
        
    }

    void DamagePlayer()
    {
        
        Vector2Int playerSlot = new Vector2Int(playerX, playerY);
        if (attackSlots.Contains(playerSlot)) 
        {
            
            StopAllCoroutines();
            
            // destroy all progress bar
            foreach (GameObject progressbar  in allProgressBar)
            {
                Destroy(progressbar);
            }

            isGundyrAttacking = false;      // 设置
            
            if (currentPlayerHP - GundyrAttackDamage <= 0)
            {
                // 玩家死亡
                currentPlayerHP = 0;
                
                // 玩家死亡逻辑
                StartCoroutine(PlayerDead());
            }
            else
            {
                currentPlayerHP -= GundyrAttackDamage;
            }

            StartCoroutine(TakingDamage());

        }
        
        // 撤掉红色
        foreach (var reddd in all_reds)
        {
            Destroy(reddd);
        }
        all_reds.Clear();
        
        
        
    }

    IEnumerator TakingDamage()
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(1));

        isPlayerDoingAction = false;
    }

    IEnumerator PlayerDead()
    {
        Instantiate(blackScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        dieText.color = Color.red;
        yield return null;
    }

    IEnumerator BeatGundyr()
    {
        yield return null;
    }






}
