using UnityEngine.SceneManagement;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    private PlayerStatus status;

    private void Start()
    {
        status = PlayerManager.Instance.playerStatus;

    }
    private void Update()
    {
        if(status.currentHp <= 0)
        {
            Debug.Log("Á×À½");
            status.currentHp = status.maxHp;
            SceneManager.LoadScene("Stage1");
        }
    }
}
