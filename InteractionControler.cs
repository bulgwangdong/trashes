using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionControler : MonoBehaviour
{
    [SerializeField] Camera cam;
  
    [SerializeField] GameObject go_ActiveCrosshair;
    RaycastHit hitInfo;
    bool isContact = false;
    public static bool isInteract = false;
    [SerializeField] ParticleSystem ps_Question;
    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] Text txt_targetName;
    DialogueManager theDM;
    
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Coursor;
    [SerializeField] GameObject go_FieldCoursor;
    [SerializeField] GameObject SpinCoursor;
    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        if (!isInteract && !EHandBookManager.isPaused)
        {
            CheckObject();
            ClickLeftBtn();
        }
    }
    void CheckObject()
    {
        if (CameraController.onlyView)
        {
            Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))
            {
                Contact();
            }


            else
            {
                NotContact();
            }

        }
        else
        {
            if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hitInfo,2))
            {
                Contact();
            }


            else
            {
                NotContact();
            }
        }


    }
    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction") || hitInfo.transform.CompareTag("Character"))
        {

            go_TargetNameBar.SetActive(true);
            txt_targetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            Rotating rotating = SpinCoursor.GetComponent<Rotating>();
            rotating.Rotate(true);
            go_ActiveCrosshair.SetActive(true);



            if (!isContact)
            {
                isContact = true;

            }
            
        }
        else
        {
            NotContact();
        }
    }
    void NotContact()
    {
        if (isContact)
        {
            go_TargetNameBar.gameObject.SetActive(false);
            isContact = false;
            Rotating rotating = SpinCoursor.GetComponent<Rotating>();
            rotating.Rotate(true);


            go_ActiveCrosshair.SetActive(false);
        }
     }
    void ClickLeftBtn()
    {
        if (!isInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }
    }
    void Interact()
    {
        isInteract = true;

        ps_Question.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_Question.GetComponent<QuestionEffect>().SetTarget(t_targetPos);
        ps_Question.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }
    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(() => QuestionEffect.isCollide);
        QuestionEffect.isCollide = false;

        InteractionEvent t_Event = hitInfo.transform.GetComponent<InteractionEvent>();

        if (hitInfo.transform.GetComponent<InteractionType>().isObject)
        {
            DialogueCall(t_Event);
        }
        else
        {
            if (t_Event != null && t_Event.GetDialogue()!=null)
            {
                DialogueCall(t_Event);
            }
            else
                TransferCall();

        }

    }

    void DialogueCall(InteractionEvent p_event)
    {
        if (!DatabaseManager.instance.eventFlags[p_event.GetEventNumber()])
        {
            theDM.SetNextEvent(p_event.GetNextEvent());

            if (p_event.GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(p_event.GetTargets());
            else if (p_event.GetAppearType() == AppearType.Disappear) theDM.SetDisAppearObjects(p_event.GetTargets());
        }
        theDM.ShowDialogue(p_event.GetDialogue());

    }

    void TransferCall()
    {
        string t_SceneName = hitInfo.transform.GetComponent<InteractionDoor>().GetSceneName();
        string t_LocationName = hitInfo.transform.GetComponent<InteractionDoor>().GetLocationName();

        StartCoroutine(FindObjectOfType<TransferManager>().Transfer(t_SceneName, t_LocationName));
    }


    public void SettingUI(bool p_flag)
    {
        go_Crosshair.SetActive(p_flag);

        if (!p_flag)
        {
            go_TargetNameBar.SetActive(false);
            go_Coursor.SetActive(false);
            go_FieldCoursor.SetActive(false);

        }
        else
        {
            if (CameraController.onlyView)
                go_Coursor.SetActive(true);
            else
                go_FieldCoursor.SetActive(true);
            go_Crosshair.SetActive(true);
            go_ActiveCrosshair.SetActive(false);
        }

        isInteract = !p_flag;
    }
}
