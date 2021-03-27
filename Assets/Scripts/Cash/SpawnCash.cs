using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCash : MonoBehaviour
{
    ObjectPooler objectPooler;
    public GameObject cashObject;
    [HideInInspector] public int xPos;
    public int yPos;
    [HideInInspector] public int zPos;
    [HideInInspector] public int cashCount;
    public int maxCashCount;
    public bool enableSpawning;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        enableSpawning = false;
        StartCoroutine(CashDrop());
    }
    
    IEnumerator CashDrop()
    {
        while (cashCount < maxCashCount)
        {
            if (enableSpawning)
            {
                xPos = Random.Range(-38, 39);
                zPos = Random.Range(-38, 29);
                objectPooler.SpawnFromPool("cash", new Vector3(xPos, yPos, zPos), cashObject.transform.rotation);
                cashCount += 1;
            }
            yield return new WaitForSeconds(2f);
        }

;    }
}
