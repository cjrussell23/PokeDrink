using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchPhase : MonoBehaviour
{
    // Other Player Scripts
    private ChatManager chatManager;
    // Game Scripts
    private PokemonController pokemonController;
    // Pokemon attributes
    private Pokemon pokemon;
    private string pokemonName;
    private Sprite pokemonSprite;
    private int catchDifficulty;
    // Grass layer for pokemon detection
    [SerializeField]
    private LayerMask grassLayerMask;
    private bool inGrass;
    // UI 
    [SerializeField]
    private Image encounterImage;
    [SerializeField]
    private Text pokemonNameText;
    [SerializeField]
    private Image pokemonImage;

    void Start()
    {
        chatManager = gameObject.GetComponent<ChatManager>();
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
    }

    // Update is called once per frame
    void Update() { }

    public void CheckForGrass()
    {
        Vector3 position = transform.position;
        if (Physics2D.OverlapCircle(position, 0.3f, grassLayerMask))
        {
            Debug.Log(gameObject.GetComponent<PlayerInfo>().playerName + " is on grass");
            inGrass = true;
            CheckForPokemon();
        }
        else
        {
            Debug.Log(gameObject.GetComponent<PlayerInfo>().playerName + " is not on grass");
            inGrass = false;
        }
    }

    public void CheckForPokemon()
    {
        int randomIndex = Random.Range(0, pokemonController.Length());
        pokemon = pokemonController.GetPokemon(randomIndex);
        pokemonName = pokemon.GetName();
        pokemonSprite = pokemon.GetSprite();
        catchDifficulty = pokemon.GetCatchDifficulty();
        Debug.Log(
            gameObject.GetComponent<PlayerInfo>().playerName + " encountered a " + pokemonName
        );
        pokemonNameText.text = pokemonName;
        pokemonImage.sprite = pokemonSprite;
        encounterImage.gameObject.SetActive(true);
        gameObject.GetComponent<Dice>().ResetRolls(1);
    }

    public void Catch(int roll)
    {
        if(!inGrass)
        {
            return;
        }
        int pokemonRoll = Random.Range(1, catchDifficulty + 1);
        if (roll >= pokemonRoll)
        {
            chatManager.CmdSendMessage("Caught the " + pokemonName);
            gameObject.GetComponent<Inventory>().AddPokemon(pokemon);
            Debug.Log(
                gameObject.GetComponent<PlayerInfo>().playerName + " caught the " + pokemonName
            );
            encounterImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(
                gameObject.GetComponent<PlayerInfo>().playerName
                    + "Failed to catch the "
                    + pokemonName
            );
            chatManager.CmdSendMessage("Failed to catch the " + pokemonName + ". It rolled a " + pokemonRoll.ToString());
            encounterImage.gameObject.SetActive(false);
        }
        // Reset pokemon name
        pokemonNameText.text = "";
        pokemonName = "";
        // Reset pokemon sprite
        pokemonSprite = null;
        pokemonImage.sprite = null;
        // Reset catch difficulty
        catchDifficulty = 0;
        pokemonRoll = 0;
        pokemon = null;
        // Reset inGrass
        inGrass = false;
    }
}
