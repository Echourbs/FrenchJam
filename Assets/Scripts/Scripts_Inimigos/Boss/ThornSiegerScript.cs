using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornSiegerScript : MonoBehaviour
{
    //lista de prontos fracos
    [HideInInspector]
    public List<GameObject> pontosFracos;

    [HideInInspector]
    public EnemyStatus BossStatus;


    // Start is called before the first frame update
    void Start()
    {
        BossStatus = this.GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
