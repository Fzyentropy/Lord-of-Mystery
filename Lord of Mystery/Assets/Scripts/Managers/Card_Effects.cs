using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card_Effects : MonoBehaviour
{
    
    
    
    
    
    
    
    
    // 记录原方法，根据 Collider 将卡牌边缘 Highlight
    /*public void Highlight_Collider(Color color) 
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 5; // 四个角加上第一个点重复一次以闭合

        // 计算BoxCollider2D的四个角
        Vector3[] corners = new Vector3[5];
        corners[0] = collider.offset + new Vector2(-collider.size.x, -collider.size.y) * 0.5f;
        corners[1] = collider.offset + new Vector2(collider.size.x, -collider.size.y) * 0.5f;
        corners[2] = collider.offset + new Vector2(collider.size.x, collider.size.y) * 0.5f;
        corners[3] = collider.offset + new Vector2(-collider.size.x, collider.size.y) * 0.5f;
        corners[4] = corners[0]; // 闭合线段

        // 转换到世界坐标
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = transform.TransformPoint(corners[i]);
            lineRenderer.SetPosition(i, corners[i]);
        }

        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color; // 高亮颜色
        lineRenderer.endColor = color; // 高亮颜色
    }*/

    // 记录原方法，Clear Highlight 
    /*public void Clear_Highlight_Collider()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startColor = Color.clear; // 高亮颜色
            lineRenderer.endColor = Color.clear; // 高亮颜色
        }
    }*/
    
    // 输入卡牌的 Collider 和 LineRenderer，根据 Collider 将卡牌边缘 Highlight 
    public void Highlight_Collider(BoxCollider2D boxCollider2D, LineRenderer lineRenderer, Color color)        // 利用 collider和 Line Renderer 来高亮 collider 边缘
    {

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 5; // 四个角加上第一个点重复一次以闭合

        // 计算BoxCollider2D的四个角
        Vector3[] corners = new Vector3[5];
        corners[0] = boxCollider2D.offset + new Vector2(-boxCollider2D.size.x, -boxCollider2D.size.y) * 0.5f;
        corners[1] = boxCollider2D.offset + new Vector2(boxCollider2D.size.x, -boxCollider2D.size.y) * 0.5f;
        corners[2] = boxCollider2D.offset + new Vector2(boxCollider2D.size.x, boxCollider2D.size.y) * 0.5f;
        corners[3] = boxCollider2D.offset + new Vector2(-boxCollider2D.size.x, boxCollider2D.size.y) * 0.5f;
        corners[4] = corners[0]; // 闭合线段

        // 转换到世界坐标
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = boxCollider2D.gameObject.transform.TransformPoint(corners[i]);
            lineRenderer.SetPosition(i, corners[i]);
        }

        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color; // 高亮颜色
        lineRenderer.endColor = color; // 高亮颜色
    }

    // 输入卡牌的 LineRenderer，清除卡牌的 Highlight
    public void Clear_Highlight_Collider(LineRenderer lineRenderer)      // 将 Line Renderer 的颜色设置为 透明
    {
        if (lineRenderer != null)
        {
            lineRenderer.startColor = Color.clear; // 高亮颜色
            lineRenderer.endColor = Color.clear; // 高亮颜色
        }
    }






    // 改变 输入的 GameObject 的 Layer
    public void Change_GameObject_Layer(GameObject objectToChange, string layerString)
    {
        objectToChange.layer = LayerMask.NameToLayer(layerString);
    }

    // 改变 输入的物件 的 Sorting Layer
    public void Change_Order_In_Layer(string sortingLayerString, params UnityEngine.Object[] objects)
    {

        foreach (UnityEngine.Object obj in objects)
        {
            if (obj is SpriteRenderer)
            {
                SpriteRenderer renderer = obj as SpriteRenderer;
                renderer.sortingLayerName = sortingLayerString;
            }
            else if (obj is TMP_Text)
            {
                TMP_Text text = obj as TMP_Text;
                text.GetComponent<Renderer>().sortingLayerName = sortingLayerString;
            }
        }
        
    }

    // 将 输入的物件 的 位置向左上方位移一点点，以制造 "抬起卡牌" 效果
    public void Apply_Dragging_Pick_Up_Effect(params UnityEngine.Object[] objects)
    {
        float x_movement = -0.1f;
        float y_movement = 0.1f;

        foreach (UnityEngine.Object obj in objects)
        {
            if (obj is SpriteRenderer)
            {
                SpriteRenderer renderer = obj as SpriteRenderer;
                renderer.transform.localPosition = new Vector3(
                    renderer.transform.localPosition.x + x_movement,
                    renderer.transform.localPosition.y + y_movement,
                    renderer.transform.localPosition.z);
            }
            else if (obj is TMP_Text)
            {
                TMP_Text text = obj as TMP_Text;
                text.transform.localPosition = new Vector3(
                    text.transform.localPosition.x + x_movement,
                    text.transform.localPosition.y + y_movement,
                    text.transform.localPosition.z);
            }
        }
        
    }
    
    // 将 输入的物件 的 位置向右下方位移一点点，以制造 "放下卡牌" 效果
    public void Apply_Dragging_Put_Down_Effect(params UnityEngine.Object[] objects)
    {
        float x_movement = 0.1f;
        float y_movement = -0.1f;

        foreach (UnityEngine.Object obj in objects)
        {
            if (obj is SpriteRenderer)
            {
                SpriteRenderer renderer = obj as SpriteRenderer;
                renderer.transform.localPosition = new Vector3(
                    renderer.transform.localPosition.x + x_movement,
                    renderer.transform.localPosition.y + y_movement,
                    renderer.transform.localPosition.z);
            }
            else if (obj is TMP_Text)
            {
                TMP_Text text = obj as TMP_Text;
                text.transform.localPosition = new Vector3(
                    text.transform.localPosition.x + x_movement,
                    text.transform.localPosition.y + y_movement,
                    text.transform.localPosition.z);
            }
        }
    }
    
    
    
    
    public void Let_Card_Location_Fade_In(GameObject card_location, float delay, float duration)
    {
        StartCoroutine(Card_Location_Fade_In(card_location, delay, duration));
    }
    
    public void Let_Card_Location_Fade_Out(GameObject card_location, float delay)
    {
        StartCoroutine(Card_Location_Fade_Out(card_location, delay));
    }
    
    
    private IEnumerator Card_Location_Fade_In(GameObject card_location, float delay, float duration)      // 生成 Sequence 卡 的时候，Fade In
    {
        
        
        // 设置为不 available
        card_location.GetComponent<Card_Location_Feature>().card_location_availability = false;

        
        ///// 设置各参数和颜色
        
        SpriteRenderer frame = card_location.GetComponent<Card_Location_Feature>().card_frame;           // 卡牌的 frame，边框
        SpriteRenderer name_tag = card_location.GetComponent<Card_Location_Feature>().card_name_tag;        // 卡牌的 name tag，名字栏
        SpriteRenderer image = card_location.GetComponent<Card_Location_Feature>().card_image;           // 卡牌的 image，图片
        TMP_Text label = card_location.GetComponent<Card_Location_Feature>().card_label;                 // 卡牌的 label，名称
        SpriteRenderer shadow = card_location.GetComponent<Card_Location_Feature>().card_shadow;          // 卡牌的 shadow 阴影
        SpriteRenderer line = card_location.GetComponent<Card_Location_Feature>().line_to_next.GetComponent<SpriteRenderer>();     // line
        
        Color frame_color = frame.color;           // 卡牌的 frame，边框
        Color name_tag_color = name_tag.color;        // 卡牌的 name tag，名字栏
        Color image_color = image.color;           // 卡牌的 image，图片
        Color label_color = label.color;                 // 卡牌的 label，名称
        Color shadow_color = shadow.color;          // 卡牌的 shadow 阴影
        Color line_color = line.color;
        
        frame.color = new Color(frame.color.r,frame.color.g,frame.color.b,0);
        name_tag.color = new Color(name_tag.color.r,name_tag.color.g,name_tag.color.b,0);
        image.color = new Color(image.color.r,image.color.g,image.color.b,0);
        label.color = new Color(label.color.r,label.color.g,label.color.b,0);
        shadow.color = new Color(shadow.color.r,shadow.color.g,shadow.color.b,0);
        line.color = new Color(line.color.r,line.color.g,line.color.b,0);
        
        
        // 如果有 delay，则等待 delay 时间
        yield return new WaitForSeconds(delay);
        
        
        ///// Fade in
        
        float timeInterval = 0.05f;  // 设置每步渐变的时间间隔

        float remainingTime = duration;

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);

            // 使用Color.Lerp进行颜色插值
            frame.color = new Color(frame.color.r,frame.color.g,frame.color.b,frame.color.a + timeInterval/duration);
            name_tag.color = new Color(name_tag.color.r,name_tag.color.g,name_tag.color.b, name_tag.color.a + timeInterval/duration);
            image.color = new Color(image.color.r,image.color.g,image.color.b, image.color.a + timeInterval/duration);
            label.color = new Color(label.color.r,label.color.g,label.color.b, label.color.a + timeInterval/duration);
            shadow.color = new Color(shadow.color.r,shadow.color.g,shadow.color.b, shadow.color.a + timeInterval/duration);
            line.color = new Color(line.color.r,line.color.g,line.color.b, line.color.a + timeInterval/duration);

            remainingTime -= timeInterval;
        }
        
        frame.color = frame_color;
        name_tag.color = name_tag_color;
        image.color = image_color;
        label.color = label_color;
        shadow.color = shadow_color;
        line.color = line_color;
        

        // 设置为 available
        card_location.GetComponent<Card_Location_Feature>().card_location_availability = true;

    }

    private IEnumerator Card_Location_Fade_Out(GameObject card_location, float delay)
    {
        // 如果有 delay，则等待 delay 时间
        yield return new WaitForSeconds(delay);
        
        // 设置为不 available
        card_location.GetComponent<Card_Location_Feature>().card_location_availability = false;
        
        ///// 设置各参数和颜色
        
        SpriteRenderer frame = card_location.GetComponent<Card_Location_Feature>().card_frame;           // 卡牌的 frame，边框
        SpriteRenderer name_tag = card_location.GetComponent<Card_Location_Feature>().card_name_tag;        // 卡牌的 name tag，名字栏
        SpriteRenderer image = card_location.GetComponent<Card_Location_Feature>().card_image;           // 卡牌的 image，图片
        TMP_Text label = card_location.GetComponent<Card_Location_Feature>().card_label;                 // 卡牌的 label，名称
        SpriteRenderer shadow = card_location.GetComponent<Card_Location_Feature>().card_shadow;          // 卡牌的 shadow 阴影
        SpriteRenderer line = card_location.GetComponent<Card_Location_Feature>().line_to_next.GetComponent<SpriteRenderer>();      // Line
        

        ///// Fade Out

        float duration = .5f;     // 设置 总时长 
        float timeInterval = 0.05f;  // 设置每步渐变的时间间隔

        float remainingTime = duration;

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);

            // 使用Color.Lerp进行颜色插值
            frame.color = new Color(frame.color.r,frame.color.g,frame.color.b,frame.color.a - timeInterval/duration);
            name_tag.color = new Color(name_tag.color.r,name_tag.color.g,name_tag.color.b, name_tag.color.a - timeInterval/duration);
            image.color = new Color(image.color.r,image.color.g,image.color.b, image.color.a - timeInterval/duration);
            label.color = new Color(label.color.r,label.color.g,label.color.b, label.color.a - timeInterval/duration);
            shadow.color = new Color(shadow.color.r,shadow.color.g,shadow.color.b, shadow.color.a - timeInterval/duration);
            line.color = new Color(line.color.r,line.color.g,line.color.b, line.color.a - timeInterval/duration);

            remainingTime -= timeInterval;
        }
        
        frame.color = Color.clear;
        name_tag.color = Color.clear;
        image.color = Color.clear;
        label.color = Color.clear;
        shadow.color = Color.clear;
        line.color = Color.clear;

        yield return new WaitForSeconds(1f);
        
        Destroy(card_location);

    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
