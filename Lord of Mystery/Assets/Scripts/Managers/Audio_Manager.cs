using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [Header("SFX")]
    public AudioSource SFX_Panel_Showup;        // 音效： 打开 Panel
    public AudioSource SFX_Panel_Close;         // 音效： 关闭 Panel
    public AudioSource SFX_Card_Click;          // 音效： 点击卡牌
    public AudioSource SFX_Card_Drop;           // 音效： 放下卡牌
    public AudioSource SFX_Panel_Body_Part_Merge_Slot;      // 音效：merge Body Part 到 Slot 上时，音效   
    public AudioSource SFX_Start_Button_Click_MouseDown;    // 音效: Start Button 点击，按下
    public AudioSource SFX_Start_Button_Click_MouseUp;    // 音效: Start Button 点击，抬起
    public AudioSource SFX_Start_Countdown;         // 音效：开始倒计时
    public AudioSource SFX_Trigger_Card_Effect;     // 音效：卡牌产出时
    public AudioSource SFX_Resource_Button;         // 音效：点击 Resource button
    public AudioSource SFX_Message_Showup;          // 音效：Message 出现
    public AudioSource SFX_Message_Fadeout;         // 音效：Message 离开

    [Header("Music")]
    public AudioSource Music_Cthulhu_part1;
    public AudioSource Music_Cthulhu_part2;

    private AudioSource current_music; // 当前播放中的AudioSource

    
    void Start()
    {
        StartCoroutine(Play_Music()); // 启动协程来播放音乐
    }

    IEnumerator Play_Music()
    {
        current_music = Music_Cthulhu_part1; // 默认从第一首歌开始播放
        
        while (true) // 无限循环播放音乐
        {
            
            current_music.Play(); // 开始播放当前的音乐

            // 使用 WaitUntil 等待当前音乐播放完成
            yield return new WaitUntil(() => !current_music.isPlaying);

            // 切换到另一个AudioSource
            current_music = (current_music == Music_Cthulhu_part1) ? Music_Cthulhu_part2 : Music_Cthulhu_part1;
        }
    }


    public void Play_AudioSource(AudioSource audio_to_play)
    {
        audio_to_play.Play();
    }





}
