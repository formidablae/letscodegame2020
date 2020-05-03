using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public float speedTimersMultiplier = 1f;
    public float baseMultiplier = 60;
    
    [SerializeField]
    private float timerApocalypse = 60 * 60 * 24 * 5; //5 giorni

    [Header("Elements")]
    [SerializeField]
    private TMP_Text notificationLabel;

    [Header("Timer Stats")]
    [SerializeField]
    private TMP_Text timerApocalypseLabel;
    [SerializeField]
    private TMP_Text timerPlayerLabel;

    [Header("Timer Shops")]
    [SerializeField]
    private TMP_Text[] timerShopsLabel;

    [Header("Main Image Result Menu")]
    [SerializeField]
    private Image imageBackgroundResult;

    private Coroutine lastCoroutine;

    // precalculcated consts
    private const int MINUTES = 60;
    private const int HOURS = 60 * 60;
    private const int DAYS = 60 * 60 * 24;

    public bool Finished { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        Finished = false;
        imageBackgroundResult.gameObject.SetActive(false);
        UpdateTimerApocalypse(timerApocalypse);
    }

    private void Update()
    {
        if (UIManager.Instance.Finished)
            return;

        if (timerApocalypse > 0)
        {
            timerApocalypse -= (speedTimersMultiplier * baseMultiplier * Time.deltaTime);
            if (timerApocalypse <= 0)
            {
                timerApocalypse = 0;
                OnGameFinished(true);
            }

            UpdateTimerApocalypse(timerApocalypse);
        }
    }

    public void UpdateTimerApocalypse(float timer)
    {
        if (timer <= 0)
        {
            timerApocalypseLabel.text = $"Durata Apocalisse: <color=green>FINITA</color>";
        }

        int intTimer = (int)timer;
        int days = intTimer / DAYS;
        int hours = (intTimer - days * DAYS) / HOURS;
        int minutes = (intTimer - days * DAYS - hours * HOURS) / MINUTES;

        timerApocalypseLabel.text = $"Durata Apocalisse: ";
        timerApocalypseLabel.text += $"<color=red>{days}g</color> ";
        timerApocalypseLabel.text += $"<color=yellow>{hours:00}h</color> ";
        timerApocalypseLabel.text += $"<color=green>{minutes:00}m</color>";
    }

    public void UpdatePlayerStats(float timer)
    {
        if (timer <= 0)
        {
            timerPlayerLabel.text = $"Sopravvienza Attuale: <color=green>COMPLETATA</color>";
        }

        int intTimer = (int)timer;
        int days = intTimer / DAYS;
        int hours = (intTimer - days * DAYS) / HOURS;
        int minutes = (intTimer - days * DAYS - hours * HOURS) / MINUTES;

        timerPlayerLabel.text = $"Sopravvienza Attuale: ";
        timerPlayerLabel.text += $"<color=green>{days}g</color> ";
        timerPlayerLabel.text += $"<color=yellow>{hours:00}h</color> ";
        timerPlayerLabel.text += $"<color=red>{minutes:00}m</color>";
    }

    public void UpdateShopTimer(ShopTimer shopTimer)
    {
        if(shopTimer.owner.Id < timerShopsLabel.Length)
        {
            TMP_Text element = timerShopsLabel[shopTimer.owner.Id];

            int intTimer = (int)shopTimer.CurrentTimerToBuy;
            int hours = intTimer / HOURS;
            int minutes = (intTimer - hours * HOURS) / MINUTES;

            element.text = $"{shopTimer.owner.Id + 1}] ";
            element.text += $"<color=yellow>{hours:00}h</color> ";
            element.text += $"<color=red>{minutes:00}m</color>";
        }
    }

    public void UpdateClosedShopTimer(ShopTimer shopTimer)
    {
        if (shopTimer.owner.Id < timerShopsLabel.Length)
        {
            TMP_Text element = timerShopsLabel[shopTimer.owner.Id];

            element.text = $"{shopTimer.owner.Id + 1}] ";
            element.text += "<color=red>CHIUSO</color>";
        }
    }

    public void NotifyItemTaken(ShopItemModel model)
    {
        string outputText = $"Hai raccolto {model.Type} che conferisce ";

        int intTimer = (int)model.IncTimer;
        int days = intTimer / DAYS;
        int hours = (intTimer - days * DAYS) / HOURS;
        int minutes = (intTimer - days * DAYS - hours * HOURS) / MINUTES;

        if(days > 0)
        {
            outputText += $"<color=green>{days} giorni</color> ";
        }

        if (hours > 0)
        {
            outputText += $"<color=yellow>{hours} ore</color> ";
        }

        if (minutes > 0)
        {
            outputText += $"<color=red>{minutes} minuti</color> ";
        }

        outputText += "durata.";
        PushNotification(outputText);
    }

    public void PushNotification(string text)
    {
        if(lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }

        notificationLabel.text = text;
        notificationLabel.gameObject.SetActive(true);
        lastCoroutine = StartCoroutine(HideTextAfterSeconds(2));
    }

    private IEnumerator HideTextAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        notificationLabel.gameObject.SetActive(false);
        lastCoroutine = null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("ShopScene").buildIndex, LoadSceneMode.Single);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("MainMenu").buildIndex, LoadSceneMode.Single);
    }

    public void OnGameFinished(bool win)
    {
        if (Finished)
            return;

        Finished = true;
        TMP_Text textComp = imageBackgroundResult.transform.Find("Content").Find("TextResult").GetComponent<TMP_Text>();
        imageBackgroundResult.gameObject.SetActive(true);

        if (win)
        {
            imageBackgroundResult.color = new Color(193 / 255, 255 / 255, 114 / 255, 1);
            textComp.color = new Color(0 / 255, 255 / 255, 10 / 255, 1);
            textComp.text = "Congratulazioni le tue provviste sono abbastanza per sopravvivere all'apocalisse!";
        } else
        {
            imageBackgroundResult.color = new Color(255 / 255, 143 / 255, 114 / 255, 1);
            textComp.color = new Color(255 / 255, 205 / 255, 0 / 255, 1);
            textComp.text = "Sembra che tu non ce l'abbia fatta a sopravvivere, riprova nella tua prossima vita!";
        }
    }
}
