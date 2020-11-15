using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necrovil : Enemy
{
    [Header("NECROVIL PROPERTIES")]
    public EnemyStatus status;
    public LayerMask attackableLayerMask;
    public float attackRange, maxFollowRange, attackCooldownTime = .5f, attackAreaWidth, attackAreaHeight;

    [SerializeField] private Transform attackCheckPoint;
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool PodeReviver;

    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Vector3 initialPosition;
    private Vector2 direction;

    private bool isDead, canAttack = true, isAttacking = false, ignoreRangeToFollow = false;
    private float distancePlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        status = GetComponent<EnemyStatus>();
    }

    protected override void Start()
    {
        player = MovementScript.player.transform;
        base.Start();        
    }

    void Update()
    {
        //Captura a distancia entre o inimigo e o jogador
        distancePlayer = Vector2.Distance(transform.position, player.position);

        if (!status.inDamage && !isDead) 
        {
            if (distancePlayer < maxFollowRange && distancePlayer > attackRange && !isAttacking && !ignoreRangeToFollow)
                FollowPlayer();
            else if (!ignoreRangeToFollow) StopFollow();
            else if (distancePlayer > attackRange && ignoreRangeToFollow) FollowPlayer(); 

            if(distancePlayer <= attackRange && canAttack) 
            {
                if (!player.gameObject.GetComponent<MovementScript>().isDead) Attack();
            }
        }

        if (status.life <= 0) 
        {
            if (PodeReviver) {
                canAttack = false;
                isDead = true;
                Die();
                PodeReviver = false;
            }
            else {
                base.animator.SetBool("dead", true);
                base.Die();
                Destroy(gameObject,4f);
            }
        }

        if (status.inDamage) TakeDamage(0);
    }

    public override void TakeDamage(int damageAmount) 
    {
        // Follow on hit
        if (!ignoreRangeToFollow)
        {
            ignoreRangeToFollow = true;
            FollowPlayer();            
        }
        // 

        base.animator.SetTrigger("damage");
        status.inDamage = false;
        //Transferir logica de dano pra cá
    }

    private void Attack() 
    {
        ignoreRangeToFollow = false;
        StartCoroutine(SetAttackCooldown());
        base.animator.SetTrigger("attack");
        isAttacking = true;
    }

    private void SetNotAttacking()
    {
        if (transform.position.x < player.position.x) {
            //inimigo vai para esquerda atras do jogador, caso ele esteje na direita
            direction = new Vector2(-1, 1);
            transform.localScale = direction;
        }
        else {
            //inimigo vai para direita atras do jogador, caso ele esteje na esquerda
            direction = new Vector2(1, 1);
            transform.localScale = direction;
        }
        isAttacking = false;
    }

    public override void Die()
    {
        base.Die();
        base.animator.SetBool("dead", true);
        GetComponent<EnemyDrop>().Drop();
    }

    //Called by an attack animation event
    private void CheckAttackHit() 
    {
        Vector2 pointA = new Vector2(transform.position.x, attackCheckPoint.position.y - attackAreaHeight / 2);
        Vector2 pointB = new Vector2(attackCheckPoint.position.x, attackCheckPoint.position.y + attackAreaHeight / 2);

        Collider2D[] collidersHit = Physics2D.OverlapAreaAll(pointA, pointB, attackableLayerMask);

        foreach (Collider2D collider in collidersHit) {
            PlayerHealth player = collider.gameObject.GetComponent<PlayerHealth>();

            if (player != null) {
                player.TakeDamage(Damage, EnemyStatus.damageTypes.physical, transform.position);
                break;
            }
        }
    }

    private IEnumerator SetAttackCooldown() 
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true; 
    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void FollowPlayer() 
    {
        base.animator.SetBool("idle", false);
        base.animator.SetBool("walking", true);

        //TODO: Refatorar corpos desses IFs
        if (transform.position.x < player.position.x) {
            //inimigo vai para esquerda atras do jogador, caso ele esteje na direita
            spriteRenderer.flipX = false;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            animator.SetBool("walking", true);
            direction = new Vector2(-1, 1);
            transform.localScale = direction;

        } else {
            //inimigo vai para direita atras do jogador, caso ele esteje na esquerda
            spriteRenderer.flipX = false;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            animator.SetBool("walking", true);
            direction = new Vector2(1, 1);
            transform.localScale = direction;
        }
    }

    //Para de seguir o jogador caso ele estiver fora do raio do inimigo
    void StopFollow() {
        base.animator.SetBool("idle", true);
        base.animator.SetBool("walking", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void OnKillPlayer() => throw new System.NotImplementedException();

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, maxFollowRange);

        Gizmos.color = Color.red;
        //A1-A2
        Vector2 pointA1 = new Vector2(transform.position.x, attackCheckPoint.position.y - attackAreaHeight / 2);
        Vector2 pointA2 = new Vector2(attackCheckPoint.position.x, attackCheckPoint.position.y - attackAreaHeight / 2);
        //B1-B2
        Vector2 pointB1 = new Vector2(transform.position.x, attackCheckPoint.position.y + attackAreaHeight / 2);
        Vector2 pointB2 = new Vector2(attackCheckPoint.position.x, attackCheckPoint.position.y + attackAreaHeight / 2);

        Gizmos.DrawLine(pointA1, pointA2);
        Gizmos.DrawLine(pointB1, pointB2);
    }
    #endif
}
