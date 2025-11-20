using UnityEngine;

public enum ColorPieza
{
    BLANCO,
    NEGRO
}

public enum TipoPieza
{
    PEON,
    TORRE,
    CABALLO,
    ALFIL,
    REINA,
    REY
}

public class AtributosPieza : MonoBehaviour
{
    public ColorPieza color;
    public TipoPieza tipo;
    public Vector2Int posicionEnTablero;

    private void OnMouseDown()
    {
        // ¡Le decimos al GameManager que esta pieza ha sido seleccionada!
        GameManager.instance.SeleccionarPieza(this.gameObject);
    }
}