//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData
{

    //GAME DATA-----------------------------------------------------------------------------------------------------------------------------

    //indicates the last scene the player was when he saved the game
    public int lastSaveScene;

    //indicates the player position when he last saved the game
    public float[] savedPlayerPos;

    //saved values of all the player's stats
    public int savedPlayerLevel;
    public float[] savedPlayerStatsMult;

    //GAME DATA-----------------------------------------------------------------------------------------------------------------------------

    public SaveData(DataManager data)
    {

        //if (delete) { }

        //aggiorna i dati da salvare in base ai valori dentro DataManager

        //GAME DATA-------------------------------------------------------------------------------------------------------------------------

        lastSaveScene = data.lastSaveScene;
        savedPlayerPos = data.savedPlayerPos;
        savedPlayerLevel = data.savedPlayerLevel;
        savedPlayerStatsMult = data.savedPlayerStatsMult;

        //GAME DATA-------------------------------------------------------------------------------------------------------------------------

    }

}
