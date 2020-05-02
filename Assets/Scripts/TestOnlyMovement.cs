using Apocalypse;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Movement : MonoBehaviour
{
    [HideInInspector]
    public bool justSpawned = false;

    [HideInInspector]
    public Vector3 playerPositionBeforeInternalShop;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float radiusAction;

    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            ShopItem nearerGameItem = GetNearerItem();

            if (nearerGameItem != null)
                nearerGameItem.OnUsed(stats);
        }
    }

    private ShopItem GetNearerItem()
    {
        Collider2D[] collisionWith = new Collider2D[3];
        int collisions = Physics2D.OverlapCircleNonAlloc(transform.position, radiusAction, collisionWith);

        ShopItem nearerGameItem = null;
        float distanceNearer = 0;

        for (int i = 0; i < collisions; i++)
        {
            ShopItem current = collisionWith[i].GetComponent<ShopItem>();
            if (current != null)
            {
                if (nearerGameItem == null)
                {
                    nearerGameItem = current;
                    distanceNearer = Vector2.Distance(transform.position, current.transform.position);
                }
                else
                {
                    float currDist = Vector2.Distance(transform.position, current.transform.position);
                    if (currDist < distanceNearer)
                    {
                        nearerGameItem = current;
                        distanceNearer = currDist;
                    }
                }
            }
        }

        return nearerGameItem;
    }
}
