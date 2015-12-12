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
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
	{
		if (order != Order.BUILD) {
			if (EnemyController.enemies.Count > 0 ) {
				FindTargetEnemy();

				_FixedUpdate();

				bool enemyIsNear = Physics2D.Linecast(transform.position, enemyChecker.position, whatIsEnemy);
				cooldown -= Time.deltaTime;
				if (!enemyIsNear) {
					Move();
				} else if(cooldown <= 0) {
					Hit();
				}
			}
		}
	}

	//for buttons usage
	public static AllayController SelectFreeAlly() {
		return allies.Find(a => a.order == Order.NONE);
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
	}
	public override void Start ()
	{
		base.Start();
		order = Order.NONE;
		allies.Add(this);
	}
	
	public override void _Death()
	{
		if (AllayController.allies.Contains(this)) {
			AllayController.allies.Remove(this);
		}
		base._Death();
	}
}

