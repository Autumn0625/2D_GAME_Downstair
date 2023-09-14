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
        //�H���q0��o�Ӱ}�C���פ����D��@�ӼƦr�s�ir
        GameObject floor = Instantiate(FloorPrefabs[r], transform);
        floor.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), -6f, 0f);
    }
}
