using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoldingFenix : MonoBehaviour
{
    public Sprite frameFenix;
    private GameObject imageFenix;
    public static GameObject molding;

    void Awake()
    {
        molding = this.gameObject;
        imageFenix = this.transform.GetChild(0).gameObject;
    }

    public void ChangeFrame()
    {
        GetComponent<Image>().sprite = frameFenix;
        imageFenix.SetActive(true);
    }
}
