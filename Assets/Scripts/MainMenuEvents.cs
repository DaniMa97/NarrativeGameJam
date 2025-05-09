using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class MainMenuEvents : MonoBehaviour
{
    UIDocument m_document;
    Button m_buttonStart;
    Button m_buttonSettings;
    //Button m_buttonCredits;
    Button m_buttonExit;

    Button m_buttonBackSettings;
    //Button m_buttonBackCredits;

    Slider m_masterSlider;
    Slider m_musicSlider;
    Slider m_sfxSlider;
    Slider m_voiceSlider;

    VisualElement m_mainMenuContainer;
    VisualElement m_creditsContainer;
    VisualElement m_settingsContainer;
    VisualElement m_sceneFade;

    float sceneTransitionTime = 0.25f;

    //If you need to do something for any of the buttons
    //private List<Button> m_menuButtons = new List<Button>();

    private void Awake()
    {
        m_document = GetComponent<UIDocument>();

        VisualElement root = m_document.rootVisualElement;

        m_buttonStart = root.Q("ButtonStart") as Button;
        m_buttonStart.RegisterCallback<ClickEvent>(OnButtonStart);
        m_buttonSettings = root.Q("ButtonSettings") as Button;
        m_buttonSettings.RegisterCallback<ClickEvent>(OnButtonSettings);
        //m_buttonCredits = root.Q("ButtonCredits") as Button;
        //m_buttonCredits.RegisterCallback<ClickEvent>(OnButtonCredits);
        m_buttonExit = root.Q("ButtonExit") as Button;
        m_buttonExit.RegisterCallback<ClickEvent>(OnButtonExit);
        m_buttonBackSettings = root.Q("ButtonBackSettings") as Button;
        m_buttonBackSettings.RegisterCallback<ClickEvent>(OnButtonBackSettings);
        //m_buttonBackCredits = root.Q("ButtonBackCredits") as Button;
        //m_buttonBackCredits.RegisterCallback<ClickEvent>(OnButtonBackCredits);

        m_masterSlider = root.Q("MasterSlider") as Slider;
        m_masterSlider.RegisterValueChangedCallback(OnMasterSliderChanged);
        m_musicSlider = root.Q("MusicSlider") as Slider;
        m_musicSlider.RegisterValueChangedCallback(OnMusicSliderChanged);
        m_sfxSlider = root.Q("SfxSlider") as Slider;
        m_sfxSlider.RegisterValueChangedCallback(OnSfxSliderChanged);
        m_voiceSlider = root.Q("VoiceSlider") as Slider;
        m_voiceSlider.RegisterValueChangedCallback(OnVoiceSliderChanged);


        m_mainMenuContainer = root.Q("MainContainer");
        m_creditsContainer = root.Q("CreditsContainer");
        m_settingsContainer = root.Q("SettingsContainer");
        m_sceneFade = root.Q("SceneFade");

        /*
        m_menuButtons = m_document.rootVisualElement.Query<Button>().ToList();
        for(int i=0; i<m_menuButtons.Count; ++i)
        {
            m_menuButtons[i].RegisterCallback<ClickEvent>(OnAnyButtonClick);
        }
        */
    }

    private void Start()
    {
        StartCoroutine(FadeInScene());
    }

    void OnButtonStart(ClickEvent evt)
    {
        StartCoroutine(GoToMainScene());
    }

    IEnumerator GoToMainScene()
    {
        m_sceneFade.style.display = DisplayStyle.Flex;
        float time = 0;
        Color color = m_sceneFade.style.backgroundColor.value;
        while (time < sceneTransitionTime)
        {
            float newAlpha = Mathf.Lerp(0, 1, time/sceneTransitionTime);
            m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, newAlpha);
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, 1);
        yield return new WaitForSeconds(0.01f);
        GameController.GetInstance().GoToScene("tutorial");
    }

    IEnumerator FadeInScene()
    {
        float time = 0;
        Color color = m_sceneFade.style.backgroundColor.value;
        while (time < sceneTransitionTime)
        {
            float newAlpha = Mathf.Lerp(1, 0, time/sceneTransitionTime);
            m_sceneFade.style.backgroundColor = new Color(color.r, color.g, color.b, newAlpha);
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_sceneFade.style.display = DisplayStyle.None;
    }

    void OnButtonSettings(ClickEvent evt)
    {
        m_mainMenuContainer.AddToClassList("mainContainerSettings");
        m_settingsContainer.RemoveFromClassList("settingsContainerOther");
    }

    void OnButtonCredits(ClickEvent evt)
    {
        m_mainMenuContainer.AddToClassList("mainContainerCredits");
        m_creditsContainer.RemoveFromClassList("creditsContainerOther");
    }

    void OnButtonBackSettings(ClickEvent evt)
    {
        m_mainMenuContainer.RemoveFromClassList("mainContainerSettings");
        m_settingsContainer.AddToClassList("settingsContainerOther");
    }

    void OnButtonBackCredits(ClickEvent evt)
    {
        m_mainMenuContainer.RemoveFromClassList("mainContainerCredits");
        m_creditsContainer.AddToClassList("creditsContainerOther");
    }

    void OnButtonExit(ClickEvent evt)
    {
        Application.Quit();
    }

    void OnMasterSliderChanged(ChangeEvent<float> evt)
    {
        SoundController.GetInstance().SetMasterVolume(evt.newValue);
    }

    void OnMusicSliderChanged(ChangeEvent<float> evt)
    {
        SoundController.GetInstance().SetMusicVolume(evt.newValue);
    }

    void OnSfxSliderChanged(ChangeEvent<float> evt)
    {
        SoundController.GetInstance().SetSfxVolume(evt.newValue);
    }

    void OnVoiceSliderChanged(ChangeEvent<float> evt)
    {
        SoundController.GetInstance().SetVoiceVolume(evt.newValue);
    }

    /*
    void OnAnyButtonClick(ClickEvent evt)
    {

    }
    */

    void OnDisable()
    {
        m_buttonStart.UnregisterCallback<ClickEvent>(OnButtonStart);

        /*
        for (int i = 0; i < m_menuButtons.Count; ++i)
        {
            m_menuButtons[i].UnregisterCallback<ClickEvent>(OnAnyButtonClick);
        }
        */
    }
}
