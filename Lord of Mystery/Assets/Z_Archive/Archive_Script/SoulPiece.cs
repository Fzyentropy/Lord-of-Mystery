using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulPiece : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // 将鼠标的屏幕坐标转换为世界坐标
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 检查是否点击到了物品
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 销毁被点击的物品
                GameManager.Number_SoulPiece++;
                Destroy(gameObject);
                
            }
        }
        
    }



    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3f);
        // Transform originalTransform = gameObject.transform;
        gameObject.transform.DOScale(new Vector3(
            gameObject.transform.localScale.x * 0.3f,
            gameObject.transform.localScale.y * 0.3f,
            gameObject.transform.localScale.z), 0.2f).OnComplete(() => Destroy(gameObject));
    }
    
}
