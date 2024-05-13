using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;


public class Input_Manager : MonoBehaviour
{
    // 相机
    public Camera mainCamera;          // 用于 get 场景中的 camera
    public bool able_to_move_camera = true;        // 用于 限制移动 camera
    public float CameraScrollOffset = 3f;
    public float CameraMoveOffset = 0.05f;
    
    public GameObject infoPanelPrefab; // Assign a prefab of the information panel in the inspector
    private GameObject selectedObject;
    
    private float mouseDownTime;

    // 鼠标交互
    public bool isClickOnObjects = true;      // 开关，鼠标点击到了带 collider 的物体，或 idle 时：true，点击到空地方：false
    private Vector3 lastMousePosition;      // 用于拖拽时临时存储鼠标位置的参数
    private Vector3 click_mouse_position;     // 用于记录点击的时候鼠标的位置，来判断是点击还是拖拽

    
    public GameObject Dragging_Object;      // 当前拖拽的 卡牌 / body part 等物体的 GameObject
    
    
    public float Time_X_Speed = 1f;
    
    // public GameObject raycast_top_object;       // 射线检测最顶层的 GameObject
    
    // public static bool isInfoPanelOut = false;       // 是否有 Panel 已经打开
    // public static GameObject panelReference;        // 打开的 Panel 的引用
    
    


    private void Start()
    {
        FindMainCamera();       // 找到场景中的 main camera

    }
    
    void Update()
    {
        MouseScroll();
        MouseLogic();
        // CheckRayCast();
        
        Hold_F_To_Speed_Up();
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////               FUNCTIONS
    /////////////////////////////////////////////////////////////////////////

    
    
    public void Start_Dragging(GameObject card)
    {

        Dragging_Object = card;
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastMousePosition.z = card.transform.position.z;  // Ensure the z position is zero
        
    }

    public void On_Dragging()
    {
        if (Dragging_Object != null)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = Dragging_Object.transform.position.z;
            
            Vector3 delta = currentMousePosition - lastMousePosition;
            Dragging_Object.transform.position += delta;
            lastMousePosition = currentMousePosition;
        }
    }

    public void End_Dragging()
    {
        if (Dragging_Object != null 
            && (Input.mousePosition - click_mouse_position).magnitude < 0.8f)
        {

            if (Dragging_Object.GetComponent<Card_Location_Feature>() != null)
            {
                Dragging_Object.GetComponent<Card_Location_Feature>().Card_Location_Click_Function();
            }
            
            if (Dragging_Object.GetComponent<Card_Body_Part_Feature>() != null)
            {
                Dragging_Object.GetComponent<Card_Body_Part_Feature>().Card_Body_Part_Mouse_Click_Function();
            }
            
            if (Dragging_Object.GetComponent<Knowledge_Feature>() != null)
            {
                Dragging_Object.GetComponent<Knowledge_Feature>().Knowledge_Click_Function();
            }
            
            /*if (Dragging_Object.GetComponent<>() != null)
            {
                
            }*/
            
        }
        
        Dragging_Object = null;
    }
    
    
    
    
    
    
    
    
    
    
    
    

    void FindMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    
    // 用于检测是不是“第一个”被raycast到的方法，过时了，直接改 z轴
    /*void CheckRayCast()           
    {
        Vector3 offsetVector = new Vector3(0, 0, 20);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red); // debug
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        GameObject topGameObject = null;
        int topLayer = 0;
        string col = "";
        
        if (hits.Length > 0)
        {
            // 遍历所有碰撞的对象
            foreach (var hit in hits)
            {
                col += "  " + hit.collider.gameObject.layer; // debug
                
                if (hit.collider.gameObject.layer > topLayer)
                {
                    topLayer = hit.collider.gameObject.layer;
                    topGameObject = hit.collider.gameObject;
                }
            }

            raycast_top_object = topGameObject;
            
            Debug.Log("射线检测层级： " + col); // debug
        }
    }*/


    void Hold_F_To_Speed_Up()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Time_X_Speed = 5f;
        }
        else
        {
            Time_X_Speed = 1f;
        }
    }



    void MouseLogic()
    {

        if (Input.GetMouseButtonDown(0))    // 按下鼠标左键时
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);     // 发出射线检测，检测是否碰到了碰撞体
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null)       // 如果没有检测到碰撞体，则开关为否
            {
                isClickOnObjects = false;
            }
            else
            {
                isClickOnObjects = true;    // 如果检测到了碰撞体，开关为是
            }

            
            if (!isClickOnObjects)
            {
                click_mouse_position = Input.mousePosition;
                lastMousePosition = Input.mousePosition;
            }
            
        }

        if (Input.GetMouseButton(0))        // 按住鼠标左键时
        {
            
            // 如果没有点到任何带 collider 的物体，则拖拽 board
            if (!isClickOnObjects 
                && able_to_move_camera)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;        // 拖拽 board 逻辑
                Vector3 worldDelta = mainCamera.ScreenToWorldPoint(
                    new Vector3(delta.x, delta.y, mainCamera.nearClipPlane)) - 
                                mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                mainCamera.transform.position -= worldDelta;
            }
            
            lastMousePosition = Input.mousePosition;        // 更新当前鼠标位置
        }

        if (Input.GetMouseButtonUp(0))      // 松开鼠标左键时
        {
            // 如果鼠标指针位置几乎没变，则可以视为点击，触发点击功能
            
            // 如果已经有 panel 打开，则关闭 panel
            if (!isClickOnObjects
                && (Input.mousePosition - click_mouse_position).magnitude < 0.5)
            {
                // 点击
                if (GameManager.GM.PanelManager.current_panel != null)    // 如果已经有 panel 打开，则关闭 panel
                {
                    GameManager.GM.PanelManager.Close_Current_Panel();      // 调用 panel manager 中的销毁 panel 方法
 
                }
            }

            isClickOnObjects = true;

        }




    }


    /// //////////////////      Mouse Logic 2023-11-12

    
    void MouseLogic_2023_11_12()           // 旧的 鼠标逻辑，射线检测，所有 collider 都能拖拽，
    {
        
        /////////////////////////////////     鼠标点击的时候，判断是否点击到了卡牌，以及判断是点击还是拖拽
        
        if (Input.GetMouseButtonDown(0))      
        {
            mouseDownTime = Time.time;   // record the time when hold down mouse left button
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
                // ShowInfoPanel(hit.point);
            }
            else
            {
                selectedObject = null;
            }

            lastMousePosition = Input.mousePosition;
        }
        
        ///////////////////////////////////     若是拖拽，点击到了卡牌 就拖动卡牌，没点击卡牌（即点击 board）则拖动 board
        
        else if (Input.GetMouseButton(0))   
        {
            if (selectedObject != null)      // 若点击到了卡牌（或任何可拖拽，带collider的物体），就拖拽卡牌
            {
                 // 将来要加的 if (selectedObject.CompareTag("") 是 Card 或者 Function，则拖动 - TODO add tag compare
                Vector3 delta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.ScreenToWorldPoint(lastMousePosition);
                delta.z = 0;
                selectedObject.transform.position += delta;
            }
            else                            // 若没有点击到
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, mainCamera.nearClipPlane)) - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                mainCamera.transform.position -= worldDelta;
            }

            lastMousePosition = Input.mousePosition;
        }
        
        
        else if (Input.GetMouseButtonUp(0))     //////////   若只是点击，且点击到了卡牌，或其他带 collider 物体，则 show Panel 
        {
            float duration = Time.time - mouseDownTime;  // Calculate the duration of the mouse button being held down

            if (duration < 0.15f) // Adjust the threshold value as needed
            {
                if (GameManager.GM.PanelManager.isPanelOpen)    // 如果已经有 panel 打开，则关闭 panel
                {
                    GameManager.GM.PanelManager.Close_Current_Panel();      // 调用 panel manager 中的销毁 panel 方法
                    GameManager.GM.PanelManager.isPanelOpen = false;        // 重新设置 panel 是否打开 为否
                }
                else
                if (selectedObject != null)
                    {
                        
                        /*Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                        if (hit.collider != null)
                        {
                            ShowInfoPanel(new Vector3(
                                mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
                                mainCamera.ScreenToWorldPoint(Input.mousePosition).y,
                                3f));
                        }*/

                        if (selectedObject.GetComponent<Card_Location_Feature>() != null)   // 判断点击的卡牌是否是 Card Location
                        {
                            selectedObject.GetComponent<Card_Location_Feature>().Open_Panel(); // 是的话打开 panel
                        }
                        
                        
                        
                        
                        // panelReference = return  collider.gameObject.  GetComponent<通用打开panel脚本>. 打开 Panel()
                        
                        // Destroy 还不能就这么 Destroy，可能还得调用 卡牌里的方法，以返还资源
                        
                        
                    }

                selectedObject = null;
                
            }
            
        }

    }


    
    
    /*private void ShowInfoPanel(Vector3 position)           //////////////////   实例化 info Panel
    {
        GameManager.GM.PanelManager.isPanelOpen = true;
        GameManager.GM.PanelManager.current_panel = Instantiate(infoPanelPrefab, position, Quaternion.identity);
        // panel.transform.localScale = Vector3.zero;

        StartCoroutine(ScaleUp(GameManager.GM.PanelManager.current_panel.transform.localScale));
    }*/

    
    private IEnumerator ScaleUp(Vector3 panelScale)        ///////////////////  弹出动画
    {
        float duration = 1.5f; 
        float elapsed = 0f;
        Vector3 scale = panelScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(scale, Vector3.one, t);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
    
    void MouseScroll()
    {
        // if (mainCamera.orthographicSize > 15 && mainCamera.orthographicSize < 30)
        //     mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * CameraScrollOffset;    // 滚轮 改变视角大小
        
        
        // 计算预期变化后的大小
        float delta = Input.GetAxis("Mouse ScrollWheel") * CameraScrollOffset;
        float newSize = mainCamera.orthographicSize - delta;

        // 如果新大小小于最小值15，并且尝试缩小，就不执行缩小
        if (newSize < 15f && delta > 0)
        {
            return;
        }
        // 如果新大小大于最大值30，并且尝试放大，就不执行放大
        if (newSize > 30f && delta < 0)
        {
            return;
        }

        // 应用新的大小
        mainCamera.orthographicSize = newSize;

    }

    public void Move_Camera_To(Vector2 targetPosition, float duration = .5f)
    {
        able_to_move_camera = false;
        
        mainCamera.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z), duration)
        .OnComplete(() =>
        {
            able_to_move_camera = true;
        });
    }
    
    
}

    
    
    
    


