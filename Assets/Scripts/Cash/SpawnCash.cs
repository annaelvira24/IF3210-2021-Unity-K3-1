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
            xPos = Random.Range(-40, 41);
            zPos = Random.Range(-40, 31);
            objectPooler.SpawnFromPool("cash", new Vector3(xPos, yPos, zPos), cashObject.transform.rotation);
//            Instantiate(cashObject, new Vector3(xPos, yPos, zPos), cashObject.transform.rotation);
            yield return new WaitForSeconds(2f);
            cashCount += 1;
        }

;    }
}
