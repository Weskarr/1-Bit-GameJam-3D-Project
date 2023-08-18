using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpawn : MonoBehaviour
{
    public List<GameObject> tools;
    public GameObject spawnPoints;
    public Transform spawnParent;

    GameObject[] _spawnPoints;

    List<GameObject> spawned = new List<GameObject>();
    Dictionary<GameObject, GameObject> spawnedLitObj = new Dictionary<GameObject, GameObject>();

    private void Start() {
        List<GameObject> s = new List<GameObject>();
        foreach(Transform child in spawnPoints.transform) {
            s.Add(child.gameObject);
        }
        _spawnPoints = s.ToArray();
    }

    public void SpawnTools(int count) {
        if (count > tools.Count) {
            Debug.LogError("Attempting to spawn more tools than available! Aborting");
            return;
        }
        int[] spawnInds = RandomRangeUnique(0, 15, count);
        for(int i = 0; i < count; i++){
            Debug.Log(spawnInds[i] + 1);
            GameObject go = Instantiate(tools[i]);
            GameObject spawnPoint = _spawnPoints[spawnInds[i]];
            go.transform.position = spawnPoint.transform.position;
            go.transform.rotation = spawnPoint.transform.rotation;
            go.transform.parent = spawnParent;
            
            if (spawnPoint.TryGetComponent<SpawnDoor>(out SpawnDoor door)) {
                EchoManager.instance.AddAlwaysLitObject(door.Door);
                spawnedLitObj[go] = door.Door;
            }

            AddTool(go);
        }
    }

    public void AddTool(GameObject tool) {
        spawned.Add(tool);
    }

    public void RemoveTool(GameObject tool) {
        if (spawnedLitObj.ContainsKey(tool)) {
            EchoManager.instance.RemoveAlwaysLitObject(spawnedLitObj[tool]);
            spawnedLitObj.Remove(tool);
        }
        spawned.Remove(tool);
    }
    

    public static int[] RandomRangeUnique(int min, int max, int count) {
        if (Mathf.Abs(min - max) <= count) {
            Debug.LogError("Range must be greater than count!");
            return null;
        }
        List<int> numbers = new List<int>();
        for(int i = 0; i < count; i++) {
            int num = UnityEngine.Random.Range(min, max);
            while (numbers.Contains(num)) {
                num = UnityEngine.Random.Range(min, max);
            }
            numbers.Add(num);
        }
        return numbers.ToArray();
    }
}
