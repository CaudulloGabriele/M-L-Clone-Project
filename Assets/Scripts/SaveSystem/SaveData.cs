//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData
{

    //GAME DATA-----------------------------------------------------------------------------------------------------------------------------

    public int lastSaveScene;

    public int savedPlayerLevel;

    public float[] savedPlayerStatsMult;

    //GAME DATA-----------------------------------------------------------------------------------------------------------------------------

    public SaveData(/*bool delete = false*/)
    {

        //if (delete) { }

        //aggiorna i dati da salvare in base ai valori dentro DataManager

        //GAME DATA-------------------------------------------------------------------------------------------------------------------------

        lastSaveScene = DataManager.lastSaveScene;
        savedPlayerLevel = DataManager.savedPlayerLevel;
        savedPlayerStatsMult = DataManager.savedPlayerStatsMult;

        //GAME DATA-------------------------------------------------------------------------------------------------------------------------

    }

}
