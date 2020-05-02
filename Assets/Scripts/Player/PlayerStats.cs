namespace Apocalypse {
    public class PlayerStats { //todo mono? vedremo text 
        public int Score { get; private set; }

        public void IncScore(int x) {
            Score += x;
        }
    }
}

