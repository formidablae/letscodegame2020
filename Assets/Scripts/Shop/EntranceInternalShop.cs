using Apocalypse;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceInternalShop : MonoBehaviour
{
    public static EntranceInternalShop Current;
    public InternalShop internalShop;

    private void Awake()
    {
        if (Current != null)
            Destroy(gameObject);

        Current = this;
        internalShop = GetComponentInParent<InternalShop>();
    }

    private void OnDestroy()
    {
        Current = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetSceneByName("ShopScene"));
            player.transform.position = player.playerPositionBeforeInternalShop;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()).completed += OnInternalShopSceneUnloaded;
        }
    }

    private void OnInternalShopSceneUnloaded(AsyncOperation obj)
    {
        GameObject.FindGameObjectWithTag("CityCamera").GetComponent<AudioListener>().enabled = true;
        Scene city = SceneManager.GetSceneByName("ShopScene");
        SceneManager.SetActiveScene(city);
    }

    public static void ForcePlayerOut(Shop caller)
    {
        if (Current == null || Current.internalShop.Owner != caller)
            return;

        Player player = FindObjectOfType<Player>();

        if (player == null)
            return;

        player.justSpawned = true;
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetSceneByName("ShopScene"));
        player.transform.position = player.playerPositionBeforeInternalShop;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()).completed += Current.OnInternalShopSceneUnloaded;
        UIManager.Instance.PushNotification("Il negozio ha appena chiuso, vai in un altro.");
    }
}
