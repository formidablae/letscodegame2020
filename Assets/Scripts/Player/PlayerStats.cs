using UnityEngine;

namespace Apocalypse {
    public class PlayerStats : MonoBehaviour {
        public float Score
        {
            get => score;
            set
            {
                score = value;
                if (score <= 0)
                {
                    score = 0;
                    UIManager.Instance.OnGameFinished(false);
                }

                UIManager.Instance.UpdatePlayerStats(score);
            }
        }

        private float score;
        private float timeMultiplier;
        private float baseMultiplier;

        [SerializeField]
        private float startWithTimer = 60 * 60 * 2; //2 ore

        private void Start()
        {
            timeMultiplier = UIManager.Instance.speedTimersMultiplier;
            baseMultiplier = UIManager.Instance.baseMultiplier;
            Score = startWithTimer;
        }

        private void Update()
        {
            if (UIManager.Instance.Finished)
                return;

            if(score > 0)
            {
                Score -= timeMultiplier * baseMultiplier * Time.deltaTime;
            }
        }
    }
}

