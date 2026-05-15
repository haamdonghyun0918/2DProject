using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : UiBase
{
    [SerializeField] private Slider Slider_LoadingBar;

    private void OnEnable()
    {
        StartCoroutine(CoStartLoadingBarEffect());
    }
    IEnumerator CoStartLoadingBarEffect()
    {
        Slider_LoadingBar.value = 0f;
        yield return new WaitForSeconds(0.2f);
        Slider_LoadingBar.value = 0.2f;
        yield return new WaitForSeconds(0.4f);
        Slider_LoadingBar.value = 0.4f;
        yield return new WaitForSeconds(0.6f);
        Slider_LoadingBar.value = 0.6f;
        yield return new WaitForSeconds(0.8f);
        Slider_LoadingBar.value = 0.8f;
        yield return new WaitForSeconds(1.0f);
        Slider_LoadingBar.value = 1.0f;
        yield return new WaitForSeconds(1.2f);
        
        this.gameObject.SetActive(false);

        if (TestProjectUiManager.Instance != null)
        {
            TestProjectUiManager.Instance.OpenTestStartUi();
        }
    }
}
