using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Jam
{
    [RequireComponent(typeof (Kibo))]
    public class KiboUserControl : MonoBehaviour
    {
        private Kibo m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<Kibo>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, m_Jump);
            m_Jump = false;
        }
    }
}
