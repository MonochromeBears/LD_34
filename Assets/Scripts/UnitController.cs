using UnityEngine;
using System.Collections;

public abstract class UnitController : MonoBehaviour {

	public int healthMax = 100;
	public int damage = 10;
	public float hitSpeed = 0.5f;
	public float speed = 5.0f;
	public UnitController enemy;
	public Transform target;
	public LayerMask whatIsEnemy;
	public bool facingRight = false;
	public Animator animator;

	protected int _health;
	protected Rigidbody2D _rigidbody2D;
	protected float cooldown = 0;
	protected Transform enemyChecker;
	private int baseLayer;

	
	public void Hit() {
		enemy.TakeADamage(damage);
		cooldown = hitSpeed;
		animator.SetBool("isHit", true);
	}
	
	public void Move() {
		float usedSpeed = facingRight ? speed : speed * -1;
		bool pathIsBlocked = Physics2D.Linecast(enemyChecker.position, enemyChecker.position, 1 << gameObject.layer);
		if (!pathIsBlocked) {
			_rigidbody2D.velocity = new Vector2(usedSpeed, _rigidbody2D.velocity.y);
			animator.SetBool("isRun", true);
    }
	}

	public void CollisionsOn() {
		gameObject.layer = baseLayer;
	}

	public void CollisionsOff() {
		gameObject.layer = 4;
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
		baseLayer = gameObject.layer;
		enemyChecker = transform.Find("enemyChecker");
		_health = healthMax;
		_rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	protected virtual void _FixedUpdate() {
		if(_health <= 0) {
			_Death();
			return;
		}
		_updateDirection();
	}
	
	protected virtual void _updateDirection() {
		if ( target != null ) {
			bool targetAtRight = target.position.x >= transform.position.x;
			if (targetAtRight != facingRight) {
				_Flip();
			}
		}
	}
	
	protected float Dist(UnitController other) {
		return Mathf.Abs(other.transform.position.x - transform.position.x);
	}
}
