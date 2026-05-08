using UnityEngine;
using UnityEngine.InputSystem;

public class HandSystem : MonoBehaviour
{
    [Header("Hand Anchors")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Layer Settings")]
    public LayerMask grabMask; 

    private GameObject leftItem;
    private GameObject rightItem;

    // We change these to accept 'InputAction.CallbackContext'
    public void OnGrabLeft(InputAction.CallbackContext context) 
    { 
        // 100% FIX: Only run if the button was JUST pressed
        if (context.started) TryAction(true); 
    }

    public void OnGrabRight(InputAction.CallbackContext context) 
    { 
        if (context.started) TryAction(false); 
    }

    void TryAction(bool isLeft)
    {
        if (isLeft && leftItem != null) { Drop(true); return; }
        if (!isLeft && rightItem != null) { Drop(false); return; }

        PickUp(isLeft);
    }

void PickUp(bool isLeft)
{
    // Shoot the ray
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;

    // The mask must include "Default" in the Inspector for this to hit!
    if (Physics.Raycast(ray, out hit, 2.5f, grabMask))
    {
        // Debug to see what you're hitting if it fails
        Debug.Log("Hit: " + hit.transform.name + " with tag: " + hit.transform.tag);

        if (hit.transform.CompareTag("Item"))
        {
            Grab(hit.transform.gameObject, isLeft);
        }
    }
}
void Grab(GameObject obj, bool isLeft)
{
    Transform hand = isLeft ? leftHand : rightHand;
    if (isLeft) leftItem = obj; else rightItem = obj;

    // 1. Set the parent first
    obj.transform.SetParent(hand);

    // 2. Check if the object has a custom GripPoint
    Transform grip = obj.transform.Find("GripPoint");

    if (grip != null)
    {
        // MATH: Offset the object so the GripPoint is at (0,0,0) of the hand
        obj.transform.localPosition = -grip.localPosition;
        obj.transform.localRotation = Quaternion.Inverse(grip.localRotation);
    }
    else
    {
        // Fallback: Just snap to center if no GripPoint exists
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    obj.layer = LayerMask.NameToLayer("HeldItem");

    if (obj.GetComponent<Rigidbody>())
        obj.GetComponent<Rigidbody>().isKinematic = true;
}

    void Drop(bool isLeft)
    {
        GameObject item = isLeft ? leftItem : rightItem;
        if (item == null) return;

        item.transform.SetParent(null);
        item.layer = LayerMask.NameToLayer("Default");

        if (item.GetComponent<Rigidbody>())
            item.GetComponent<Rigidbody>().isKinematic = false;
        
        if (isLeft) leftItem = null; else rightItem = null;
    }
}