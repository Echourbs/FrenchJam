using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogueTrigger[] soul;

    public void endGame()
    {
        int failed = 0;
        foreach (DialogueTrigger d in soul)
        {
            if (d.helped())
            {
                if (failed == 0)
                {
                    d.fail();
                }
                ++failed;
            }
        }
    }
}
