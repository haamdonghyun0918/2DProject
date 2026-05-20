using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

public class SlotCardUi : UiBase
{
    [SerializeField] private Text text_Name;
    [SerializeField] private Text text_Description;
    [SerializeField] private Text text_Damage;
    [SerializeField] private Image image_Card;
    [SerializeField] private Image image_Icon;
    [SerializeField] private Image image_Damage;

    public void SetUp(CardData cardData)
    {
        if (cardData == null) return;

        text_Name.text = cardData.Name;
        text_Description.text = cardData.Description;
        text_Damage.text = cardData.Damage.ToString();

        Sprite iCard = Resources.Load<Sprite>(cardData.ImageCardAddress);
        Sprite iIcon = Resources.Load<Sprite>(cardData.ImageIconAddress);
        Sprite iDamage = Resources.Load<Sprite>(cardData.ImageDamageAddress);
        if (iCard != null) image_Card.sprite = iCard;
        if (iIcon != null) image_Icon.sprite = iIcon;
        if (iDamage != null) image_Damage.sprite = iDamage;
    }
}