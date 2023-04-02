using UnityEngine;

public class PlayerDetectorToWin : MonoBehaviour
{
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            winPanel.SetActive(true);
        }
    }
}