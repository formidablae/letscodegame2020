using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopItemManager : MonoBehaviour
{
    public static ShopItemManager Instance;

    [SerializeField]
    private List<ShopItemModel> shopItemModels;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public List<ShopItemModel> PickRandomItems(int min, int max, List<ShopItemModel> outputList)
    {
        int n = Random.Range(min, max);
        outputList.Clear();

        for (int i = 0; i < n; i++)
        {
            int rndIndex = Random.Range(0, shopItemModels.Count);
            outputList.Add(shopItemModels[rndIndex]);
        }

        return outputList;
    }
}
