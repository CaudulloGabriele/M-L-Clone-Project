//si occupa di tenere conto delle variabili salvate quando viene caricato il gioco
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //riferimento all'istanza unica di questo script nel gioco
    public static DataManager instance;

    /*
     * Indica il file di salvataggio corrente
     * Viene messo di default a stringa vuota per impedire al giocatore di creare uno slot con lo stesso nome
     * In quanto il giocatore non potrà mai creare uno slot con come nome una stringa vuota(guarda ScrollMultipleSaves)
    */
    private static string currentSaveSlotName = "";
    [SerializeField]
    private  string currentSaveSlotNameDebug = "";
    /// <summary>
    /// Indica il nome da usare quando non è stato caricato nessun salvataggio
    /// </summary>
    public static readonly string defaultSaveSlotName = "";
    //indica se vogliamo caricare i dati ad inizio scena o meno(PER DEBUG, COMMENTARE A GIOCO FINITO)
    [SerializeField]
    private bool loadData = true;
    //riferimento a tutti gli script che usano l'interfaccia per l'aggiornamento dei dati nel GameManag
    public static List<IUpdateData> dataToSave = new List<IUpdateData>();

    //GAME DATA-------------------------------------------------------------------------------------------------------------------------------

    //indicates the last scene the player was when he saved the game
    public int lastSaveScene;

    //indicates the player position when he last saved the game
    public float[] savedPlayerPos;

    //saved values of all the player's stats
    public int savedPlayerLevel;
    public float[] savedPlayerStatsMult;

    //GAME DATA-------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
    {
        //imposta l'istanza del DataManager, se non esiste già
        if (!instance) instance = this;
        //altrimenti, questa istanza si distrugge
        else { Destroy(gameObject); return; }
        //prima di tutto cancella i dati in modo che i valori statici tornino inizialmente al loro valore originale
        DataErased();
        //inizializza l'array dei nomi degli slot salvati nel SaveSystem
        SaveSystem.InitializeArrayOfSaveSlots();
        //se i dati stavano venendo cancellati, indica che il cancellamento è finito in quanto si stanno per caricare i dati
        if (SaveSystem.isDeleting) { SaveSystem.isDeleting = false; }
        //carica i dati salvati
        if (loadData)
        {
            
            OnGameLoad(SaveSystem.LoadGame(this, currentSaveSlotName));

            if (currentSaveSlotName != "") { Debug.Log("Caricato salvataggio: " + currentSaveSlotName); }
            else { Debug.Log("Caricato salvataggio di default, per l'inizio del gioco"); }

        }
        else { Debug.LogError("NON SONO STATI AGGIORNATI I DATI PERCHE' loadData E' MESSO A FALSE"); }
        //dopo il caricamento dei dati controlla se gli array sono vuoti, nel qual caso li inizializza
        InizializeEmptyArrays();

        UpdateListOfDataUpdaters();

    }

    //DEBUG-----------------------------------------------------------------------------------------------------------------------------------
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentSaveSlotName = defaultSaveSlotName;
            SaveSystem.ClearData(currentSaveSlotName, this);
            SceneChange.StaticLoadThisScene(gameObject.scene.name);
            Debug.LogError("CANCELLATI DATI CON IL TASTO: F1");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentSaveSlotName = defaultSaveSlotName;
            currentSaveSlotNameDebug = defaultSaveSlotName;
            SaveSystem.EraseAllData();
            SceneChange.StaticLoadThisScene(gameObject.scene.name);
            Debug.LogError("CANCELLATI DATI CON IL TASTO: F2");
        }

    }
    //DEBUG-----------------------------------------------------------------------------------------------------------------------------------


    public static void UpdateListOfDataUpdaters()
    {
        //viene svuotata la lista di script che devono salvare i dati
        dataToSave.Clear();
        //viene creato un'array recipiente con tutti gli script che devono salvare dati(anche quelli inattivi)
        var recipient = FindObjectsOfType<MonoBehaviour>(true).OfType<IUpdateData>();
        //inizializza la lista di script che devono salvare i dati, aggiungendo tutti gli elementi nella lista recipiente
        foreach (IUpdateData elem in recipient)
        {
            
            dataToSave.Add(elem);

            elem.OnLoad();
        
        }

    }

    /// <summary>
    /// Carica i dati salvati in SaveData
    /// </summary>
    /// <param name="sd"></param>
    public void OnGameLoad(SaveData sd)
    {
        //se esistono dati di salvataggio...
        if (sd != null)
        {
            //...vengono caricati

            //GAME DATA-----------------------------------------------------------------------------------------------------------------------

            lastSaveScene = sd.lastSaveScene;
            savedPlayerPos = sd.savedPlayerPos;
            savedPlayerLevel = sd.savedPlayerLevel;
            savedPlayerStatsMult = sd.savedPlayerStatsMult;

            //GAME DATA-----------------------------------------------------------------------------------------------------------------------

            //Debug.Log("Caricati dati salvati");

        } //altrimenti, tutti i dati vengono messi al loro valore originale, in quanto non si è trovato un file di salvataggio
        else { DataErased(); }

    }
    /// <summary>
    /// Riporta tutti i dati salvati al loro valore originale
    /// </summary>
    public void DataErased()
    {
        //tutti gli array vengono svuotati
        EmptyArrays();

        lastSaveScene = 2;
        savedPlayerLevel = 1;

        Debug.LogWarning("Cancellati dati");
    }
    /// <summary>
    /// Riporta tutti gli array al loro valore originale
    /// </summary>
    private void EmptyArrays()
    {
        //variabile di controllo che indicherà quanti cicli hanno fatto i cicli for sottostanti
        //int nControl = 0;

        if (savedPlayerStatsMult != null) for (int i = 0; i < 3; i++) { savedPlayerStatsMult[i] = 1; }

        if (savedPlayerPos != null) for (int i = 0; i < 3; i++) { savedPlayerPos = new float[2]; }

        //Debug.Log("Cicli fatti per NOME_ARRAY: " + nControl); nControl = 0;
    }
    /// <summary>
    /// Controlla, per ogni array, se è nullo, nel qual caso inizializza l'array nel modo necessario
    /// </summary>
    private void InizializeEmptyArrays()
    {

        if (savedPlayerStatsMult == null)
        {

            savedPlayerStatsMult = new float[3];
            for (int i = 0; i < 3; i++) { savedPlayerStatsMult[i] = 1; }

        }

        if (savedPlayerPos == null)
        {

            savedPlayerPos = new float[3];
            for (int i = 0; i < 3; i++) { savedPlayerPos[i] = 0; }

        }

    }
    /// <summary>
    /// Carica i dati dal file di salvataggio del nome indicato dal parametro e ricarica la scena
    /// </summary>
    /// <param name="nameOfSlot"></param>
    public void LoadNewData(string nameOfSlot)
    {
        //indica il nome dello slot di salvataggio da caricare al riavvio della scena
        currentSaveSlotName = nameOfSlot;
        currentSaveSlotNameDebug = nameOfSlot;
        //carica la scena in cui è stato effettuato il salvataggio
        SceneChange.StaticLoadThisScene(lastSaveScene, true);
        Debug.Log(nameOfSlot + " | " + "Player Level: " + savedPlayerLevel);
    }
    /// <summary>
    /// Aggiorna i dati da salvare nel GameManag prima di salvare i dati
    /// </summary>
    private void UpdateDataBeforeSave()
    {
        Debug.Log("Inizio aggiornamento dei dati prima del salvataggio");
        //variabile di controllo, per vedere quante volte viene svolto il ciclo foreach
        int n = 0;
        //viene richiamata la funzione dell'interfaccia per aggiornare i dati di ogni elemento nella lista
        foreach (IUpdateData elem in dataToSave) { elem.UpdateData(); n++; }

        //Debug.Log("Aggiornati dati nel GameManag. Il numero di elementi aggiornati sono: " + n);
    }
    /// <summary>
    /// Salva i dati dopo averli aggiornati
    /// </summary>
    public void SaveDataAfterUpdate(string saveSlotName)
    {
        //salva i dati ogni volta che si va da una scena all'altra, se i dati non stanno venendo cancellati e si sono caricati i dati...
        if (!SaveSystem.isDeleting && loadData)
        {
            //...imposta il nome dello slot di salvataggio corrente...
            currentSaveSlotName = saveSlotName;
            currentSaveSlotNameDebug = saveSlotName;
            //...aggiorna i dati...
            UpdateDataBeforeSave();
            //...e salva i dati
            SaveSystem.DataSave(this, currentSaveSlotName);

            Debug.Log("Dati aggiornati e salvati nel salvataggio di nome: " + currentSaveSlotName);
        }
        else Debug.LogError("Dati non aggiornati, perchè stanno venendo cancellati");

    }
    /// <summary>
    /// Ritorna il nome dello slot di salvataggio caricato
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentlyLoadedSlotName() { return currentSaveSlotName; }

}
