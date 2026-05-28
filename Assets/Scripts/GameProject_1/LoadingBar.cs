using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : UiBase
{
    [SerializeField] private Slider Slider_LoadingBar;

    private void OnEnable()
    {
        Slider_LoadingBar.value = 0.0f;

        if (GameDataManager.Instance != null)
        {
            StartCoroutine(GameDataManager.Instance.CoLoadAllData(onProgress: UpdateProgressBar, onComplete: OnLoadingComplete));
        }
        else
        {
            Debug.Log("GmaeDataManager를 호출하지 못했습니다.");
        }
    }
    private void UpdateProgressBar(float progress)
    {
        Slider_LoadingBar.value = progress;
    }
    private void OnLoadingComplete()
    {
        this.gameObject.SetActive(false);

        if (UiManager.Instance != null)
        {
            UiManager.Instance.OpenMainUi();
        }
    }
}
