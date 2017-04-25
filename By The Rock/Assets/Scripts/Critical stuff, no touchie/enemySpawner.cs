using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemySpawner : MonoBehaviour {

    public Transform enemyPre;
    public Transform bossPre;
    List<Transform> enemies;
    Transform boss;

    public int wave = 0;
    int counter;
    int counterMax;
    
    // Use this for initialization
    void Start () {

        enemies = new List<Transform>();
        counterMax = 500;
        addEnemies(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager1.instance.paused) return;
        if (!GameManager1.instance.playerDead && !GameManager1.instance.inShop) counter++;
        if (counter > counterMax)
        {
            addEnemies(5 + 3 * wave);
            counterMax += 50 + 20 * wave;
            wave++;
            counter = 0;
        }
        GameManager1.instance.currentWave = wave;
    }

    void addEnemies(int num)
    {
        if (wave == 10)
        {
            boss = (Transform)Instantiate(bossPre, new Vector3(transform.position.x, transform.position.y, transform.position.z - 6), Quaternion.identity);
        }
        else
        {
            float x = -1.5f * (num / 2);
            for (int i = 0; i < num; i++)
            {
                enemies.Add((Transform)Instantiate(enemyPre, new Vector3(transform.position.x + x, transform.position.y, transform.position.z - 6), Quaternion.identity));
                x += 1.5f;
            }
        }
    }
	
	
}
