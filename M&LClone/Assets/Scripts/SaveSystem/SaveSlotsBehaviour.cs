//Si occupa di tutte le caratteristiche di ogni slot di salvataggio
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotsBehaviour : MonoBehaviour
{
    //indica il nome di questo slot di salvataggio
    private string saveSlotName;
    //riferimento al testo che indica il nome dello slot
    private Text saveSlotNameText;
    //riferimento all'immagine di questo slot di salvataggio
    private Image saveSlotImage;
    //riferimento al manager dei salvataggi multipli
    [SerializeField]
    private ScrollMultipleSaves sms = default;
    //indica il colore che lo slot deve avere nel caso non sia quello caricato
    private Color unloadedColor;
    //indica il colore che lo slot deve avere nel caso sia quello caricato
    [SerializeField]
    private Color currentlyLoadedColor = Color.grey;


    private void Awake()
    {
        //ottiene il riferimento al testo che indica il nome dello slot
        saveSlotNameText = transform.GetChild(0).GetComponent<Text>();
        //ottiene il riferimento all'immagine di questo slot di salvataggio
        saveSlotImage = GetComponent<Image>();
        //ottiene il colore di quando lo slot non è quello caricato
        unloadedColor = saveSlotImage.color;

    }

    /// <summary>
    /// Permette di cambiare il nome dello slot di salvataggio
    /// </summary>
    /// <param name="newName"></param>
    public void ChangeName(string newName)
    {
        //cambia il nome dello slot di salvataggio
        saveSlotName = newName;
        //cambia il testo che indica il nome dello slot di salvataggio
        saveSlotNameText.text = newName;
    
    }
    /// <summary>
    /// Ritorna il nome di questo slot di salvataggio
    /// </summary>
    /// <returns></returns>
    public string GetName() { return saveSlotName; }
    /// <summary>
    /// Salva i dati attuali di gioco in questo slot di salvataggio
    /// </summary>
    public void SaveInThisSlot() { sms.SaveInTheSlot(saveSlotName); }
    /// <summary>
    /// Carica i dati di questo slot di salvataggio
    /// </summary>
    public void LoadThisSlot() { sms.LoadThisSaveSlot(saveSlotName); }
    /// <summary>
    /// Cancella questo slot di salvataggio per sempre
    /// </summary>
    public void DeleteThisSaveSlot() { sms.DeleteSaveSlot(saveSlotName); }
    /// <summary>
    /// Cambia il colore dell'immagine di questo slot di salvataggio, per indicare se i dati caricati sono quelli di questo slot o meno
    /// </summary>
    public void IsCurrentlyLoaded(bool loaded) { if (saveSlotImage) saveSlotImage.color = loaded ? currentlyLoadedColor : unloadedColor; }

}
