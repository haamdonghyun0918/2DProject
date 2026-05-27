using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class SlotCardUi : UiBase, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text text_Name;
    [SerializeField] private Text text_Description;
    [SerializeField] private Image image_Card;
    [SerializeField] private Image image_Icon;
    
    private CardData myCardData;
    private bool isDragging = false;
    private RectTransform rectTransform;

    private LineRenderer targetingLine;
    public void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetUp(CardData cardData)
    {
        myCardData = cardData;
        if (cardData == null) return;

        text_Name.text = cardData.Name;
        text_Description.text = cardData.Description;

        Sprite iCard = Resources.Load<Sprite>(cardData.ImageCardAddress);
        Sprite iIcon = Resources.Load<Sprite>(cardData.ImageIconAddress);
        if (iCard != null) image_Card.sprite = iCard;
        if (iIcon != null) image_Icon.sprite = iIcon;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBounce);
        rectTransform.DOAnchorPosY(30f, 0.2f).SetRelative(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(-30f, 0.2f).SetRelative(true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        CreateTargetingLine();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (targetingLine != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 startPos = transform.position;
            startPos.z -= 1f;
            mouseWorldPos.z = startPos.z;

            targetingLine.SetPosition(0, startPos);
            targetingLine.SetPosition(1, mouseWorldPos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (targetingLine != null) Destroy(targetingLine.gameObject);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameMonster hitMonster = null;

        foreach (RaycastResult result in results)
        {
            GameMonster monster = result.gameObject.GetComponentInParent<GameMonster>();
            if (monster != null)
            {
                hitMonster = monster;
                break;
            }
        }

        if (hitMonster != null)
        {
            bool isUsed = GameManager.Instance.UseCardOnMonster(myCardData, hitMonster);

            if (isUsed)
            {
                transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
            }
            else
            {
                ResetCardPosition();
            }
        }
        else
        {
            ResetCardPosition();
        }
    }

    private void ResetCardPosition()
    {
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(-30f, 0.2f).SetRelative(true);
    }
    private void CreateTargetingLine()
    {
        GameObject lineObj = new GameObject("TargetingLine");
        targetingLine = lineObj.AddComponent<LineRenderer>();

        AnimationCurve arrowCurve = new AnimationCurve();

        // 1. 선의 시작 (카드 쪽) : 얇은 화살표 몸통
        arrowCurve.AddKey(new Keyframe(0f, 0.15f));

        // 2. 화살촉 직전 : 여기까지 몸통 굵기 유지
        arrowCurve.AddKey(new Keyframe(0.85f, 0.15f));

        // 3. 화살촉 시작 : 여기서부터 화살촉 윗부분처럼 굵어짐
        arrowCurve.AddKey(new Keyframe(0.85f, 0.6f));

        // 4. 선의 끝 (마우스 쪽) : 화살촉 끝부분처럼 뾰족하게 0으로 모임
        arrowCurve.AddKey(new Keyframe(1f, 0f));

        // 깎아낸 화살표 모양을 적용!
        targetingLine.widthCurve = arrowCurve;

        // 전체 화살표 크기를 키우고 싶다면 이 숫자를 조절하세요 (예: 1.5f, 2f)
        targetingLine.widthMultiplier = 1f;

        Shader uiShader = Shader.Find("Sprites/Default");
        if (uiShader != null)
        {
            targetingLine.material = new Material(uiShader);
        }

        targetingLine.startColor = Color.red;
        targetingLine.endColor = Color.red;

        targetingLine.sortingLayerName = "Default";
        targetingLine.sortingOrder = 555500000;

        targetingLine.positionCount = 2;
    }
}