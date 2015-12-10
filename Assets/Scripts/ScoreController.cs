using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

    public Text scoreText;
    private int score = 0;


	// Use this for initialization
	void Start () {
        Score(0);
	}

    public int GetScore() {
        return score;
    }

    public void Score(int new_score) {
        score = new_score;
        scoreText.text = string.Format("{0:000000}", score);
    }

    public void AddScore(int add_score) {
        Score(score + add_score);
    }

    public void Reset() {
        Score(0);
    }
}
