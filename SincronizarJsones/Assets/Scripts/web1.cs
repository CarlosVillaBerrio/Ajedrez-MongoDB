using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class web1 : MonoBehaviour
{
    // PARA EL METODO 3 USANDO JSON
    [System.Serializable]
    public struct estructuraDatosWeb
    {
        [System.Serializable]
        public struct registro
        {
            public string nombre;
            public int puntaje;
        }
        public registro[] registros;
        public string fechaHora;
    }

    public estructuraDatosWeb datos;
    
    [ContextMenu("Leer")]
    public void LeerSimple()
    {
        StartCoroutine(CorrutinaLeer());
    }

    IEnumerator CorrutinaLeer()
    {
        UnityWebRequest web = UnityWebRequest.Get("http://pipasjourney.com/compartido/pruebaJson.txt");
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            datos = JsonUtility.FromJson<estructuraDatosWeb>(web.downloadHandler.text);

        }
        else
        {
            Debug.LogWarning("Hubo un problema al leer el archivo");
        }
    }
    
    [ContextMenu("Escribir")]
    public void EscribirSimple()
    {
        StartCoroutine(CorrutinaEscribir());
    }

    IEnumerator CorrutinaEscribir()
    {
        WWWForm form = new WWWForm();
        form.AddField("archivo", "pruebaJson.txt");
        form.AddField("texto", JsonUtility.ToJson(datos));
        
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
}