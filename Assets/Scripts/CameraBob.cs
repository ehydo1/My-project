using UnityEngine;

public class CameraBiologicalFollow : MonoBehaviour
{
    [Header("Tracking")]
    public Transform targetBone; // Drag your Hip or Spine bone here
    public Vector3 offset;       // Adjust this until the camera is at head height
    
    [Header("Smoothing")]
    [Range(1f, 50f)] public float followSharpness = 15f; 

    void LateUpdate()
    {
        if (targetBone == null) return;

        // 1. Calculate where the camera SHOULD be based on the bone
        // Using TransformPoint accounts for the player's rotation
        Vector3 targetPos = targetBone.TransformPoint(offset);

        // 2. Smoothly move to that position
        // This 'Lerp' removes the high-frequency jitter from animations
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSharpness);

        // NOTE: We do NOT touch rotation here. 
        // Your PlayerController script is still rotating this camera via mouse.
    }
}