using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public void GamePlayScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GamePlay");
    }
}
