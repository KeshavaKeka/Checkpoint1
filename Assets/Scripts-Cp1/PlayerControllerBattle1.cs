using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBattle1 : MonoBehaviour
{
    public Rigidbody rigidBody;
    public FixedJoystick joystick;
    public float speed;
    public GameManagerBattle1 gameManager;
    public Animator anim;

    public GameObject arrow;

    private float minX = -13;
    private float maxX = 13;
    private float minZ = -11;
    private float maxZ = 13;
    public bool isMoving;

    private bool isShooting = false;

    private void Start()
    {
        anim = GameObject.Find("PlayerCharacter").GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameManager.isGameActive)
        {
            isMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;
            anim.SetBool("IsWalking", isMoving && !isShooting);
        }
    }

    private void FixedUpdate()
    {
        //if (gameManager.isGameActive && !isShooting)
        if (gameManager.isGameActive)
        {
            rigidBody.velocity = new Vector3(joystick.Horizontal * speed, rigidBody.velocity.y, joystick.Vertical * speed);

            float clampedX = Mathf.Clamp(rigidBody.position.x, minX, maxX);
            float clampedZ = Mathf.Clamp(rigidBody.position.z, minZ, maxZ);
            rigidBody.position = new Vector3(clampedX, rigidBody.position.y, clampedZ);

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rigidBody.velocity * Time.deltaTime);
            }
        }
    }

    public void ShootArrow()
    {
        if (!isShooting)
        {
            isShooting = true;
            anim.SetBool("Shoot", true);
            StartCoroutine(ShootArrowCoroutine());
            StartCoroutine(ResetShootAnimation());
        }
    }

    private IEnumerator ShootArrowCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z + 3.5f);
        Instantiate(arrow, pos, transform.rotation);
    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Shoot", false);
        isShooting = false;
    }
}
