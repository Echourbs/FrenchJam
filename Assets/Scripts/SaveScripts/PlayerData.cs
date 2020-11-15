using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool hasFenix;
    public int level;
    public float[] position;
    public string playerName;
    public string playerDate;
    public int playerXP;

    public PlayerData(PlayerStats player)
    {
        level = player.playerLevel;
        playerName = PlayerStats.playerName;
        playerDate = PlayerStats.playerDate;
        playerXP = player.playerXP;
        hasFenix = PlayerStats.hasFenix;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}

