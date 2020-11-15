using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthScript : MonoBehaviour
{
    public static GameObject bossLifeBar;
    public HealthBarScript bossHealth;
    EnemyStatus bossStatus;
    public GameObject boss;

    [SerializeField]
    Text textLifebar;
    [SerializeField]
    Text textLifebarSombra;
    Color shadowTextColor;

    public List<GameObject> childs;

    public string bossName;

    void Awake()
    {
        bossLifeBar = this.gameObject;
    }

    void Start()
    {
        Invoke("StartDelayed", Time.deltaTime * 3);
    }

    void StartDelayed()
    {
        bossHealth = GetComponentInChildren<HealthBarScript>();
        bossStatus = boss.GetComponent<EnemyStatus>();
        bossStatus.gradientHealthBar = bossHealth;
        bossStatus.lifeBar = bossHealth.GetComponent<Image>();


        shadowTextColor = textLifebarSombra.color;

        //desativa os filhos e ele mesmo
        for (int I = 0; I < childs.Count; I++)
        {
            childs[0].SetActive(false);
        }

        this.gameObject.SetActive(false);
    }

    public void reloadStatusWithNoDeactivation()
    {
        bossHealth = GetComponentInChildren<HealthBarScript>();
        bossStatus = boss.GetComponent<EnemyStatus>();
        bossStatus.gradientHealthBar = bossHealth;
        bossStatus.lifeBar = bossHealth.GetComponent<Image>();


        shadowTextColor = textLifebarSombra.color;

        //desativa os filhos e ele mesmo
        for (int I = 0; I < childs.Count; I++)
        {
            childs[0].SetActive(false);
        }
    }

    public void LoadBossLifBar()
    {
        //religa os filhos
        for (int I = 0; I < childs.Count; I++)
        {
            childs[0].SetActive(true);
        }

        textLifebar.text = bossName;
        textLifebarSombra.text = textLifebar.text;


        bossStatus.StatusBarUpdate();
    }

    public void UnableLifeBar()
    {
        for (int I = 0; I < childs.Count; I++)
        {
            childs[0].SetActive(false);
        }

        textLifebar.text = "";
        textLifebarSombra.text = "";

        this.gameObject.SetActive(false);
    }

}
