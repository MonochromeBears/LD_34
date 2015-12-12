using UnityEngine;
using System.Collections;

public class EnemyController : UnitController
{

	// Use this for initialization
	public override void Start()
	{
		base.Start();
	}
	
	// Update is called once per frame
	void Update()
	{
		_FixedUpdate();
		bool enemyIsNear = Physics2D.Linecast(transform.position, enemyChecker.position, whatIsEnemy);
		cooldown -= Time.deltaTime;
		print (cooldown);
		if (!enemyIsNear) {
			Move();
		} else if(cooldown <= 0) {
			Hit();
		}

//		_Move();
	}
}

