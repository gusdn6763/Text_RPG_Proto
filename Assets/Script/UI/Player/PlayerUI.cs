using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerStatusUI;
    [SerializeField] private GameObject playerChest;
    [SerializeField] private GameObject playerBag;
    [SerializeField] private GameObject tradUI;

    public void OpenPlayerStatus()
    {
        playerStatusUI.gameObject.SetActive(true);
        playerBag.gameObject.SetActive(true);

        playerChest.gameObject.SetActive(false);
        tradUI.gameObject.SetActive(false);
    }

    public void OpenPlayerChest()
    {
        playerChest.gameObject.SetActive(true);
        playerBag.gameObject.SetActive(true);

        playerStatusUI.gameObject.SetActive(false);
        tradUI.gameObject.SetActive(false);
    }

    public void OpenPlayerInventory()
    {
        playerBag.gameObject.SetActive(true);

        playerStatusUI.gameObject.SetActive(false);
        playerChest.gameObject.SetActive(false);
        tradUI.gameObject.SetActive(false);
    }

    public void OpenTrade()
    {
        playerBag.gameObject.SetActive(true);
        tradUI.gameObject.SetActive(true);

        playerStatusUI.gameObject.SetActive(false);
        playerChest.gameObject.SetActive(false);
    }
}
