using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health = 500;
    public float timeDestroy = 1.5f;
    public Animator animator;
    public GameObject sonidoMuerte;

    public GameObject coins;
    public GameObject hearts;
    public GameObject sword;
    public GameObject shield;

    public int maxCoins = 5;
    public int maxHearts = 3;
    public int maxSwords = 2;
    public int maxShields = 2;

    public void TakeDamage(int damage) {
        this.health -= damage;

        // Play animacion de herida
        animator.SetTrigger("hurt");

        if(health <= 0) {
            Die();
        }
    }

    void Die() {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

        // Dropear items
        dropItems();

        // Play animacion de muerto
        animator.SetBool("isDead", true);

        // Añadir puntuacion 
        ScoreManager.instance.ChangeScore(100);

        // Destruir al enemigo
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Sonido
        Instantiate(sonidoMuerte);
        Object.Destroy(gameObject, timeDestroy);
    }

    void dropItems() {
        int numCoins = Random.Range(1, maxCoins);
        int numHearts = Random.Range(0, maxHearts);
        int numSwords = Random.Range(0, maxSwords);
        int numShields = Random.Range(0, maxShields);

        float baseX = gameObject.transform.position.x;
        float baseY = gameObject.transform.position.y + 2.0f;
        float baseZ = gameObject.transform.position.z;

        SpawnItems(coins, numCoins, baseX, baseY, baseZ);
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


}
