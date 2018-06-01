using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private string towerName;

    private Enemy target;

    public Enemy Target
    {
        get { return target; }
    }

    private Queue<Enemy> enemyList = new Queue<Enemy>();

    private bool canAttack = true;

    //[SerializeField]
    private float attackSpeed;

    private float attackTimer;

    // Holds tower projectile type
    //[SerializeField]
    private string projectileType;

    //[SerializeField]
    private float projectileSpeed;

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    //[SerializeField]
    private float damage;     

    public float Damage
    {
        get
        {
            return damage;
        }
    }
    public bool isSelected { get; set; }

    private Animator animator;

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = transform.parent.GetComponent<Animator>();
        towerName = this.transform.parent.gameObject.name;

		//keep track of the towers purchased on the scene
		if (towerName == "SlingshotTower(Clone)") {
			SceneController.Instance.purSlingshot += 1;
		}
		if (towerName == "SupersoakerTower(Clone)") {
			SceneController.Instance.purWater += 1;
		}
		if (towerName == "RifleTower(Clone)") {
			SceneController.Instance.purShotgun += 1;
		}

        // load tower data from tower file
        TextAsset towerData = Resources.Load(towerName) as TextAsset;
        string[] value = towerData.text.Split(' ');
        this.attackSpeed = float.Parse(value[0]);
        this.projectileType = value[1];
        this.projectileSpeed = float.Parse(value[2]);
        this.damage = float.Parse(value[3]);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Attack();
        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.parent.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Used to select or deselect a tower
    /// </summary>
    public void SelectTower()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
        isSelected = spriteRenderer.enabled;
    }
          
    /// <summary>
    /// Handles targetting enemy
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemyList.Enqueue(collision.GetComponent<Enemy>());
        }
    }

    /// <summary>
    /// Handles target leaving tower range
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            target = null;
        }
    }

    /// <summary>
    /// Handles tower attacks, checks if tower is able to fire again
    /// and if target is present. 
    /// </summary>
    private void Attack()
    {
        // checks if enough time has passed to allow tower to fire again
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        // gets new target if no target
        if (target == null && enemyList.Count > 0)
        {
            target = enemyList.Dequeue();
        }

        // checks if target is present and alive, then fires at target
        // handles tower moving to face target
        if (target != null && target.isAlive)
        {
            if (canAttack)
            {
                Fire();

                // rotate tower to follow target
                Vector2 direction = target.transform.position - transform.position;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                canAttack = false;
            }
        }
    }

    /// <summary>
    /// Handles firing at enemy
    /// </summary>
    private void Fire()
    {
        // create projectile at tower location 
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;

        projectile.Initialize(this);
    }

}
