using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraType{
    ObjectFront,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
    ShowCutScene,
    HideCutScene,
    AppearSlideCG,
    DisappearSlideCG,
    ChangeSlideCG,
}

public enum AppearType
{
    None,
    Appear,
    Disappear,
}
[System.Serializable]

public class Dialogue 
{
    [Header("카메라가 타겟팅할 대상")]
    public Transform tf_target;
    public CameraType cameraType;
    
    [HideInInspector]
    public string name;
    [HideInInspector]
    public string[] contexts;
    [HideInInspector]
    public string[] spriteName;
    [HideInInspector]
    public string[] VoiceName;
}
[System.Serializable]
public class DialogueEvent
{
    public string name;
    public EventTiming eventTiming;
    [Space]
    [Space]
    [Space]

    public Vector2 line;
    public Dialogue[] dialogues;
    [Space]
    public Vector2 lineB;
    public Dialogue[] dialoguesB;


    [Space]
    [Space]
    [Space]
    public AppearType appearType;
    public GameObject[] go_Targets;

    [Space]
    public GameObject go_NextEvent;

    public bool isSame;
    public bool isFreeTime;
}


[System.Serializable]
public class EventTiming
{
    public int eventNum;
    public int[] eventConditions;
    public bool conditionFlag;
    public int eventEndNum;
}



