using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int score = 0;
	public int rages = 0;
	public int builds = 0;
	public int winScore = 1000;
	public int waves = 0;
	public float levelTime = 300f;
	public GameObject allay;
	public GameObject enemy;
	public Transform[] enemySpawns;
	public Transform allaySpawn;

	private float countdown;
	private float[] buttonsState = {0f, 0f};

	// Use this for initialization
	void Start ()
	{
		countdown = levelTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		countdown -= Time.deltaTime;

		float leftBtn = Input.GetAxis("Fire1");
		if (leftBtn != buttonsState[0]) {
			buttonsState[0] = leftBtn;
			if (leftBtn == 1) {
				print("Left Btn pressed");
			}
		}

		float rightBtn = Input.GetAxis("Fire2");
		if (rightBtn != buttonsState[1]) {
			buttonsState[1] = rightBtn;
			if (rightBtn == 1) {
				print("Right Btn pressed");
			}
		}

		if ((levelTime - countdown > waves * 20 ) && EnemyController.enemies.Count < 3) {
			SpawnWave();
		}
	}

	void SpawnWave()
	{
		waves++;
		//1...4
		double difficulty = 1 + 1.0 * (levelTime - countdown) / levelTime + 2.0 * score / winScore;
		int count = Mathf.FloorToInt(3 * (float)difficulty);
		int attack = Mathf.RoundToInt(5 + (float)difficulty);
		int hp = Mathf.RoundToInt(300 * (float)difficulty / count);

		enemy.GetComponent<UnitController>().healthMax = hp;
		enemy.GetComponent<UnitController>().damage = attack;

		for(int i = 0; i < count; i++)
		{
			int gate = Random.Range(0, enemySpawns.Length);
			enemy.transform.position = enemySpawns[gate].position;
			GameObject newEnemy = Object.Instantiate(enemy) as GameObject;
		}
	}

	void RespawnAllies() {
		int maxAllies = 3 + Mathf.RoundToInt(3 * score / winScore);
	}
}

