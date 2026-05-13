using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    private bool playerInRange = false;
    private float stayTimer = 0f;
    private const float REQUIRED_STAY_TIME = 1.5f;

    void Update() {
        if (playerInRange) {
            stayTimer += Time.deltaTime;
            if (stayTimer >= REQUIRED_STAY_TIME) {
                int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextIndex < SceneManager.sceneCountInBuildSettings) {
                    SceneManager.LoadScene(nextIndex);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D plyr) {
        if (plyr.gameObject.CompareTag("Player")) {
            playerInRange = true;
            stayTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D plyr) {
        if (plyr.gameObject.CompareTag("Player")) {
            playerInRange = false;
            stayTimer = 0f;
        }
    }
}
