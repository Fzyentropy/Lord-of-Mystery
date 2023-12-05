using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEditor.Experimental.GraphView;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardSouls_Manager : MonoBehaviour
{

    public static CardSouls_Manager CardSoulsManager;
    
    // Player
    [Header("Player")]
    public GameObject player;
    public int playerHP = 100;
    public int currentPlayerHP;
    public int estukFlaskNumber = 3;
    public int estukFlaskHeal = 45;
    public float playerMoveTime = 2f;
    public float playerAttackTime = 1.5f;
    public float playerEstukFlaskTime = 2f;
    public bool isPlayerDead = false;

    // Gundyr
    [Header("Gundyr")]
    public GameObject gundyr;
    public int gundyrHP = 1000;
    public int currentGundyrHP;
    public float gundyrAttackTime = 3f;

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
    public GameObject gundyrHealthBarWrapper;
    public GameObject gundyrName;

    public GameObject BlackScreen;
    public TMP_Text dieText;
    public GameObject blackScreenPrefab;

    public TMP_Text estukFlaskNumberText;
    
    // Attack
    [Header("Attack")]
    public List<Vector2Int> attackSlots;
    public int GundyrAttackDamage = 40;
    public int PlayerAttackDamage = 110;
    public GameObject red_prefab;
    public List<GameObject> all_reds;
    public bool isGundyrAttacking;
    
    // Audio
    public AudioSource gundyrMusic;
    public AudioSource swordHit;
    public AudioSource slay;
    public AudioSource youDied;
    public AudioSource EstukFlaskDrink;
    public AudioSource dodge;
    public AudioSource gundyrAttack1;
    public AudioSource gundyrAttack2;
    public AudioSource gundyrAttack3;
    public AudioSource gundyrMoan1;
    public AudioSource HeirOfFireDestroyed;
    private AudioSource gundyrAttackSFX;




    private void Awake()
    {
        CardSoulsManager = this;
        
    }

    private void Start()
    {
        PlacePlayer();
        SetHP();
        
        StartCoroutine(FacingGundyr());
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

        if (estukFlaskNumberText == null)
        {
            estukFlaskNumberText = GameObject.Find("Estuk Flask num").GetComponent<TMP_Text>();
        }

        isPlayerDoingAction = true;
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
        if (gundyrHealthBar.activeSelf)
        {
            gundyrHealthBar.transform.localScale =
                new Vector3(gundyrScale, gundyrHealthBar.transform.localScale.y, gundyrHealthBar.transform.localScale.z);
        }
        
        estukFlaskNumberText.text = estukFlaskNumber.ToString();
    }

    public IEnumerator FacingGundyr()
    {
        isGundyrAttacking = true;
        // 淡入
        BlackScreen.SetActive(true);

        Image black = BlackScreen.GetComponent<Image>();
        
        float elapsedTime = 0.05f;
        float elapsedStep = 0.03f;

        while (black.color.a > 0.05)
        {
            black.color = new Color(0, 0, 0, black.color.a - elapsedStep);
            yield return new WaitForSeconds(elapsedTime);
        }
        black.color = Color.clear;

        isPlayerDoingAction = false;
        
        // yield return new WaitForSeconds(1f);
        gundyrMusic.Play();
        yield return new WaitForSeconds(2f);
        gundyrHealthBar.SetActive(true);
        gundyrHealthBarWrapper.SetActive(true);
        gundyrName.SetActive(true);
        yield return new WaitForSeconds(2f);
        isGundyrAttacking = false;

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

        yield return StartCoroutine(CallPlayerProgressBar(playerMoveTime));


        // 触移动
        dodge.Play();   // 播放 SFX dodge
        player.transform.position = slot.gameObject.transform.position;
        playerX = slot.x;
        playerY = slot.y;

        isPlayerDoingAction = false;
    }



    public IEnumerator Attack()             // 攻击
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(playerAttackTime));

        
        // 攻击逻辑
        
       
        
        if (playerX == 1)
        {
            
            if (currentGundyrHP - PlayerAttackDamage <= 0)
            {
                currentGundyrHP = 0;
                StopAllCoroutines();
                StartCoroutine(BeatGundyr());
            }
            else
            {
                swordHit.Play();        // 播放 打击音效
                currentGundyrHP -= PlayerAttackDamage;
            }
            
        }
        
        isPlayerDoingAction = false;
       
    }


    public IEnumerator Heal()
    {
        isPlayerDoingAction = true;

        yield return StartCoroutine(CallPlayerProgressBar(playerEstukFlaskTime));

        
        // 回血逻辑
        StartCoroutine(HealOverTime(estukFlaskHeal));
        isPlayerDoingAction = false;
    }

    private IEnumerator HealOverTime(int totalHealAmount) 
    {
        int amountHealed = 0; // 已经回复的血量
        float healInterval = 0.025f;
        int healEachTime = 1;

        EstukFlaskDrink.Play();     // 播放音效
        
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
            
            
            yield return StartCoroutine(CallGundyrProgressBar(gundyrAttackTime));

            
            
            // 检查是否对玩家造成伤害
            DamagePlayer();
            
            isGundyrAttacking = false;

    }

    void ChooseAttackSlots()
    {
        // int attackMoveIndicator = 0;
        int[] indicators = new []{0};
        
        // 古达攻击算法

        // □ ■ ■ □
        // □ □ □ □
        if (playerX == 1 && (playerY == 2 || playerY == 3))
        {
            indicators = new[] { 1, 1, 2, 3 };

        }
        // □ □ □ □
        // □ ■ ■ □
        else if (playerX == 2 && (playerY == 2 || playerY == 3))
        {
            indicators = new[] { 1, 5, 7, 8 };
        }
        // ■ □ □ □
        // □ □ □ □
        else if (playerX == 1 && playerY == 1)
        {
            indicators = new[] { 1, 2, 4 };
        }
        // □ □ □ ■
        // □ □ □ □
        else if (playerX == 1 && playerY == 4)
        {
            indicators = new[] { 1, 3, 6 };
        }
        // □ □ □ □
        // ■ □ □ □
        else if (playerX == 2 && playerY == 1)
        {
            indicators = new[] { 2, 7, 4 };
        }
        // □ □ □ □
        // □ □ □ ■
        else if (playerX == 2 && playerY == 4)
        {
            indicators = new[] { 6, 8 };
        }
        
        int indicator = Random.Range(0, indicators.Length);
        int attackMoveIndicator = indicators[indicator];
        
        Debug.Log(attackMoveIndicator + "   " + indicators.Length);
        
        
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

            gundyrAttackSFX = gundyrAttack1;    // 设置音效 1
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
            
            gundyrAttackSFX = gundyrAttack1;    // 设置音效 1
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
            
            gundyrAttackSFX = gundyrAttack1;    // 设置音效 1
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
            
            gundyrAttackSFX = gundyrAttack2;    // 设置音效 2
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
            
            gundyrAttackSFX = gundyrAttack2;    // 设置音效 2
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
            
            gundyrAttackSFX = gundyrAttack2;    // 设置音效 2
        }
        
        // □ ■ □ □
        // □ ■ ■ □
        if (attackMoveIndicator == 7)
        {
            attackSlots = new List<Vector2Int>()
            {
                // new Vector2Int(2, 1), 
                new Vector2Int(2, 2),
                new Vector2Int(2, 3),
                new Vector2Int(1, 2)
            };
            
            gundyrAttackSFX = gundyrAttack1;    // 设置音效 1
        }
        
        // □ □ ■ □
        // □ ■ ■ □
        if (attackMoveIndicator == 8)
        {
            attackSlots = new List<Vector2Int>()
            {
                new Vector2Int(1, 3), 
                new Vector2Int(2, 2),
                new Vector2Int(2, 3),
                // new Vector2Int(2, 4)
            };
            
            gundyrAttackSFX = gundyrAttack1;    // 设置音效 1
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
        if (attackSlots.Contains(playerSlot) && !isPlayerDead) 
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
                StartCoroutine(CallPlayerProgressBar(1));
                // 玩家死亡
                currentPlayerHP = 0;
                slay.Play();        // 播放 slay 音效
                isPlayerDead = true;
                // 玩家死亡逻辑
                StartCoroutine(PlayerDead());
            }
            else
            {
                gundyrAttackSFX.Play();     // 播放 古达攻击 音效
                currentPlayerHP -= GundyrAttackDamage;
                StartCoroutine(TakingDamage());
            }

            

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
        isPlayerDoingAction = true;     // disable player action
        yield return new WaitForSeconds(0.5f);
        Instantiate(blackScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        yield return new WaitForSeconds(1f);
        
        youDied.Play();     // 播放音效
        dieText.text = "YOU DIED";
        dieText.fontSize = 175;
        dieText.color = Color.red;
        StartCoroutine(YouDiedScale());
        yield return new WaitForSeconds(5f);
        StartCoroutine(PlayerBackToFirelink());
    }

    IEnumerator YouDiedScale()              // 让 YOU DIED 逐渐放大
    {
        float scaleStep = 0.01f;
        float timeStep = 0.05f;
        float currentScaleX = dieText.gameObject.transform.localScale.x;
        while (dieText.gameObject.transform.localScale.x < 1.3f * currentScaleX)
        {
            dieText.gameObject.transform.localScale = new Vector3(
                dieText.gameObject.transform.localScale.x + dieText.gameObject.transform.localScale.x * scaleStep,
                dieText.gameObject.transform.localScale.y + dieText.gameObject.transform.localScale.y * scaleStep,
                dieText.gameObject.transform.localScale.z);
            yield return new WaitForSeconds(timeStep);
        }

        
    }

    IEnumerator PlayerBackToFirelink()
    {
        float volumeStep = 0.03f;
        float screenStep = 0.07f;
        float timeStep = 0.05f;
        Image black = BlackScreen.GetComponent<Image>();

        while (gundyrMusic.volume > 0.05 || black.color.a < 0.95)
        {
            gundyrMusic.volume -= volumeStep;
            black.color = new Color(0, 0, 0, black.color.a + screenStep);
            if (gundyrMusic.volume <= 0.05)
            {
                gundyrMusic.volume = 0;
            }

            if (black.color.a >= 0.95)
            {
                black.color = Color.black;
            }

            yield return new WaitForSeconds(timeStep);
        }

        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene("Prototype_CardSouls_FirelinkShrine");

    }

    IEnumerator BeatGundyr()
    {
        // 处理善后工作

        // destroy all progress bar
        foreach (GameObject progressbar  in allProgressBar)
        {
            Destroy(progressbar);
        }
        
        // 撤掉红色
        foreach (var reddd in all_reds)
        {
            Destroy(reddd);
        }
        all_reds.Clear();

        isGundyrAttacking = true;      // 设置
        
        
        
        slay.Play();        // 播放 slay 音效

        yield return new WaitForSeconds(4f);
        
        Instantiate(blackScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        HeirOfFireDestroyed.Play();     // 播放音效
        dieText.text = "HEIR OF FIRE DESTROYED";
        dieText.fontSize = 117;

        float colorStep = 0.05f;
        float timeStep = 0.05f;
        while (dieText.color.a < 0.95)
        {
            dieText.color = new Color(1,0.86F,0.39F, dieText.color.a + colorStep);
            if (dieText.color.a >= 0.95)
            {
                dieText.color = new Color(1, 0.86F, 0.39F, 1);
            }

            yield return new WaitForSeconds(timeStep);
        }
        

        yield return new WaitForSeconds(7f);

        StartCoroutine(PlayerBackToFirelink());


    }
    
    






}
