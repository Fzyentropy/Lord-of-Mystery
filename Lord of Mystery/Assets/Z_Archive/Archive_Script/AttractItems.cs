using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttractItems : MonoBehaviour
{
    public float radius = 5f; // 吸引道具的范围
    public LayerMask itemLayer; // 道具的层
    public float durationModifier = 0.5f; // 道具移动到目标位置的时间

    void Update()
    {
        // 检测范围内的所有道具
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, radius, itemLayer);

        foreach (var item in items)
        {
            if (Vector2.Distance(transform.position, item.transform.position) <= radius)
            {
                float distance = Vector2.Distance(transform.position, item.transform.position);
                // 使用 DOTween 将道具移动到被放置物品的位置
                item.transform.DOMove(transform.position, distance * durationModifier)
                    .OnComplete(()=>
                    {
                        Destroy(item.gameObject);
                        GameManager.Number_SoulPiece++;
                    });
                
            }
        }
    }

    // 在 Scene 视图中绘制吸引范围（仅在编辑模式下可见）
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
