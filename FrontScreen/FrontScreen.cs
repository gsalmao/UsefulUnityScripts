using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
 * 
 *          THIS SCRIPT USES GAME EVENTS, ODIN INSPECTOR AND DOTWEEN
 * 
 */

/// <summary>
/// This script controls the front screen of the game, mostly attached to the GameManager.
/// You can use this singleton to make fades between and during scenes, tint the screen and 
/// show images. Use FrontScreenSetup SO to configure how it is going to behave.
/// </summary>
public class FrontScreen : MonoBehaviour
{
    public static FrontScreen Instance;

    [SerializeField] private Image              image;
    [SerializeField] private Image              fade;
    [SerializeField] private FrontScreenSetup   defaultScreen;

    [FoldoutGroup("Events")]
    [Title("Fade-In",TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private GameEvent       OnFadeInBegin;
    [FoldoutGroup("Events")]
    [SerializeField] private GameEvent       OnFadeInEnd;

    [FoldoutGroup("Events")]
    [Title("Fade-Out",TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private GameEvent       OnFadeOutBegin;
    [FoldoutGroup("Events")]
    [SerializeField] private GameEvent       OnFadeOutEnd;

    [FoldoutGroup("Events")]
    [Title("Show Front Image", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private GameEvent OnShowFrontImageBegin;
    [FoldoutGroup("Events")]
    [SerializeField] private GameEvent OnShowFrontImageEnd;

    [FoldoutGroup("Events")]
    [Title("Hide Front Image", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private GameEvent OnHideFrontImageBegin;
    [FoldoutGroup("Events")]
    [SerializeField] private GameEvent OnHideFrontImageEnd;

    private FrontScreenSetup curScreen;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        curScreen = defaultScreen;
    }

    private IEnumerator screenAnim(GameEvent eventBegin, GameEvent eventEnd, Color colorBegin, Color colorEnd, Image imageToAnimate, float time)
    {
        eventBegin.Invoke();
        imageToAnimate.color = colorBegin;
        imageToAnimate.DOColor(colorEnd, time);
        yield return new WaitForSecondsRealtime(time);
        eventEnd.Invoke();
    }

    [HorizontalGroup("Buttons")]
    [VerticalGroup("Buttons/Left")]
    [GUIColor(0f,1f,0f)]
    [Button("Fade-In")]
    public void FadeIn()
    {
        StartCoroutine(screenAnim(  OnFadeInBegin,
                                    OnFadeInEnd,
                                    Color.clear,
                                    curScreen.fadeColor,
                                    fade,
                                    curScreen.fadeFadeInTime));
    }

    [VerticalGroup("Buttons/Right")]
    [GUIColor(0f, 1f, 0f)]
    [Button("Fade-Out")]
    public void FadeOut()
    {
        StartCoroutine(screenAnim(  OnFadeOutBegin,
                                    OnFadeOutEnd,
                                    curScreen.fadeColor,
                                    Color.clear,
                                    fade,
                                    curScreen.fadeFadeOutTime));
    }

    [VerticalGroup("Buttons/Left")]
    [GUIColor(0f, 1f, 0f)]
    [Button("Show Front Image")]
    public void ShowFrontImage()
    {
        StartCoroutine(screenAnim(  OnShowFrontImageBegin,
                                    OnShowFrontImageEnd,
                                    Color.clear,
                                    Color.white,
                                    image,
                                    curScreen.showImageTime));
    }

    [VerticalGroup("Buttons/Right")]
    [GUIColor(0f, 1f, 0f)]
    [Button("Hide Front Image")]
    public void HideFrontImage()
    {
        StartCoroutine(screenAnim(OnHideFrontImageBegin, OnHideFrontImageEnd, Color.white, Color.clear, image, curScreen.hideImageTime));
    }

    [GUIColor(0f, 1f, 0f)]
    [Button("Set New Front Screen")]
    public void SetNewFrontScreen(FrontScreenSetup newFrontScreen)
    {
        curScreen       = newFrontScreen;
        image.sprite    = newFrontScreen.imageSprite;
    }

    [GUIColor(0f, 1f, 0f)]
    [Button("Default Front Screen")]
    public void ResetToDefaultFrontScreen()
    {
        curScreen       = defaultScreen;
        image.sprite    = defaultScreen.imageSprite;
    }

    [GUIColor(0f, 1f, 0f)]
    [Button("Is fade on top of image?")]
    public void FadeOnTopOfImage(bool fadeOnTop)
    {
        if (fadeOnTop)
            fade.rectTransform.SetAsLastSibling();
        else
            fade.rectTransform.SetAsFirstSibling();
    }
}
