using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
	Rigidbody2D rigidbody2d;
    float projectileRange = 15.0f;

	// Start is called before the first frame update
	void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > projectileRange) 
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force) 
    {
        rigidbody2d.AddForce(direction * force);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null) 
        {
            enemy.Fix();
        }
        Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject);
	}
}
