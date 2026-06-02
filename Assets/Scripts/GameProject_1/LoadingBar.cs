using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : UiBase
{
    // 유니티에 있는 슬라이더Ui를 스크립트와 연결해주는 부분
    [SerializeField] private Slider Slider_LoadingBar;

    private void OnEnable()
    {
        Slider_LoadingBar.value = 0.0f;

        // GameDataManager에서 데이터 로딩을 부탁하는 코루틴 방식
        if (GameDataManager.Instance != null)
        {
            StartCoroutine(GameDataManager.Instance.CoLoadAllData(onProgress: UpdateProgressBar, onComplete: OnLoadingComplete));
        }
        else
        {
            Debug.Log("GmaeDataManager를 호출하지 못했습니다.");
        }
    }

    // GameDataManager가 float를 호출합니다.
    private void UpdateProgressBar(float progress)
    {
        Slider_LoadingBar.value = progress;
    }

    // 로딩이 100% 완료되었을 때, 다음 화면으로 넘어가는 처리를 하는 부분
    private void OnLoadingComplete()
    {
        this.gameObject.SetActive(false);

        if (UiManager.Instance != null)
        {
            UiManager.Instance.OpenMainUi();
        }
    }
}