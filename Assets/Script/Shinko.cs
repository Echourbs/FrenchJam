using System;
using UnityEngine;

#pragma warning disable 649
namespace Jam
{
    public class Shinko : MonoBehaviour, ICharacter
    {
        [SerializeField] private float m_MaxSpeed = 7f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps.
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        public bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_WallCheck1;
        private Transform m_WallCheck2;
        public Rigidbody2D m_Rigidbody2D;
        private Animator _anim;
        private float _traveledWallDistance = 0;
        private bool _wallRiding = false;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private Transform _sprite;
        private Vector3 _origin;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_WallCheck1 = transform.Find("WallCheck1");
            m_WallCheck2 = transform.Find("WallCheck2");
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            _sprite = transform.Find("Sprite");
            _anim = _sprite.gameObject.GetComponent<Animator>();
            _origin = _sprite.localPosition;
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
                    _traveledWallDistance = 30;
                    break;
                }
            }
        }

        public void stop()
        {
            _anim.SetFloat("Speed", 0);
        }

        private void startWallRide()
        {
            Debug.Log("Start");
            _sprite.rotation = m_FacingRight ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
            _sprite.localPosition += new Vector3(-0.4f, 0, 0);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            _wallRiding = true;
        }

        private void stopWallRide()
        {
            _wallRiding = false;
            _sprite.localPosition = _origin;
            _sprite.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void Move(float move, bool jump)
        {
            Vector2 toMove = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
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
            else if (Physics2D.OverlapArea(m_WallCheck1.position, m_WallCheck2.position, m_WhatIsGround) && !m_Grounded)
            {
                if (!_wallRiding)
                {
                    startWallRide();
                }
                if (move != 0 && _traveledWallDistance > 0)
                {
                    toMove.y = m_MaxSpeed;
                    --_traveledWallDistance;
                }
            }
            else
            {
                stopWallRide();
            }
            m_Rigidbody2D.velocity = Vector2.MoveTowards(m_Rigidbody2D.velocity, toMove, Time.deltaTime * (m_Grounded || _wallRiding ? 80:30));

            // If the player should jump...
            if (m_Grounded && jump && !_wallRiding)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }

            if (_wallRiding && jump && !m_Grounded)
            {
                m_Rigidbody2D.velocity = new Vector2(0, 0);
                m_Rigidbody2D.AddForce(new Vector2((m_FacingRight ? -m_JumpForce : m_JumpForce), m_JumpForce));
                stopWallRide();
                Flip();
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
