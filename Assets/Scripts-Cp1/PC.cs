using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public Rigidbody rigidBody;
    public FixedJoystick joystick;
    public float speed;
    public GameManagerBattle1 gameManager;
    public Animator anim;

    public GameObject arrow;

    private float minZ = -11;
    private float maxZ = 50;
    public bool isMoving;

    private void Update()
    {
        if (gameManager.isGameActive)
        {
            isMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;
            anim.SetBool("IsWalking", isMoving);
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            rigidBody.velocity = new Vector3(joystick.Horizontal * speed, rigidBody.velocity.y, joystick.Vertical * speed);
            float clampedZ = Mathf.Clamp(rigidBody.position.z, minZ, maxZ);
            rigidBody.position = new Vector3(rigidBody.position.x, rigidBody.position.y, clampedZ);

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rigidBody.velocity * Time.deltaTime);
            }
        }
    }

    public void ShootArrow()
    {
        anim.SetBool("Shoot", true);
        StartCoroutine(ShootArrowCoroutine());
        StartCoroutine(ResetShootAnimation());
    }

    private IEnumerator ShootArrowCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
        Instantiate(arrow, pos, transform.rotation);
    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Shoot", false);
    }
}
