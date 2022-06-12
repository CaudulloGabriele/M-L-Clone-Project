using UnityEngine;

public class SavePoint : Interactable, IUpdateData
{
    //reference to the DataManager
    private DataManager dataManager;

    //reference to the position the player has to have when loading a save file
    [SerializeField]
    private Transform savePosition;
    private Vector2 savePos;


    protected override void Awake()
    {
        base.Awake();

        //obtains the reference to tha DataManager
        dataManager = PermanentRefs.instance.GetDataManager();

        //obtains the position the player has to have when loading a save file
        savePos = savePosition.position;

    }


    public override void Interact()
    {
        base.Interact();

        //saves the game
        dataManager.SaveDataAfterUpdate(DataManager.GetCurrentlyLoadedSlotName());


        Debug.Log("INTERACTED WITH SAVE POINT: " + dataManager.savedPlayerPos[0] + " | " + dataManager.savedPlayerPos[1]);
    }

    public void OnLoad()
    {
        //when the scene is loaded, the player is positioned in the saved position
        //PermanentRefs.instance.GetPlayer().position = new Vector2(dataManager.savedPlayerPos[0], dataManager.savedPlayerPos[1]);
        //Debug.Log("POSITIONING PLAYER: " + dataManager.savedPlayerPos[0] + " | " + dataManager.savedPlayerPos[1]);
    }

    public void UpdateData()
    {

        float[] positionToSave = { savePos.x, savePos.y };
        dataManager.savedPlayerPos = positionToSave;

        dataManager.lastSaveScene = SceneChange.GetCurrentlyLoadedSceneIndex();

    }
}
