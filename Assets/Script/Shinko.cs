using System;
using UnityEngine;

#pragma warning disable 649
namespace Jam
{
    public class Shinko : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 7f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps.
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_WallCheck1;
        private Transform m_WallCheck2;
        private Rigidbody2D m_Rigidbody2D;
        private Animator _anim;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_WallCheck1 = transform.Find("WallCheck1");
            m_WallCheck2 = transform.Find("WallCheck2");
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    break;
                }
            }
        }


        public void Move(float move, bool jump)
        {
            _anim.SetFloat("Speed", Math.Abs(move));
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            else if (Physics2D.OverlapArea(m_WallCheck1.position, m_WallCheck2.position, m_WhatIsGround))
            {
                move = 0;
            }

            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        public bool facingRight()
        {
            return m_FacingRight;
        }


        public void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
