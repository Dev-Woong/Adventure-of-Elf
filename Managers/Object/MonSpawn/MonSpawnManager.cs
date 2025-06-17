using System.Collections;
using UnityEditor;
using UnityEngine;

public class MonSpawnManager : MonoBehaviour
{
    public static int monCount = 0;
    public float monSpawnTime = 0;
   // public string monName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Update()
    {
        monSpawnTime += Time.deltaTime;
        if (monSpawnTime >= 0.5f&&monCount<=6&&GameManager.Instance.SceneNum==0)
        {
            Vector3 spawnWolfPosition = new Vector3(Random.Range(2f, 4f), -2.5f, 0);
            var wolfSpawn = MonsterPoolManager.instance.GetObject("Monster_Wolf");
            wolfSpawn.transform.position = spawnWolfPosition;
            monCount++;
            monSpawnTime = 0;   
        }
        if (monSpawnTime >= 3f && monCount <= 6 && GameManager.Instance.SceneNum == 1)
        {
            Vector3 spawnPinkPosition = new Vector3(Random.Range(2f, 4f), -2.8f, 0);
            var pinkSpawn = MonsterPoolManager.instance.GetObject("Monster_Pink");
            pinkSpawn.transform.position = spawnPinkPosition;
            monCount++;
            monSpawnTime = 0;
        }
    }
}
