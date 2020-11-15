using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITransition : MonoBehaviour
{
    public static UITransition instance;
    [Range(0f,5f)]
    public float transitionDuration = 1f;
    public Image blackPanel;
    private bool fadingIn = false, fadingOut = false;
    private float currentAlpha = 0f;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        blackPanel = GetComponent<Image>();
    }

    void Update()
    {
        if(fadingIn || fadingOut)
        {
            if (fadingIn)
            {
                ActivatePanelImage();
                currentAlpha += transitionDuration * Time.deltaTime;

                if (currentAlpha >= 1)
                {
                    //When panel is black
                    currentAlpha = 1;
                    fadingIn = false;                    
                }
            }
            else if (fadingOut)
            {
                currentAlpha -= transitionDuration * Time.deltaTime;

                if (currentAlpha <= 0)
                {
                    //When panel is transparent
                    currentAlpha = 0;
                    fadingOut = false;
                    DeactivatePanelImage();
                }
            }

            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, currentAlpha);
        }

    }

    public static void FadeIn()
    {
        instance.fadingIn = true;
    }

    public static void FadeOut()
    {
        instance.fadingOut = true;
    }

    public static void FadeInThenOut()
    {
        
        instance.StartCoroutine(instance.FadeInThenOutCo());
    }

    private IEnumerator FadeInThenOutCo()
    {
        FadeIn();
        print("11111");
        yield return new WaitForSeconds(instance.transitionDuration);
        print("22222");
        FadeOut();
    }

    private void ActivatePanelImage()
    {
        blackPanel.enabled = true;
    }

    private void DeactivatePanelImage()
    {
        blackPanel.enabled = false;
    }
}
