using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class OpcionesScript : MonoBehaviour
{
    public Slider slider;
    public int sliderValue;
    public Toggle toggle;

    public TMP_Dropdown resolucionesDropdown;
    Resolution[] resoluciones;
    void Start()
    {
        slider.value = PlayerPrefs.GetInt("volumenAudio", 5);
        AudioListener.volume = slider.value;
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        RevisarResolucion();
    }
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropdown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;
        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
            resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        resolucionesDropdown.AddOptions(opciones);
        resolucionesDropdown.value = resolucionActual;
        resolucionesDropdown.RefreshShownValue();

        resolucionesDropdown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }
    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropdown.value);
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }
    public void ChangeSlider(int valor)
    {
        PlayerPrefs.SetInt("volumenAudio", sliderValue);
        sliderValue = valor;
        AudioListener.volume = slider.value;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
}
