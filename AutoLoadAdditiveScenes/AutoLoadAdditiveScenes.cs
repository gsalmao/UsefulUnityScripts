#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Very handy script to load additive scenes with your main scene and GameManager scene.
/// MAKE SURE TO CHECK EVERY PATH. And use string references instead of hardcoded around the code...
/// </summary>
[InitializeOnLoad]
public class AutoLoadAdditiveScenes {

    public static List<string> additiveScenesToLoad;



    //Insert here all the additive scenes paths you need.



    private const string gameManagerPath        = "Assets/Scenes/GameManager.unity";
    private const string additive1              = "Assets/Scenes/AdditiveScenes/additive1.unity";
    private const string additive2              = "Assets/Scenes/AdditiveScenes/additive2.unity";
    private const string additive3              = "Assets/Scenes/AdditiveScenes/additive3.unity";
    private const string additive4              = "Assets/Scenes/AdditiveScenes/additive4.unity";
    private const string additive5              = "Assets/Scenes/AdditiveScenes/additive5.unity";



    static AutoLoadAdditiveScenes()
    {
        EditorSceneManager.sceneOpened += LoadAdditiveScenes;
        additiveScenesToLoad = new List<string>();
    }

    public static void LoadAdditiveScenes(Scene scene, OpenSceneMode mode)
    {
        if (mode == OpenSceneMode.Additive)
            return;
        additiveScenesToLoad.Clear();



        if (scene.name != "GameManager")     //Includes the GameManager in every scene.
            additiveScenesToLoad.Add(gameManagerPath);



        //For every scene you need additive scenes, create an if-statement and add the additive scenes paths on the list. That is all.



        if(scene.name == "RandomSceneName")
        {
            additiveScenesToLoad.Add(additive1);
            additiveScenesToLoad.Add(additive2);
        }

        if (scene.name == "Scene1" || scene.name == "Scene2")
        {
            additiveScenesToLoad.Add(additive3);
            additiveScenesToLoad.Add(additive4);
            additiveScenesToLoad.Add(additive5);
        }

        foreach (string additivePath in additiveScenesToLoad)
            EditorSceneManager.OpenScene(additivePath, OpenSceneMode.Additive);
    }
}
//#endif