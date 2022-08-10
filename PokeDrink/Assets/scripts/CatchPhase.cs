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
    public bool inGrass;
    // UI 
    [SerializeField]
    private Image encounterImage;
    [SerializeField]
    private Text pokemonNameText;
    [SerializeField]
    private Image pokemonImage;
    [SerializeField] private Text catchDifficultyText;

    // Battle 
    public bool inBattle;
    public LayerMask playerLayerMask;

    void Start()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        chatManager = gameObject.GetComponent<ChatManager>();
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
    }

    // Update is called once per frame
    void Update() { }
    public bool CheckForBattle(){
        // Check if player is next to another player
        Vector3 position = transform.position;
        if (Physics2D.OverlapCircle(position, 1.0f, playerLayerMask))
        {
            Debug.Log("Player is next to another player, Starting Battle");
            inBattle = true;
        }
        else
        {
            inBattle = false;
        }
        return inBattle;
    }

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
        int pokemonRarity;
        int random = Random.Range(1, 101);
        if (random <= 70)
        {
            pokemonRarity = 1; // common
        }
        else if (random <= 85)
        {
            pokemonRarity = 2; // uncommon
        }
        else if (random <= 95)
        {
            pokemonRarity = 3; // rare
        }
        else
        {
            pokemonRarity = 4; // Legendary
        }
        pokemon = pokemonController.GetRandomPokemon(pokemonRarity);
        pokemonName = pokemon.GetName();
        pokemonSprite = pokemon.GetSprite();
        catchDifficulty = pokemon.GetCatchDifficulty();
        Debug.Log(
            gameObject.GetComponent<PlayerInfo>().playerName + " encountered a " + pokemonName
        );
        pokemonNameText.text = pokemonName;
        pokemonImage.sprite = pokemonSprite;
        catchDifficultyText.text = pokemonName + " :CP " + catchDifficulty;
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
        DisableCatchUI();
    }
    // Test function
    public void AddRandomPokemonToParty(){
        pokemon = pokemonController.GetRandomPokemon(Random.Range(1, 4));
        gameObject.GetComponent<Inventory>().AddPokemon(pokemon);
        Debug.Log("Added " + pokemon.GetName() + " to party");
    }
    public void DisableCatchUI()
    {
        pokemonNameText.text = "";
        pokemonName = "";
        pokemonSprite = null;
        pokemonImage.sprite = null;
        catchDifficulty = 0;
        pokemon = null;
        inGrass = false;
        encounterImage.gameObject.SetActive(false);
    }
}
