using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCash : MonoBehaviour
{
    public GameObject cashObject;
    [HideInInspector] public int xPos;
    public int yPos;
    [HideInInspector] public int zPos;
    [HideInInspector] public int cashCount;
    public int maxCashCount;

    void Start()
    {
        StartCoroutine(CashDrop());
    }
    
    IEnumerator CashDrop()
    {
        while (cashCount < maxCashCount)
        {
            xPos = Random.Range(-40, 41);
            zPos = Random.Range(-40, 31);
            Instantiate(cashObject, new Vector3(xPos, yPos, zPos), cashObject.transform.rotation);
            yield return new WaitForSeconds(2f);
            cashCount += 1;
        }

;    }
}
