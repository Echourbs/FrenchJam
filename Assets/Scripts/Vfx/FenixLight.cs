using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FenixLight : MonoBehaviour
{
    [SerializeField]
    public Light2D light;

    [SerializeField]
    float IntencidadeMin, IntencidadeMax, StepMim,StepMax;
    float IntencidadeAtual;
    void Start()
    {
        IntencidadeAtual = IntencidadeMin;
        Aumentar();
    }

    void Aumentar()
    {
        IntencidadeAtual += Random.Range(StepMim,StepMax);

        if (IntencidadeAtual >= IntencidadeMax)
        {
            Invoke("Diminuir", 0.1f);
        }
        else
        {
            Invoke("Aumentar",0.1f);
        }
    }

    void Diminuir()
    {
        IntencidadeAtual -= Random.Range(StepMim, StepMax);

        if (IntencidadeAtual <= IntencidadeMin)
        {
            Invoke("Aumentar", 0.1f);
        }
        else
        {
            Invoke("Diminuir", 0.1f);
        }
    }

    private void Update()
    {
        light.intensity = Mathf.Clamp(IntencidadeAtual, IntencidadeMin, IntencidadeMax);
    }
}
