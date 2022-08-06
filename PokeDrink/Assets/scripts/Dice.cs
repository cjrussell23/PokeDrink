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
	public override void OnStartAuthority() {
        movementCounter = GetComponent<MovementCounter>();
        chatManager = GetComponent<ChatManager>();
        image = diceButton.GetComponent<Image>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}
    private void Update() {
        if(!hasAuthority){return;}
        if(Input.GetKeyDown(KeyCode.R)){
            StartCoroutine("RollTheDice");
        }
    }
    private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }
    public void ButtonClick()
    {
        StartCoroutine("RollTheDice");
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
}
