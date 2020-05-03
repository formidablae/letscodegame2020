using System;
using System.Collections.Generic;
using UnityEngine;

public class InternalShop : MonoBehaviour
{
    public Vector3 GetSpawnPointPosition => spawnPosition.position;

    public Shop Owner { get; set; }

    [SerializeField]
    private Transform spawnPosition;
    private ShopItem[] availableItemSpot;

    private void Awake()
    {
        availableItemSpot = GetComponentsInChildren<ShopItem>(true);

        for(int i = 0; i < availableItemSpot.Length; i++)
        {
            if(!availableItemSpot[i].NeedChangeSprite)
                availableItemSpot[i].gameObject.SetActive(false);
        }
    }

    public void SetupShop()
    {
        List<int> indexAlreadyUsed = new List<int>();

        for(int i = 0; i < Owner.ItemsShop.Count; i++)
        {
            int index;

            try
            {
                index = GenerateUntilUnique();
            } catch (StackOverflowException)
            {
                break;
            }

            ShopItem curr = availableItemSpot[index];

            if(!curr.NeedChangeSprite)
                curr.gameObject.SetActive(true);

            curr.Setup(Owner.ItemsShop[i]);
        }
        
        int GenerateUntilUnique()
        {
            int currIndex = UnityEngine.Random.Range(0, availableItemSpot.Length);
            if (indexAlreadyUsed.Contains(currIndex))
            {
                return GenerateUntilUnique();
            }

            indexAlreadyUsed.Add(currIndex);
            return currIndex;
        }
    }
}
