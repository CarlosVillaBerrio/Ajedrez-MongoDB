using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class web : MonoBehaviour
{
    // PARA EL METODO 2
    public string nombre;
    public int puntaje;
    public int nivel;


    //--------------------------- metodo 1: texto simple
    [ContextMenu("Leer Simple")]
    public void LeerSimple()
    {
        StartCoroutine(CorrutinaLeerSimple());
    }

    IEnumerator CorrutinaLeerSimple()
    {
        UnityWebRequest web = UnityWebRequest.Get("http://pipasjourney.com/compartido/pruebaTutorial.txt");
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            Debug.Log(web.downloadHandler.text);

        }
        else
        {
            Debug.LogWarning("Hubo un problema con la peticion web");
        }
    }
    
    [ContextMenu("Escribir Simple")]
    public void EscribirSimple()
    {
        StartCoroutine(CorrutinaEscribirSimple());
    }

    IEnumerator CorrutinaEscribirSimple()
    {
        WWWForm form = new WWWForm();
        form.AddField("archivo", "pruebaTutorial.txt");
        form.AddField("texto", "Hola. Primera prueba completa desde Unity Yay!!!");
        
        UnityWebRequest web = UnityWebRequest.Post(
            "http://pipasjourney.com/compartido/escribir.php", form
            );
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            Debug.Log(web.downloadHandler.text);

        }
        else
        {
            Debug.LogWarning("Hubo un problema con la peticion web");
        }
    }

    [ContextMenu("Leer varios datos sin JSON")]
    public void LeerVariosSinJson()
    {
        StartCoroutine(CorrutinaLeerVariosSinJson());
    }

    //-------------------------------- metodo 2: varios datos sin Json
    IEnumerator CorrutinaLeerVariosSinJson()
    {
        UnityWebRequest web = UnityWebRequest.Get("http://pipasjourney.com/compartido/pruebaVariosSinJson.txt");
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if (!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            string textoOriginal = web.downloadHandler.text; // Minuto 11:20 
            string[] partes = textoOriginal.Split('♣');
            nombre = partes[0];
            puntaje = int.Parse(partes[1]);
            nivel = int.Parse(partes[2]);

        }
        else
        {
            Debug.LogWarning("Hubo un problema con la peticion web");
        }
    }

    [ContextMenu("Escribir varios datos sin JSON")]
    public void EscribirVariosSinJson()
    {
        StartCoroutine(CorrutinaEscribirVariosSinJson());
    }

    IEnumerator CorrutinaEscribirVariosSinJson()
    {
        WWWForm form = new WWWForm();
        form.AddField("archivo", "pruebaVariosSinJson.txt");
        form.AddField("texto", nombre + "♣" + puntaje.ToString() + "♣" + nivel.ToString());
        
        UnityWebRequest web = UnityWebRequest.Post(
            "http://pipasjourney.com/compartido/escribir.php", form
            );
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            Debug.Log(web.downloadHandler.text);

        }
        else
        {
            Debug.LogWarning("Hubo un problema con la peticion web");
        }
    }

    //-------------------------------- metodo 3: varios datos usando Json



}
