using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	// Public variables
	public float moveSpeed;
	public bool vertical;
	public float changeTime = 3.0f;
	public AudioClip enemyFixed;
	public ParticleSystem smokeEffect;

	// Private variables
	AudioSource audioSource;
	Animator animator;
	Rigidbody2D rigidbody2d;
	float timer;
	int direction = 1;
	bool broken = true;

	// Start is called before the first frame update
	void Start()
    {
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
		timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
		timer -= Time.deltaTime;

		if (timer < 0)
		{
			direction = -direction;
			timer = changeTime;
		}
	}

	// FixedUpdate has the same call rate as the physics system
	void FixedUpdate()
	{
		if (!broken) 
		{
			return;
		}

		Vector2 position = rigidbody2d.position;
		
		if (vertical)
		{
			position.y = position.y + moveSpeed * direction * Time.deltaTime;
			animator.SetFloat("Move X", 0);
			animator.SetFloat("Move Y", direction);
		}
		else 
		{
			position.x = position.x + moveSpeed * direction * Time.deltaTime;
			animator.SetFloat("Move X", direction);
			animator.SetFloat("Move Y", 0);
		}

		rigidbody2d.MovePosition(position);

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		PlayerController player = other.gameObject.GetComponent<PlayerController>();

		if (player != null)
		{
			player.ChangeHealth(-1);
		}
	}

	public void Fix() 
	{
		audioSource.Stop();
		animator.SetTrigger("Fixed");
		audioSource.PlayOneShot(enemyFixed);
		smokeEffect.Stop();
		broken = false;
		rigidbody2d.simulated = false;
	}
}
