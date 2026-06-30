using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }
}