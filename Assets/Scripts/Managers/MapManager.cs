using System;
using UnityEngine;

[Serializable]
public class MapManager
{
    public int m_MapNumber;            
    public Transform m_SpawnPoint;
    [HideInInspector] public GameObject m_Instance;                         


    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

    }


}
