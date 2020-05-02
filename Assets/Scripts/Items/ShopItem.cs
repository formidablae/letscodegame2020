using UnityEngine;
using System.Collections;
using System;
using Apocalypse;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ShopItem : MonoBehaviour
{
    public ShopItemModel Model
    {
        get => model;
        set
        {
            model = value;
            spriteRenderer.sprite = model.Sprite;
        }
    }

    private ShopItemModel model;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnUsed(PlayerStats caller)
    {
        caller.Score += model.IncTimer;
        Destroy(gameObject);
    }
}

public enum ShopItemType
{
    Pasta,
    Carne,
    CartaIgenica,
    Patatine
}
