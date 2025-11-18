using UnityEngine;

public class Pieza : MonoBehaviour
{
    void OnMouseDown()
    {
        // Cuando se hace clic en una pieza, avisamos al GameManager
        GameManager.instance.SeleccionarPieza(this.gameObject);
    }
}