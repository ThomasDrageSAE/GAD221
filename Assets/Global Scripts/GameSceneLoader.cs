using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : Singleton<GameSceneLoader>
{
    public string currentGameScene { get; private set; }
    public string previousGameScene { get; private set; }

    public string globalSceneName = "Global";
    public string initialGameScene;
    
    public event Action<string> OnGameSceneLoaded;
    public event Action<string> OnGameSceneUnloaded;

    // Global Scene - Single scene that's always loaded, meant for things that should be always loaded and accessed by whole game.
    // Game Scene - Any other scene, Should only ever be one loaded at a time.
    
    override protected void Awake()
    {
        base.Awake();

        List<string> loadedGameScenes = GetLoadedGameScenes();

        // If a game scene is already open in the editor set that as the current game scene.
        if (loadedGameScenes.Count > 0)
        {
            StartCoroutine(InitialEditorSceneLoad(loadedGameScenes));
        }

        // Otherwise load initial game scene normally.
        else
        {
            LoadScene(initialGameScene);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadGameScene(sceneName));
    }

    private List<string> GetLoadedGameScenes()
    {
        List<string> gameScenes = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != globalSceneName)
            {
                gameScenes.Add(scene.name);
            }
        }

        return gameScenes;
    }

    private IEnumerator InitialEditorSceneLoad(List<string> loadedGameScenes)
    {
        // If more than one game scene is loaded in editor (which is not allowed), unload them all and default to the initial game scene. 
        if (loadedGameScenes.Count > 1)
        {
            Debug.LogWarning("GameSceneLoader - InitialEditorSceneLoad - More than 1 GameScene open. Unloading them & defaulting to initial game scene.");

            foreach (string sceneName in loadedGameScenes)
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
                if (unloadOperation == null)
                {
                    Debug.LogWarning("GameSceneLoader - InitialEditorSceneLoad - Game Scene failed to unload: " + sceneName);
                }

                else
                {
                    while (!unloadOperation.isDone)
                    {
                        yield return null;
                    }
                }
            }

            LoadScene(initialGameScene);
            yield break;
        }

        yield return null; // Single frame wait to allow event subscription to finish.

        currentGameScene = loadedGameScenes[0];
        OnGameSceneLoaded?.Invoke(currentGameScene);
    }

    private IEnumerator LoadGameScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(currentGameScene))
        {
            string previousSceneName = currentGameScene;
            previousGameScene = previousSceneName;
            
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(previousGameScene);
            if (unloadOperation == null)
            {
                Debug.LogWarning("GameSceneLoader - LoadGameScene - Game Scene failed to unload: " + previousGameScene);
            }

            else
            {
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }
                
                OnGameSceneUnloaded?.Invoke(previousGameScene);
            }
        }
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOperation == null)
        {
            Debug.LogWarning("GameSceneLoader - LoadGameScene - Game Scene failed to load: " + sceneName);
        }

        else
        {
            while (!loadOperation.isDone)
            {
                yield return null;
            }
            
            currentGameScene = sceneName;
            OnGameSceneLoaded?.Invoke(sceneName);
        }
    }
}