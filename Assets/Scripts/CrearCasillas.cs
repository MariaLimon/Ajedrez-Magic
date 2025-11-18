using UnityEngine;

public class CrearCasillas : MonoBehaviour
{
    public GameObject CasillaPreFab;
    public int Ancho = 8; // Valores por defecto para un tablero de ajedrez
    public int Alto = 8;

    public Material Negro;
    public Material Blanco;

    // --- NUEVO ---
    // Permite mover todo el tablero desde el Inspector
    public Vector3 posicionCentral = Vector3.zero;

    // --- NUEVO ---
    // Guardaremos una referencia a cada casilla para poder acceder a ellas más tarde
    public GameObject[,] casillas; 

    void Start()
    {
        Crear();
    }

    public void Crear()
    {
        // Inicializamos el array de casillas
        casillas = new GameObject[Ancho, Alto];
        
        int cont = 0;
        for (int i = 0; i < Ancho; i++)
        {
            for (int x = 0; x < Alto; x++)
            {
                // --- CAMBIO CLAVE ---
                // Calculamos la posición para que el tablero quede centrado en 'posicionCentral'
                // i - (Ancho / 2.0f) mueve el eje X
                // x - (Alto / 2.0f) mueve el eje Z
                Vector3 posicionCasilla = new Vector3(i - (Ancho / 2.0f), 0, x - (Alto / 2.0f)) + posicionCentral;
                
                GameObject casillaTemp = Instantiate(CasillaPreFab, posicionCasilla, Quaternion.identity);
                casillaTemp.name = "Casilla_" + i + "_" + x; // Les damos un nombre útil
                casillaTemp.transform.parent = this.transform; // Opcional: organiza la jerarquía

                if ((i + x) % 2 == 0)
                {
                    casillaTemp.GetComponent<Casiilla>().PonerColor(Negro);
                }
                else
                {
                    casillaTemp.GetComponent<Casiilla>().PonerColor(Blanco);
                }
                
                casillaTemp.GetComponent<Casiilla>().NumCasilla = cont;
                casillaTemp.GetComponent<Casiilla>().coordenada = new Vector2Int(i, x); // --- NUEVO ---

                // --- NUEVO ---
                // Guardamos la casilla en nuestro array
                casillas[i, x] = casillaTemp;

                cont++;
            }
        }
    }
}