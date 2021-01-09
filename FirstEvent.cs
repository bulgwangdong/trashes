using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FirstEvent : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text explain;
    [SerializeField] Image explainImage;

    GameObject selected;

  void SetText()
    {
        selected = eventSystem.currentSelectedGameObject;

        if(selected != null)
        {

            string e_Text = selected.GetComponent<Explains>().ExplainText;
            Sprite e_Image = selected.GetComponent<Explains>().ExplainSprite;

            if (e_Text != null)
            {
                explain.text = e_Text;

            }
            if(e_Image != null)
            {
                explainImage.sprite = e_Image;
            }

        }

    }


    private void Update()
    {
        SetText();
    }

}
