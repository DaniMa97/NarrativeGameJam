using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance;

    bool isMaskOn = false;

    GameObject player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.name == "tutorial")
        {
            player = GameObject.Find("Player");
        }
        else
        {
            player = null;
        }
    }

    public void ChangeMask()
    {
        isMaskOn = !isMaskOn;

        if(player != null)
        {
            Vector3 newPos = player.transform.position;
            
            if(isMaskOn)
            {
                newPos.x += 10000;
            }
            else
            {
                newPos.x -= 10000;
            }

            player.transform.position = newPos;
        }
    }

    public bool IsMaskOn()
    {
        return isMaskOn;
    }

    public void KillPlayer()
    {
        print("The player is dead");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("tutorial");
    }
}
