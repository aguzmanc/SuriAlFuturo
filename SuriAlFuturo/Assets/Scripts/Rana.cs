using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rana : MonoBehaviour
{
    [Header("Puntos de movimiento (mundo)")]
    [SerializeField] List<Vector3> _puntos = new List<Vector3>();

    [Header("Configuracion de distancia")]
    [Tooltip("Distancia maxima para considerar un punto como destino valido")]
    [SerializeField] float _distanciaMaxima = 5f;

    [Header("Configuracion de tiempo")]
    [Tooltip("Tiempo minimo de espera entre saltos (segundos)")]
    [SerializeField] float _tiempoEsperaMin = 1f;
    [Tooltip("Tiempo maximo de espera entre saltos (segundos)")]
    [SerializeField] float _tiempoEsperaMax = 3f;

    [Header("Configuracion del salto")]
    [Tooltip("Duracion del salto en segundos")]
    [SerializeField] float _duracionSalto = 0.5f;
    [Tooltip("Altura maxima de la curva del salto")]
    [SerializeField] float _alturaSalto = 1.5f;
    [Tooltip("Curva de animacion del salto (X: tiempo 0-1, Y: altura 0-1)")]
    [SerializeField] AnimationCurve _curvaSalto = AnimationCurve.EaseInOut(0, 0, 1, 0);

    public event System.Action<float> OnProgresoSalto;

    // Lectura para RanaEditor
    public List<Vector3> Puntos => _puntos;
    public float DistanciaMaxima => _distanciaMaxima;
    public float YFijo => _yFijo;
    public bool Moving => _moving;

    int _indicePuntoActual = -1;
    float _yFijo;
    bool _moving;




    void Start()
    {
        _yFijo = transform.position.y;
        _moving = false;

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
            Vector3 destino = new Vector3(_puntos[indiceElegido].x, _yFijo, _puntos[indiceElegido].z);

            Vector3 direccion = destino - transform.position;
            direccion.y = 0f;
            if (direccion.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direccion.normalized);

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

        _moving = true;
        while (tiempoTranscurrido < _duracionSalto)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / _duracionSalto);

            Vector3 pos = Vector3.Lerp(origen, destino, t);
            pos.y = _yFijo + _curvaSalto.Evaluate(t) * _alturaSalto;
            transform.position = pos;

            OnProgresoSalto?.Invoke(t);
            yield return null;
        }

        _moving = false;

        transform.position = new Vector3(destino.x, _yFijo, destino.z);
        OnProgresoSalto?.Invoke(1f);
    }

    List<int> ObtenerPuntosValidos()
    {
        List<int> validos = new List<int>();
        Vector3 posActual = new Vector3(transform.position.x, 0f, transform.position.z);

        for (int i = 0; i < _puntos.Count; i++)
        {
            if (i == _indicePuntoActual) continue;

            Vector3 planar = new Vector3(_puntos[i].x, 0f, _puntos[i].z);
            if (Vector3.Distance(posActual, planar) <= _distanciaMaxima)
                validos.Add(i);
        }

        return validos;
    }

    int ObtenerIndicePuntoMasCercano()
    {
        int mejor = 0;
        float minDist = float.MaxValue;
        Vector3 posActual = new Vector3(transform.position.x, 0f, transform.position.z);

        for (int i = 0; i < _puntos.Count; i++)
        {
            Vector3 planar = new Vector3(_puntos[i].x, 0f, _puntos[i].z);
            float d = Vector3.Distance(posActual, planar);
            if (d < minDist) { minDist = d; mejor = i; }
        }

        return mejor;
    }

    void OnDrawGizmos()
    {
        if (_puntos == null || _puntos.Count == 0) return;

        // Forzar matriz identidad: los puntos ya estan en mundo, no deben transformarse
        Gizmos.matrix = Matrix4x4.identity;

        float yBase = Application.isPlaying ? _yFijo : transform.position.y;

        for (int i = 0; i < _puntos.Count; i++)
        {
            Vector3 p = new Vector3(_puntos[i].x, yBase, _puntos[i].z);

            Gizmos.color = (i == _indicePuntoActual) ? Color.green : Color.yellow;
            Gizmos.DrawSphere(p, 0.15f);

            for (int j = i + 1; j < _puntos.Count; j++)
            {
                Vector3 q = new Vector3(_puntos[j].x, yBase, _puntos[j].z);
                float dist = Vector3.Distance(new Vector3(p.x, 0f, p.z), new Vector3(q.x, 0f, q.z));

                if (dist <= _distanciaMaxima)
                {
                    Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.5f);
                    Gizmos.DrawLine(p, q);
                }
            }
        }

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        DrawGizmoCircle(new Vector3(transform.position.x, yBase, transform.position.z), _distanciaMaxima, 32);
    }

    void DrawGizmoCircle(Vector3 center, float radius, int segments)
    {
        float step = 360f / segments;
        Vector3 prev = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float a = i * step * Mathf.Deg2Rad;
            Vector3 next = center + new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}