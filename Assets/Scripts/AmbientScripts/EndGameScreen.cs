using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    public Text PressText;
    bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
        bool locked = true;
        Vector4 cor = PressText.color;
        cor.w = 0;
        PressText.color = cor;
        Invoke("unlock", 1f);
    }

    void unlock()
    {
        locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(locked == false)
        {
            PressText.color = Vector4.MoveTowards(PressText.color, Vector4.one, Time.deltaTime / 2);
            if (Input.anyKeyDown)
            {
                SceneManager.LoadSceneAsync(0);
            }
        }
    }
}
