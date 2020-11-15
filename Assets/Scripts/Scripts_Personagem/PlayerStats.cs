using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [Header("Main Player Stats")]
    public static bool hasFenix;
    public static string playerName;
    public static string playerDate;
    public int playerXP = 0;
    public int playerLevel = 1;
    public int playerHP = 50;//baseline = 50, +5 each level

    [Header("Player Attributes")]
    public List<PlayerAttributes> Attributes = new List<PlayerAttributes>();

    [Header("Player Skills Enabled")]
    public List<Skills> PlayerSkills = new List<Skills>();

    [Header("Verification of Loading")]
    public static bool isLoad;
    PlayerData data;

    void Start()
    {
        //Verifica se o jogador entrou pelo botao carregar no menu
        if (isLoad)
        {
            LoadPlayer(CanvasScript.saveID);
        }
        else
        {
            SavePlayer();          
        }

        print(playerName);
    }

    void Update()
    {
        
        //Botão para salvar (Temporario para testes)
        if (Input.GetKeyDown(KeyCode.F))
        {
            SavePlayer();
        }

        //Botão para carregar (Temporario para testes)
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayer(CanvasScript.saveID);
        }

    }

    public void SavePlayer()
    {
        playerDate = System.DateTime.Now.ToString();
        SaveSystem.SavePlayer(this);       
    }

    public void LoadPlayer(int i)
    {
        if (i == 0)
        {
            data = SaveSystem.LoadPlayer0();
        }
        if (i == 1)
        {
            data = SaveSystem.LoadPlayer1();
        }
        if (i == 2)
        {
            data = SaveSystem.LoadPlayer2();
        }
        if (i == 3)
        {
            data = SaveSystem.LoadPlayer3();
        }

        //Carrega o nome, o xp, o level do jogador e sua posição
        hasFenix = data.hasFenix;
        playerName = data.playerName;
        playerXP = data.playerXP;
        playerLevel = data.level;
        playerDate = data.playerDate;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        if (data.hasFenix)
        {
            GetComponent<MovementScript>().EnableFenix();
        }
        return;
    }
}

