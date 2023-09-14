using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] FloorPrefabs;
    // Start is called before the first frame update
    public void SpawnFloor()
    {
        int r = Random.Range(0, FloorPrefabs.Length);
        //隨機從0到這個陣列長度之間挑選一個數字存進r
        GameObject floor = Instantiate(FloorPrefabs[r], transform);
        floor.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), -6f, 0f);
    }
}
