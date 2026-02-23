using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// Variables related to player character movement
	public InputAction moveAction;
    public InputAction talkAction;
	Rigidbody2D rigidbody2d;
	Vector2 move;
	public float moveSpeed = 5.0f;

    // Variables related to audio;
    AudioSource audioSource;
    public AudioClip damageClip;
    public AudioClip projectileClip;

	// Variables related to player character animation
	Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

	// Variables related to health system
	public int maxHealth = 5;
	int currentHealth;
	public int health { get { return currentHealth; }}

    // Variables related to invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

	// Variables related to character projectile
	public GameObject projectilePrefab;

	// Start is called before the first frame update
	void Start()
    {
        moveAction.Enable();
        talkAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) 
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
		animator.SetFloat("Look Y", moveDirection.y);
		animator.SetFloat("Speed", move.magnitude);

		if (isInvincible) 
        { 
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0) 
            { 
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            FindNPC();
        }
    }

	// FixedUpdate has the same call rate as the physics system 
	void FixedUpdate()
	{
		Vector2 position = (Vector2)rigidbody2d.position + moveSpeed * Time.deltaTime * move;
		rigidbody2d.MovePosition(position);
	}

    public void PlaySound(AudioClip clip) 
    { 
        audioSource.PlayOneShot(clip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0) 
        {
            if (isInvincible) 
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
			audioSource.PlayOneShot(damageClip);
			animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void FindNPC() 
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null) 
        {
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();

            if (npc != null) 
            {
				UIHandler.instance.DisplayDialogue();
            }
        }
    }

	void Launch() 
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        audioSource.PlayOneShot(projectileClip);
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }
}
