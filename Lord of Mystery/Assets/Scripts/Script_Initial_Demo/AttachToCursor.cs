using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttachToCursor : MonoBehaviour, IPointerClickHandler
{
    public GameObject indicatorPrefab; // 指示器的预制体
    public GameObject itemPrefab; // 真正的道具的预制体
    private GameObject indicatorInstance; // 指示器的实例
    private bool isAttachedToCursor = false; // 是否附加到光标
    
    public int soulPrice = 15;
    
    
    // 当点击 UI 图标时调用
    public void OnPointerClick(PointerEventData eventData)
    {
        if (indicatorInstance == null && GameManager.Number_SoulPiece >= soulPrice)
        {
            // 实例化指示器并附加到光标
            indicatorInstance = Instantiate(indicatorPrefab);
            isAttachedToCursor = true;
        }
    }
    

    void Update()
    {
        if (isAttachedToCursor)
        {
            // 让指示器跟随光标
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            indicatorInstance.transform.position = cursorPosition;

            // 如果在场景中点击鼠标左键，则销毁指示器并实例化真正的道具
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Destroy(indicatorInstance);
                Instantiate(itemPrefab, cursorPosition, Quaternion.identity);
                isAttachedToCursor = false;
                GameManager.Number_SoulPiece -= soulPrice;
            }
            if (Input.GetMouseButtonDown(1))
            {
                isAttachedToCursor = false;
                Destroy(indicatorInstance);
            }
        }
    }
}
