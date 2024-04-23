using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card_Manager : MonoBehaviour
{




    
    
    

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
