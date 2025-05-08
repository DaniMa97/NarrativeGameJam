using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance;

    bool isMaskOn = false;

    public static GameController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeMask()
    {
        isMaskOn = !isMaskOn;
    }

    public bool GetIsMaskOn()
    {
        return isMaskOn;
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("tutorial");
    }
}
