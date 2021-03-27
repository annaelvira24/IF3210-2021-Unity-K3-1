using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShop : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public GameObject[] m_ShopLine;
    public Color m_SelectedItemColor;
    public Color m_OwnedItemColor;
    public Color m_CancelColor;
    public bool m_ShopOpened;

    private string m_ActionButton;
    private string m_FireButton;
    private int m_SelectedItem;
    private bool[] m_OwnedItem;
    private int[] m_ItemPrice;
    void Start()
    {
        m_ActionButton = "Action" + m_PlayerNumber;
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ShopOpened = false;
        m_SelectedItem = 1;
        m_OwnedItem = new bool[] {false, true, false, false};
        m_ItemPrice = new int[] { 0, 0, 5, 10 };
    }

    void Update()
    {
        if (!m_ShopOpened && Input.GetButtonDown(m_ActionButton))
        // Open shop UI
        {
            m_ShopOpened = true;

            UpdateSelection();
        }

        else if (m_ShopOpened && Input.GetButtonDown(m_ActionButton))
        // Scroll through shop items
        {

            if (m_SelectedItem < 4)
            {
                m_ShopLine[m_SelectedItem].GetComponent<Text>().color = Color.white;
                m_SelectedItem++;
            }
            else
            {
                m_ShopLine[m_SelectedItem].GetComponent<Text>().color = m_CancelColor;
                m_SelectedItem = 1;
            }

            UpdateSelection();

        }
        else if (m_ShopOpened && Input.GetButtonDown(m_FireButton))
        // Select shop item
        {
            if (m_SelectedItem < 4)
            {
                if (m_OwnedItem[m_SelectedItem])
                {
                    UpdateShellType();
                    CloseShop();
                }
                else if (m_ItemPrice[m_SelectedItem] <= gameObject.GetComponent<TankCash>().cashAmount)
                {
                    gameObject.GetComponent<TankCash>().cashAmount -= m_ItemPrice[m_SelectedItem];
                    gameObject.GetComponent<TankCash>().UpdateCashInfo();
                    m_OwnedItem[m_SelectedItem] = true;
                    UpdateShellType();
                    CloseShop();

                }
            }
            else
            {
                CloseShop();
                m_SelectedItem = 1;
            }
        }
    }

    private void UpdateSelection()
    {
        m_ShopLine[0].GetComponent<Text>().text = "- SWITCH WEAPON -";
        m_ShopLine[1].GetComponent<Text>().text = "NORMAL";
        m_ShopLine[2].GetComponent<Text>().text = "HEAVY";
        m_ShopLine[3].GetComponent<Text>().text = "WIDE";
        m_ShopLine[4].GetComponent<Text>().text = "CANCEL";

        m_ShopLine[m_SelectedItem].GetComponent<Text>().text = "> " + m_ShopLine[m_SelectedItem].GetComponent<Text>().text;
        m_ShopLine[m_SelectedItem].GetComponent<Text>().color = m_SelectedItemColor;

        for (int i = 1; i < 4; i++)
        {
            if (m_OwnedItem[i])
            {
                m_ShopLine[i].GetComponent<Text>().text += " (OWNED)";
                if (m_SelectedItem != i)
                {
                    m_ShopLine[i].GetComponent<Text>().color = m_OwnedItemColor;

                }
            }
            else
            {
                m_ShopLine[i].GetComponent<Text>().text += " - " + m_ItemPrice[i] + " CASH";
            }
        }
    }

    private void UpdateShellType()
    {
        gameObject.GetComponent<TankShooting>().m_shellType = m_SelectedItem - 1;
    }

    private void CloseShop()
    {
        m_ShopOpened = false;

        m_ShopLine[0].GetComponent<Text>().text = "";
        m_ShopLine[1].GetComponent<Text>().text = "";
        m_ShopLine[2].GetComponent<Text>().text = "";
        m_ShopLine[3].GetComponent<Text>().text = "";
        m_ShopLine[4].GetComponent<Text>().text = "";
    }
}
