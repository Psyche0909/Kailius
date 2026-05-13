using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraGameOver : MonoBehaviour {
	public TextMeshProUGUI textScore;

	void Start() {
		if (textScore != null && ScoreManager.instance != null) {
			textScore.text = ScoreManager.instance.getScoreTotal().ToString();
		}
	}
}
