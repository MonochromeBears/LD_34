using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : UnitController
{
	public static List<EnemyController> enemies = new List<EnemyController>();
	// Use this for initialization
	
	// Update is called once per frame
	void Update()
	{
		animator.SetBool("isRun", false);
		cooldown -= Time.deltaTime;
    	if (cooldown <= 0) {
			animator.SetBool("isHit", false);
    	}
		if (AllayController.allies.Count > 0) {
			findNearestAlly();
			_FixedUpdate();
			bool enemyIsNear = Physics2D.Linecast(transform.position, enemyChecker.position, whatIsEnemy);
			bool enemy2IsNear = Physics2D.Linecast(transform.position, enemyChecker.position, 1 << LayerMask.NameToLayer("Water"));
			cooldown -= Time.deltaTime;
			if (!enemyIsNear && !enemy2IsNear) {
				Move();
			} else if(cooldown <= 0) {
				Hit();
				Stop();
			}
		} else {
			target = GameController.alySpawn;
			if (Mathf.Abs(target.position.x - transform.position.x) < 1 ) {
				if(cooldown <= 0) {					
					//Hit the mushroom
					GameController.hits++;
					animator.SetBool("isHit", true);
					cooldown = hitSpeed;
				}
			} else {
				_FixedUpdate();
				Move();
			}
		}

//		_Move();
	}

	public override void Start()
	{
		base.Start();
		enemies.Add(this);
	}

	public override void _Death()
	{
		if (EnemyController.enemies.Contains(this)) {
			EnemyController.enemies.Remove(this);
		}
		base._Death();
	}

	void findNearestAlly() {
		AllayController enemyClosest = AllayController.allies[0];
		foreach (AllayController en in AllayController.allies ) {
			if(Dist(en) < Dist(enemyClosest)) {
				enemyClosest = en;
	      	}
	    }
		enemy = enemyClosest;
		target = enemy.transform;
	}
}

