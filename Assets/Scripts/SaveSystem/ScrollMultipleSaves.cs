//Si occupa di mostrare i file di salvataggio esistenti nella scroll view e interfacciarli con l'utente
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollMultipleSaves : MonoBehaviour
{
    
    [SerializeField]
    private GameObject saveSlotTemplate = default, //riferimento al template da usare per creare i nuovi slot di salvataggio
        saveSlotCreationMenu = default; //riferimento al menù in cui il giocatore può creare un nuovo slot di salvataggio

    //lista contenente tutti gli script degli slot di salvataggio esistenti
    private List<SaveSlotsBehaviour> allSaveSlotsBehaviours = new List<SaveSlotsBehaviour>();
    //riferimento al bottone per tornare indietro dal menù di creazione di slot di salvataggi nuovi
    private GameObject cancelButton;
    //riferimento al content della view del menù scroll
    [SerializeField]
    private Transform content = default;
    //riferimento al layout group del content(che si occupa di come mostrare i vari slot di salvataggio)
    private VerticalLayoutGroup contentLayoutGroup;
    //riferimento all'InputField che si occupa di dare un nome al nuovo slot di salvataggio da creare
    private InputField newSaveSlotNameIF;
    //riferimento al testo d'errore
    [SerializeField]
    private Text errorText = default;
    //riferimento all'Animator del testo d'errore
    private Animator errorTextAnim;
    //indica quanto più in basso ogni slot deve andare per non essere addosso al precedente
    [SerializeField]
    private int nextSlotPosOffset = -100;
    //indica il testo di default per gli slot di salvataggio
    [SerializeField]
    private string defaultNewSlotName = "New Save Slot";
    //lista di tutti i nomi degli slot di salvataggio presenti in lizza
    private List<string> allSaveSlotsNames = new List<string>();


    private void Start()
    {
        //ottiene il riferimento al layout group del content
        contentLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
        //cambia il modo in cui vengono visualizzati gli slot nel content
        contentLayoutGroup.padding.top = nextSlotPosOffset;
        //ottiene il riferimento all'InputField che si occupa di dare un nome al nuovo slot di salvataggio da creare
        newSaveSlotNameIF = saveSlotCreationMenu.transform.GetChild(0).GetComponent<InputField>();
        //cambia il testo iniziale dell'InputField per i nomi degli slot al nome di default per gli slot di salvataggio
        newSaveSlotNameIF.text = defaultNewSlotName;
        //aggiorna il content con tanti slot di salvataggio quanti sono quelli salvati
        UpdateContent();
        //se ci sono già salvataggi esistenti, disattiva il menù di creazione di nuovi slot di salvataggio
        if (allSaveSlotsNames.Count > 0) { saveSlotCreationMenu.SetActive(false); }
        //altrimenti non esistono salvataggi, quindi...
        else
        {
            //...non disattiva il menù, ma disattiva il bottone per uscire dal menù
            cancelButton = saveSlotCreationMenu.transform.GetChild(1).gameObject;
            cancelButton.SetActive(false);

        }
        //ottiene il riferimento all'Animator del testo d'errore
        errorTextAnim = errorText.GetComponent<Animator>();
        //infine, disattiva sè stesso
        gameObject.SetActive(false);

    }

    //DEBUG-----------------------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {

            for (int i = 1; i <= 100; i++) { CreateNewSaveSlot("Debug " + i); }
            Debug.LogError("DEBUG: Provato a creare 100 nuovi salvataggi uguali ma con nomi diversi!(Tasto usato: F12)");
        }

    }
    //DEBUG-----------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Crea gli slots di salvataggio salvati in memoria
    /// </summary>
    private void UpdateContent()
    {
        //per ogni nome degli slot di salvataggio nell'array di nomi salvati...
        foreach (string saveSlotName in SaveSystem.everySaveSlotsName)
        {
            //...crea un nuovo slot di salvataggio nel content(comunicando che non bisogna salvare dati nel GameManag)...
            CreateNewSaveSlot(saveSlotName, true);

            //Debug.Log("Inizializzato slot di salvataggio con nome: " + saveSlotName);
        }

    }
    /// <summary>
    /// Ritorna una versione corretta del nome ricevuto come parametro
    /// </summary>
    /// <param name="nameToSolve"></param>
    /// <returns></returns>
    private string SolveNameIssues(string nameToSolve)
    {
        //Debug.Log("Nome da sistemare: " + nameToSolve);

        //crea una stringa locale vuota che conterrà il nome corretto
        string correctedName = "";
        //crea un array locale contenente tutti i caratteri del nome ricevuto come parametro
        char[] textChars = nameToSolve.ToCharArray();
        //iniziando dall'ultimo carattere, per ogni carattere presente nell'array...
        for (int _char = textChars.Length - 1; _char > -1; _char--)
        {
            //Debug.Log("Carattere che sta venendo controllato: " + textChars[_char]);

            //...se il carattere è uno spazio vuoto...
            if (textChars[_char] == ' ')
            {
                //...lo rimuove dall'array di caratteri
                char[] copy = (char[])textChars.Clone();
                textChars = new char[copy.Length - 1];
                for (int i = 0; i < textChars.Length; i++) { textChars[i] = copy[i]; }

            }
            else //altrimenti il nome finisce con un carattere accettabile, quindi...
            {
                //...al nome corretto, finora vuoto, viene aggiunto ogni carattere rimasto nell'array dopo il controllo...
                foreach (char c in textChars) { correctedName += c.ToString(); }
                //...ed esce dal ciclo
                break;

            }

        }

        //Debug.Log("Nome dopo controllo: " + nameToSolve);

        //alla fine dei controlli, ritorna il nome corretto
        return correctedName;

    }
    /// <summary>
    /// Controlla se non sono rimaste lettere nell'input field per il salvataggio da creare
    /// </summary>
    /// <param name="inputText"></param>
    public bool CheckInputText(string inputText)
    {
        //variabile locale che indica il risultato del controllo sul testo
        bool allRight = true;
        //stringa locale che indicherà un possibile errore
        string errorMsg = "";
        //se la stringa di testo è vuota...
        if (inputText == "")
        {
            //...riporta il nome nell'Input Field per i nuovi slot al valore di default...
            newSaveSlotNameIF.text = defaultNewSlotName;
            //...comunica che il nome inserito non va bene...
            allRight = false;
            //...e cambia il messaggio d'errore
            errorMsg = "You need to insert a name for the save slot you want to create!";

        } //altrimenti, se il nome inserito è uguale a quello di un altro slot di salvataggio...
        else if (SaveSlotAlreadyExists(inputText))
        {
            //...comunica che il nome inserito non va bene...
            allRight = false;
            //...e cambia il messaggio d'errore
            errorMsg = inputText + " already exists!";

        }
        //se dopo i controlli qualcosa è andato storto, mostra il messaggio d'errore
        if (!allRight) { ShowError(errorMsg); }
        //infine, ritorna il risultato del controllo
        return allRight;

    }
    /// <summary>
    /// Crea un nuovo slot di salvataggio e gli salva i dati all'interno
    /// </summary>
    /// <param name="saveSlotName"></param>
    public void CreateNewSaveSlot(string saveSlotName, bool initializing = false)
    {
        //prima di creare uno slot di salvataggio controlla se il nome è idoneo, se non sta venendo inizializzato dalla lista di slot già salvati
        if (!initializing) saveSlotName = SolveNameIssues(saveSlotName);
        //se non ci sono problemi con il nome da voler dare allo slot da creare...
        if (CheckInputText(saveSlotName))
        {
            //...disattiva il menù di creazione di slot di salvataggio...
            saveSlotCreationMenu.SetActive(false);
            //...crea un nuovo slot...
            GameObject newSaveSlot = Instantiate(saveSlotTemplate, content);
            //...prende il riferimento allo script da slot di salvataggio dello slot appena creato...
            SaveSlotsBehaviour ssb = newSaveSlot.GetComponent<SaveSlotsBehaviour>();
            //...e ne cambia il nome con quello desiderato...
            ssb.ChangeName(saveSlotName);
            //...se non si sta inizializzando la lista ma creando un nuovo slot, salva i dati nel nuovo slot...
            if (!initializing) SaveInTheSlot(saveSlotName);
            //...lo aggiunge alla lista di script degli slot di salvataggio...
            allSaveSlotsBehaviours.Add(ssb);
            //...fa in modo che solo lo slot attualmente caricato sia evidenziato...
            HighlightLoadedSaveSlot();
            //...infine, aggiunge questo nome alla lista di nomi...
            allSaveSlotsNames.Add(saveSlotName);
            //...e come ultima cosa controlla se il bottone per uscire dal menù di creazione di slot è disattivato, nel qual caso lo riattiva
            if (cancelButton && !cancelButton.activeSelf) { cancelButton.SetActive(true); }

        } //altrimenti, comunica che esiste già uno slot con questo nome
        else { Debug.LogError("SAVE SLOT \"" + saveSlotName + "\" ALREADY EXISTS!"); }

    }
    /// <summary>
    /// Crea un nuovo slot con il testo all'interno dell'InputField che indica il suo nome(viene richiamato dal bottone "Create")
    /// </summary>
    public void CreateNewSaveSlot() { CreateNewSaveSlot(newSaveSlotNameIF.text); }
    /// <summary>
    /// Cambia i colori degli slot di salvataggio, in base al salvataggio appena caricato
    /// </summary>
    private void HighlightLoadedSaveSlot()
    {
        //ottiene il nome del salvataggio attualmente caricato
        string currentlyLoadedSaveSlotName = DataManager.GetCurrentlyLoadedSlotName();
        //cambia il colore di ogni slot in lista, per indicare se è lo slot in cui si sta giocando
        foreach (SaveSlotsBehaviour ssb in allSaveSlotsBehaviours) { ssb.IsCurrentlyLoaded(ssb.GetName() == currentlyLoadedSaveSlotName); }

    }
    /// <summary>
    /// Dice se esiste già uno slot di salvataggio con il nome indicato
    /// </summary>
    /// <param name="newSaveSlotName"></param>
    /// <returns></returns>
    private bool SaveSlotAlreadyExists(string newSaveSlotName)
    {
        //crea un variabile locale che indica se lo slot esiste o meno
        bool slotExists = false;
        //cicla ogni nome nella lista di nomi degli slot di salvataggio esistenti
        foreach (string saveSlotName in allSaveSlotsNames)
        {
            //se uno slot ha lo stesso nome del parametro passato, esce dal ciclo e comunica che lo slot esiste già
            if (newSaveSlotName == saveSlotName) { slotExists = true; break; }

        }
        //ritorna il risultato del controllo
        return slotExists;

    }
    /// <summary>
    /// Ritorna il GameObject dello slot di salvataggio con il nome desiderato
    /// </summary>
    /// <param name="nameOfSlot"></param>
    /// <returns></returns>
    private GameObject GetSaveSlot(string nameOfSlot)
    {
        //crea una variabile locale che indica lo slot da ritornare
        GameObject saveSlot = null;
        //per ogni slot di salvataggio esistente in lista...
        foreach (SaveSlotsBehaviour ssb in allSaveSlotsBehaviours)
        {
            //...se il nome dello slot è uguale a quello dello slot desiderato, ritorna il GameObject del figlio ciclato ed esce dal ciclo
            if (ssb.GetName() == nameOfSlot) { saveSlot = ssb.gameObject; break; }

        }
        //infine, ritorna lo slot di salvataggio trovato
        return saveSlot;

    }
    /// <summary>
    /// Mostra il messaggio d'errore
    /// </summary>
    /// <param name="errorMessage"></param>
    private void ShowError(string errorMessage)
    {
        //cambia il testo d'errore con il parametro ricevuto
        errorText.text = errorMessage;
        //fa ripartire l'Animator del testo d'errore, in caso sia già attivo
        errorTextAnim.Rebind();
        //fa partire l'animazione che mostra il testo d'errore
        errorTextAnim.SetTrigger("ShowError");

    }
    /// <summary>
    /// Salva i dati nello slot selezionato
    /// </summary>
    /// <param name="slotToSave"></param>
    public void SaveInTheSlot(string slotToSave) { DataManager.instance.SaveDataAfterUpdate(slotToSave); }
    /// <summary>
    /// Carica i dati nello slot selezionato
    /// </summary>
    /// <param name="slotToLoad"></param>
    public void LoadThisSaveSlot(string slotToLoad) { DataManager.instance.LoadNewData(slotToLoad); }
    /// <summary>
    /// Cancella lo slot di salvataggio selezionato
    /// </summary>
    /// <param name="slotToDelete"></param>
    public void DeleteSaveSlot(string slotToDelete)
    {
        //se esiste lo slot che si vuole cancellare...
        if (SaveSlotAlreadyExists(slotToDelete))
        {
            //...cancella il file di salvataggio indicato dal nome dello slot...
            SaveSystem.ClearData(slotToDelete, null);
            //...viene rimosso il nome della lista degli slot presenti...
            allSaveSlotsNames.Remove(slotToDelete);
            //...e distrugge lo slot desiderato
            Destroy(GetSaveSlot(slotToDelete));

            Debug.LogError("CANCELLATO SALVATAGGIO: " + slotToDelete);
        }
        else { Debug.LogError("NON SI PUO' CANCELLARE LO SLOT \"" + slotToDelete + "\" PERCHE' NON ESISTE!"); }
        //QUESTO ERRORE QUA SOPRA NON VIENE MOSTRATO AL GIOCATORE PERCHE' TEORICAMENTE NON DOVREBBE MAI ESSERE IN GRADO DI CANCELLARE DATI INESISTENTI
    }

}