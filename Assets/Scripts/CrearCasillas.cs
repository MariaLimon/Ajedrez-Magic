using UnityEngine;

public class CrearCasillas : MonoBehaviour
{

    public GameObject CasillaPreFab;
    public int Ancho;
    public int Alto;

    public Material Negro;
    public Material Blanco;

    public void Crear()
    {
        int cont = 0;
        for(int i = 0;i< Ancho; i++)
        {
            for (int x = 0; x < Alto; x++)
            {
                GameObject casillaTemp = Instantiate(CasillaPreFab, new Vector3(i, 0, x), Quaternion.identity);
                if ((i + x) % 2 == 0)
                {
                    casillaTemp.GetComponent<Casiilla>().PonerColor(Negro);
                }
                else
                {
                    casillaTemp.GetComponent<Casiilla>().PonerColor(Blanco);
                }
                casillaTemp.GetComponent<Casiilla>().NumCasilla = cont;
                cont++;
            }
            
        }
    }
}
