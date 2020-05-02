using UnityEngine;

namespace Apocalypse {
    public class PlayerStats : MonoBehaviour {
        public int Score
        {
            get => score;
            set
            {
                score = value;
                Debug.Log(score);
                //Update UI
            }
        }

        private int score;
    }
}

