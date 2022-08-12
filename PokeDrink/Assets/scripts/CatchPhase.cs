using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchPhase : MonoBehaviour
{
    // Other Player Scripts
    private ChatManager chatManager;
    private Badges badges;
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
    private Text encounterMessage;
    [SerializeField]
    private Image pokemonImage;
    [SerializeField] private Text catchDifficultyText;

    // Battle 
    public bool inBattle;
    [SerializeField]
    public LayerMask gymLayerMask;
    private int gymRoll;
    private Pokemon gymPokemon;
    private string gymLeaderName;
    private PlayerInfo playerInfo;
    private Dice dice;

    void Start()
    {
        badges = GetComponent<Badges>();
        dice = GetComponent<Dice>();
        playerInfo = GetComponent<PlayerInfo>();
        chatManager = gameObject.GetComponent<ChatManager>();
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
    }
    public bool CheckForBattle(){
        Debug.Log("CheckForBattle");
        Vector3 position = transform.position;
        Collider2D gym = Physics2D.OverlapCircle(position, 0.3f, gymLayerMask);
        if (gym)
        {
            // Check if player has the right badges to enter the gym
            GymEncounter gymEncounter = gym.gameObject.GetComponent<GymEncounter>();
            if (badges.GetBadges() + 1 == gymEncounter.GetGymID()){
                Debug.Log("Player is in a gym, Startin battle");
                inBattle = true;
                StartBattle(gymEncounter);
            }
            else {
                Debug.Log("Player is in a gym, but doesn't have the right badges");
                chatManager.localPlayerMessage("You don't have the right badges to enter this gym");
                inBattle = false;
            }
        }
        else
        {
            Debug.Log("Player is not in a gym");
            inBattle = false;
        }
        return inBattle;
    }
    public void StartBattle(GymEncounter gymEncounter){
        Debug.Log("StartBattle");
        Pokemon[] gymPokemonParty = gymEncounter.GetPokemonParty();
        gymPokemon = gymPokemonParty[Random.Range(0, gymPokemonParty.Length)];
        gymLeaderName = gymEncounter.GetGymLeaderName();
        gymRoll = Random.Range(1, gymPokemon.GetCatchDifficulty() + 1);
        encounterMessage.text = gymLeaderName + " sent out " + gymPokemon.GetName();
        pokemonImage.sprite = gymPokemon.GetSprite();
        catchDifficultyText.text = gymPokemon.GetName() + " :CP" + gymPokemon.GetCatchDifficulty();
        encounterImage.gameObject.SetActive(true);
        gameObject.GetComponent<Dice>().ResetRolls(1);
    }
    public void Attack(int roll, Pokemon pokemon){
        Debug.Log("Attack");
        if (roll > gymRoll)
        {
            encounterMessage.text = pokemon.GetName() + " defeated " + gymLeaderName + "'s " + gymPokemon.GetName() + "!";
            badges.AddBadge();
            chatManager.CmdSendMessage(pokemon.GetName() + " defeated " + gymLeaderName + "'s " + gymPokemon.GetName() + "!");
        }
        else
        {
            encounterMessage.text = pokemon.GetName() + " lost to " + gymLeaderName + "'s " + gymPokemon.GetName() + "!";
            chatManager.CmdSendMessage(pokemon.GetName() + " lost to " + gymLeaderName + "'s " + gymPokemon.GetName() + "!");
        }
        StartCoroutine(WaitForText());
    }
    private IEnumerator WaitForText(){
        yield return new WaitForSeconds(1.5f);
        encounterImage.gameObject.SetActive(false);
        DisableCatchUI();
        playerInfo.ChangePlayerReadyState();
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
        encounterMessage.text = "A Wild " + pokemonName + " Appeared!";
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
            encounterMessage.text = "You caught the " + pokemonName + "!";
            Debug.Log(
                gameObject.GetComponent<PlayerInfo>().playerName + " caught the " + pokemonName
            );
        }
        else
        {
            Debug.Log(
                gameObject.GetComponent<PlayerInfo>().playerName
                    + "Failed to catch the "
                    + pokemonName
            );
            encounterMessage.text = pokemonName + " ran away...";
            chatManager.CmdSendMessage("Failed to catch the " + pokemonName + ". It rolled a " + pokemonRoll.ToString());
        }
        StartCoroutine(WaitForText());
    }
    // Test function
    public void AddRandomPokemonToParty(){
        pokemon = pokemonController.GetRandomPokemon(Random.Range(1, 4));
        gameObject.GetComponent<Inventory>().AddPokemon(pokemon);
        Debug.Log("Added " + pokemon.GetName() + " to party");
    }
    public void DisableCatchUI()
    {
        encounterMessage.text = "";
        pokemonName = "";
        pokemonSprite = null;
        pokemonImage.sprite = null;
        catchDifficulty = 0;
        pokemon = null;
        inGrass = false;
        encounterImage.gameObject.SetActive(false);
    }
    public void RunAway(){
        encounterMessage.text = "You ran away!";
        dice.ResetRolls(0);
        StartCoroutine(WaitForText());
        playerInfo.ToggleGameStateImage(false);
    }
}
