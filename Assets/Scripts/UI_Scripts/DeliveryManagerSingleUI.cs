using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeUI(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconTemplate)
        {
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.ingridientKitchenSOList)
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate,iconContainer);
            iconTemplateTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
            iconTemplateTransform.gameObject.SetActive(true);
        }

    }
}
