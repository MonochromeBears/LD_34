using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int score = 0;
	public int rages = 0;
	public int builds = 0;
	public int winScore = 500;
	public int waves = 0;
	public float levelTime = 300f;
	public float enemyRespawnTime = 1f;
	public float allayRespawnTime = 2f;
	public float respawnLock = 0f;
	public GameObject allay;
	public GameObject enemy;
	public Transform[] enemySpawns;
	public Transform allaySpawn;
	public Transform[] bigMushroom;
	public Transform[] buildingsAndShit;
	public int currentMushroom = -1;
	public int currentBuildings = -1;
	
	private int enemiesToSpawnLeft = 0;
	private float countdown;
	private float enemyRespawnCountdown;
	private float allayRespawnCountdown;
	private float[] buttonsState = {0f, 0f};

	public static Transform alySpawn;
	public static int hits = 0;
	// Use this for initialization
	void Start ()
	{
		countdown = levelTime;
		allayRespawnCountdown = allayRespawnTime;
		enemyRespawnCountdown = enemyRespawnTime;
		alySpawn = allaySpawn;
		foreach (Transform trans in bigMushroom ) {
			trans.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
		}
		foreach (Transform trans in buildingsAndShit ) {
			trans.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
		}
		currentMushroom = -1;
		currentBuildings = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (score >= winScore) {
			Application.LoadLevel(0);
		}
		if (countdown <= 0) {
			Application.LoadLevel(0);
		}
		countdown -= Time.deltaTime;
		allayRespawnCountdown -= Time.deltaTime;


		float leftBtn = Input.GetAxis("Fire1");
		if (leftBtn != buttonsState[0]) {
			buttonsState[0] = leftBtn;
			if (leftBtn == 1) {
				AllayController ally = AllayController.SelectFreeAlly();
				if (ally != null ) {
					ally.changeOrder(AllayController.Order.BUILD, 10);
				}
			}
		}

		float rightBtn = Input.GetAxis("Fire2");
		if (rightBtn != buttonsState[1]) {
			buttonsState[1] = rightBtn;
			if (rightBtn == 1) {
				AllayController ally = AllayController.SelectFreeAlly();
				if (ally != null ) {
					ally.changeOrder(AllayController.Order.ATTACK, 30);
					rages++;
				}
			}
		}

		if ((levelTime - countdown > waves * 20 ) && EnemyController.enemies.Count < 3) {
			SpawnWave();
		}
		EnemySpawner();

		if (allayRespawnCountdown <= 0) {
			RespawnAllies();
			allayRespawnCountdown = allayRespawnTime;
			//TODO: move somewhere else
			//buidling - score calc
			AllayController.allies.ForEach(ally => {
				if (ally.isBuilding) {
					score += 1;
				}
			});
		}

		//mushroom health
		if (hits > 0 ) {
			hits -= 1;
			if (score > 0) {
				score -= 1;
			}
		}

		updateVillage();
	}

	//score
	void updateVillage() {
		int targetMushroom = Mathf.FloorToInt((float)((bigMushroom.Length - 1) * score) / winScore);
		if (currentMushroom < targetMushroom ) {
			if (currentMushroom >= 0) {
				bigMushroom[currentMushroom].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
			}
			currentMushroom = targetMushroom;
			bigMushroom[currentMushroom].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
		int targetBuildings = Mathf.RoundToInt((float)((buildingsAndShit.Length - 1) * score) / winScore);
		if (currentBuildings < targetBuildings) {
			currentBuildings = targetBuildings;
			buildingsAndShit[currentBuildings].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
	}

	void SpawnWave()
	{
		waves++;
		//1...4
		double difficulty = 1 + 1.0 * (levelTime - countdown) / levelTime + 2.0 * score / winScore;
		int count = Mathf.FloorToInt(3 * (float)difficulty);
		enemiesToSpawnLeft += count;
		int attack = Mathf.RoundToInt(3 + (float)difficulty);
		int hp = Mathf.RoundToInt(300 * (float)difficulty / count);

		enemy.GetComponent<UnitController>().healthMax = hp;
		enemy.GetComponent<UnitController>().damage = attack;
	}

	void EnemySpawner() {
		enemyRespawnCountdown -= Time.deltaTime;
		bool needSpawn = enemiesToSpawnLeft > 0 && enemyRespawnCountdown <= 0;
		if (needSpawn) {
			int gate = Random.Range(0, enemySpawns.Length);
			enemy.transform.position = enemySpawns[gate].position;
			GameObject newEnemy = Object.Instantiate(enemy) as GameObject;
			enemyRespawnCountdown = enemyRespawnTime;
			enemiesToSpawnLeft--;
		}
	}

	void RespawnAllies()
	{
		int maxAllies = 3 + Mathf.RoundToInt(3 * score / winScore);
		if (AllayController.allies.Count < maxAllies) {			
			if (respawnLock > 0 ) {
				respawnLock -= 2;
			} else {
				allay.transform.position = allaySpawn.position;
				allay.GetComponent<UnitController>().healthMax = 100;
				allay.GetComponent<UnitController>().damage = 20 + rages;
				GameObject newAlly = Object.Instantiate(allay) as GameObject;
			}
		} else {
			respawnLock = 16f;	//respawn start delay
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect (0, 0, 200, 25), "Left time: " + (countdown / 60));
		GUI.Box(new Rect (0, 25, 200, 25), "Score: " + (score));
	}
}

