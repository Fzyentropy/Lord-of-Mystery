using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card_Location
{
    public string Id;
    public string Label;
    public string Image;
    public string Description;
    public string Absorbed_Label;       // 吸收满后替换的标题
    public string Absorbed_Description;     // 吸收满后替换的描述
    public string Card_Type;

    public int Rank;
    public string Occupation;

    public bool Burn;
    public int Use_Time;
    public int Counter;
    public bool Only;
    public int Time;
    
    // 临时，用于 prototype，模拟其他卡功能
    public bool Auto_Start;     // 自动开始倒计时
    public bool Repeatable;     // 是否重复
    public bool Stable;       // 是否可以移动
    public bool Invisible_Description;     // 卡牌的效果是否显示，不显示则为 ? 。 false：正常显示， true：不显示，默认为 false正常显示

    public Body_Part_Data Require_Body_Part;
    public Resource_Data Require_Resource;
    public Resource_Data Produce_Resource;
    public Body_Part_Data Produce_Body_Part;

    public List<string> Start_Effect;       // 刚生成时就发动的效果
    public List<string> Start_Countdown_Effect;  // 刚开始倒计时时，触发的效果
    public List<string> Produce_Message;
    public List<string> Produce_Card_Automatic;
    public List<string> Produce_Card_Action;
    public List<string> Produce_Card_Event;
    public List<string> Produce_Card_Location;
    public List<string> Produce_Special_Effect;

}


// Card_Location 的包装类，便于用 JsonUtility 反序列化
[System.Serializable]
public class Card_Location_Wrapper
{
    public List<Card_Location> List_Card_Location;
}


