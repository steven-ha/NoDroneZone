using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // hold current target
    private Enemy target;

    private Tower parentTower;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        TrackTarget();
	}

    public void Initialize(Tower parent)
    {
        this.parentTower = parent;
        this.target = parent.Target;
    }

    // Handles projectile movement towards target
    private void TrackTarget()
    {
        if (target != null && target.isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parentTower.ProjectileSpeed);

            Vector2 direction = target.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
        else if (!target.isAlive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    /// <summary>
    /// Handles Projectile collisions with enemies
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (target.gameObject == collision.gameObject)
            {
                GameManager.Instance.Pool.ReleaseObject(gameObject);

                 //Subtract HP from the target drone, based on the Attacking tower's damage
                target.health = target.health - parentTower.Damage;

                 

            }
        }
    }
}
