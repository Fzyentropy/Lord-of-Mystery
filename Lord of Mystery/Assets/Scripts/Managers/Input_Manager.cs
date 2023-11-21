using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Input_Manager : MonoBehaviour
{
    public Camera mainCamera;          // remember to assign a camera here
    public GameObject infoPanelPrefab; // Assign a prefab of the information panel in the inspector
    private GameObject selectedObject;
    private Vector3 lastMousePosition;
    
    public Vector3 mouse_click_position;        // 点击时记录鼠标位置，方便判定点击还是拖拽

    public float CameraScrollOffset = 3f;       // 鼠标滚轮缩放 速率 
    public float CameraMoveOffset = 0.05f;      // 鼠标拖拽移动 速率


    

    private void Start()
    {
        FindMainCamera();       // 找到场景中的 camera
        
        
    }
    
    void Update()
    {
        
        MouseScroll();
        MouseLogic();
        
    }
    
    
    /////////////////////////////////////////////////////////////////////////               FUNCTIONS

    
    void FindMainCamera()       // 找到场景中的 camera
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    
    void MouseScroll()              // 鼠标滚轮可以缩放场景
    {
        
        // 加上判定条件，以限制缩放大小
        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * CameraScrollOffset;    // 滚轮 改变视角大小

    }
    
    void MouseLogic()           ////  Mouse Logic - 鼠标点击的时候，判断是否点击到了卡牌，以及判断是点击还是拖拽
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            mouse_click_position = Input.mousePosition;     // 点击时记录鼠标的位置，以判断点击或拖拽

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);                         // 射线检测
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
        
        else 
        
        if (Input.GetMouseButton(0))   
        {

            if (selectedObject != null)      // 若点击到了卡牌（或任何可拖拽，带collider的物体），就拖拽卡牌
            {
                 // 将来要加的 if (selectedObject.CompareTag("") 是 Card 或者 Function，则拖动 - TODO add tag compare
                Vector3 delta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.ScreenToWorldPoint(lastMousePosition);
                delta.z = 0;
                selectedObject.transform.position += delta;
            }
            else                            // 若没有点击到，则移动 camera（拖动 board）
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, mainCamera.nearClipPlane)) - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                mainCamera.transform.position -= worldDelta;
            }

            lastMousePosition = Input.mousePosition;
        }
        
        
        else if (Input.GetMouseButtonUp(0))     //////////   若只是点击，且点击到了卡牌，或其他带 collider 物体，则 show Panel 
        {

            if (Input.mousePosition == mouse_click_position)    // 若 当前鼠标位置与按下的时候相等，说明是点击，则 ——
            {
                if (selectedObject == null)
                {
                    
                }
                if (GameManager.GM.PanelManager.is_panel_open)      // 假如已经有 panel 打开，则关闭 panel
                {
                    Close_Current_Panel();
                    GameManager.GM.PanelManager.is_panel_open = false;
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

                        if (selectedObject.GetComponent<Card_Location_Feature>() != null)
                        {
                            GameManager.GM.PanelManager.current_panel_reference = selectedObject.GetComponent<Card_Location_Feature>().Open_Panel();
                        }
                        
                        
                        
                        
                        // panelReference = return  collider.gameObject.  GetComponent<通用打开panel脚本>. 打开 Panel()
                        
                        // Destroy 还不能就这么 Destroy，可能还得调用 卡牌里的方法，以返还资源
                        
                        
                    }

                selectedObject = null;
                
            }
            
        }

    }

    public void Close_Current_Panel()
    {
        
        Destroy(GameManager.GM.PanelManager.current_panel_reference);
        
        // 在此处添加 任何处理 有关返还资源的
        
    }
    
    
    private void ShowInfoPanel(Vector3 position)           //////////////////   实例化 info Panel
    {
        GameManager.GM.PanelManager.is_panel_open = true;
        GameManager.GM.PanelManager.current_panel_reference = Instantiate(infoPanelPrefab, position, Quaternion.identity);
        // panel.transform.localScale = Vector3.zero;

        StartCoroutine(ScaleUp(GameManager.GM.PanelManager.current_panel_reference.transform.localScale));
    }

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
    

    
    
}

    
    
    
    


