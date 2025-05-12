using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SceneDirection : MonoBehaviour
{
    private string mainSceneName = "Main Scene";
    private string loginSceneName = "Login Scene";
    private string worldMapSceneName = "World Map Scene";
    private string lineUpSceneName = "LineUp Scene";
    private string summonSceneName = "Summon Scene";
    private string battleSceneName = "Combat Scene";

    [Header("Fade Settings")]
    [SerializeField] private SceneFade _sceneFade;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (_sceneFade == null)
        {
            _sceneFade = FindObjectOfType<SceneFade>();
            if (_sceneFade == null)
            {
                Debug.LogError("SceneFade component not found in the scene.");
            }
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(_sceneFade.FadeIn(fadeDuration));
    }

    public void GoToMainScene()
    {
        StartCoroutine(TransitionToScene(mainSceneName));
    }

    public void GoToLoginScene()
    {
        LoginController.Instance.SignOut();
        Destroy(LoginController.Instance.gameObject);
        StartCoroutine(TransitionToScene(loginSceneName));
    }

    public void GoToWorldMapScene()
    {
        StartCoroutine(TransitionToScene(worldMapSceneName));
    }

    public void GoToLineUpScene()
    {
        StartCoroutine(TransitionToScene(lineUpSceneName));
    }

    public void GoToSummonScene()
    {
        StartCoroutine(TransitionToScene(summonSceneName));
    }

    public void GoToBattleScene()
    {
        StartCoroutine(TransitionToScene(battleSceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return StartCoroutine(_sceneFade.FadeOut(fadeDuration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(_sceneFade.FadeIn(fadeDuration));
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
