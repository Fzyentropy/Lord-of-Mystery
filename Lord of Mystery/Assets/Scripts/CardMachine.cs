using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class CardMachine : MonoBehaviour
{

    public bool isAutomatic = true;
    public bool isCoverByBody = false;


    private void Start()
    {
        InitializeProgressBar();
        AddColliderAndRigidbody();
        
        InitializeFundLocation();
        InitializePhysicalStrengthLocation();
        InitializeSpiritLocation();
        InitializeSoulLocation();
        InitializeSpirituality_Infused_MaterialLocation();
        InitializeKnowledgeLocation();
        InitializeBeliefLocation();
        InitializeMadnessLocation();
        InitializePutrefactionLocation();
        InitializeGodhoodLocation();
    }

    private void Update()
    {

        if (isAutomatic)
        {
            AccumulateProgressBar();
        }
        else
        {
            if (isCoverByBody)
            {
                AccumulateProgressBar();
            }
        }

    }




    
    public virtual IEnumerator EndOfProgress()   ///////////////////////////////   Write the 
    {
        yield return null;

    }
    
    
    
    ///
    /// ////////////////////////////////////////////////////////////////////////////      资源 - 种类，处理，
    /// 资源数量：9
    /// 资源计数、资源是否开始计数，等参数，记录于 Game Manager
    ///  是不是就应该写在 Game Manager ???
    
    
    
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Fund
    /// </summary>

    
    public GameObject Icon_Fund;
    [HideInInspector]
    public GameObject Location_Fund;
    
    
    ///
    /// 代码拖拽 资源面板，Fund 的 location
    /// Start()
    public void InitializeFundLocation()
    {
        if (Location_Fund == null)
        {
            Location_Fund = GameObject.Find("Fund_Icon");
        }
    }

    // Produce Fund
    public void Produce_Fund(int Number)
    {
        GameObject AvatarFund = Instantiate(Icon_Fund,   // 生成 Fund icon，在此卡处
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);
        
        AvatarFund.transform.DOMove(Location_Fund.transform.position, 1f)   // 移动 icon 到资源面板，移动完成后销毁
            .OnComplete(() =>
            {
                Destroy(AvatarFund);
                GameManager.Amount_Fund++;            // 移动完成后，添加 Fund
            });
    }
    
    // Reduce Fund
    public void Reduce_Fund(int Number)      // 移动 Duration : 1s
    {
        GameManager.Amount_Fund -= Number;   // 减去 Fund
        
        GameObject AvatarFund = Instantiate(Icon_Fund,    // 在资源面板位置，生成 Fund icon
            new Vector3(
                Location_Fund.transform.position.x,
                Location_Fund.transform.position.y,
                4), Quaternion.identity);
        
        AvatarFund.transform.DOMove(transform.position, 1f)   // 移动 icon 到，此卡，移动完成后销毁
            .OnComplete(() =>
            {
                Destroy(AvatarFund);
            });
    }
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Fund End
    /// </summary>
    
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Physical Strength
    /// </summary>

    
    public GameObject Icon_PhysicalStrength;
    [HideInInspector]
    public GameObject Location_PhysicalStrength;

    ///
    /// 代码拖拽 资源面板，Physical Strength 的 location
    /// Start()
    public void InitializePhysicalStrengthLocation()
    {
        if (Location_PhysicalStrength == null)
        {
            Location_PhysicalStrength = GameObject.Find("Physical_Strength_Icon");
        }
    }

    // Produce Physical Strength
    public void Produce_PhysicalStrength(int Number)
    {
        GameObject AvatarPhysicalStrength = Instantiate(Icon_PhysicalStrength,   // 生成 PhysicalStrength icon，在此卡处
        new Vector3(
            transform.position.x,
            transform.position.y,
            4), Quaternion.identity);
        
        AvatarPhysicalStrength.transform.DOMove(Location_PhysicalStrength.transform.position, 1f)   // 移动 icon 到资源面板，移动完成后销毁
            .OnComplete(() =>
            {
                Destroy(AvatarPhysicalStrength);
                GameManager.Amount_PhysicalStrength++;            // 移动完成后，添加 PhysicalStrength
            });
    }
    
    // Reduce Physical Strength
    public void Reduce_PhysicalStrength(int Number)      // 移动 Duration : 1s
    {
        GameManager.Amount_PhysicalStrength -= Number;   // 减去 PhysicalStrength
        
        GameObject AvatarPhysicalStrength = Instantiate(Icon_PhysicalStrength,    // 在资源面板位置，生成 PhysicalStrength icon
            new Vector3(
                Location_PhysicalStrength.transform.position.x,
                Location_PhysicalStrength.transform.position.y,
                4), Quaternion.identity);
        
        AvatarPhysicalStrength.transform.DOMove(transform.position, 1f)   // 移动 icon 到，此卡，移动完成后销毁
            .OnComplete(() =>
            {
                Destroy(AvatarPhysicalStrength);
            });
    }


    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Spirit
    /// </summary>
    public GameObject Icon_Spirit;
    [HideInInspector]
    public GameObject Location_Spirit;

    public void InitializeSpiritLocation()
    {
        if (Location_Spirit == null)
        {
            Location_Spirit = GameObject.Find("Spirit_Icon");
        }
    }

    public void Produce_Spirit(int Number)
    {
        GameObject AvatarSpirit = Instantiate(Icon_Spirit,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarSpirit.transform.DOMove(Location_Spirit.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSpirit);
                GameManager.Amount_Spirit++;
            });
    }

    public void Reduce_Spirit(int Number)
    {
        GameManager.Amount_Spirit -= Number;

        GameObject AvatarSpirit = Instantiate(Icon_Spirit,
            new Vector3(
                Location_Spirit.transform.position.x,
                Location_Spirit.transform.position.y,
                4), Quaternion.identity);

        AvatarSpirit.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSpirit);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Spirit  End
    /// </summary>


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Soul
    /// </summary>
    public GameObject Icon_Soul;
    [HideInInspector]
    public GameObject Location_Soul;

    public void InitializeSoulLocation()
    {
        if (Location_Soul == null)
        {
            Location_Soul = GameObject.Find("Soul_Icon");
        }
    }

    public void Produce_Soul(int Number)
    {
        GameObject AvatarSoul = Instantiate(Icon_Soul,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarSoul.transform.DOMove(Location_Soul.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSoul);
                GameManager.Amount_Soul++;
            });
    }

    public void Reduce_Soul(int Number)
    {
        GameManager.Amount_Soul -= Number;

        GameObject AvatarSoul = Instantiate(Icon_Soul,
            new Vector3(
                Location_Soul.transform.position.x,
                Location_Soul.transform.position.y,
                4), Quaternion.identity);

        AvatarSoul.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSoul);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Soul  End
    /// </summary>

    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Spirituality_Infused_Material
    /// </summary>
    public GameObject Icon_Spirituality_Infused_Material;
    [HideInInspector]
    public GameObject Location_Spirituality_Infused_Material;

    public void InitializeSpirituality_Infused_MaterialLocation()
    {
        if (Location_Spirituality_Infused_Material == null)
        {
            Location_Spirituality_Infused_Material = GameObject.Find("Spirituality_Infused_Material_Icon");
        }
    }

    public void Produce_Spirituality_Infused_Material(int Number)
    {
        GameObject AvatarSpirituality_Infused_Material = Instantiate(Icon_Spirituality_Infused_Material,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarSpirituality_Infused_Material.transform.DOMove(Location_Spirituality_Infused_Material.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSpirituality_Infused_Material);
                GameManager.Amount_SpiritualityInfusedMaterial++;
            });
    }

    public void Reduce_Spirituality_Infused_Material(int Number)
    {
        GameManager.Amount_SpiritualityInfusedMaterial -= Number;

        GameObject AvatarSpirituality_Infused_Material = Instantiate(Icon_Spirituality_Infused_Material,
            new Vector3(
                Location_Spirituality_Infused_Material.transform.position.x,
                Location_Spirituality_Infused_Material.transform.position.y,
                4), Quaternion.identity);

        AvatarSpirituality_Infused_Material.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarSpirituality_Infused_Material);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Spirituality_Infused_Material  End
    /// </summary>

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Knowledge
    /// </summary>
    public GameObject Icon_Knowledge;
    [HideInInspector]
    public GameObject Location_Knowledge;

    public void InitializeKnowledgeLocation()
    {
        if (Location_Knowledge == null)
        {
            Location_Knowledge = GameObject.Find("Knowledge_Icon");
        }
    }

    public void Produce_Knowledge(int Number)
    {
        GameObject AvatarKnowledge = Instantiate(Icon_Knowledge,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarKnowledge.transform.DOMove(Location_Knowledge.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarKnowledge);
                GameManager.Amount_Knowledge++;
            });
    }

    public void Reduce_Knowledge(int Number)
    {
        GameManager.Amount_Knowledge -= Number;

        GameObject AvatarKnowledge = Instantiate(Icon_Knowledge,
            new Vector3(
                Location_Knowledge.transform.position.x,
                Location_Knowledge.transform.position.y,
                4), Quaternion.identity);

        AvatarKnowledge.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarKnowledge);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Knowledge  End
    /// </summary>

    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Belief
    /// </summary>
    public GameObject Icon_Belief;
    [HideInInspector]
    public GameObject Location_Belief;

    public void InitializeBeliefLocation()
    {
        if (Location_Belief == null)
        {
            Location_Belief = GameObject.Find("Belief_Icon");
        }
    }

    public void Produce_Belief(int Number)
    {
        GameObject AvatarBelief = Instantiate(Icon_Belief,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarBelief.transform.DOMove(Location_Belief.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarBelief);
                GameManager.Amount_Belief++;
            });
    }

    public void Reduce_Belief(int Number)
    {
        GameManager.Amount_Belief -= Number;

        GameObject AvatarBelief = Instantiate(Icon_Belief,
            new Vector3(
                Location_Belief.transform.position.x,
                Location_Belief.transform.position.y,
                4), Quaternion.identity);

        AvatarBelief.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarBelief);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Belief  End
    /// </summary>

    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Madness
    /// </summary>
    public GameObject Icon_Madness;
    [HideInInspector]
    public GameObject Location_Madness;

    public void InitializeMadnessLocation()
    {
        if (Location_Madness == null)
        {
            Location_Madness = GameObject.Find("Madness_Icon");
        }
    }

    public void Produce_Madness(int Number)
    {
        GameObject AvatarMadness = Instantiate(Icon_Madness,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarMadness.transform.DOMove(Location_Madness.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarMadness);
                GameManager.Amount_Madness++;
            });
    }

    public void Reduce_Madness(int Number)
    {
        GameManager.Amount_Madness -= Number;

        GameObject AvatarMadness = Instantiate(Icon_Madness,
            new Vector3(
                Location_Madness.transform.position.x,
                Location_Madness.transform.position.y,
                4), Quaternion.identity);

        AvatarMadness.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarMadness);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Madness  End
    /// </summary>

    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Putrefaction
    /// </summary>
    public GameObject Icon_Putrefaction;
    [HideInInspector]
    public GameObject Location_Putrefaction;

    public void InitializePutrefactionLocation()
    {
        if (Location_Putrefaction == null)
        {
            Location_Putrefaction = GameObject.Find("Putrefaction_Icon");
        }
    }

    public void Produce_Putrefaction(int Number)
    {
        GameObject AvatarPutrefaction = Instantiate(Icon_Putrefaction,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarPutrefaction.transform.DOMove(Location_Putrefaction.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarPutrefaction);
                GameManager.Amount_Putrefaction++;
            });
    }

    public void Reduce_Putrefaction(int Number)
    {
        GameManager.Amount_Putrefaction -= Number;

        GameObject AvatarPutrefaction = Instantiate(Icon_Putrefaction,
            new Vector3(
                Location_Putrefaction.transform.position.x,
                Location_Putrefaction.transform.position.y,
                4), Quaternion.identity);

        AvatarPutrefaction.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarPutrefaction);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Putrefaction  End
    /// </summary>

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Godhood
    /// </summary>
    public GameObject Icon_Godhood;
    [HideInInspector]
    public GameObject Location_Godhood;

    public void InitializeGodhoodLocation()
    {
        if (Location_Godhood == null)
        {
            Location_Godhood = GameObject.Find("Godhood_Icon");
        }
    }

    public void Produce_Godhood(int Number)
    {
        GameObject AvatarGodhood = Instantiate(Icon_Godhood,
            new Vector3(
                transform.position.x,
                transform.position.y,
                4), Quaternion.identity);

        AvatarGodhood.transform.DOMove(Location_Godhood.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarGodhood);
                GameManager.Amount_Godhood++;
            });
    }

    public void Reduce_Godhood(int Number)
    {
        GameManager.Amount_Godhood -= Number;

        GameObject AvatarGodhood = Instantiate(Icon_Godhood,
            new Vector3(
                Location_Godhood.transform.position.x,
                Location_Godhood.transform.position.y,
                4), Quaternion.identity);

        AvatarGodhood.transform.DOMove(transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(AvatarGodhood);
            });
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////   Godhood  End
    /// </summary>

    
    

    ///
    /// ////////////////////////////////////////////////////////////////////////////      资源  结束
    ///
    
    
    
    
    ///
    /// ////////////////////////////////////////////////////////////////////////////      进度条
    ///
    
    
    public float productionInterval = 10.0f;   // 进度条，进度分母，可调整时间
    private float elapsedSinceLastProduction = 0.0f;   // 进度条，积累进度的参数
    
    public GameObject progressBarPrefab; // 进度条 Object，square sprite prefab here
    private GameObject progressBar;   // 进度条指代
    private Vector3 initialScale;     // 进度条 scale
    public GameObject position_progressbar;   // 进度条位置
    
    /// 
    ///  进度条初始化
    ///  Start()
    
    void InitializeProgressBar()
    {
        progressBar = Instantiate(progressBarPrefab, transform.position, Quaternion.identity);
        progressBar.transform.localPosition = position_progressbar.transform.position; // Adjust the position as needed
        progressBar.transform.parent = transform; // Make the progress bar a child of the card
        
        initialScale = progressBar.transform.localScale;
        progressBar.transform.localScale = new Vector3(0, initialScale.y, initialScale.z);   // 进度条设置为 0
        
    }

    /// 
    ///  进度条积累逻辑
    ///  Update()

    void AccumulateProgressBar()
    {
        elapsedSinceLastProduction += Time.deltaTime;           // counter progressing
        if (elapsedSinceLastProduction >= productionInterval)   // 如果大于，执行逻辑，重置
        {
            ProgressEndImplementation();                               // 要执行的逻辑
            elapsedSinceLastProduction -= productionInterval;   // 重置
        }
        UpdateProgressBar();                                    // 更新进度条
    }

    /// 
    ///  进度条积累逻辑
    ///  Update()
    void ProgressEndImplementation()                           ///////////////////// 进度积累完成后，执行的逻辑
    {
        StartCoroutine(EndOfProgress());
    }
    
    /// 
    ///  进度条外化显示
    ///  AccumulateProgressBar() - Update()

    private void UpdateProgressBar()
    {
        float progress = elapsedSinceLastProduction / productionInterval;
        progressBar.transform.localScale = new Vector3(progress * initialScale.x, initialScale.y, initialScale.z);
    }

    
    ///
    /// ////////////////////////////////////////////////////////////////////////////      进度条  结束
    ///
    
    
    
    
    private void AddColliderAndRigidbody()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        // 检查是否存在Rigidbody2D组件，如果不存在则添加它并设置为Kinematic
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
            rb2d.isKinematic = true;
        }
    }
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("YOU"))
        {
            Debug.Log("CARD COVERED");
            isCoverByBody = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("YOU"))
        {
            Debug.Log("CARD GONE");
            isCoverByBody = false;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
}

