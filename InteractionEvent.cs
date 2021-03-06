﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false;

    [SerializeField] DialogueEvent[] dialogueEvent;
    int currentCount;

    private void Start()
    {
        bool t_flag = CheckEvent();

    
            gameObject.SetActive(t_flag);


    }
    Dialogue[] SettingDialogue(Dialogue[] p_dialogue, int p_lineX, int p_lineY)
    {

        Dialogue[] t_dialogues = DatabaseManager.instance.GetDialogue(p_lineX, p_lineY);


        for (int i = 0; i < dialogueEvent[currentCount].dialogues.Length; i++)
        {
            t_dialogues[i].tf_target = p_dialogue[i].tf_target;
            t_dialogues[i].cameraType = p_dialogue[i].cameraType;
        }
        return t_dialogues;
    }


    public bool CheckEvent()
    {
        bool t_flag = true;

        for (int x = 0; x < dialogueEvent.Length; x++)
        {
            t_flag = true;
            // 등장조건과 일치하지 않을경우 등장시키지 않음
            for (int i = 0; i < dialogueEvent[x].eventTiming.eventConditions.Length; i++)
            {
                if (DatabaseManager.instance.eventFlags[dialogueEvent[x].eventTiming.eventConditions[i]] != dialogueEvent[x].eventTiming.conditionFlag)
                {
                    t_flag = false;
                    break;
                }
            }

            // 등장조건과 관계없이 퇴장조건과 일치할경우 무조건 등장시키지 않음
            if (DatabaseManager.instance.eventFlags[dialogueEvent[x].eventTiming.eventEndNum])
            {
                t_flag = false;
            }

            if (t_flag)
            {
                currentCount = x;
                break;
            }

        }



        return t_flag;
    }
    public Dialogue[] GetDialogue()
    {
        if (DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventEndNum])
        {
            return null;
        }
        // 상호작용 전 대화
        if (!DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] || dialogueEvent[currentCount].isSame)
        {
            DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] = true;
            dialogueEvent[currentCount].dialogues = SettingDialogue(dialogueEvent[currentCount].dialogues, (int)dialogueEvent[currentCount].line.x, (int)dialogueEvent[currentCount].line.y);
            return dialogueEvent[currentCount].dialogues;
        }
        else // 상호작용 후 대화
        {
            dialogueEvent[currentCount].dialoguesB = SettingDialogue(dialogueEvent[currentCount].dialoguesB, (int)dialogueEvent[currentCount].lineB.x, (int)dialogueEvent[currentCount].lineB.y);
   
            return dialogueEvent[currentCount].dialoguesB;

        }

    }


    public AppearType GetAppearType()
    {
        return dialogueEvent[currentCount].appearType;
    }
    public GameObject[] GetTargets()
    {
        return dialogueEvent[currentCount].go_Targets;
    }

    public GameObject GetNextEvent() 
    {
        return dialogueEvent[currentCount].go_NextEvent;
    }

    public int GetEventNumber()
    {
        CheckEvent();
        return dialogueEvent[currentCount].eventTiming.eventNum;
    }

    private void Update()
    {

        if (isAutoEvent && DatabaseManager.isFinish && TransferManager.isFinished)
        {
            DialogueManager theDM = FindObjectOfType<DialogueManager>();
            DialogueManager.isWaiting = true;

            if (GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(GetTargets());
            else if (GetAppearType() == AppearType.Disappear) theDM.SetDisAppearObjects(GetTargets());

            theDM.ShowDialogue(GetDialogue());
            theDM.SetNextEvent(GetNextEvent());

            gameObject.SetActive(false);
        }
    }
}
