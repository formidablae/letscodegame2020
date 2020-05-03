using UnityEngine;

public class ShopTimer : MonoBehaviour
{
    public float CurrentTimerToBuy => currentTimerToBuy;

    [Header("Buy Timer")]
    [SerializeField]
    private float minTimeToBuy = 60 * 60 * 2;
    [SerializeField]
    private float maxTimeToBuy = 60 * 60 * 8;

    private float currentTimeToBuy;

    [Header("Wait Timers")]
    [SerializeField]
    private float timeToWait = 60 * 60 * 2;

    [HideInInspector]
    public Shop owner;
    private float currentTimerToBuy;
    private float currentTimerToWait;
    private bool isClosed;

    private float timeMultiplier;
    private float baseMultiplier;

    void Start()
    {
        timeMultiplier = UIManager.Instance.speedTimersMultiplier;
        baseMultiplier = UIManager.Instance.baseMultiplier;

        currentTimeToBuy = Random.Range(minTimeToBuy, maxTimeToBuy);
        currentTimerToBuy = currentTimeToBuy;
        currentTimerToWait = 0;
        isClosed = false;
    }

    void Update()
    {
        if (UIManager.Instance.Finished)
            return;

        if (isClosed)
            UpdateTimeToWait();
        else
            UpdateTimeToBuy();
    }

    public void UpdateTimeToBuy()
    {
        currentTimerToBuy -= timeMultiplier * baseMultiplier * Time.deltaTime;

        if (currentTimerToBuy <= 0)
        {
            currentTimeToBuy = Random.Range(minTimeToBuy, maxTimeToBuy);
            currentTimerToBuy = currentTimeToBuy;
            isClosed = true;

            owner.OnShopTimerCompleted();
            UIManager.Instance.UpdateClosedShopTimer(this);
        } else
        {
            UIManager.Instance.UpdateShopTimer(this);
        }
    }

    public void UpdateTimeToWait()
    {
        currentTimerToWait += timeMultiplier * baseMultiplier * Time.deltaTime;

        if (currentTimerToWait >= timeToWait)
        {
            currentTimerToWait = 0;
            isClosed = false;

            owner.OnShopTimerRestart();
        }
    }
}
