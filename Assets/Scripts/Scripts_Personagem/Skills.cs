using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "RPG Generator/Player/Create Skill")]
public class Skills : ScriptableObject
{
    public string description;
    public Sprite icon;
    public int levelNeeded;
    public int xpNeeded;

    public List<PlayerAttributes> Attributes = new List<PlayerAttributes>();

}


