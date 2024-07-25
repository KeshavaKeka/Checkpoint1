using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float attackRange = 3.3f;
    private float attackCooldown = 1f;

    private Transform wallTarget;
    private float attackTimer;
    private float range = 5.0f;
    private float rand;
    private Vector3 porPos;
    private Animator anim;
    private float minX = -13;
    private float maxX = 13;
    private float minZ = -12;
    private float maxZ = 13;
    private float ypos;

    private float speed = 12;

    private Damage damage;

    private Rigidbody rigidBody;

    void Start()
    {
        GameObject enemyChar1 = GameObject.Find("EnemyChar1");
        if (enemyChar1 != null)
        {
            anim = enemyChar1.GetComponent<Animator>();
        }

        ypos = gameObject.transform.position.y;
        rigidBody = gameObject.GetComponent<Rigidbody>();

        GameObject portugese = GameObject.Find("Portugese");
        if (portugese != null)
        {
            damage = portugese.GetComponent<Damage>();
            wallTarget = portugese.transform;
        }

        rand = Random.Range(-range, range);
        if (wallTarget != null)
        {
            porPos = new Vector3(wallTarget.transform.position.x + rand, wallTarget.transform.position.y, wallTarget.transform.position.z + 1);
        }
        attackTimer = 0f;
    }

    void Update()
    {
        float clampedX = Mathf.Clamp(rigidBody.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(rigidBody.position.z, minZ, maxZ);
        rigidBody.position = new Vector3(clampedX, ypos, clampedZ);

        if(attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (wallTarget != null)
        {
            float distanceToWall = Vector3.Distance(transform.position, porPos);

            if (distanceToWall <= attackRange)
            {
                if (attackTimer <= 0f)
                {
                    AttackWall();
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                MoveTowardsWall();
            }
        }
    }

    void MoveTowardsWall()
    {
        if(anim!=null)
            anim.SetBool("Run", true);
        Vector3 direction = (porPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        transform.position = Vector3.MoveTowards(transform.position, porPos, speed * Time.deltaTime);
        // Add your movement code here (e.g., using a NavMeshAgent if you want more advanced movement)
    }

    void AttackWall()
    {
        if(anim!=null)
            anim.SetBool("Run", false);
        if (damage != null)
        {
            damage.takeDamage();
        }
    }
}
