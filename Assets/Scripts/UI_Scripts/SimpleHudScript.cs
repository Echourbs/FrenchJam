using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHudScript : MonoBehaviour
{
    Text coinAmountText;
    public static int coinAmount;

    void Start()
    {
        coinAmountText = GetComponentInChildren<Text>();
        coinAmount = 0;
    }

    void Update()
    {
        coinAmountText.text = coinAmount.ToString();
    }
}
