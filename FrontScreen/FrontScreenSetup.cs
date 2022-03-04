using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 *          THIS SCRIPT USES ODIN INSPECTOR
 * 
 */

/// <summary>
/// Front Screen Scriptable Object. To configure how the front screen is going to behave.
/// </summary>
[CreateAssetMenu(fileName = "FrontScreenSetup", menuName = "GameManager/FrontScreenSetup", order = 2)]
public class FrontScreenSetup : ScriptableObject
{

    [Title("Front Screen Image")]
    public Sprite imageSprite;
    public float showImageTime;
    public float hideImageTime;

    [Title("Front Screen Fade")]
    public Color fadeColor;
    public float fadeFadeInTime;
    public float fadeFadeOutTime;

}
