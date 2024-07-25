using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovementBattle1 : MonoBehaviour
{
    public float speed = 40.0f;
    private float topBound = 15.0f;
    private float lowerBound = -15.0f;
    private Strike strike;
    void Start()
    {
       strike = GameObject.Find("Strike Area").GetComponent<Strike>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < lowerBound)
        {
            Destroy(gameObject);
        }
        if (transform.position.x > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < lowerBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if (strike.otherObj.Contains(other))
            {
                strike.otherObj.Remove(other);
            }
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
