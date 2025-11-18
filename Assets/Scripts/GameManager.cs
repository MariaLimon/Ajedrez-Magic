using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton para acceso fácil

    private GameObject piezaSeleccionada;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Llamado por la pieza cuando se hace clic en ella
    public void SeleccionarPieza(GameObject pieza)
    {
        // Si ya había una pieza seleccionada, la deseleccionamos
        if (piezaSeleccionada != null)
        {
            // Aquí podrías devolverla a su posición original o cambiar su color
        }

        piezaSeleccionada = pieza;
        Debug.Log("Pieza seleccionada: " + pieza.name);
    }

    // Llamado por la casilla cuando se hace clic en ella
    public void IntentarMoverA(GameObject casillaDestino)
    {
        if (piezaSeleccionada != null)
        {
            Debug.Log("Moviendo " + piezaSeleccionada.name + " a " + casillaDestino.name);

            // --- MOVIMIENTO SIMPLE ---
            // Obtenemos la posición de la casilla y movemos la pieza
            Vector3 destino = casillaDestino.transform.position;
            destino.y = 0.2f; // La misma altura que las piezas iniciales
            
            piezaSeleccionada.transform.position = destino;

            // Deseleccionamos la pieza después de moverla
            piezaSeleccionada = null;
        }
        else
        {
            Debug.Log("No hay ninguna pieza seleccionada para mover.");
        }
    }
}