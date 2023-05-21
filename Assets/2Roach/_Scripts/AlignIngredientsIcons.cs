using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignIngredientsIcons : MonoBehaviour
{
    [SerializeField] GameObject _iconPrefab;
    [SerializeField] float _xOffset = 0.75f;

    public void ClearIconsList() {
        foreach (Transform icon in transform) {
            Object.Destroy(icon.gameObject);
        }
    } 

    public void DisplayIngredientIcons(List<Ingredient> ingredients) {
        int index = -Mathf.RoundToInt(ingredients.Count / 2);
        foreach (Ingredient ing in ingredients) {
            GameObject newIcon = GameObject.Instantiate(_iconPrefab, transform);
            newIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ing.Icon;
            newIcon.transform.localPosition = new Vector3(_xOffset * index, newIcon.transform.localPosition.y, newIcon.transform.localPosition.z);

            index++;
        }
    }
}
