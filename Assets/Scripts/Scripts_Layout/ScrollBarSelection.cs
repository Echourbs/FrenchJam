using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollBarSelection : MonoBehaviour
{
    Scrollbar sb;
    Navigation customNav;
    public Button voltar, control;

    void Start()
    {
        sb = GetComponent<Scrollbar>();
        customNav = new Navigation();
        customNav.mode = Navigation.Mode.Explicit;
    }
   
    void Update()
    {
        print(sb.value);
        if(sb.value >= 1)
        {
            customNav.selectOnUp = control;
            customNav.selectOnDown = null;
            GetComponent<Scrollbar>().navigation = customNav;
        }

        else if(sb.value <= 0)
        {
            customNav.selectOnUp = null;
            customNav.selectOnDown = voltar;
            GetComponent<Scrollbar>().navigation = customNav;
        }
    }
}
