using UnityEngine;
using System.Collections;

public class ShopTimer : MonoBehaviour
{
    [Header("Buy Timer")]
    [SerializeField]
    private float timeToBuy;

    [Header("Wait Timers")]
    [SerializeField]
    private float timeToWait;

    public Shop owner;
    private float currentTimerToBuy;
    private float currentTimerToWait;
    private bool isClosed;

    void Start()
    {
        currentTimerToBuy = 0;
        currentTimerToWait = 0;
        isClosed = false;
    }

    void Update()
    {
        if (isClosed)
            UpdateTimeToWait();
        else
            UpdateTimeToBuy();
    }

    public void UpdateTimeToBuy()
    {
        currentTimerToBuy += Time.deltaTime;

        if (currentTimerToBuy >= timeToBuy)
        {
            currentTimerToBuy = 0;
            isClosed = true;

            owner.OnShopTimerCompleted();
        }
    }

    public void UpdateTimeToWait()
    {
        currentTimerToWait += Time.deltaTime;

        if (currentTimerToWait >= timeToWait)
        {
            currentTimerToWait = 0;
            isClosed = false;

            owner.OnShopTimerRestart();
        }
    }
}
