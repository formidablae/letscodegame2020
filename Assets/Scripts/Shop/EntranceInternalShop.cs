using Apocalypse;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceInternalShop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            if (player.justSpawned)
            {
                player.justSpawned = false;
                return;
            }

            player.justSpawned = true;
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
}
