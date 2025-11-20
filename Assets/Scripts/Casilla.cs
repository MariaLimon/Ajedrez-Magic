using UnityEngine;

public class Casilla : MonoBehaviour
{
    public Material colorCasilla;
    public Vector2Int coordenada;

    private Material materialOriginal;
    private MeshRenderer miRenderer;

    void Awake()
    {
        miRenderer = GetComponent<MeshRenderer>();
        // Ya NO guardamos el material aquí.
    }

    void OnMouseDown()
    {
        print("Clic en casilla: " + coordenada);
        GameManager.instance.IntentarMoverA(this.gameObject);
    }

    public void Iluminar(Material materialResaltado)
    {
        miRenderer.material = materialResaltado;
        colorCasilla = materialResaltado;
    }

    public void Apagar()
    {
        // Ahora sí, materialOriginal tiene el color correcto
        miRenderer.material = materialOriginal;
        colorCasilla = materialOriginal;
    }

    // --- ¡ESTE MÉTODO ES CLAVE! ---
    public void SetMaterialOriginal(Material mat)
    {
        materialOriginal = mat;
    }
}