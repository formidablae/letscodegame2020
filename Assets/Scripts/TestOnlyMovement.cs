using Apocalypse;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Rigidbody2D))]
public class TestOnlyMovement : MonoBehaviour
{
    [HideInInspector]
    public bool justSpawned = false;

    [HideInInspector]
    public Vector3 playerPositionBeforeInternalShop;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float radiusAction;

    private Rigidbody2D rb2D;
    private PlayerStats stats;
    private Vector2 velocity;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        velocity = rb2D.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            velocity.y = speed;
        } else if (Input.GetKey(KeyCode.S))
        {
            velocity.y = -speed;
        } else
        {
            velocity.y = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
        } else if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
        } else
        {
            velocity.x = 0;
        }

        rb2D.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShopItem nearerGameItem = GetNearerItem();

            if (nearerGameItem != null)
                nearerGameItem.OnUsed(stats);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAction);
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
