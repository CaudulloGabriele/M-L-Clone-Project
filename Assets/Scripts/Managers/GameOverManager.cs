using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{

    private DataManager dataManager;

    private Animator anim;

    private CanvasGroup canvasGroup;


    [SerializeField]
    private float waitBeforeShowingGameOver = 3f;
    private int waitBeforeShowingGameOverMilliseconds;


    private void Awake()
    {

        anim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        waitBeforeShowingGameOverMilliseconds = (int)(waitBeforeShowingGameOver * 1000);

    }

    private void Start()
    {

        dataManager = PermanentRefs.instance.GetDataManager();

    }


    public async void SetGameOverState(bool state, string sceneToLoad = "")
    {

        GameStateManager.SetIfGameIsOver(state);

        if (state) await Task.Delay(waitBeforeShowingGameOverMilliseconds);

        string triggerName = state ? "GameOver" : "GameReturn";
        anim.SetTrigger(triggerName);

        canvasGroup.blocksRaycasts = state;

        if (!state) SceneChange.StaticLoadThisScene(sceneToLoad, true);

    }

    public void RetryFromLastSave()
    {

        string retrySceneName = SceneChange.GetSceneNameByIndex(dataManager.lastSaveScene);
        SetGameOverState(false, retrySceneName);

    }

    public void ReturnToMainMenu()
    {

        SetGameOverState(false, "MainMenu");

    }

}
