using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    private bool allowStrike = false;
    private Transform player;
    public List<Collider> otherObj = new List<Collider>();
    private Animator anim;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        anim = GameObject.Find("PlayerCharacter").GetComponent<Animator>();
    }

    private void Update()
    {
        if (otherObj.Count == 0)
        {
            allowStrike = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && other != null)
        {
            otherObj.Add(other);
            allowStrike = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && other != null)
        {
            otherObj.Remove(other);
        }
    }

    public void StrikeEnemy()
    {
        anim.SetBool("Punch", true);
        StartCoroutine(ResetPunchAnimation());

        if (allowStrike && otherObj.Count != 0)
        {
            Collider des = otherObj[0];

            if (des != null && des.gameObject != null)
            {
                Vector3 direction = (des.transform.position - player.position).normalized;
                direction.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                player.rotation = Quaternion.Slerp(player.rotation, lookRotation, 10f);

                otherObj.RemoveAt(0);
                StartCoroutine(PunchEnemy(des));
            }
        }
    }

    private IEnumerator PunchEnemy(Collider des)
    {
        yield return new WaitForSeconds(0.5f);

        if (des != null && des.gameObject != null)
        {
            Destroy(des.gameObject);
        }
    }

    private IEnumerator ResetPunchAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Punch", false);
    }
}
