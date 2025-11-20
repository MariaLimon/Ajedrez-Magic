using UnityEngine;

public class CrearCasillas : MonoBehaviour
{
    public GameObject CasillaPreFab;
    public int Ancho = 8;
    public int Alto = 8;

    public Material Negro; // Material para las casillas negras
    public Material Blanco; // Material para las casillas blancas

    public Vector3 posicionCentral = Vector3.zero;
    public GameObject[,] casillas;

    void Start()
    {
        Crear();
    }

    public void Crear()
    {
        casillas = new GameObject[Ancho, Alto];

        int cont = 0;
        for (int i = 0; i < Ancho; i++)
        {
            for (int x = 0; x < Alto; x++)
            {
                Vector3 posicionCasilla = new Vector3(i - (Ancho / 2.0f), 0, x - (Alto / 2.0f)) + posicionCentral;

                GameObject casillaTemp = Instantiate(CasillaPreFab, posicionCasilla, Quaternion.identity);
                casillaTemp.name = "Casilla_" + i + "_" + x;
                casillaTemp.transform.parent = this.transform;

                // --- ¡CÓDIGO MODIFICADO Y OPTIMIZADO! ---
                // Obtenemos el script de la casilla primero
                Casilla scriptCasilla = casillaTemp.GetComponent<Casilla>();
                if (scriptCasilla != null)
                {
                    // Asignamos la coordenada
                    scriptCasilla.coordenada = new Vector2Int(i, x);

                    // Obtenemos el Renderer para cambiar el color
                    Renderer rendererCasilla = casillaTemp.GetComponent<Renderer>();
                    if (rendererCasilla != null)
                    {
                        Material materialAsignado; // Variable para guardar el material que usaremos

                        if ((i + x) % 2 == 0)
                        {
                            materialAsignado = Negro;
                        }
                        else
                        {
                            materialAsignado = Blanco;
                        }

                        // Asignamos el material a la casilla
                        rendererCasilla.material = materialAsignado;

                        // --- ¡LÍNEA CLAVE! ---
                        // Le decimos al script de la casilla cuál es su material original
                        scriptCasilla.SetMaterialOriginal(materialAsignado);
                    }
                    else
                    {
                        Debug.LogError("El prefab de la casilla no tiene un componente Renderer.");
                    }
                }
                else
                {
                    Debug.LogError("El prefab de la casilla no tiene el script Casilla.");
                }

                casillas[i, x] = casillaTemp;
                cont++;
            }
        }

        // --- ¡NUEVO CÓDIGO AÑADIDO! ---
        // Una vez que el tablero está completamente creado, notificamos al GameManager.
        Debug.Log("Tablero creado. Buscando GameManager para inicializarlo...");

        // Buscamos la instancia del GameManager en la escena
        if (GameManager.instance != null)
        {
            // Le pasamos la referencia a este script (el "creador") para que tenga acceso al array de casillas.
            GameManager.instance.InicializarConTablero(this);
        }
        else
        {
            Debug.LogError("ERROR CRÍTICO: No se encontró un objeto con el script GameManager en la escena. Asegúrate de que existe.");
        }
    }
}