using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputManager : MonoBehaviour
{
    public Camera mainCamera;          // remember to assign a camera here
    public GameObject infoPanelPrefab; // Assign a prefab of the information panel in the inspector
    private GameObject selectedObject;
    private Vector3 lastMousePosition;
    private float mouseDownTime;

    public float CameraScrollOffset = 3f;
    public float CameraMoveOffset = 0.05f;

    public bool isInfoPanelOut = false;
    private GameObject panelReference;


    private void Start()
    {
        FindMainCamera();
        
        
    }
    
    void Update()
    {
        
        MouseScroll();
        
        MouseLogic();
        
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////               FUNCTIONS
    /////////////////////////////////////////////////////////////////////////

    

    void FindMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    
    

    /// <summary>
    /// //////////////////      Mouse Logic
    /// </summary>
    
    void MouseLogic()
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
            if (selectedObject != null)
            {
                Vector3 delta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.ScreenToWorldPoint(lastMousePosition);
                delta.z = 0;
                selectedObject.transform.position += delta;
            }
            else
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, mainCamera.nearClipPlane)) - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                mainCamera.transform.position -= worldDelta;
            }

            lastMousePosition = Input.mousePosition;
        }
        
        
        else if (Input.GetMouseButtonUp(0))       //////////////   若只是点击，且点击到了卡牌，则 show Panel 
        {
            float duration = Time.time - mouseDownTime;  // Calculate the duration of the mouse button being held down

            if (duration < 0.15f) // Adjust the threshold value as needed
            {
                if (isInfoPanelOut)
                {
                    Destroy(panelReference);
                    isInfoPanelOut = false;
                }
                else
                if (selectedObject != null)
                    {
                        // Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                        // RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                        // if (hit.collider != null)
                        // {
                            ShowInfoPanel(new Vector3(
                                mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
                                mainCamera.ScreenToWorldPoint(Input.mousePosition).y,
                                3f));
                        // }
                        
                    }
                
                
                selectedObject = null;
                
            }
            
        }
        
        
        
        
        
    }
    
    
    private void ShowInfoPanel(Vector3 position)           //////////////////   实例化 info Panel
    {
        isInfoPanelOut = true;
        panelReference = Instantiate(infoPanelPrefab, position, Quaternion.identity);
        // panel.transform.localScale = Vector3.zero;

        StartCoroutine(ScaleUp(panelReference.transform.localScale));
    }

    private IEnumerator ScaleUp(Vector3 panelScale)        ///////////////////  弹出动画
    {
        float duration = 0.5f; 
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
        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * CameraScrollOffset;    // 滚轮 改变视角大小

    }
    
    
}

    
    
    
    


