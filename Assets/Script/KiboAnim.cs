using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jam
{
    public class KiboAnim : MonoBehaviour
    {
        public Kibo kibo;
        public UnityEvent fadeOutEnd;
        // Start is called before the first frame update
        void Start()
        {
            if (fadeOutEnd == null)
                fadeOutEnd = new UnityEvent();
            kibo = transform.parent.gameObject.GetComponent<Kibo>();
        }

        public void OnFadeOutEnd()
        {
            fadeOutEnd.Invoke();
        }

        public void LockMovement()
        {
            kibo.CanMove = false;
        }

        public void FreeMovement()
        {
            kibo.CanMove = true;
        }
    }
}
