using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllayController : UnitController
{
	public enum Order {
		NONE,
		BUILD,
		ATTACK
	}
	public static List<AllayController> allies = new List<AllayController>();	Order order;
	public float orderTimeLeft;
	public bool isBuilding = false;
	public Transform heal;
	public Transform rage;
	public AudioSource berserkSound;

	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
	{
		animator.SetBool("isRun", false);
		cooldown -= Time.deltaTime;
    	if (cooldown <= 0) {
			animator.SetBool("isHit", false);
		}
		if (order != Order.NONE) {
			orderTimeLeft -= Time.deltaTime;
			if (orderTimeLeft < 0 ) {
				if (order == Order.ATTACK) {
					damage /= 2;
					rage.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
				}
				if (order == Order.BUILD) {
					heal.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
					CollisionsOn();
					isBuilding = false;
				}
				order = Order.NONE;
			}
		}
		if (order != Order.BUILD) {
			if (EnemyController.enemies.Count > 0 ) {
				CollisionsOn();
				FindTargetEnemy();

				_FixedUpdate();

				bool enemyIsNear = Physics2D.Linecast(transform.position, enemyChecker.position, whatIsEnemy);
				if (!enemyIsNear) {
					Move();
				} else if(cooldown <= 0) {
					Hit();
					Stop();
				}
			} else {
				if (order == Order.ATTACK) {
					CollisionsOff();
					enemy = null;
					target = null;
					_FixedUpdate();
					Move();
				}
			}
		} else {
			if (!isBuilding) {
				_FixedUpdate();
				Move();
				if (Mathf.Abs(transform.position.x - target.position.x) < 1) {
					isBuilding = true;
				}
			} else {
				Stop();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		_Death();
	}
	
	//for buttons usage
	public static AllayController SelectFreeAlly() {
		return allies.Find(a => a.order == Order.NONE);
	}

	//issue an order
	public void changeOrder(Order ord, int duration) {
		this.order = ord;
		this.orderTimeLeft = duration;
		if (ord == Order.ATTACK) {
			this.damage *= 2;
			rage.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
//			berserkSound.Play();
		}
		if (ord == Order.BUILD) {
			heal.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			CollisionsOff();
			target = GameController.alySpawn.transform;
		}
		isBuilding = false;
	}

	//set enemy value
	void FindTargetEnemy() {
		int alliesLeft = allies
			.FindAll(ally => ally.transform.position.x <= transform.position.x)
			.FindAll(ally => ally.order != Order.BUILD)
			.Count;
		int alliesRight = allies
			.FindAll(ally => ally.transform.position.x >= transform.position.x)
			.FindAll(ally => ally.order != Order.BUILD)
			.Count;
		EnemyController enemyClosest = EnemyController.enemies[0];
		foreach (EnemyController en in EnemyController.enemies ) {
			if (alliesLeft == alliesRight ) {
				if(Dist(en) < Dist(enemyClosest)) {
					enemyClosest = en;
				}
			} else if (alliesLeft > alliesRight ) {
				if (en.transform.position.x >= transform.position.x) {
					if ((Dist(en) < Dist(enemyClosest)) || (enemyClosest.transform.position.x < transform.position.x)) {
						enemyClosest = en;
					}
				}
			} else if (alliesLeft < alliesRight ) {				
				if (en.transform.position.x < transform.position.x) {
					if ((Dist(en) < Dist(enemyClosest)) || (enemyClosest.transform.position.x >= transform.position.x)) {
						enemyClosest = en;
					}
				}
			}
		}
		enemy = enemyClosest;
		target = enemy.transform;
	}
	public override void Start ()
	{
		base.Start();
		order = Order.NONE;
		allies.Add(this);
		heal.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
		rage.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
	}
	
	public override void _Death()
	{
		if (AllayController.allies.Contains(this)) {
			AllayController.allies.Remove(this);
		}
		base._Death();
	}
	
	public override void TakeADamage(int dmg) {
		if (order == Order.BUILD) {
			dmg = dmg * 2;
		} else if(order == Order.ATTACK) {
			dmg = Mathf.CeilToInt((float) 0.8 * dmg);
		}

		base.TakeADamage(dmg);
	}
}

