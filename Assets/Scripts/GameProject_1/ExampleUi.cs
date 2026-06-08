using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ExampleUi : UiBase
{
    [SerializeField] private UiButton button_Left;
    [SerializeField] private UiButton button_Right;
    [SerializeField] private UiButton button_Close;
    
    [Header("데이터 드리븐 파트")]
    [SerializeField] private Image image_Example;
    [SerializeField] private Text text_Example;

    private List<ExampleData> exampleList = new List<ExampleData>();
    private int currentIndex = 0;

    private void OnEnable()
    {
        button_Left.BindOnClickButtonEvent(OnClickLeft);
        button_Right.BindOnClickButtonEvent(OnClickRight);
        button_Close.BindOnClickButtonEvent(OnClickClose);
        
        if (GameDataManager.Instance != null && GameDataManager.Instance.ExampleDataList != null)
        {
            exampleList = GameDataManager.Instance.ExampleDataList.Values.ToList();
        }

        currentIndex = 0;
        UpdateExampleInfo();
    }

    private void UpdateExampleInfo()
    {
        // 데이터가 아예 없다면 중단
        if (exampleList == null || exampleList.Count == 0) return;

        // 현재 인덱스에 해당하는 데이터를 가져옵니다.
        ExampleData currentData = exampleList[currentIndex];

        // 텍스트 적용
        if (text_Example != null)
        {
            text_Example.text = currentData.Text;
        }

        // 이미지 적용 (Resources 폴더에서 동적 로드)
        if (image_Example != null && !string.IsNullOrEmpty(currentData.Image))
        {
            Sprite loadedSprite = Resources.Load<Sprite>(currentData.Image);
            if (loadedSprite != null)
            {
                image_Example.sprite = loadedSprite;
            }
            else
            {
                Debug.LogWarning($"[ExampleUi] 이미지를 찾을 수 없습니다: {currentData.Image}");
            }
        }

        // 좌우 버튼 활성화/비활성화 로직
        // 첫 번째 페이지(0)면 왼쪽 버튼 숨기기, 아니면 보이기
        if (button_Left != null)
        {
            bool shouldShowLeft = currentIndex > 0;

            // 현재 상태와 목표 상태가 다를 때만 실행 (즉, 켜지거나 꺼질 때만)
            if (button_Left.gameObject.activeSelf != shouldShowLeft)
            {
                button_Left.gameObject.SetActive(shouldShowLeft);

                // 버튼이 숨겨졌다가 다시 나타나는 순간이라면, 날아간 이벤트를 다시 달아줍니다.
                if (shouldShowLeft)
                {
                    button_Left.BindOnClickButtonEvent(OnClickLeft);
                }
            }
        }

        // 마지막 페이지(리스트길이 - 1)면 오른쪽 버튼 숨기기, 아니면 보이기
        if (button_Right != null)
        {
            bool shouldShowRight = currentIndex < exampleList.Count - 1;

            // 현재 상태와 목표 상태가 다를 때만 실행
            if (button_Right.gameObject.activeSelf != shouldShowRight)
            {
                button_Right.gameObject.SetActive(shouldShowRight);

                // 버튼이 숨겨졌다가 다시 나타나는 순간이라면, 날아간 이벤트를 다시 달아줍니다.
                if (shouldShowRight)
                {
                    button_Right.BindOnClickButtonEvent(OnClickRight);
                }
            }
        }
    }

    private void OnClickLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateExampleInfo();
        }
    }   
    
    private void OnClickRight()
    {
        if (currentIndex < exampleList.Count - 1)
        {
            currentIndex++;
            UpdateExampleInfo();
        }
    }

    private void OnClickClose()
    {
        UiManager.Instance.CloseExampleUi();
    }
}