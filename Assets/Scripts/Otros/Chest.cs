using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Chest : MonoBehaviour {
    public GameObject coins;
    public GameObject gems;
    public GameObject hearts;
    public GameObject sword;
    public GameObject shield;
    public Sprite openChestSprite;

    public int maxCoins = 11;
    public int maxGems = 6;
    public int maxHearts = 3;
    public int maxSwords = 2;
    public int maxShields = 2;

    private bool range = false;
    private bool open = true;

    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && range && open) {
            gameObject.GetComponent<SpriteRenderer>().sprite = openChestSprite;
            generar();
            open = false;
        }
    }

    public void generar() {
        int numCoins = Random.Range(1, maxCoins);
        int numGems = Random.Range(1, maxGems);
        int numHearts = Random.Range(1, maxHearts);
        int numSwords = Random.Range(0, maxSwords);
        int numShields = Random.Range(0, maxShields);

        float baseX = gameObject.transform.position.x;
        float baseY = gameObject.transform.position.y + 3.0f;
        float baseZ = gameObject.transform.position.z;

        SpawnItems(coins, numCoins, baseX, baseY, baseZ);
        SpawnItems(gems, numGems, baseX, baseY, baseZ);
        SpawnItems(hearts, numHearts, baseX, baseY, baseZ);
        SpawnItems(sword, numSwords, baseX, baseY, baseZ);
        SpawnItems(shield, numShields, baseX, baseY, baseZ);
    }

    void SpawnItems(GameObject prefab, int count, float x, float y, float z) {
        for (int i = 0; i < count; i++) {
            float scatterX = Random.Range(-1.5f, 1.5f);
            GameObject item = Instantiate(prefab, new Vector3(x + scatterX, y, z), Quaternion.identity);
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            range = true;
        }
    }

    public void openChest() {
        if (open) {
            gameObject.GetComponent<SpriteRenderer>().sprite = openChestSprite;
            generar();
            open = false;
        }
    }
}
