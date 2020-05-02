using UnityEngine;

[System.Serializable]
public class ShopItemModel
{
    public int IncTimer => incTimer;
    public ShopItemType Type => type;
    public Sprite Sprite => sprite;

    [SerializeField]
    private int incTimer;
    [SerializeField]
    private ShopItemType type;
    [SerializeField]
    private Sprite sprite;
}
