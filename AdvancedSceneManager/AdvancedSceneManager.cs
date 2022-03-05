using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 
 *          THIS SCRIPT USES GAME EVENTS AND ODIN INSPECTOR
 * 
 *          This class uses an outside enum (ScenesEnum) and
 *          dictionary to change the enum to the scene name...
 *          so keep that in mind if you want to use this code.
 *          
 *          You can't directly reference a scene in Unity outside the editor.
 *          
 *          Shame on you, Unity.
 * 
 */


/// <summary>
/// This Advanced Scene Manager simplifies the process of loading/unloading scenes and their
/// additive scenes. It must be referenced by a GameManager singleton to be called globally.
/// </summary>
public class AdvancedSceneManager : MonoBehaviour
{
    [Title("Scene Loaders")]
    [SerializeField] private List<SceneLoader> sceneLoaders;

    [FoldoutGroup("Events")]
    public GameEvent        OnSceneChangeBegin;
    [FoldoutGroup("Events")]
    public GameEvent        OnSceneChangeEnd;

    private Dictionary<Enums.Scenes, SceneLoader> sceneLoadersDict;

    private SceneLoader     currentSceneLoader;
    private SceneLoader     nextSceneLoader;
    private AsyncOperation  operation;

    private void Awake()
    {
        sceneLoadersDict = new Dictionary<Enums.Scenes, SceneLoader>();
        foreach (SceneLoader sceneLoader in sceneLoaders)
            sceneLoadersDict.Add(sceneLoader.primaryScene, sceneLoader);
    }

    /// <summary>
    /// Unload all current Scenes and load the next ones.
    /// </summary>
    [GUIColor(0f, 1f, 0f)]
    [Button("Change Scenes")]
    public void ChangeScenes(Enums.Scenes nxtScene)
    {
        StartCoroutine(SwitchScenes(nxtScene));
    }

    /// <summary>
    /// Unload current scenes and load the next ones, keeping the repeated scenes loaded.
    /// </summary>
    [GUIColor(0f, 1f, 0f)]
    [Button("Change Scenes (And keep repeated ones)")]
    public void ChangeAndKeepScenes(Enums.Scenes nxtScene)
    {
        StartCoroutine(SwitchScenes(nxtScene, true));
    }

    private IEnumerator SwitchScenes(Enums.Scenes nxtScene, bool keepRepeatedScenes = false)
    {
        if(!sceneLoadersDict.ContainsKey(nxtScene))
        {
            nextSceneLoader                 = ScriptableObject.CreateInstance<SceneLoader>();
            nextSceneLoader.additiveScenes  = new List<Enums.Scenes>();
            nextSceneLoader.primaryScene    = nxtScene;
        }
        else
            nextSceneLoader = sceneLoadersDict[nxtScene];


        OnSceneChangeBegin.Invoke();

        if(currentSceneLoader != null)
            yield return UnloadCurrentScenes(keepRepeatedScenes);

        yield return LoadNextScenes(nxtScene);

        currentSceneLoader = nextSceneLoader;
        SetActiveScene();
        OnSceneChangeEnd.Invoke();
    }

    private IEnumerator UnloadCurrentScenes(bool keepRepeatedScenes)
    {
        string sceneName    = ConstsScenes.scnEnumToStr[currentSceneLoader.primaryScene];
        operation           = SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitUntil(() => operation.isDone);

        foreach(Enums.Scenes additiveScene in currentSceneLoader.additiveScenes)
        {
            if(keepRepeatedScenes && HasRepeatedScene(additiveScene))
                continue;

            sceneName       = ConstsScenes.scnEnumToStr[additiveScene];
            operation       = SceneManager.UnloadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
        }
    }

    private bool HasRepeatedScene(Enums.Scenes sceneToCheck)
    {
        foreach (Enums.Scenes scene in nextSceneLoader.additiveScenes)
            if (sceneToCheck == scene)
                return true;
        return false;
    }


    private IEnumerator LoadNextScenes(Enums.Scenes nxtScene)
    {
        string nxtSceneStr = ConstsScenes.scnEnumToStr[nxtScene];
        operation = SceneManager.LoadSceneAsync(nxtSceneStr, LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation.isDone);
        
        if(sceneLoadersDict.ContainsKey(nxtScene))
            foreach (Enums.Scenes additiveScene in sceneLoadersDict[nxtScene].additiveScenes)
            { 
                string nxtAdditiveSceneStr = ConstsScenes.scnEnumToStr[additiveScene];
                operation = SceneManager.LoadSceneAsync(nxtAdditiveSceneStr, LoadSceneMode.Additive);
                yield return new WaitUntil(() => operation.isDone);
            }
    }

    private void SetActiveScene()
    {
        string activeSceneName;

        if (currentSceneLoader.AdditiveAsActive)
            activeSceneName = ConstsScenes.scnEnumToStr[currentSceneLoader.additiveScenes[currentSceneLoader.index]];
        else
            activeSceneName = ConstsScenes.scnEnumToStr[currentSceneLoader.primaryScene];

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeSceneName));
    }
}
