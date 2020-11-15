using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TriggerCameraEvent : MonoBehaviour
{
    CinemachineVirtualCamera cam;

    public Image ImageToFade;


    [SerializeField]
    float desirableFov;

    [SerializeField]
    float cameraDesirableCenterHeight;

    [SerializeField]
    float timeForFullChange;

    Transform newCameraCenterOnBossFight;

    [SerializeField]
    bool destroyOnDone;

    public Transform BossTransform;
    Transform PseudoBoss;

    bool castChange = false;

    public bool loadEndScene = false;
    public string cenaFim;

    public float align = 9f;

    public bool resetAlign = false;
    public float resetAlignValue;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        

        Vector4 cor = ImageToFade.color;
        cor.w = 0;
        ImageToFade.color = cor;

        ImageToFade.raycastTarget = false;
        ImageToFade.maskable = false;
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == MovementScript.player)
        {
            //gera um ponto vazio como novo pivot para levantar a altura da camera;
            newCameraCenterOnBossFight = new GameObject().transform;
            newCameraCenterOnBossFight.name = "newCameraCenterOnBossFight";

            newCameraCenterOnBossFight.position = MovementScript.player.transform.position;
            newCameraCenterOnBossFight.transform.SetParent(MovementScript.player.transform);

            //correção de posição da camera
            //faz um calculo de numero fracionado menor que 1 correspondente a distancia entre o fov e o fov desejavel e multiplica a altura desejavel para reposicionar a camera
            float CorrectionPercent = cam.m_Lens.FieldOfView / desirableFov;

            newCameraCenterOnBossFight.localPosition = new Vector3(0, cameraDesirableCenterHeight * CorrectionPercent, 0);
            MovementScript.player.GetComponent<PlayerCameraControl>().mapedCameraTargetTransform = newCameraCenterOnBossFight;

            PseudoBoss = new GameObject().transform;
            PseudoBoss.transform.position = BossTransform.transform.position;
            PseudoBoss.transform.position += new Vector3(0, -align, 0);

            cam.Follow = PseudoBoss;
            cam.LookAt = PseudoBoss;


            //BossTransform.gameObject.GetComponent<FogKeeperMeleeAtackScript>();

            ChangeCamera();
            
           


        }
    }

    
    void Update()
    {
        if (castChange)
        {
            Vector4 cor = ImageToFade.color;
            cor.w = 1;

            ImageToFade.color = Vector4.MoveTowards(ImageToFade.color, cor, Time.deltaTime);
            Invoke("CallFade", Time.deltaTime);
        }
       

        
    }

    void ChangeCamera()
    {
        if (cam.m_Lens.FieldOfView != desirableFov || newCameraCenterOnBossFight.localPosition.y != cameraDesirableCenterHeight)
        {

            //float calculatedValueFov = cam.m_Lens.FieldOfView / timeForFullChange;
            float calculatedValueFov = desirableFov / timeForFullChange;

            cam.m_Lens.FieldOfView = Mathf.MoveTowards(cam.m_Lens.FieldOfView, desirableFov, calculatedValueFov * Time.deltaTime);

            float calculatedSpeedForHeight = cameraDesirableCenterHeight / timeForFullChange;

            newCameraCenterOnBossFight.localPosition = new Vector3(0, Mathf.MoveTowards(newCameraCenterOnBossFight.localPosition.y, cameraDesirableCenterHeight, calculatedSpeedForHeight * Time.deltaTime), 0);

            Invoke("ChangeCamera", Time.deltaTime);
        }
        else
        {
            if (loadEndScene)
            {
                castChange = true;
            }

            if(destroyOnDone == true)
            {
                Invoke("resetPlayerCameraToPlayer", 1.1f);
                
                Destroy(gameObject,1.15f);
            }
        }
        
    }

    void resetPlayerCameraToPlayer()
    {
        if (loadEndScene == true)
        {
            SceneManager.LoadSceneAsync(cenaFim);
        }

        if(resetAlign == true)
        {
            newCameraCenterOnBossFight.transform.position += new Vector3(0, resetAlignValue, 0);
        }

        cam.Follow = newCameraCenterOnBossFight;
        cam.LookAt = newCameraCenterOnBossFight;
        Destroy(PseudoBoss.gameObject);
    }
}
