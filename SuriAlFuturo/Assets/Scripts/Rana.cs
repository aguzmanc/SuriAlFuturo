using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rana : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    [SerializeField] List<Vector3> _puntos = new List<Vector3>();

    [Header("Configuración de distancia")]
    [Tooltip("Distancia máxima para considerar un punto como destino válido")]
    [SerializeField] float _distanciaMaxima = 5f;

    [Header("Configuración de tiempo")]
    [Tooltip("Tiempo mínimo de espera entre saltos (segundos)")]
    [SerializeField] float _tiempoEsperaMin = 1f;
    [Tooltip("Tiempo máximo de espera entre saltos (segundos)")]
    [SerializeField] float _tiempoEsperaMax = 3f;

    [Header("Configuración del salto")]
    [Tooltip("Duración del salto en segundos")]
    [SerializeField] float _duracionSalto = 0.5f;
    [Tooltip("Altura máxima de la curva del salto")]
    [SerializeField] float _alturaSalto = 1.5f;
    [Tooltip("Curva de animación del salto (X: tiempo 0-1, Y: altura 0-1)")]
    [SerializeField] AnimationCurve _curvaSalto = AnimationCurve.EaseInOut(0, 0, 1, 0);

    // Propiedades de lectura para RanaEditor
    public List<Vector3> Puntos => _puntos;
    public float DistanciaMaxima => _distanciaMaxima;
    public float YFijo => _yFijo;

    int _indicePuntoActual = -1;
    float _yFijo;

    void Start()
    {
        _yFijo = transform.position.y;

        if (_curvaSalto == null || _curvaSalto.length == 0)
        {
            _curvaSalto = new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 2f),
                new Keyframe(0.5f, 1f, 0f, 0f),
                new Keyframe(1f, 0f, -2f, 0f)
            );
        }

        if (_puntos.Count > 0)
            _indicePuntoActual = ObtenerIndicePuntoMasCercano();

        StartCoroutine(RutinaMovimiento());
    }

    IEnumerator RutinaMovimiento()
    {
        while (true)
        {
            float espera = Random.Range(_tiempoEsperaMin, _tiempoEsperaMax);
            yield return new WaitForSeconds(espera);

            List<int> puntosValidos = ObtenerPuntosValidos();

            if (puntosValidos.Count == 0)
                continue;

            int indiceElegido = puntosValidos[Random.Range(0, puntosValidos.Count)];
            Vector3 destino = _puntos[indiceElegido];

            yield return StartCoroutine(Saltar(destino));

            _indicePuntoActual = indiceElegido;
        }
    }

    IEnumerator Saltar(Vector3 destino)
    {
        Vector3 origen = transform.position;
        origen.y = _yFijo;
        destino.y = _yFijo;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < _duracionSalto)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / _duracionSalto);

            Vector3 posicionBase = Vector3.Lerp(origen, destino, t);
            posicionBase.y = _yFijo + _curvaSalto.Evaluate(t) * _alturaSalto;
            transform.position = posicionBase;

            Vector3 direccion = destino - origen;
            direccion.y = 0f;
            if (direccion.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direccion.normalized);

            yield return null;
        }

        transform.position = new Vector3(destino.x, _yFijo, destino.z);
    }

    List<int> ObtenerPuntosValidos()
    {
        List<int> validos = new List<int>();
        Vector3 posActual = new Vector3(transform.position.x, 0f, transform.position.z);

        for (int i = 0; i < _puntos.Count; i++)
        {
            if (i == _indicePuntoActual) continue;

            Vector3 puntoPlanar = new Vector3(_puntos[i].x, 0f, _puntos[i].z);
            if (Vector3.Distance(posActual, puntoPlanar) <= _distanciaMaxima)
                validos.Add(i);
        }

        return validos;
    }

    int ObtenerIndicePuntoMasCercano()
    {
        int indiceMasCercano = 0;
        float distanciaMinima = float.MaxValue;
        Vector3 posActual = new Vector3(transform.position.x, 0f, transform.position.z);

        for (int i = 0; i < _puntos.Count; i++)
        {
            Vector3 puntoPlanar = new Vector3(_puntos[i].x, 0f, _puntos[i].z);
            float d = Vector3.Distance(posActual, puntoPlanar);

            if (d < distanciaMinima)
            {
                distanciaMinima = d;
                indiceMasCercano = i;
            }
        }

        return indiceMasCercano;
    }

    void OnDrawGizmos()
    {
        if (_puntos == null || _puntos.Count == 0) return;

        float yBase = Application.isPlaying ? _yFijo : transform.position.y;

        for (int i = 0; i < _puntos.Count; i++)
        {
            Vector3 p = new Vector3(_puntos[i].x, yBase, _puntos[i].z);

            Gizmos.color = (i == _indicePuntoActual) ? Color.green : Color.yellow;
            Gizmos.DrawSphere(p, 0.15f);

            for (int j = i + 1; j < _puntos.Count; j++)
            {
                Vector3 q = new Vector3(_puntos[j].x, yBase, _puntos[j].z);
                float dist = Vector3.Distance(
                    new Vector3(p.x, 0f, p.z),
                    new Vector3(q.x, 0f, q.z)
                );

                if (dist <= _distanciaMaxima)
                {
                    Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.5f);
                    Gizmos.DrawLine(p, q);
                }
            }
        }

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        DrawGizmoCircle(
            new Vector3(transform.position.x, yBase, transform.position.z),
            _distanciaMaxima, 32
        );
    }

    void DrawGizmoCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prev = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 next = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}