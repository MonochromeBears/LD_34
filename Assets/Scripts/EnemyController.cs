using UnityEngine;
using System.Collections;

public class EnemyController : UnitController
{

	// Use this for initialization
	void Start()
	{
		_Init();
	}
	
	// Update is called once per frame
	void Update()
	{
		_FixedUpdate();
		bool enemyIsNear = Physics2D.Linecast(transform.position, enemyChecker.position, whatIsEnemy);
		if (!enemyIsNear) {
			_Move();
		}

//		_Move();
	}
}

