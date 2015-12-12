using UnityEngine;
using System.Collections;

public abstract class UnitController : MonoBehaviour {

	public int healthMax = 100;
	public int damage = 10;
	public float hitSpeed = 0.5f;
	public float speed = 5.0f;
	public UnitController enemy;
	public LayerMask whatIsEnemy;
	public bool facingRight = false;

	protected int _health;
	protected Rigidbody2D _rigidbody2D;
	protected float cooldown = 0;
	protected Transform enemyChecker;
	
	public void Hit() {
		enemy.TakeADamage(damage);
		cooldown = hitSpeed;
	}
	
	public void Move() {
		float usedSpeed = facingRight ? speed : speed * -1;
		_rigidbody2D.velocity = new Vector2(usedSpeed, _rigidbody2D.velocity.y);
	}

	private void _Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public virtual void TakeADamage(int dmg) {
		_health -= dmg;
	}
	
	public virtual void _Death() {
		DestroyObject(this.gameObject);
	}
	
	public virtual void Start() {
		enemyChecker = transform.Find("enemyChecker");
		_health = healthMax;
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	protected virtual void _FixedUpdate() {
		if(_health <= 0) {
			_Death();
			return;
		}
		_updateDirection();
	}
	
	protected virtual void _updateDirection() {
		bool enemyAtRight = enemy.transform.position.x >= transform.position.x;
		if (enemyAtRight != facingRight) {
			_Flip();
		}
	}
	
	protected float Dist(UnitController other) {
		return Mathf.Abs(other.transform.position.x - transform.position.x);
	}
}
