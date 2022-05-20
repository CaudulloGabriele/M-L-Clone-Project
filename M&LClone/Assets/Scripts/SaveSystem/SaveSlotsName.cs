//Si occupa di salvare tutti i nomi dei file di salvataggio esistenti
[System.Serializable]

public class SaveSlotsName
{
    //array di tutti i nomi degli slot di salvataggio esistenti
    public string[] everySaveSlotsName;

    public SaveSlotsName(string[] updatedSlotsName) { everySaveSlotsName = updatedSlotsName;  }

}
