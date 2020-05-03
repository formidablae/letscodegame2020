using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Apocalypse {
    public class HomeUI : MonoBehaviour {
        private Button _startBtn;

        private void Awake() {
            _startBtn = GetComponent<Button>();
        }

        void Start () {
            _startBtn.onClick.AddListener(startGame);
        }

        private void startGame() {
            Debug.Log("lll");
            SceneManager.LoadScene("ShopScene");
        }
    }
}
