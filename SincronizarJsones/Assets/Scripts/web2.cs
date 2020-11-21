using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class web2 : MonoBehaviour
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
        public List<registro> registros;
    }

    public estructuraDatosWeb datos;

    public Transform tabla;
    public GameObject plantillaRegistros;
    public GameObject nuevo;
    int cantidadRegistros = 5; // REGISTROS QUE SE MUESTRAN EN LA TABLA
    public int miPuntaje;

    public TMPro.TMP_InputField miNombre;

    private void Start()
    {
        Leer(CrearTablaPasarDatosYChequear);
    }

    [ContextMenu("Leer")]
    public void Leer(System.Action accionAlTerminar)
    {
        StartCoroutine(CorrutinaLeer(accionAlTerminar));
    }

    IEnumerator CorrutinaLeer(System.Action accionAlTerminar) // COMO SON PROCESOS ASINCRONOS USAMOS CORRUTINAS
    {
        UnityWebRequest web = UnityWebRequest.Get("http://pipasjourney.com/compartido/tablaHiscoreJson.txt");
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            // todo ok
            datos = JsonUtility.FromJson<estructuraDatosWeb>(web.downloadHandler.text);
            accionAlTerminar();
        }
        else
        {
            Debug.LogWarning("Hubo un problema al leer el archivo");
        }
    }
    
    [ContextMenu("Escribir")]
    public void Escribir()
    {
        StartCoroutine(CorrutinaEscribir());
    }

    IEnumerator CorrutinaEscribir()
    {
        WWWForm form = new WWWForm();
        form.AddField("archivo", "tablaHiscoreJson.txt");
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

    [ContextMenu("Crear tabla")]
    void CrearTabla()
    {
        for (int i = 0; i < cantidadRegistros; i++)
        {
            GameObject inst = Instantiate(plantillaRegistros, tabla);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -50f);
            inst.name = i.ToString();
        }
    }

    [ContextMenu("Pasar datos a tabla")]
    void PasarDatosATabla()
    {
        for (int i = 0; i < cantidadRegistros; i++)
        {
            tabla.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = datos.registros[i].nombre;
            tabla.GetChild(i).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = datos.registros[i].puntaje.ToString();
        }
        
    }

    [ContextMenu("Chequear si corresponde")]
    void ChequearSiCorrespondeNuevoHiscore()
    {
        if(miPuntaje > datos.registros[cantidadRegistros - 1].puntaje)
        {
            // SI, CORRESPONDE
            tabla.gameObject.SetActive(false);
            nuevo.gameObject.SetActive(true);
        }
        else
        {
            tabla.gameObject.SetActive(true);
            nuevo.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Insertar nuevo registro")]
    void InsertarNuevoRegistro()
    {
        // SABER EN QUE POSICION TIENE QUE INSERTAR

        for (int i = 0; i < cantidadRegistros; i++)
        {
            if(miPuntaje > datos.registros[i].puntaje)
            {
                // INSERTO
                datos.registros.Insert(i, new estructuraDatosWeb.registro()
                {
                    nombre = miNombre.text,
                    puntaje = miPuntaje
                });
                break;
            }
        }
    }

    void CrearTablaPasarDatosYChequear()
    {
        CrearTabla();
        PasarDatosATabla();
        ChequearSiCorrespondeNuevoHiscore();
    }

    public void InputTermino()
    {
        nuevo.gameObject.SetActive(false);
        tabla.gameObject.SetActive(true);
        Leer(InsertarYEscribir);
    }

    void InsertarYEscribir()
    {
        InsertarNuevoRegistro();
        Escribir();
        PasarDatosATabla();
    }

    /*
     *          PRIMERA LECTURA:
     *                                                                            |---> SI NO CORRESPONDE, NO HACE FALTA HACER MAS NADA
     *          LEER (ASINCRONICA) > CREAR TABLA > PASAR DATOS A TABLA > CHEQUEAR +
     *                                                                            |---> SI CORRESPONDE, HAY QUE ESPERAR EL INPUT
     *                                                              
     *          -----------------------
     *          
     *          ESCRITURA:
     *          
     *          INPUT > RELEER (ASINCRONICA) > INSERTAR > ESCRIBIR [ > LEER ]
     * 
     * */
}