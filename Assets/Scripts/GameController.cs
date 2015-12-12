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

	// Use this for initialization
	void Start ()
	{
		SpawnWave();
		countdown = levelTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		countdown -= Time.deltaTime;
	}

	void SpawnWave()
	{
		waves++;
		//1...4
		double difficulty = 1 + 1.0 * (levelTime - countdown) / levelTime + 2.0 * score / winScore;
		int count = Mathf.FloorToInt(3 * (float)difficulty);
		int attack = Mathf.RoundToInt(5 + (float)difficulty);
		int hp = Mathf.RoundToInt(150 * (float)difficulty / count);

		enemy.GetComponent<UnitController>().healthMax = hp;
		enemy.GetComponent<UnitController>().damage = attack;

		for(int i = 0; i < count; i++)
		{
			int gate = Random.Range(0, enemySpawns.Length);
			enemy.transform.position = enemySpawns[gate].position;
			GameObject newEnemy = Object.Instantiate(enemy) as GameObject;
		}
	}
}

