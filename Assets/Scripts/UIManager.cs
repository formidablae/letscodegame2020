using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Elements")]
    [SerializeField]
    private TMP_Text notificationLabel;

    private Coroutine lastCoroutine;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
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
}
