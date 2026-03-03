using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Rana))]
public class RanaEditor : Editor
{
    static readonly Color _colorPuntoNormal = new Color(1f, 0.85f, 0f);
    static readonly Color _colorPuntoSelected = new Color(0f, 1f, 0.4f);
    static readonly Color _colorLinea = new Color(0.3f, 0.8f, 1f, 0.7f);
    static readonly Color _colorCirculo = new Color(1f, 0.5f, 0f, 0.25f);

    Rana _rana;
    bool _modoEdicionPuntos;
    int _puntoSeleccionado = -1;

    void OnEnable()
    {
        _rana = (Rana)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("Rana - Configuración", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        DrawPropertiesExcluding(serializedObject, "m_Script", "_puntos");

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Puntos de movimiento", EditorStyles.boldLabel);

        GUI.backgroundColor = _modoEdicionPuntos ? new Color(0.4f, 1f, 0.5f) : Color.white;
        if (GUILayout.Button(_modoEdicionPuntos ? "Salir del modo edición" : " Editar puntos en Escena"))
        {
            _modoEdicionPuntos = !_modoEdicionPuntos;
            _puntoSeleccionado = -1;
            SceneView.RepaintAll();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("➕ Agregar punto"))
        {
            Undo.RecordObject(_rana, "Agregar punto Rana");
            // Nuevo punto en local, desplazado aleatoriamente desde el origen local
            Vector3 nuevoLocal = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            _rana.Puntos.Add(nuevoLocal);
            _puntoSeleccionado = _rana.Puntos.Count - 1;
            EditorUtility.SetDirty(_rana);
            SceneView.RepaintAll();
        }

        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("🗑️ Limpiar todos"))
        {
            if (EditorUtility.DisplayDialog("Limpiar puntos", "¿Eliminar todos los puntos?", "Sí", "Cancelar"))
            {
                Undo.RecordObject(_rana, "Limpiar puntos Rana");
                _rana.Puntos.Clear();
                _puntoSeleccionado = -1;
                EditorUtility.SetDirty(_rana);
                SceneView.RepaintAll();
            }
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(6);

        if (_rana.Puntos.Count == 0)
        {
            EditorGUILayout.HelpBox("No hay puntos. Agrega al menos uno.", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < _rana.Puntos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.color = (i == _puntoSeleccionado) ? _colorPuntoSelected : Color.white;
                if (GUILayout.Button($"P{i}", GUILayout.Width(30)))
                {
                    _puntoSeleccionado = (_puntoSeleccionado == i) ? -1 : i;
                    SceneView.RepaintAll();
                }
                GUI.color = Color.white;

                // Mostrar y editar coordenadas locales X y Z
                Vector3 local = _rana.Puntos[i];
                EditorGUI.BeginChangeCheck();
                float nuevoX = EditorGUILayout.FloatField(local.x, GUILayout.Width(60));
                float nuevoZ = EditorGUILayout.FloatField(local.z, GUILayout.Width(60));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_rana, "Mover punto Rana");
                    _rana.Puntos[i] = new Vector3(nuevoX, 0f, nuevoZ);
                    EditorUtility.SetDirty(_rana);
                    SceneView.RepaintAll();
                }

                GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
                if (GUILayout.Button("✕", GUILayout.Width(24)))
                {
                    Undo.RecordObject(_rana, "Eliminar punto Rana");
                    _rana.Puntos.RemoveAt(i);
                    if (_puntoSeleccionado >= _rana.Puntos.Count) _puntoSeleccionado = -1;
                    EditorUtility.SetDirty(_rana);
                    SceneView.RepaintAll();
                    break;
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space(6);
        if (_modoEdicionPuntos)
        {
            EditorGUILayout.HelpBox(
                "Modo edición activo:\n" +
                "• Arrastra las esferas para mover puntos (solo XZ)\n" +
                "• Ctrl+Click en la escena para agregar un punto nuevo",
                MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (_rana == null || _rana.Puntos == null) return;

        float yMundo = _rana.transform.position.y;
        UnityEngine.Event e = UnityEngine.Event.current;

        // Ctrl+Click para agregar punto
        if (_modoEdicionPuntos && e.type == EventType.MouseDown && e.button == 0 && e.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            float t = (yMundo - ray.origin.y) / ray.direction.y;

            if (t > 0f)
            {
                Vector3 hitMundo = ray.origin + ray.direction * t;
                hitMundo.y = yMundo;

                // Convertir posición mundo a local antes de guardar
                Vector3 hitLocal = _rana.transform.InverseTransformPoint(hitMundo);
                hitLocal.y = 0f;

                Undo.RecordObject(_rana, "Agregar punto Rana");
                _rana.Puntos.Add(hitLocal);
                _puntoSeleccionado = _rana.Puntos.Count - 1;
                EditorUtility.SetDirty(_rana);
                e.Use();
                Repaint();
            }
        }

        // Círculo de distancia máxima
        Handles.color = _colorCirculo;
        Vector3 posRana = new Vector3(_rana.transform.position.x, yMundo, _rana.transform.position.z);
        DrawHandleCircle(posRana, _rana.DistanciaMaxima);

        // Líneas entre puntos dentro del rango
        for (int i = 0; i < _rana.Puntos.Count; i++)
        {
            Vector3 mundoI = _rana.transform.TransformPoint(_rana.Puntos[i]);
            Vector3 pi = new Vector3(mundoI.x, yMundo, mundoI.z);

            for (int j = i + 1; j < _rana.Puntos.Count; j++)
            {
                Vector3 mundoJ = _rana.transform.TransformPoint(_rana.Puntos[j]);
                Vector3 pj = new Vector3(mundoJ.x, yMundo, mundoJ.z);
                float dist = Vector3.Distance(
                    new Vector3(pi.x, 0f, pi.z),
                    new Vector3(pj.x, 0f, pj.z)
                );

                if (dist <= _rana.DistanciaMaxima)
                {
                    Handles.color = _colorLinea;
                    Handles.DrawDottedLine(pi, pj, 4f);
                }
            }
        }

        // Handles por punto
        for (int i = 0; i < _rana.Puntos.Count; i++)
        {
            Vector3 mundo = _rana.transform.TransformPoint(_rana.Puntos[i]);
            Vector3 p = new Vector3(mundo.x, yMundo, mundo.z);
            bool seleccionado = (i == _puntoSeleccionado);

            Handles.color = seleccionado ? _colorPuntoSelected : _colorPuntoNormal;

            Handles.Label(p + Vector3.up * 0.4f, $"P{i}",
                new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = { textColor = seleccionado ? _colorPuntoSelected : _colorPuntoNormal }
                });

            float tamano = HandleUtility.GetHandleSize(p) * 0.12f;
            if (Handles.Button(p, Quaternion.identity, tamano, tamano * 1.5f, Handles.SphereHandleCap))
            {
                _puntoSeleccionado = (_puntoSeleccionado == i) ? -1 : i;
                Repaint();
            }

            if (_modoEdicionPuntos)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 nuevaPosMundo = Handles.FreeMoveHandle(
                    p,
                    HandleUtility.GetHandleSize(p) * 0.15f,
                    Vector3.zero,
                    Handles.CircleHandleCap
                );

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_rana, "Mover punto Rana");
                    // Forzar Y mundo, luego convertir a local
                    nuevaPosMundo.y = yMundo;
                    Vector3 nuevoLocal = _rana.transform.InverseTransformPoint(nuevaPosMundo);
                    nuevoLocal.y = 0f;
                    _rana.Puntos[i] = nuevoLocal;
                    _puntoSeleccionado = i;
                    EditorUtility.SetDirty(_rana);
                }
            }
        }
    }

    void DrawHandleCircle(Vector3 center, float radius, int segments = 48)
    {
        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            points[i] = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
        }

        Handles.DrawPolyLine(points);
    }
}