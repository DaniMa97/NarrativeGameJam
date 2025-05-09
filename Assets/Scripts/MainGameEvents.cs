using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class MainGameEvents : MonoBehaviour
{
    UIDocument m_document;

    VisualElement m_sceneFade;
    VisualElement m_deathScene;
    VisualElement m_titleScene;

    Button m_restart;
    Button m_mainMenuDeath;
    Button m_mainMenuWin;

    float sceneTransitionTime = 0.25f;

    private void Awake()
    {
        m_document = GetComponent<UIDocument>();

        VisualElement root = m_document.rootVisualElement;

        m_deathScene = root.Q("DeathContainer");
        m_titleScene = root.Q("TitleContainer");
        m_sceneFade = root.Q("SceneFade");

        m_restart = root.Q("ButtonRestart") as Button;
        m_restart.RegisterCallback<ClickEvent>(OnButtonRestart);
        m_mainMenuDeath = root.Q("ButtonMenuDeath") as Button;
        m_mainMenuDeath.RegisterCallback<ClickEvent>(OnButtonGoMainMenu);
        m_mainMenuWin = root.Q("ButtonMenuWin") as Button;
        m_mainMenuWin.RegisterCallback<ClickEvent>(OnButtonGoMainMenu);
    }

    private void Start()
    {
        StartCoroutine(FadeInScene());
        m_titleScene.style.display = DisplayStyle.None;
        m_deathScene.style.display = DisplayStyle.None;
    }

    public void PlayerDeath()
    {
        m_deathScene.style.display = DisplayStyle.Flex;
    }

    public void PlayerWin()
    {
        m_titleScene.style.display = DisplayStyle.Flex;
    }

    IEnumerator FadeInScene()
    {
        float time = 0;
        Color color = m_sceneFade.style.backgroundColor.value;
        while (time < sceneTransitionTime)
        {
            float newAlpha = Mathf.Lerp(1, 0, time / sceneTransitionTime);
            m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, newAlpha);
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_sceneFade.style.display = DisplayStyle.None;
    }

    void OnButtonRestart(ClickEvent evt)
    {
        StartCoroutine(GoToScene("tutorial"));
    }

    void OnButtonGoMainMenu(ClickEvent evt)
    {
        StartCoroutine(GoToScene("MainMenuScene"));
    }

    IEnumerator GoToScene(string scene)
    {
        m_sceneFade.style.display = DisplayStyle.Flex;
        float time = 0;
        Color color = m_sceneFade.style.backgroundColor.value;
        while (time < sceneTransitionTime)
        {
            float newAlpha = Mathf.Lerp(0, 1, time / sceneTransitionTime);
            m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, newAlpha);
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, 1);
        yield return new WaitForSeconds(0.01f);
        GameController.GetInstance().GoToScene(scene);
    }
}
