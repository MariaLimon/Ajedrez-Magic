using UnityEngine;

public class InicializadorPiezas : MonoBehaviour
{
    // --- Arrastra tus prefabs aquí desde el Inspector ---
    public GameObject peonBlancoPrefab, peonNegroPrefab;
    public GameObject torreBlancaPrefab, torreNegraPrefab;
    public GameObject caballoBlancoPrefab, caballoNegroPrefab;
    public GameObject alfilBlancoPrefab, alfilNegroPrefab;
    public GameObject reinaBlancaPrefab, reinaNegraPrefab;
    public GameObject reyBlancoPrefab, reyNegroPrefab;

    // Referencia al script que maneja el tablero
    private CrearCasillas gestorTablero;

    void Start()
    {
        // Obtenemos la referencia al otro script en el mismo objeto
        gestorTablero = GetComponent<CrearCasillas>();
        
        // Esperamos un frame a que el tablero se cree antes de colocar las piezas
        // O mejor, llama a ColocarPiezas() desde Tablerocript después de Crear().
        // Para este ejemplo, lo llamamos aquí.
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
            CrearPiezaEn(i, 1, peonBlancoPrefab);
            CrearPiezaEn(i, 6, peonNegroPrefab);
        }

        // --- Piezas blancas ---
        CrearPiezaEn(0, 0, torreBlancaPrefab);
        CrearPiezaEn(7, 0, torreBlancaPrefab);
        CrearPiezaEn(1, 0, caballoBlancoPrefab);
        CrearPiezaEn(6, 0, caballoBlancoPrefab);
        CrearPiezaEn(2, 0, alfilBlancoPrefab);
        CrearPiezaEn(5, 0, alfilBlancoPrefab);
        CrearPiezaEn(3, 0, reinaBlancaPrefab);
        CrearPiezaEn(4, 0, reyBlancoPrefab);

        // --- Piezas negras ---
        CrearPiezaEn(0, 7, torreNegraPrefab);
        CrearPiezaEn(7, 7, torreNegraPrefab);
        CrearPiezaEn(1, 7, caballoNegroPrefab);
        CrearPiezaEn(6, 7, caballoNegroPrefab);
        CrearPiezaEn(2, 7, alfilNegroPrefab);
        CrearPiezaEn(5, 7, alfilNegroPrefab);
        CrearPiezaEn(3, 7, reinaNegraPrefab);
        CrearPiezaEn(4, 7, reyNegroPrefab);
    }

    void CrearPiezaEn(int x, int y, GameObject prefabPieza)
    {
        if (prefabPieza == null) return;

        GameObject casilla = gestorTablero.casillas[x, y];
        if (casilla != null)
        {
            // Instanciamos la pieza ligeramente encima de la casilla
            Vector3 posicionPieza = casilla.transform.position;
            posicionPieza.y = 1.1f; // Un poco elevada para que no "clipee" con el tablero
            
            Instantiate(prefabPieza, posicionPieza, Quaternion.identity);
        }
    }
}