using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espreitador : Enemy
{
    [Header("Sensors")]
    [SerializeField] private LayerMask layer;
    [SerializeField] private Transform sensorFloor1;
    [SerializeField] private Transform sensorFloor2;
    [SerializeField] private Transform sensorWall1;
    [SerializeField] private Transform sensorWall2;

    [Header("ESPREITADOR PROPERTIES")]
    public LayerMask attackableLayerMask;
    public float attackRange, rangeToAttack, rangeToStartFollowing, attackCooldownTime;

    [SerializeField] private Transform attackCheckPoint;
    [SerializeField] private bool podeReviver;
    [SerializeField] private float walkRange;
    [SerializeField] private float moveSpeed;

    private EnemyStatus status;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector2 direction;

    private bool isDead, canAttack = true, isAttacking = false, isPatrolling = true, ignoreRangeToFollow = false;
    private float distancePlayer;
    private bool movingForward = true;

    private float walkMax, walkMin;
    private bool isFollowing;
    private int directionInt = -1;

    public override void OnKillPlayer()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = MovementScript.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        status = GetComponent<EnemyStatus>();

        walkMax = transform.position.x + walkRange;
        walkMin = transform.position.x - walkRange;

        isFollowing = false;
        isAttacking = false;
        isDead = false;

        float rDir = Random.Range(0, 2);
        // directionInt = (rDir == 0 ? 1 : -1);

        if (rDir == 0) 
        {
            Flip();
            directionInt = 1;
        }
        else 
        {
            directionInt = -1;
        }

        // transform.rotation = Quaternion.Euler(0, (directionInt == -1 ? 0 : 180), 0);
        StartCoroutine(RandDirection());
    }

    void FixedUpdate()
    {
        //Captura a distancia entre o inimigo e o jogador
        distancePlayer = Vector2.Distance(transform.position, player.position);

        // if (isPatrolling)
        // {
        //     if (movingForward)
        //         GoToTargetPoint();
        //     else GoToStartPoint();
        // }      
    }

    private void Update()
    {
        // print("isAttack: " + isAttacking);

        if (!isFollowing)
        {
            moveSpeed = 2.0f;
            transform.Translate(directionInt * moveSpeed * Time.deltaTime, 0, 0);
            //
            bool wall = Physics2D.Linecast(sensorWall1.position, sensorWall2.position, layer);
            bool floor1 = Physics2D.Raycast(sensorFloor1.position, Vector3.forward, Mathf.Infinity, layer);
            bool floor2 = Physics2D.Raycast(sensorFloor2.position, Vector3.forward, Mathf.Infinity, layer);
            //
            if( (!floor1 && floor2) || wall)
            {
                if (transform.right.x > 0)
                    transform.rotation = Quaternion.Euler(0, (directionInt == -1 ? 180 : 0), 0);
                else if (transform.right.x < 0)
                    transform.rotation = Quaternion.Euler(0, (directionInt == -1 ? 0 : 180), 0);
            }

            // if (!floor1 && !floor2 && !wall) canChangeDirection = true;
            // else canChangeDirection = false;
        }

        if (!isDead)
        {
            if (distancePlayer < rangeToStartFollowing && distancePlayer > rangeToAttack )
            {
                if (ignoreRangeToFollow) 
                {
                    ignoreRangeToFollow = false;
                    return;
                }

                isFollowing = true;
                FollowPlayer();
            }
            else if(distancePlayer > rangeToStartFollowing && !ignoreRangeToFollow) StopFollow();
            else if (ignoreRangeToFollow) FollowPlayer();
            
            if (distancePlayer < rangeToAttack && canAttack)
            {
                if (!player.gameObject.GetComponent<MovementScript>().isDead)
                {
                    isAttacking = true;
                    base.animator.SetBool("attack", true);
                    animator.SetBool("idle", false);
                    base.animator.SetBool("walking", false);
                    canAttack = false;
                    Invoke("nge", attackCooldownTime);
                }
            }
        }

        if (isAttacking)
        {
            moveSpeed = 0;
            if (!canAttack) animator.SetBool("idle", true);
        }

        if (status.life <= 0)
        {
            if (podeReviver) {
                isDead = true;
                Die();
                podeReviver = false;
            }
            else {
                base.Die();
                base.animator.SetBool("attack", false);
                animator.SetBool("walking", false);
                animator.SetBool("dead", true);
                Destroy(gameObject, 4f);
            }
        }

        if (status.inDamage) TakeDamage(0);
    }

    protected virtual void GoToStartPoint() => Debug.LogWarning("Not Implemented");
    public void DamageOff() => status.inDamage = false;

    // protected virtual void GoToTargetPoint()
    // {
    //     transform.Translate(direction*moveSpeed);

    //     //Back to start
    //     if (Vector2.Distance(transform.position, targetPosition) <= 0.1f)
    //     {
    //         Flip();
    //         movingForward = false;
    //         CalculateExpectedDirection();
    //     }
    // }

    // protected virtual void CalculateExpectedDirection()
    // {
    //     if (movingForward)
    //         direction = CalculateDirectionToTarget();
    //     else direction = CalculateDirectionToStart();
    // }

    // protected virtual Vector3 CalculateDirectionToTarget() => Vector3.Normalize(targetPosition - transform.position);
    // protected virtual Vector3 CalculateDirectionToStart() => Vector3.Normalize(startPosition - transform.position);

    public override void TakeDamage(int damageAmount)
    {
        // Follow on hit
        if (!isFollowing && !ignoreRangeToFollow)
        {
            ignoreRangeToFollow = true;
            FollowPlayer();            
        }
        // 

        base.animator.SetTrigger("damage");
        status.inDamage = false;
        //Transferir logica de dano pra cá
    }

    public override void Die()
    {
        base.Die();
        base.animator.SetBool("attack", false);
        animator.SetBool("walking", false);
        animator.SetBool("dead", true);
        GetComponent<EnemyDrop>().Drop();
    }

    //Called by an attack animation event
    public void NotAttacking()
    {
        base.animator.SetBool("attack", false);
        isAttacking = false;
    }

    //Called by an attack animation event
    public void CheckAttackHit()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackCheckPoint.position, attackRange, attackableLayerMask);

        foreach (Collider2D collider in collidersHit)
        {
            PlayerHealth player = collider.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(Damage, EnemyStatus.damageTypes.physical, transform.position);
                break;
            }
        }
    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void FollowPlayer()
    {
        isFollowing = true;
        animator.SetBool("idle", false);
        
        if (!isAttacking)
        {
            base.animator.SetBool("walking", true);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), moveSpeed * Time.deltaTime);
        }

        bool wall = Physics2D.Linecast(sensorWall1.position, sensorWall2.position, layer);
        bool floor1 = Physics2D.Raycast(sensorFloor1.position, Vector3.forward, Mathf.Infinity, layer);
        bool floor2 = Physics2D.Raycast(sensorFloor2.position, Vector3.forward, Mathf.Infinity, layer);
        bool bloqueioNaFrente = (!floor1 && floor2) || wall;

        if (transform.position.x < player.position.x && !isAttacking && !bloqueioNaFrente)
        {
            moveSpeed = 4f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.position.x > player.position.x && !isAttacking && !bloqueioNaFrente)
        {
            moveSpeed = 4f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if( ((transform.position.x > player.position.x && transform.right.x > 0) ||
            (transform.position.x < player.position.x && transform.right.x < 0)) && bloqueioNaFrente) 
        {
            moveSpeed = 0f;
            base.animator.SetBool("attack", true);
            animator.SetBool("idle", false);
            base.animator.SetBool("walking", false);
        }
    }

    //Para de seguir o jogador caso ele estiver fora do raio do inimigo
    void StopFollow()
    {
        isFollowing = false;
        isAttacking = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private IEnumerator RandDirection() 
    {
        float rand = Random.Range(3f, 12f);
        yield return new WaitForSeconds(rand);

        float rDir = Random.Range(0, 2);
        if (directionInt != (rDir == 0 ? 1 : -1))
        {
            Flip();
            directionInt = (rDir == 0 ? 1 : -1);
        }

        StartCoroutine(RandDirection());
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheckPoint.position, attackRange);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, rangeToStartFollowing);

    }
    #endif
}
