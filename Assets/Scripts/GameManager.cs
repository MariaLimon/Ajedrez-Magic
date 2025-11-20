using UnityEngine;
using System.Collections.Generic;
using System; // Necesario para usar Dictionary

public class GameManager : MonoBehaviour
{
    // Singleton para acceso fácil desde cualquier script
    public static GameManager instance { get; private set; }

    [Header("Configuración Visual")]
    public Material materialCasillaIluminada;
    public Material materialCasillaCaptura;

    [Header("Referencias al Tablero")]
    public CrearCasillas gestorTablero { get; private set; }
    private GameObject[,] casillas;

    // --- ¡NUEVO REGISTRO DE PIEZAS! ---
    // Es mucho más rápido y fiable que buscar en la jerarquía
    private Dictionary<Vector2Int, AtributosPieza> registroDePiezas = new Dictionary<Vector2Int, AtributosPieza>();

    [Header("Estado del Juego")]
    private AtributosPieza piezaSeleccionada;
    private List<Casilla> casillasIluminadas = new List<Casilla>();
    private ColorPieza turnoActual = ColorPieza.BLANCO;

    void Awake()
    {
        // Configuración del Singleton
        if (instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        // El GameManager ya no hace nada al inicio.
        // Simplemente espera a que CrearCasillas lo inicialice.
        Debug.Log("GameManager creado. Esperando la referencia del tablero...");
    }

    #region Lógica de Selección y Movimiento

    // Llamado por la pieza cuando se hace clic en ella
    public void SeleccionarPieza(GameObject piezaObj)
    {
        AtributosPieza pieza = piezaObj.GetComponent<AtributosPieza>();

        // 1. Validar que es una pieza y que es su turno
        if (pieza == null || pieza.color != turnoActual)
        {
            Debug.Log("No es tu turno o la casilla está vacía.");
            DeseleccionarPieza(); // Limpiamos cualquier selección previa
            return;
        }

        // 2. Si ya había una pieza seleccionada, la deseleccionamos primero
        if (piezaSeleccionada != null && piezaSeleccionada != pieza)
        {
            DeseleccionarPieza();
        }

        // 3. Seleccionamos la nueva pieza y calculamos sus movimientos
        piezaSeleccionada = pieza;
        Debug.Log($"Pieza seleccionada: {pieza.tipo} de color {pieza.color}");

        List<Vector2Int> movimientos = CalcularMovimientosPosibles(pieza);
        IluminarCasillas(movimientos);
    }

    // Llamado por la casilla cuando se hace clic en ella
    public void IntentarMoverA(GameObject casillaDestinoObj)
    {
        if (piezaSeleccionada == null)
        {
            Debug.Log("No hay ninguna pieza seleccionada para mover.");
            return;
        }

        // 1. Obtenemos el script Casilla y sus coordenadas
        Casilla scriptCasilla = casillaDestinoObj.GetComponent<Casilla>();
        if (scriptCasilla == null)
        {
            Debug.LogError("El objeto de destino no tiene el script Casilla.");
            return;
        }
        Vector2Int posicionDestino = scriptCasilla.coordenada;

        // 2. Validamos el movimiento
        List<Vector2Int> movimientosPosibles = CalcularMovimientosPosibles(piezaSeleccionada);
        if (!movimientosPosibles.Contains(posicionDestino))
        {
            Debug.Log("Movimiento no válido.");
            return;
        }

        // 3. Ejecutamos el movimiento
        MoverPieza(piezaSeleccionada, posicionDestino);

        // 4. Cambiamos el turno y limpiamos la selección
        CambiarTurno();
        DeseleccionarPieza();
    }

    #endregion

    #region Métodos de Lógica Interna

    private void MoverPieza(AtributosPieza pieza, Vector2Int nuevaPosicion)
    {
        // 1. Comprobar si hay una pieza enemiga en la casilla de destino (captura)
        AtributosPieza piezaEnDestino = GetPiezaEn(nuevaPosicion);
        if (piezaEnDestino != null && piezaEnDestino.color != pieza.color)
        {
            Debug.Log($"Capturando pieza: {piezaEnDestino.tipo}");
            Destroy(piezaEnDestino.gameObject);
            // La eliminamos del registro
            registroDePiezas.Remove(nuevaPosicion);
        }

        // 2. Mover la pieza físicamente
        GameObject casillaDestino = casillas[nuevaPosicion.x, nuevaPosicion.y];
        Vector3 destino = casillaDestino.transform.position;
        destino.y = pieza.gameObject.transform.position.y; // Mantenemos la altura

        pieza.gameObject.transform.position = destino;

        // --- ¡LA LÍNEA CLAVE QUE FALTABA! ---
        // Re-asignamos la pieza como hija de la NUEVA casilla para mantener la jerarquía limpia
        pieza.gameObject.transform.SetParent(casillaDestino.transform, true);

        // 3. Actualizar la posición lógica de la pieza
        Vector2Int posicionAntigua = pieza.posicionEnTablero;
        pieza.posicionEnTablero = nuevaPosicion;

        // 4. Actualizamos el registro
        registroDePiezas.Remove(posicionAntigua); // La quitamos de su vieja posición
        registroDePiezas[nuevaPosicion] = pieza;   // La ponemos en la nueva
    }

    private void CambiarTurno()
    {
        turnoActual = (turnoActual == ColorPieza.BLANCO) ? ColorPieza.NEGRO : ColorPieza.BLANCO;
        Debug.Log($"--- Ahora es el turno de las piezas {turnoActual} ---");
    }

    private void DeseleccionarPieza()
    {
        piezaSeleccionada = null;
        LimpiarCasillasIluminadas();
    }

    // --- NUEVO MÉTODO CLAVE ---
    // Este método será llamado por el script CrearCasillas
    public void InicializarConTablero(CrearCasillas creador)
    {
        gestorTablero = creador;
        casillas = gestorTablero.casillas;
        Debug.Log("¡GameManager inicializado correctamente! El array de casillas tiene " + casillas.Length + " elementos.");
    }

    #endregion

    #region Cálculo de Movimientos (Solo Peón por ahora)

    public List<Vector2Int> CalcularMovimientosPosibles(AtributosPieza pieza)
    {
        List<Vector2Int> movimientos = new List<Vector2Int>();

        if (pieza.tipo == TipoPieza.PEON)
        {
            int direccion = (pieza.color == ColorPieza.BLANCO) ? 1 : -1;
            Vector2Int posicionActual = pieza.posicionEnTablero;

            Vector2Int destinoAdelante = new Vector2Int(posicionActual.x, posicionActual.y + direccion);
            if (EstaDentroDelTablero(destinoAdelante) && GetPiezaEn(destinoAdelante) == null)
            {
                movimientos.Add(destinoAdelante);
            }

            int filaInicial = (pieza.color == ColorPieza.BLANCO) ? 1 : 6;
            if (posicionActual.y == filaInicial)
            {
                Vector2Int destinoDoble = new Vector2Int(posicionActual.x, posicionActual.y + (2 * direccion));
                if (EstaDentroDelTablero(destinoDoble) && GetPiezaEn(destinoDoble) == null)
                {
                    movimientos.Add(destinoDoble);
                }
            }

            Vector2Int capturaIzq = new Vector2Int(posicionActual.x - 1, posicionActual.y + direccion);
            if (EstaDentroDelTablero(capturaIzq))
            {
                AtributosPieza piezaIzq = GetPiezaEn(capturaIzq);
                if (piezaIzq != null && piezaIzq.color != pieza.color)
                {
                    movimientos.Add(capturaIzq);
                }
            }

            Vector2Int capturaDer = new Vector2Int(posicionActual.x + 1, posicionActual.y + direccion);
            if (EstaDentroDelTablero(capturaDer))
            {
                AtributosPieza piezaDer = GetPiezaEn(capturaDer);
                if (piezaDer != null && piezaDer.color != pieza.color)
                {
                    movimientos.Add(capturaDer);
                }
            }
        }
        return movimientos;
    }

    #endregion

    #region Gestión de Casillas y Tablero

    public void IluminarCasillas(List<Vector2Int> posiciones)
    {
        LimpiarCasillasIluminadas();

        foreach (Vector2Int pos in posiciones)
        {
            if (EstaDentroDelTablero(pos))
            {
                GameObject casillaObj = casillas[pos.x, pos.y];
                Casilla scriptCasilla = casillaObj.GetComponent<Casilla>();

                if (scriptCasilla != null)
                {
                    // --- ¡LÓGICA CLAVE! ---
                    // Comprobamos si hay una pieza enemiga en esta posición
                    AtributosPieza piezaEnDestino = GetPiezaEn(pos);
                    bool esMovimientoDeCaptura = (piezaEnDestino != null && piezaEnDestino.color != piezaSeleccionada.color);

                    Material materialAUsar;
                    if (esMovimientoDeCaptura)
                    {
                        materialAUsar = materialCasillaCaptura; // Usamos el material de captura
                    }
                    else
                    {
                        materialAUsar = materialCasillaIluminada; // Usamos el material normal
                    }

                    if (materialAUsar != null)
                    {
                        scriptCasilla.Iluminar(materialAUsar);
                        casillasIluminadas.Add(scriptCasilla);
                    }
                }
            }
        }
    }

    public void LimpiarCasillasIluminadas()
    {
        foreach (Casilla casilla in casillasIluminadas)
        {
            casilla.Apagar();
        }
        casillasIluminadas.Clear();
    }

    public bool EstaDentroDelTablero(Vector2Int posicion)
    {
        return posicion.x >= 0 && posicion.x < 8 && posicion.y >= 0 && posicion.y < 8;
    }

    // --- NUEVO MÉTODO DE REGISTRO ---
    public void RegistrarPieza(AtributosPieza pieza, Vector2Int posicion)
    {
        if (registroDePiezas.ContainsKey(posicion))
        {
            Debug.LogWarning($"Ya hay una pieza registrada en la posición {posicion}. Sobreescribiendo.");
        }
        registroDePiezas[posicion] = pieza;
        Debug.Log($"Pieza {pieza.tipo} registrada en {posicion}");
    }

    // --- MÉTODO GETPIEZAEN NUEVO Y DEFINITIVO ---
    public AtributosPieza GetPiezaEn(Vector2Int posicion)
    {
        if (!EstaDentroDelTablero(posicion)) return null;

        // Buscamos la pieza en nuestro diccionario. Es instantáneo y no falla.
        if (registroDePiezas.TryGetValue(posicion, out AtributosPieza pieza))
        {
            return pieza;
        }

        // Si no la encuentra en el diccionario, es que no hay pieza ahí.
        return null;
    }

    #endregion
}