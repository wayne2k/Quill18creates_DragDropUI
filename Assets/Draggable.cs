using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum ItemTypes { WEAPON, HEAD, CHEST, LEGS, FEET, INVENTORY }
	public ItemTypes itemType = ItemTypes.WEAPON;
	
	[HideInInspector] public Transform parentToReturnTo;
	[HideInInspector] public Transform placeholderParent;


	Vector3 dragOffset;
	GameObject placeholder = null;

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("Begin Drag");
		placeholder = new GameObject ("PlaceHolder");
		placeholder.transform.SetParent (transform.parent);
		LayoutElement layoutElement = placeholder.AddComponent <LayoutElement> ();
		layoutElement.preferredWidth = GetComponent<LayoutElement> ().preferredWidth;
		layoutElement.preferredHeight = GetComponent<LayoutElement> ().preferredHeight;
		layoutElement.flexibleWidth = 0f;
		layoutElement.flexibleHeight = 0f;

		placeholder.transform.SetSiblingIndex (transform.GetSiblingIndex ());

		dragOffset = transform.position - (Vector3) eventData.position;

		parentToReturnTo = transform.parent;
		placeholderParent = parentToReturnTo;
		transform.SetParent (transform.parent.parent);

		GetComponent<CanvasGroup> ().blocksRaycasts = false;


	}

	public void OnDrag (PointerEventData eventData)
	{
//		Debug.Log ("Draggign");

		transform.position = (Vector3) eventData.position + dragOffset;

		if (placeholder.transform.parent != placeholderParent)
		{
			placeholder.transform.SetParent (placeholderParent);
		}

		int newSiblingIndex = placeholderParent.childCount;

		for (int i = 0; i < placeholderParent.childCount; i++)
		{
			if (transform.position.x < placeholderParent.GetChild (i).transform.position.x)
			{
				newSiblingIndex = i;

				if (placeholder.transform.GetSiblingIndex () < newSiblingIndex)
				{
					newSiblingIndex --;
				}
				break;
			}
		}
		placeholder.transform.SetSiblingIndex (newSiblingIndex);
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		Debug.Log ("End Drag");

		dragOffset = Vector3.zero;

		transform.SetParent (parentToReturnTo);
		transform.SetSiblingIndex (placeholder.transform.GetSiblingIndex ());

		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		Destroy (placeholder);
	}
}
