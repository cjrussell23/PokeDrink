using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class Dice : NetworkBehaviour {
    [SerializeField] private Button diceButton;
    [SerializeField] private Text diceButtonText;
    private MovementCounter movementCounter;
    private ChatManager chatManager;
    private GameManager gameManager;
    private Inventory inventory;
    // On Movement state, player gets one roll of the dice.
    public int remainingRolls;
	public override void OnStartAuthority() {
        inventory = GetComponent<Inventory>();
        remainingRolls = 1;
        movementCounter = GetComponent<MovementCounter>();
        chatManager = GetComponent<ChatManager>();
	}
    private void Update() {
        if(!hasAuthority){return;}
        if(Input.GetKeyDown(KeyCode.R)){
            ButtonClick();
        }
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
            chatManager.localPlayerMessage("You have no rolls left!");
        }
    }
    private IEnumerator RollTheDice()
    {
        int diceSize;
        Pokemon randomPokemon = inventory.GetRandomPokemon();
        
        if (randomPokemon == null)
        {
            diceSize = 6;
        }
        else
        {
            diceSize = randomPokemon.GetCatchDifficulty();
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
        if (randomPokemon != null){
            Debug.Log("Rolling the dice with " + randomPokemon.GetName());
            chatManager.CmdSendMessage("Rolled a " + finalSide.ToString() + " with " + randomPokemon.GetName());
        }
        else {
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
            gameObject.GetComponent<CatchPhase>().Catch(finalSide);
        }
    }
    public void ResetRolls(int rolls)
    {
        remainingRolls = rolls;
    }
}
