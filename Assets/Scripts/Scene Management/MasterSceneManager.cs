using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MasterSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Camera _camera;
    [SerializeField] string _mainMenuScene;
    [SerializeField] string _firstLoginScene;
    [SerializeField] string _splashScreen;
    [SerializeField] string[] _advices;
    [SerializeField] TextMeshProUGUI _adviceText;
    public Scene _currentScene;

    public Action OnSceneCompleteLoading = delegate { };

    GameProgressionService _gameProgressionService;

    void Start()
    {
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();


        if (_gameProgressionService.Load() == null)
        {
            LoadScene(_firstLoginScene);
        }
        else
        {
            LoadScene(_mainMenuScene);
        }
    }

    private void OnDestroy()
    {
        if (_currentScene.name != _firstLoginScene)
        {
            _gameProgressionService.Save();
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        if (!sceneToLoad.Equals(_currentScene.name))
        {
            StartCoroutine(LoadSceneCoroutine(sceneToLoad));
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneToLoad)
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;

        SetAdviceText();

        if (sceneToLoad != _firstLoginScene)
        {
            _gameProgressionService.Save();
        }

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        foreach (Camera c in Camera.allCameras)
        {
            if (c != _camera)
            {
                c.gameObject.SetActive(false);
            }
        }

        _camera.gameObject.SetActive(true);

        if (_currentScene.isLoaded)
        {
            AsyncOperation unloadSceneOperation = SceneManager.UnloadSceneAsync(_currentScene);
            while (!unloadSceneOperation.isDone)
            {
                yield return null;
            }
        }


        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!loadSceneOperation.isDone)
        {
            yield return null;
        }

        _camera.gameObject.SetActive(false);
        _currentScene = SceneManager.GetSceneAt(1);
        SceneManager.SetActiveScene(_currentScene);

        OnSceneCompleteLoading?.Invoke();

        if (sceneToLoad != _firstLoginScene)
        {
            _gameProgressionService.Load();
        }

        yield return new WaitForSeconds(1f);

        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        _canvasGroup.gameObject.SetActive(false);
    }

    public void SetAdviceText()
    {
        string advice = _advices[UnityEngine.Random.Range(0, _advices.Length)];

        _adviceText.text = advice;
    }
}
