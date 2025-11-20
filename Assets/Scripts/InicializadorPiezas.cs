using UnityEngine;

public class InicializadorPiezas : MonoBehaviour
{
    // --- ARRASTRA AQUÍ TUS 6 PREFABS BASE (solo los modelos) ---
    public GameObject peonPrefab;
    public GameObject torrePrefab;
    public GameObject caballoPrefab;
    public GameObject alfilPrefab;
    public GameObject reinaPrefab;
    public GameObject reyPrefab;

    // --- ARRRASTRA AQUÍ TUS MATERIALES ---
    public Material materialBlanco;
    public Material materialNegro;

    private CrearCasillas gestorTablero;

    void Start()
    {
        // --- ¡CAMBIO CLAVE! ---
        // Obtenemos la referencia del gestor del tablero desde el GameManager
        // Así, ambos scripts usan la MISMA referencia
        gestorTablero = GameManager.instance.gestorTablero;

        if (gestorTablero == null)
        {
            Debug.LogError("ERROR: No se pudo obtener la referencia del tablero desde el GameManager. Asegúrate de que el tablero se crea antes que las piezas.");
            return;
        }

        // Esperamos un frame a que el tablero se cree antes de colocar las piezas
        Invoke("ColocarPiezas", 0.1f);
    }

    void ColocarPiezas()
    {
        if (gestorTablero == null || gestorTablero.casillas == null)
        {
            Debug.LogError("El gestor del tablero o su array de casillas no está disponible.");
            return;
        }

        // --- Fila de peones ---
        for (int i = 0; i < 8; i++)
        {
            CrearPiezaEn(i, 1, peonPrefab, TipoPieza.PEON, ColorPieza.BLANCO);
            CrearPiezaEn(i, 6, peonPrefab, TipoPieza.PEON, ColorPieza.NEGRO);
        }

        // --- Piezas blancas (fila 0) ---
        CrearPiezaEn(0, 0, torrePrefab, TipoPieza.TORRE, ColorPieza.BLANCO);
        CrearPiezaEn(7, 0, torrePrefab, TipoPieza.TORRE, ColorPieza.BLANCO);
        CrearPiezaEn(1, 0, caballoPrefab, TipoPieza.CABALLO, ColorPieza.BLANCO);
        CrearPiezaEn(6, 0, caballoPrefab, TipoPieza.CABALLO, ColorPieza.BLANCO);
        CrearPiezaEn(2, 0, alfilPrefab, TipoPieza.ALFIL, ColorPieza.BLANCO);
        CrearPiezaEn(5, 0, alfilPrefab, TipoPieza.ALFIL, ColorPieza.BLANCO);
        CrearPiezaEn(3, 0, reinaPrefab, TipoPieza.REINA, ColorPieza.BLANCO);
        CrearPiezaEn(4, 0, reyPrefab, TipoPieza.REY, ColorPieza.BLANCO);

        // --- Piezas negras (fila 7) ---
        CrearPiezaEn(0, 7, torrePrefab, TipoPieza.TORRE, ColorPieza.NEGRO);
        CrearPiezaEn(7, 7, torrePrefab, TipoPieza.TORRE, ColorPieza.NEGRO);
        CrearPiezaEn(1, 7, caballoPrefab, TipoPieza.CABALLO, ColorPieza.NEGRO);
        CrearPiezaEn(6, 7, caballoPrefab, TipoPieza.CABALLO, ColorPieza.NEGRO);
        CrearPiezaEn(2, 7, alfilPrefab, TipoPieza.ALFIL, ColorPieza.NEGRO);
        CrearPiezaEn(5, 7, alfilPrefab, TipoPieza.ALFIL, ColorPieza.NEGRO);
        CrearPiezaEn(3, 7, reinaPrefab, TipoPieza.REINA, ColorPieza.NEGRO);
        CrearPiezaEn(4, 7, reyPrefab, TipoPieza.REY, ColorPieza.NEGRO);
    }

    // --- MÉTODO SUPER-PODEROSO ---
    void CrearPiezaEn(int x, int y, GameObject prefabPieza, TipoPieza tipo, ColorPieza color)
    {
        if (prefabPieza == null) return;

        GameObject casilla = gestorTablero.casillas[x, y];
        if (casilla != null)
        {
            Vector3 posicionPieza = casilla.transform.position;
            posicionPieza.y = 1.1f;

            // --- ¡NUEVO MÉTODO DEFINITIVO! ---
            // 1. Instanciamos la pieza en el mundo, SIN asignarle un padre todavía.
            // Esto asegura que su escala sea la del prefab (1,1,1).
            GameObject nuevaPieza = Instantiate(prefabPieza, posicionPieza, Quaternion.identity);

            // 2. AHORA sí, la asignamos como hija de la casilla.
            // El 'true' le dice a Unity que mantenga la posición, rotación y escala mundiales,
            // ajustando la escala local automáticamente para compensar la del padre.
            nuevaPieza.transform.SetParent(casilla.transform, true);

            // --- El resto del código sigue igual ---
            Rigidbody rb = nuevaPieza.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            nuevaPieza.AddComponent<MeshCollider>();
            Renderer rendererPieza = nuevaPieza.GetComponent<Renderer>();
            if (rendererPieza != null)
            {
                rendererPieza.material = (color == ColorPieza.BLANCO) ? materialBlanco : materialNegro;
            }

            AtributosPieza atributos = nuevaPieza.AddComponent<AtributosPieza>();
            atributos.tipo = tipo;
            atributos.color = color;
            atributos.posicionEnTablero = new Vector2Int(x, y);
        }
    }
}