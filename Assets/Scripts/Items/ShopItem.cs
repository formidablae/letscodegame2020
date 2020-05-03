using UnityEngine;
using Apocalypse;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ShopItem : MonoBehaviour
{
    public bool NeedChangeSprite => CompareTag("FridgeItem") || CompareTag("FrozenItem");
    private ShopItemModel model;

    [Header("Fridge and Forzen Only")]
    [SerializeField]
    private Sprite spriteItemEmpty;
    [SerializeField]
    private Sprite spriteItemAvailable;

    private bool isEmpty = true;

    public void Setup(ShopItemModel model)
    {
        this.model = model;
        isEmpty = false;

        if(NeedChangeSprite)
        {
            GetComponent<SpriteRenderer>().sprite = spriteItemAvailable;
        }
    }

    public void OnUsed(PlayerStats caller)
    {
        if (isEmpty)
            return;

        UIManager.Instance.NotifyItemTaken(model);
        caller.Score += model.IncTimer;

        if(NeedChangeSprite)
        {
            isEmpty = true;
            GetComponent<SpriteRenderer>().sprite = spriteItemEmpty;
        } else
        {
            Destroy(gameObject);
        }
    }
}

public enum ShopItemType
{
    Pasta,
    Carne,
    CartaIgenica,
    Patatine
}
