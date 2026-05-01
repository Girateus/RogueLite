using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTheDungeon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")|| other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Welcome");
            SceneManager.LoadScene("DungeonGeneration");
        }
    }
}
