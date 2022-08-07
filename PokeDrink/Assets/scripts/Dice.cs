using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class Dice : NetworkBehaviour {
    private Sprite[] diceSides;
    [SerializeField] private Button diceButton;
    private MovementCounter movementCounter;
    private ChatManager chatManager;
    private Image image;
    // On Movement state, player gets one roll of the dice.
    public int remainingRolls;
	public override void OnStartAuthority() {
        remainingRolls = 1;
        movementCounter = GetComponent<MovementCounter>();
        chatManager = GetComponent<ChatManager>();
        image = diceButton.GetComponent<Image>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
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
        int randomDiceSide = 0;
        int finalSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 5);
            image.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        finalSide = randomDiceSide + 1;
        chatManager.CmdSendMessage("Rolled a " + finalSide.ToString());
        movementCounter.Movement = finalSide;
    }
    public void ResetRolls()
    {
        remainingRolls = 1;
    }
}
