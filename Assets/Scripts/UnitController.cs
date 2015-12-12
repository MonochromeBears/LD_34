using UnityEngine;
using System.Collections;

public abstract class UnitController : MonoBehaviour {

	public int healthMax = 100;
	public int damage = 10;
	public float smoothTime = 0.1f;
	public float speed = 5.0f;
	public UnitController enemy;
	public Transform enemyChecker;
	public LayerMask whatIsEnemy;

	protected int _health;
	protected bool _facingRight = false;
	protected Rigidbody2D _rigidbody2D;

	private void _Flip() {
		_facingRight = !_facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public virtual void TakeADamage(int dmg) {
		_health -= dmg;
	}
	
	public virtual void _Death() {
		DestroyObject(this.gameObject);
		if (AllayController.allies.Contains(this)) {
			AllayController.allies.Remove(this);
		}
	}
	
	protected virtual void _Init() {
		enemyChecker = transform.Find("enemyChecker");
		_health = healthMax;
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	protected virtual void _FixedUpdate() {
		_updateDirection();
	}
	
	protected virtual void _Move() {
		float usedSpeed = _facingRight ? speed : speed * -1;
		_rigidbody2D.velocity = new Vector2(usedSpeed, _rigidbody2D.velocity.y);
	}
	
	protected void _updateDirection() {
		bool enemyAtRight = enemy.transform.position.x >= transform.position.x;
		if (enemyAtRight != _facingRight) {
			_Flip();
		}
	}
}
