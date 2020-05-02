using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ShopTimer))]
public class Shop : MonoBehaviour
{
    public List<ShopItemModel> ItemsShop { get; private set; }

    private bool isClosed;
    private bool needRefresh;

    private void Awake()
    {
        isClosed = false;
        needRefresh = true;
        ItemsShop = new List<ShopItemModel>();
        GetComponent<ShopTimer>().owner = this;
    }

    public void OnShopTimerCompleted()
    {
        isClosed = true;
    }

    public void OnShopTimerRestart()
    {
        isClosed = false;
        needRefresh = true;
    }

    private void RefreshItems()
    {
        ShopItemManager.Instance.PickRandomItems(0, 20, ItemsShop);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement player = collision.gameObject.GetComponent<Movement>();

        if (player != null)
        {
            if(isClosed)
            {
                Debug.Log("Shop closed, try again later");
                return;
            }

            if(player.justSpawned)
            {
                player.justSpawned = false;
                return;
            }

            if(needRefresh)
                RefreshItems();

            SceneManager.LoadScene("InternalShop", LoadSceneMode.Additive);
            SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetSceneByName("InternalShop"));

            player.justSpawned = true;
            player.playerPositionBeforeInternalShop = player.transform.position;
            StartCoroutine(SetupPlayerInternalShop(player.gameObject));
        }
    }

    private IEnumerator SetupPlayerInternalShop(GameObject player)
    {
        yield return null;
        GameObject.FindGameObjectWithTag("CityCamera").GetComponent<AudioListener>().enabled = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("InternalShop"));
        InternalShop internalShop = FindObjectOfType<InternalShop>();
        player.gameObject.transform.position = internalShop.GetSpawnPointPosition;
        internalShop.Owner = this;
        internalShop.SetupShop();
    }
}
