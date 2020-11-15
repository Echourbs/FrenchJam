using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MenuInicial : MonoBehaviour
{
    //GameObject exitGame;
    //public GameObject CanvasSaveAndLoud;

    [Header("Menu Buttons")]
    public GameObject newgameButton;
    public GameObject loadButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public GameObject save1Button;
    public GameObject voltarButton;
    public GameObject exitCanvas;
    public GameObject noButton;
    public GameObject inputFieldName;

    [Header("Menu Pages")]
    public GameObject popUpSair;
    public GameObject popUpUserName;
    public GameObject canvasMenuInicial;
    public GameObject canvasCarregarNovoJogo;

    [Header("Instruction Text")]
    [SerializeField] Text loadOrNewGame;

    [Header("Colors")]
    Color bege;
    Color laranja;
    [HideInInspector] public bool loading;
    int page;
    bool canChangePage;

    private void Start()
    {
        page = 0;
        canChangePage = true;
        bege = hexColor(17, 6, 6, 255);
        laranja = hexColor(245, 181, 98, 255);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newgameButton);
        //newgameButton.GetComponentInChildren<Text>().color = bege;
    }

    public static Vector4 hexColor(float r, float g, float b, float a)
    {
        Vector4 color = new Vector4(r / 255, g / 255, b / 255, a / 255);
        return color;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && canChangePage && page == 0)
        {
            page--;
            canChangePage = false;
        }
        else if (Input.GetButtonDown("Cancel") && canChangePage && page == -1)
        {
            ExitCanvasOff();
            canChangePage = false;
        }

        if (Input.GetButtonDown("Cancel") && canChangePage && page == 1)
        {
            NewGameOff();
            canChangePage = false;
        }

        if (Input.GetButtonDown("Cancel") && canChangePage && page == 2)
        {
            Page1();
            canChangePage = false;
        }

        //Identifica qual "página" está no menu
        switch (page)
        {
            case -1:
                canChangePage = true;
                popUpSair.SetActive(true);
                break;
            case 0:
                canChangePage = true;
                popUpSair.SetActive(false);
                canvasMenuInicial.SetActive(true);
                canvasCarregarNovoJogo.SetActive(false);
                break;
            case 1:
                popUpUserName.SetActive(false);
                canChangePage = true;
                break;
            case 2:
                canChangePage = true;
                break;
            default:
                break;
        }
    }

    //Liga canvas de SairDoJogo 
    public void ExitCanvasOn()
    {
        popUpSair.SetActive(true);
        EventSystem.current.SetSelectedGameObject(noButton);
        page = -1;
        newgameButton.GetComponentInChildren<Text>().color = laranja;
    }

    //Desliga canvas de SairDoJogo 
    public void ExitCanvasOff()
    {
        EventSystem.current.SetSelectedGameObject(exitButton);
        page = 0;
        exitButton.GetComponentInChildren<Text>().color = bege;
    }
    
    //Desliga Novo Jogo
    public void NewGameOff()
    {
        EventSystem.current.SetSelectedGameObject(newgameButton);
        
        
        voltarButton.GetComponentInChildren<Text>().color = laranja;
        page = 0;        
        newgameButton.GetComponentInChildren<Text>().color = bege;
    }

    //Chama a tela de new game (Tela do jogo)
    public void ChamaNovoJogo()
    {
        EventSystem.current.SetSelectedGameObject(save1Button);
        loadOrNewGame.text = "Novo Jogo";
        page = 1;
        save1Button.GetComponentInChildren<Text>().color = bege;
        loading = false;
        PlayerStats.isLoad = loading;
    }

    //Abre Input de colocar nome
    public void OpenInputNameBox()
    {
        page = 2;
        EventSystem.current.SetSelectedGameObject(inputFieldName);
    }

    //Liga canvas de Load
    public void ChamaLoad()
    {
        EventSystem.current.SetSelectedGameObject(save1Button);
        loadOrNewGame.text = "Carregar Jogo";
        page = 1;
        save1Button.GetComponentInChildren<Text>().color = bege;
        loading = true;
        PlayerStats.isLoad = loading;
    }

    //Liga canvas da página 1 sem interferir na booleana "loading"
    void Page1()
    {
        EventSystem.current.SetSelectedGameObject(save1Button);
        page = 1;
        save1Button.GetComponentInChildren<Text>().color = bege;
    }

    //Fecha o aplicativo/jogo
    public void CloseGame()
    {
        Application.Quit();
    }

}
