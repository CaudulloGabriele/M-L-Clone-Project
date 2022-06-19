using UnityEngine;

public class BattleHUD : MonoBehaviour
{

    [Tooltip("Contains all sprites of the various dodges in this order:\n" +
             "0) Jump\n" +
             "1) Vanish\n")
    ]
    [SerializeField]
    private Sprite[] allDodgesSprites;
    private static Sprite[] staticAllDodgesSprites;

    [Tooltip("Contains all sprites of the various counters in this order:\n" +
             "0) Deflect Bullets\n" +
             "1) Spiky Body\n")
    ]
    [SerializeField]
    private Sprite[] allCountersSprites;
    private static Sprite[] staticAllCountersSprites;


    private void Awake()
    {
        //statically copies the arrays of sprites
        staticAllDodgesSprites = allDodgesSprites;
        staticAllCountersSprites = allCountersSprites;

    }


    /// <summary>
    /// Returns the Sprite of the desired dodge or counter
    /// </summary>
    /// <param name="wantsDodge"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Sprite GetDodgeCounterSpriteByIndex(bool wantsDodge, int index)
    {

        Sprite dodgeCounterSprite;

        if (wantsDodge) dodgeCounterSprite = staticAllDodgesSprites[index];
        else dodgeCounterSprite = staticAllCountersSprites[index];

        return dodgeCounterSprite;

    }

}
