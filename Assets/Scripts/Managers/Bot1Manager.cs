using System;
using UnityEngine;

[Serializable]
public class Bot1Manager
{       
    [HideInInspector] public int m_BotNumber;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public GameObject m_TargetTank;
    [HideInInspector] public GameObject m_Instance;                        

    private Bot1Movement m_Movement;       
    private BotShooting m_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<Bot1Movement>();
        m_Shooting = m_Instance.GetComponent<BotShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_Shooting.m_anim = m_Instance.GetComponent<Animator>();
        m_Movement.m_anim = m_Instance.GetComponent<Animator>();

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Movement.m_TargetTank = m_TargetTank;

        Debug.Log(m_PlayerNumber);

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = (Color.white);
        }
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }
}
