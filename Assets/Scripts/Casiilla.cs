using UnityEngine;

public class Casiilla : MonoBehaviour
{
    public Material colorCasilla;
    public int NumCasilla = 1;
    
    public Vector2Int coordenada;

    void OnMouseDown()
    {
        print("Clic en casilla: " + coordenada);
        GameManager.instance.IntentarMoverA(this.gameObject);
    }
    
    public void PonerColor(Material Color_)
    {
        GetComponent<MeshRenderer>().material = Color_;
        colorCasilla = Color_;
    }
}