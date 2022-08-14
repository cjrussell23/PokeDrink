using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class Dice : NetworkBehaviour
{
    [SerializeField]
    private Button diceButton;

    [SerializeField]
    private Text diceButtonText;
    private MovementCounter movementCounter;
    private ChatManager chatManager;
    private GameManager gameManager;
    private PlayerInfo playerInfo;
    private Inventory inventory;
    public bool playerBattle;

    // On Movement state, player gets one roll of the dice.
    public int remainingRolls;

    public override void OnStartAuthority()
    {
        playerBattle = false;
        playerInfo = GetComponent<PlayerInfo>();
        inventory = GetComponent<Inventory>();
        remainingRolls = 1;
        movementCounter = GetComponent<MovementCounter>();
        chatManager = GetComponent<ChatManager>();
    }

    public void Update()
    {
        if (remainingRolls < 1)
        {
            diceButton.interactable = false;
        }
        else
        {
            diceButton.interactable = true;
        }
    }

    public void SetPlayerBattle(bool inBattle)
    {
        this.playerBattle = inBattle;
    }

    public void ClearDiceText()
    {
        diceButtonText.text = "";
    }

    public void ButtonClick()
    {
        if (remainingRolls > 0)
        {
            remainingRolls--;
            StartCoroutine("RollTheDice");
        }
        else
        {
            chatManager.localPlayerMessage("You have to Ready Up before you can roll again!");
        }
    }

    private IEnumerator RollTheDice()
    {
        playerInfo.gameStateImage.gameObject.SetActive(false);
        int diceSize;
        Pokemon pokemonToRollWith = inventory.GetNextPokemonToRollWith();
        if (pokemonToRollWith == null)
        {
            diceSize = 4;
        }
        else
        {
            diceSize = pokemonToRollWith.GetCatchDifficulty();
        }
        int randomDiceSide = 0;
        int finalSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(1, diceSize + 1);
            diceButtonText.text = randomDiceSide.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        finalSide = randomDiceSide;
        if (playerBattle)
        {
            chatManager.CmdSendMessage(
                "Rolled a " + finalSide.ToString() + " with " + pokemonToRollWith.GetName()
            );
        }
        else if (pokemonToRollWith != null)
        {
            Debug.Log("Rolling the dice with " + pokemonToRollWith.GetName());
            // Only anounce roll if it is a crit success or crit failure
            if (diceSize >= 10)
            {
                if (finalSide == diceSize)
                {
                    chatManager.CmdSendMessage(
                        "Rolled a "
                            + finalSide.ToString()
                            + " with "
                            + pokemonToRollWith.GetName()
                            + "! Give a player a drink for that awesome performance!"
                    );
                }
                else if (finalSide == 1)
                {
                    chatManager.CmdSendMessage(
                        "Rolled a "
                            + finalSide.ToString()
                            + " with "
                            + pokemonToRollWith.GetName()
                            + ". That's a crit failure. Take a drink and think about how much your "
                            + pokemonToRollWith.GetName()
                            + " sucks!"
                    );
                }
            }
        }
        else
        {
            chatManager.CmdSendMessage("Rolled a " + finalSide.ToString());
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (gameManager.currentGameState == GameManager.GameState.Movement)
        {
            movementCounter.Movement = finalSide;
        }
        if (gameManager.currentGameState == GameManager.GameState.Catch)
        {
            CatchPhase catchPhase = GetComponent<CatchPhase>();
            if (catchPhase.inGrass)
            {
                catchPhase.Catch(finalSide);
            }
            else if (catchPhase.inBattle)
            {
                catchPhase.Attack(finalSide, pokemonToRollWith);
            }
        }
        playerBattle = false;
    }

    public void ResetRolls(int rolls)
    {
        remainingRolls = rolls;
    }
}
