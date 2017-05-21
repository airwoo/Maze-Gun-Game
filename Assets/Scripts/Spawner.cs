using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;				//reference to enemy so we can spawn it into the game	

	Wave currentWave;				//reference to our current wave
	int currentWaveNumber;


	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;

	void Start(){
		NextWave ();
	}

	void Update(){
		if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

			Enemy spawnedEnemy = Instantiate (enemy, Vector3.zero, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}

	void OnEnemyDeath(){
		enemiesRemainingAlive--;

		if (enemiesRemainingAlive == 0) {
			NextWave ();
		}
	}

	void NextWave(){
		currentWaveNumber++;
		print ("Wave: " + currentWaveNumber);
		if (currentWaveNumber - 1 < waves.Length) {
			currentWave = waves [currentWaveNumber - 1];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;
		}
	}

	[System.Serializable]			//need this so we can see waves in the inspector

	public class Wave {
		public int enemyCount;
		public float timeBetweenSpawns;

	}
}
