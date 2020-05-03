using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Apocalypse;

[RequireComponent(typeof(ShopTimer))]
public class Shop : MonoBehaviour
{
    public uint Id => id;
    public List<ShopItemModel> ItemsShop { get; private set; }

    [SerializeField]
    private uint id;
    public Transform outsideSpawnPoint;

    private bool isClosed;
    private bool needRefresh;
    public bool playerInside = false;

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
        EntranceInternalShop.ForcePlayerOut(this);
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
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            if(isClosed)
            {
                UIManager.Instance.PushNotification("Il negozio è chiuso, riprova pià tardi.");
                return;
            }

            playerInside = true;

            if (needRefresh)
                RefreshItems();

            SceneManager.LoadScene("InternalShop", LoadSceneMode.Additive);
            SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetSceneByName("InternalShop"));
            player.playerPositionBeforeInternalShop = outsideSpawnPoint.position;
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
