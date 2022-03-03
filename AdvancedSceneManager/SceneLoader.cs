using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/*
 * 
 *          THIS SCRIPT USES ODIN INSPECTOR
 * 
 */

/// <summary>
/// AdvancedSceneManager Scriptable Object, to make your life easier.
/// </summary>
[CreateAssetMenu(fileName = "SceneLoader", menuName = "Scenes/SceneLoader", order = 1)]
public class SceneLoader : ScriptableObject
{
    public Enums.Scenes primaryScene;
    public List<Enums.Scenes> additiveScenes;
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    public bool AdditiveAsActive;
    [VerticalGroup("Split/Right")]
    [ShowIf("AdditiveAsActive")]
    [LabelWidth(50)]
    public int index;
}