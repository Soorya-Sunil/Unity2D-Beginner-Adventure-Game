using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int amount;

	void OnTriggerEnter2D(Collider2D other)
	{
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth) 
        {
			controller.ChangeHealth(amount);
			Destroy(gameObject);
		}
	}
}
