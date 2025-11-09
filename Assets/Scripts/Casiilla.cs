using UnityEngine;

public class Casiilla : MonoBehaviour
{
    public Material colorCasilla;
    public int NumCasilla=1;

    void OnMouseDown()
    {
        print(NumCasilla.ToString());
    }
    
    public void PonerColor(Material Color_)
    {
        GetComponent<MeshRenderer>().material = Color_;
        colorCasilla = Color_;
    }
}
