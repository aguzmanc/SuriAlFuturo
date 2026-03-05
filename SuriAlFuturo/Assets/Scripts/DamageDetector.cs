using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var mov = GetComponentInParent<CharacterMovement>();    

        mov.Damage();
    }
}
