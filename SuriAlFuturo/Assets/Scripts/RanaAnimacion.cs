using UnityEngine;

[RequireComponent(typeof(Rana))]
public class RanaAnimacion : MonoBehaviour
{

    [SerializeField] Animator _animator;

    //Nombre del parámetro Float en el AnimatorController (0 = inicio, 1 = fin del salto)" 
    [SerializeField] string _parametroSalto = "ProgresoSalto";

    Rana _rana;

    void Awake()
    {
        _rana = GetComponent<Rana>();
    }

    void OnEnable()
    {
        _rana.OnProgresoSalto += OnProgresoSalto;
    }

    void OnDisable()
    {
        _rana.OnProgresoSalto -= OnProgresoSalto;
    }

    void OnProgresoSalto(float progreso)
    {
        _animator.SetFloat(_parametroSalto, progreso);
    }
}