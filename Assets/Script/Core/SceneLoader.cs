using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadTitle()
    {
        LoadScene("Title");
    }

    public void LoadStage1()
    {
        LoadScene("Stage1");
    }

    public void LoadStage2()
    {
        LoadScene("Stage2");
    }
}
