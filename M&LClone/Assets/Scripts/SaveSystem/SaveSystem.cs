//si occupa di salvare e caricare i dati di gioco salvati
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {


    private static string saveNamesFileName = "SaveSlotsNames", //indica il nome del file che contiene i nomi di tutti i file di salvataggio esistenti
        saveFilesExtension = ".sav"; //indica l'estensione dei file di salvataggio

    //indica ad altri script se si stanno cancellando i dati
    public static bool isDeleting;
    //array di tutti i nomi degli slot di salvataggio esistenti
    public static string[] everySaveSlotsName = new string[0];


    /// <summary>
    /// Salva i dati presenti nel GameManag nel salvataggio richiesto
    /// </summary>
    /// <param name="dataManager"></param>
    /// <param name="saveSlotName"></param>
    public static void DataSave(/*DataManager dataManager, */string saveSlotName)
    {
        //i dati vengono salvati se non stanno venendo cancellati
        if (!isDeleting)
        {
            //SALVA I DATI DI GIOCO NELLO SLOT DI SALVATAGGIO SCELTO------------------------------------------------------------------------

            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //stringa che indica il percorso in cui creare il salvataggio
            string path = Application.persistentDataPath + "/" + saveSlotName + saveFilesExtension;
            //flusso di dati per creare il salvataggio
            FileStream fs = new FileStream(path, FileMode.Create);
            //aggiorna i dati da salvare e...
            SaveData sd = new SaveData(/*dataManager*/);
            //...e li formatta
            bf.Serialize(fs, sd);
            //chiude il flusso di dati
            fs.Close();

            //SALVA I DATI DI GIOCO NELLO SLOT DI SALVATAGGIO SCELTO------------------------------------------------------------------------



            //SALVA I NOMI DEGLI SLOT DI SALVATAGGIO ESISTENTI------------------------------------------------------------------------------

            //aggiorna l'array di slot salvati(se il nome dello slot ricevuto non è nullo)
            if (saveSlotName != "") UpdateArrayOfSaveSlots(false, saveSlotName);
            //formattatore binario per criptare e decriptare i dati di salvataggio
            bf = new BinaryFormatter();
            //stringa che indica il percorso in cui creare il salvataggio per tutti i nomi degli slot di salvataggio
            path = Application.persistentDataPath + saveNamesFileName + saveFilesExtension;
            //flusso di dati per creare il salvataggio
            fs = new FileStream(path, FileMode.Create);
            //indica da che parte prendere i dati da salvare
            SaveSlotsName ssn = new SaveSlotsName(everySaveSlotsName);
            //formatta i dati salvati
            bf.Serialize(fs, ssn);
            //chiude il flusso di dati
            fs.Close();

            //SALVA I NOMI DEGLI SLOT DI SALVATAGGIO ESISTENTI------------------------------------------------------------------------------

            //Debug.Log("Dati salvati");
        }
        //else { Debug.Log("DATI NON SALVATI, IN QUANTO STANNO VENENDO CANCELLATI"); }

    }
    /// <summary>
    /// Carica i dati dello slot di salvataggio scelto
    /// </summary>
    /// <param name="saveSlotName"></param>
    /// <returns></returns>
    public static SaveData LoadGame(string saveSlotName)
    {
        //stringa che indica il percorso in cui cercare il salvataggio da caricare
        string path = Application.persistentDataPath + "/" + saveSlotName + saveFilesExtension;
        //se il file esiste lo carica
        if (File.Exists(path))
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //flusso di dati per aprire il file che contiene i dati di salvataggio
            FileStream fs = new FileStream(path, FileMode.Open);
            //ottiene i dati di salvataggio e li decripta(se il nome dello slot è una stringa vuota, istanzia un nuovo SaveData)
            SaveData sd = (saveSlotName != "") ? bf.Deserialize(fs) as SaveData : new SaveData();
            //chiude il flusso di dati
            fs.Close();
            //ritorna i dati di salvataggio
            return sd;

        }
        else //altrimenti, il file non esiste, quindi...
        {
            //...comunica l'errore...
            Debug.LogError("Salvataggio non trovato in " + path);
            //...e non ritorna niente
            return null;

        }

    }
    /// <summary>
    /// Cancella i dati dallo slot di salvataggio scelto
    /// </summary>
    /// <param name="dataManager"></param>
    /// <param name="saveSlotName"></param>
    public static void ClearData(string saveSlotName, DataManager dataManager = null)
    {
        //comunica agli altri script che si stanno cancellando i dati di gioco
        isDeleting = true;
        //stringa che indica il percorso in cui cercare il salvataggio da cancellare
        string path = Application.persistentDataPath + "/" + saveSlotName + saveFilesExtension;
        //invece di cancellare i dati li sovrascrive
        if (File.Exists(path))
        {
            //cancella i dati nel GameManag
            if (dataManager) dataManager.DataErased();
            //cancella il file di salvataggio
            File.Delete(path);
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //stringa che indica il percorso in cui creare il salvataggio per tutti i nomi degli slot di salvataggio
            path = Application.persistentDataPath + saveNamesFileName + saveFilesExtension;
            //flusso di dati per creare il salvataggio
            FileStream fs = new FileStream(path, FileMode.Create);
            //indica da che parte prendere i dati da salvare
            SaveSlotsName ssn = new SaveSlotsName(everySaveSlotsName);
            //formatta i dati salvati
            bf.Serialize(fs, ssn);
            //chiude il flusso di dati
            fs.Close();

        } //altrimenti il file non esiste, quindi comunica l'errore in console
        else { Debug.LogError("File non esiste in: " + path + ".\nNon può essere cancellato"); }
        //aggiorna l'array di nomi di slot eliminando il nome del file cancellato
        UpdateArrayOfSaveSlots(true, saveSlotName);
        //comunica che non si stanno più cancellando i dati
        isDeleting = false;
        //infine, aggiorna i dati
        DataSave(/*null,*/ "");

    }
    /// <summary>
    /// Inizializza l'array dei nomi degli slot di salvataggio con quello salvato
    /// </summary>
    public static void InitializeArrayOfSaveSlots()
    {
        //stringa che indica il percorso in cui cercare il salvataggio da caricare
        string path = Application.persistentDataPath + saveNamesFileName + saveFilesExtension;
        //se il file esiste lo carica
        if (File.Exists(path))
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //flusso di dati per aprire il file che contiene i dati di salvataggio
            FileStream fs = new FileStream(path, FileMode.Open);
            //ottiene i dati di salvataggio e li decripta
            SaveSlotsName ssn = bf.Deserialize(fs) as SaveSlotsName;
            //chiude il flusso di dati
            fs.Close();
            //aggiorna l'array contenente tutti i nomi degli slot di salvataggio
            everySaveSlotsName = (string[])ssn.everySaveSlotsName.Clone();


            Debug.LogWarning("Array di nomi di file di salvataggio dopo inizializzamento:");
            for (int i = 0; i < everySaveSlotsName.Length; i++) { Debug.LogWarning(i + ") " + everySaveSlotsName[i]); }
        }
        //altrimenti il file non esiste, quindi comunica l'errore
        else { Debug.LogError("Salvataggio non trovato in " + path); }

    }
    /// <summary>
    /// Aggiorna l'array dei nomi degli slot di salvataggio
    /// </summary>
    /// <param name="delete"></param>
    /// <param name="desiredSaveSlotName"></param>
    private static void UpdateArrayOfSaveSlots(bool delete, string desiredSaveSlotName)
    {
        //crea una variabile locale che indica se è stato troato o meno il file richiesto
        bool saveSlotFound = false;
        //crea una variavile locale che indica l'indice in cui è stato trovato il file richiesto, se trovato
        int saveSlotIndex = -1;
        //cicla ogni nome dei file di salvataggio esistenti
        foreach (string saveSlotName in everySaveSlotsName)
        {
            //incrementa l'indice che indica a che punto siamo arrivati nel ciclo
            saveSlotIndex++;
            //se esiste il nome dello slot desiderato...
            if (saveSlotName == desiredSaveSlotName)
            {
                //...comunica che è stato trovato il file di salvataggio...
                saveSlotFound = true;
                //...ed esce dal ciclo
                break;

            }

        }
        //se bisogna togliere dall'array il nome del save slot corrente...
        if (delete)
        {
            //...se il file di salvataggio è stato trovato...
            if (saveSlotFound)
            {
                //...viene rimosso dalla lista di nomi di file di salvataggio con un paio di passaggi...
                //...viene creata una copia dell'array...
                var copy = (string[])everySaveSlotsName.Clone();
                //...l'array dei nomi degli slot verrà rinnovato per avere un posto in meno...
                everySaveSlotsName = new string[copy.Length - 1];
                //...infine, riporta i valori della copia nell'array appena rinnovato, senza quello all'indice cancellato
                int j = 0;
                for (int i = 0; i < copy.Length; i++)
                {

                    if (i != saveSlotIndex) { everySaveSlotsName[j] = copy[i]; j++; }

                }

                //Debug.Log("INDICE DA EVITARE: " + saveSlotIndex + " -> COPY LENGHT: " + copy.Length + " -> NEW LENGHT: " + everySaveSlotsName.Length);
                //Debug.LogError("CANCELLATO SALVATAGGIO: " + desiredSaveSlotName + " AD INDICE: " + saveSlotIndex);
            }
            else { Debug.LogError("Non si può togliere dalla lista \"" + desiredSaveSlotName + "\" perchè non c'è!"); }

        }
        else //altrimenti bisogna controllare e aggiungere il save slot corrente, quindi...
        {
            //...se non è stato trovato alcun file di salvataggio con il nome richiesto...
            if (!saveSlotFound)
            {
                //...viene aggiunto all'array di nomi di file di salvataggio con un paio di passaggi...
                //...viene creata una copia dell'array...
                var copy = (string[])everySaveSlotsName.Clone();
                //...l'array dei nomi degli slot verrà rinnovato per avere un posto in più...
                everySaveSlotsName = new string[copy.Length + 1];
                //...riporta i valori della copia nell'array appena rinnovato...
                for (int i = 0; i < copy.Length; i++) { everySaveSlotsName[i] = copy[i]; }
                //...infine, aggiunge all'ultimo indice il nome del nuovo slot di salvataggio
                everySaveSlotsName[copy.Length] = desiredSaveSlotName;

                //Debug.Log("NUOVO ULTIMO INDICE: " + copy.Length + " -> COPY LENGHT: " + copy.Length + " -> NEW LENGHT: " + everySaveSlotsName.Length);
            }

        }


        Debug.LogError("Array di nomi di file di salvataggio dopo aggiornamento:");
        for (int i = 0; i < everySaveSlotsName.Length; i++) { Debug.LogError(i + ") " + everySaveSlotsName[i]); }
    }
    /// <summary>
    /// Cancella tutti dati di salvataggio e tutti gli slot esistenti
    /// </summary>
    public static void EraseAllData()
    {
        //per ogni nome di slot di salvataggio nell'array, vengono cancellati i rispettivi file di salvataggio
        foreach(string saveSlotName in everySaveSlotsName) ClearData(saveSlotName, null);
        //l'array di nomi torna al valore originale
        everySaveSlotsName = new string[0];
        //infine, cancella anche il file che tiene conto degli slot di salvataggi presenti
        File.Delete(Application.persistentDataPath + saveNamesFileName + saveFilesExtension);
        Debug.LogError("Cancellati tutti i dati e file di salvataggio, anche quello per i nomi di file esistenti");
    }

    //SISTEMA DI SALVATAGGIO SINGOLO--------------------------------------------------------------------------------------------------------

    /*
    /// <summary>
    /// Salva i dati di gioco
    /// </summary>
    /// <param name="g"></param>
	public static void DataSave(GameManag g)
    {
        //i dati vengono salvati se non stanno venendo cancellati
        if (!isDeleting)
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //stringa che indica il percorso in cui creare il salvataggio
            string path = Application.persistentDataPath + "/saveData.ass";
            //flusso di dati per creare il salvataggio
            FileStream fs = new FileStream(path, FileMode.Create);
            //indica da che parte prendere i dati da salvare
            SaveData sd = new SaveData(g);
            //formatta i dati salvati
            bf.Serialize(fs, sd);
            //chiude il flusso di dati
            fs.Close();
            //Debug.Log("Dati salvati");
        }
        //else { Debug.Log("DATI NON SALVATI, IN QUANTO STANNO VENENDO CANCELLATI"); }
        Debug.LogError("SI STA USANDO IL METODO DI SALVATAGGIO SINGOLO");
    }
    */

    /*
    /// <summary>
    /// Carica i dati salvati
    /// </summary>
    /// <returns></returns>
    public static SaveData LoadGame() {
        //stringa che indica il percorso in cui cercare il salvataggio da caricare
        string path = Application.persistentDataPath + "/saveData.ass";
        //se il file esiste lo carica
        if (File.Exists(path))
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //flusso di dati per aprire il file che contiene i dati di salvataggio
            FileStream fs = new FileStream(path, FileMode.Open);
            //ottiene i dati di salvataggio e li decripta
            SaveData sd = bf.Deserialize(fs) as SaveData;
            //chiude il flusso di dati
            fs.Close();
            //ritorna i dati di salvataggio
            return sd;

        }
        else //altrimenti, il file non esiste, quindi...
        {
            //...comunica l'errore...
            //Debug.LogError("Salvataggio non trovato in " + path);
            //...e non ritorna niente
            return null;

        }
        Debug.LogError("SI STA USANDO IL METODO DI SALVATAGGIO SINGOLO");
    }
    */

    /*
    /// <summary>
    /// Cancella i dati salvati
    /// </summary>
    public static void ClearData(GameManag g)
    {
        //comunica agli altri script che si stanno cancellando i dati di gioco
        isDeleting = true;
        //stringa che indica il percorso in cui cercare il salvataggio da cancellare
        string path = Application.persistentDataPath + "/saveData.ass";
        //cancella i dati di salvataggio se esistono, mantenendo il numero massimo di monete
        //if (File.Exists(path)) { File.Delete(path); g.MaintainMaxCoins(); g.DataErased(); Debug.Log("CANCELLATI DATI DI SALVATAGGIO"); }


        //invece di cancellare i dati li sovrascrive
        if (File.Exists(path))
        {
            //cancella i dati nel GameManag(tranne le monete massime nei livelli)
            g.DataErased();
            //comunica temporaneamente che non si stanno cancellando i dati(in questo modo può salvare i nuovi dati)
            isDeleting = false;
            //salva i nuovi dati(facendo così finta che sono stati cancellati i dati)
            DataSave(g);
            //comunica che si stanno cancellando i dati(in modo che altri script non provino a cancellare, salvare o caricare i dati)
            isDeleting = true;
            //cancella il file di salvataggio
            File.Delete(path);

        }
        //else { Debug.LogError("File non esiste"); }
        /*
        //comunica che i dati sono stati cancellati
        isDeleting = false;
        //salva nuovamente i dati, in modo da creare un nuovo SaveData
        DataSave(g);
        //infine, carica i dati nel GameManag
        g.OnGameLoad(g.sd);

        g.lastCompletedLevel = 0;

        DataSave(g);

        isDeleting = true;
        
    }
    */

}
