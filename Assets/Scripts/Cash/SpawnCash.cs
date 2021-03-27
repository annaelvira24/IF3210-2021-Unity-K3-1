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

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        StartCoroutine(CashDrop());
    }
    
    IEnumerator CashDrop()
    {
        while (cashCount < maxCashCount)
        {
            xPos = Random.Range(-38, 39);
            zPos = Random.Range(-38, 29);
            objectPooler.SpawnFromPool("cash", new Vector3(xPos, yPos, zPos), cashObject.transform.rotation);
            yield return new WaitForSeconds(2f);
            cashCount += 1;
        }

;    }
}
