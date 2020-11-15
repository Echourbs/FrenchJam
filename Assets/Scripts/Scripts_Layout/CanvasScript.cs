using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class CanvasScript : MonoBehaviour
{
    public Text save1, save2, save3;

    //Nome da cena do jogo
    public string gameScene;

    public GameObject popUpNomeSave;
    public GameObject instructionSave;
    public GameObject levelManager;
    private Text txtIstructionSave;
    public GameObject btnPronto, btnSim;

    MenuInicial menu;

    public InputField inputField;
    public static int saveID;

    private string path;
    PlayerData data;

    void Start()
    {
        instructionSave.SetActive(false);
        txtIstructionSave = instructionSave.GetComponent<Text>();
        btnPronto.SetActive(true);
        btnSim.SetActive(false);
        menu = levelManager.GetComponent<MenuInicial>();

        if (File.Exists(Application.persistentDataPath + "/save1.save"))
        {
            data = SaveSystem.LoadPlayer1();
            save1.text = data.playerName + " " + data.playerDate;
            //save1sombra.text = data.playerName + " " + data.playerDate;
        }
        else
        {
            save1.text = "VAZIO";
        }

        if (File.Exists(Application.persistentDataPath + "/save2.save"))
        {
            data = SaveSystem.LoadPlayer2();
            save2.text = data.playerName + " " + data.playerDate;
            //save2sombra.text = data.playerName + " " + data.playerDate;
        }
        else
        {
            save2.text = "VAZIO";
        }

        if (File.Exists(Application.persistentDataPath + "/save3.save"))
        {
            data = SaveSystem.LoadPlayer3();
            save3.text = data.playerName + " " + data.playerDate;
            //save3sombra.text = data.playerName + " " + data.playerDate;
        }
        else
        {
            save3.text = "VAZIO";
        }
    }

    public void Save1()
    {
        path = Application.persistentDataPath + "/save1.save";
        if (!menu.loading)
        {
            popUpNomeSave.SetActive(true);
            menu.OpenInputNameBox();

        }

        if (File.Exists(path) && menu.loading)
        {
            PlayerStats.isLoad = true;
            SceneManager.LoadScene(gameScene);
        }
        else if (!File.Exists(path) && menu.loading)
        {
            EventSystem.current.SetSelectedGameObject(menu.save1Button);
            instructionSave.SetActive(true);
            txtIstructionSave.text = "NÃO HÁ DADOS";
        }
        saveID = 1;
    }

    public void Save2()
    {
        saveID = 2;
        path = Application.persistentDataPath + "/save2.save";
        if (!menu.loading)
        {
            popUpNomeSave.SetActive(true);
            menu.OpenInputNameBox();

        }

        if (File.Exists(path) && menu.loading)
        {
            PlayerStats.isLoad = true;
            SceneManager.LoadScene(gameScene);
        }
        else if (!File.Exists(path) && menu.loading)
        {
            instructionSave.SetActive(true);
            txtIstructionSave.text = "NÃO HÁ DADOS";
        }
    }

    public void Save3()
    {
        saveID = 3;
        path = Application.persistentDataPath + "/save3.save";
        if (!menu.loading)
        {
            popUpNomeSave.SetActive(true);
            menu.OpenInputNameBox();
        }

        if (File.Exists(path) && menu.loading)
        {
            PlayerStats.isLoad = true;
            SceneManager.LoadScene(gameScene);
        }
        else if (!File.Exists(path) && menu.loading)
        {
            instructionSave.SetActive(true);
            txtIstructionSave.text = "NÃO HÁ DADOS";
        }
    }

    public void OnValueChange()
    {
        PlayerStats.playerName = inputField.text;
    }

    public void ChamaSave()
    {      
        path = Application.persistentDataPath + "/save" + saveID + ".save";
        if(inputField.text == string.Empty)
        {
            instructionSave.SetActive(true);
            txtIstructionSave.text = "Nome não pode ser vazio...";
        }

        else
        {
            if (File.Exists(path) && !menu.loading)
            {
                instructionSave.SetActive(true);
                txtIstructionSave.text = "Você tem certeza que quer substituir o save?";
                btnPronto.SetActive(false);
                btnSim.SetActive(true);
            }
            else if (!File.Exists(path) && !menu.loading)
            {
                PlayerStats.isLoad = false;
                SceneManager.LoadScene(gameScene);
            }
        }      
    }

    public void Fechar()
    {
        popUpNomeSave.SetActive(false);
        inputField.text = "";
    }

    public void SubstituirSave()
    {
        PlayerStats.isLoad = false;
        SceneManager.LoadScene(gameScene);
    }

    public void ApagarSave1()
    {
        path = Application.persistentDataPath + "/save1.save";
        if (File.Exists(path))
        {
            File.Delete(path);
            save1.text = "VAZIO";
        }
        else
        {
            instructionSave.SetActive(true);

            txtIstructionSave.text = "NÃO HÁ SAVE AQUI";
        }
    }

    public void ApagarSave2()
    {
        path = Application.persistentDataPath + "/save2.save";
        if (File.Exists(path))
        {
            File.Delete(path);
            save2.text = "VAZIO";
        }
        else
        {
            instructionSave.SetActive(true);

            txtIstructionSave.text = "NÃO HÁ SAVE AQUI";
        }
    }

    public void ApagarSave3()
    {
        path = Application.persistentDataPath + "/save3.save";
        if (File.Exists(path))
        {
            File.Delete(path);
            save3.text = "VAZIO";
        }
        else
        {
            instructionSave.SetActive(true);

            txtIstructionSave.text = "NÃO HÁ SAVE AQUI";
        }
    }
}
