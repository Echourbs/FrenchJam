using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IndividualButtonScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler // required interface for OnSelect
{
    Color bege, laranja;
    bool onHover;
    EventSystem m_EventSystem;
    UnityAction m_myAction;
    BaseEventData bdt;
    GameObject initialButton;
    Button btn;
    [SerializeField] private bool inGameSceneButton;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        onHover = false;
        bege = MenuInicial.hexColor(17, 6, 6, 255);
        laranja = MenuInicial.hexColor(245, 181, 98, 255);
        if (this.gameObject.name == "ButtonSave1" || this.gameObject.name == "ButtonStart" || this.gameObject.name == "ButtonNão")
        {
            GetComponentInChildren<Text>().color = bege;
        }
        else
        {
            GetComponentInChildren<Text>().color = laranja;
        }
        //initialButton = EventSystem.current.currentSelectedGameObject;
    }

    private void Update()
    {
        Button b = GetComponent<Button>();
        ColorBlock cb = b.colors;
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            cb.normalColor = Color.white;
            b.colors = cb;
        }
        else
        {
            cb.normalColor = new Color(1, 1, 1, 0);
            b.colors = cb;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!inGameSceneButton)
        {
            GetComponentInChildren<Text>().color = bege;
        }
    }

    public void TaskOnClick()
    {
        if (!inGameSceneButton)
        {
            GetComponentInChildren<Text>().color = laranja;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!inGameSceneButton)
        {
            GetComponentInChildren<Text>().color = laranja;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {       
        EventSystem.current.SetSelectedGameObject(null);
        if (!inGameSceneButton)
        {
            GetComponentInChildren<Text>().color = bege;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!inGameSceneButton)
        {
            GetComponentInChildren<Text>().color = laranja;
        }
    }
}
