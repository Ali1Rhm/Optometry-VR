using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
