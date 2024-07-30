using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{
    private float attackRange = 3.3f;
    private float attackCooldown = 1f;

    private Transform wallTarget;
    private float attackTimer;
    private Animator anim;
    private float minX = -13;
    private float maxX = 13;
    private float minZ = -12;
    private float maxZ = 13;
    private float ypos;

    private float speed = 7;

    private Damage damage;

    private Rigidbody rigidBody;

    void Start()
    {
        Transform child = transform.Find("EnemyChar2");
        GameObject enemyChar2 = child.gameObject;
        if (enemyChar2 != null)
        {
            anim = enemyChar2.GetComponent<Animator>();
        }

        ypos = gameObject.transform.position.y;
        rigidBody = gameObject.GetComponent<Rigidbody>();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            damage = player.GetComponent<Damage>();
            wallTarget = player.transform;
        }
        attackTimer = 0f;
    }

    void Update()
    {
        float clampedX = Mathf.Clamp(rigidBody.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(rigidBody.position.z, minZ, maxZ);
        rigidBody.position = new Vector3(clampedX, ypos, clampedZ);

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (wallTarget != null)
        {
            float distanceToWall = Vector3.Distance(transform.position, wallTarget.position);

            if (distanceToWall <= attackRange)
            {
                if (attackTimer <= 0f)
                {
                    AttackPlayer();
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }


    void MoveTowardsPlayer()
    {
        if(anim!=null)
            anim.SetBool("Run", true);
        Vector3 direction = (wallTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        transform.position = Vector3.MoveTowards(transform.position, wallTarget.position, speed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        if(anim!=null)
            anim.SetBool("Run", false);
        if (damage != null)
        {
            damage.takeDamagePlayer();
        }
    }
}
