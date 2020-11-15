using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadGame : MonoBehaviour
{
    [SerializeField]
    string saveAndLoud;

    void Start()
    {
        
        if (File.Exists(Application.persistentDataPath + "/save1.save") || File.Exists(Application.persistentDataPath + "/save2.save") || File.Exists(Application.persistentDataPath + "/save3.save"))
        {
            GetComponent<Button>().enabled = true;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
            var colors = GetComponent<Button>().colors;
            colors.pressedColor = Color.gray;
            colors.normalColor = Color.gray;
            colors.selectedColor = Color.gray;
            colors.highlightedColor = Color.gray;
            GetComponent<Button>().colors = colors;
            GetComponent<Button>().enabled = false;
        }
        
    }

    //Chama a tela de Load
    public void ChamaLoad()
    {

        if (File.Exists(Application.persistentDataPath + "/save1.save") || File.Exists(Application.persistentDataPath + "/save2.save") || File.Exists(Application.persistentDataPath + "/save3.save"))
        {
            //PlayerStats.isLoad = true;
            //UnityEngine.SceneManagement.SceneManager.LoadScene(saveAndLoud);
        }
        else
        {
            Debug.Log("Não há dados");
        }
       
    }
}
