using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

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
    private string gymPokemonType;
    private Pokemon gymPokemon;
    private string gymLeaderName;
    private PlayerInfo playerInfo;
    private Dice dice;
    // Events
    public bool inEvent;
    [SerializeField] private Text eventText;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private LayerMask eventLayerMask;
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
                Debug.Log("Player is in a gym, Starting battle");
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
    public void CheckForEvent(){
        Debug.Log("CheckForEvent");
        Vector3 position = transform.position;
        Collider2D eventCollider = Physics2D.OverlapCircle(position, 0.3f, eventLayerMask);
        if (eventCollider)
        {
            eventCollider.gameObject.SetActive(false);
            inEvent = true;
            Debug.Log("Player is in an event");
            // eventText.text = eventCollider.gameObject.GetComponent<gameEvent>().GetEventMessage();
            eventText.text = "";
            eventPanel.SetActive(true);
            IEnumerator writingMessage = WriteMessage(eventCollider.gameObject.GetComponent<gameEvent>().GetEventMessage(), eventText);
            StartCoroutine(writingMessage);
        }
    }
    public IEnumerator WriteMessage(string message, Text textObject){
        char[] characters = message.ToCharArray();
        string currentMessage = "";
        foreach (char c in characters){
            currentMessage += c;
            textObject.text = currentMessage;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void CloseEvent(){
        Debug.Log("CloseEvent");
        inEvent = false;
        eventPanel.SetActive(false);
    }
    public void StartBattle(GymEncounter gymEncounter){
        Debug.Log("StartBattle");
        Pokemon[] gymPokemonParty = gymEncounter.GetPokemonParty();
        gymPokemon = gymPokemonParty[UnityEngine.Random.Range(0, gymPokemonParty.Length)];
        gymPokemonType = gymPokemon.GetPokemonType();
        gymLeaderName = gymEncounter.GetGymLeaderName();
        gymRoll = UnityEngine.Random.Range(1, gymPokemon.GetCatchDifficulty() + 1);
        encounterMessage.text = gymLeaderName + " sent out " + gymPokemon.GetName();
        pokemonImage.sprite = gymPokemon.GetSprite();
        catchDifficultyText.text = gymPokemon.GetName() + " :CP" + gymPokemon.GetCatchDifficulty();
        encounterImage.gameObject.SetActive(true);
        gameObject.GetComponent<Dice>().ResetRolls(1);
    }
    public void Attack(int roll, Pokemon pokemon){
        Debug.Log("Attack");
        string pokemonType = pokemon.GetPokemonType();
        double playerAttackIsEffective = CheckForEffectiveness(pokemonType, gymPokemonType);
        double gymAttackIsEffective = CheckForEffectiveness(gymPokemonType, pokemonType);
        int playerRollWithBonus = (int)Math.Round(roll * playerAttackIsEffective);
        int gymRollWithBonus = (int)Math.Round(gymRoll * gymAttackIsEffective);
        if (playerAttackIsEffective == 2){
            chatManager.localPlayerMessage("Your attack was super effective");
        }
        else if (playerAttackIsEffective == 0.5){
            chatManager.localPlayerMessage("Your attack was not very effective");
        }
        if (gymAttackIsEffective == 2){
            chatManager.localPlayerMessage("The gym's attack was super effective");
        }
        else if (gymAttackIsEffective == 0.5){
            chatManager.localPlayerMessage("The gym's attack was not very effective");
        }
        if (playerRollWithBonus > gymRollWithBonus)
        {
            encounterMessage.text = pokemon.GetName() + " defeated " + gymLeaderName + "'s " + gymPokemon.GetName() + "!";
            badges.AddBadge();
            chatManager.CmdSendMessage(pokemon.GetName() + " defeated " + gymLeaderName + "'s " + gymPokemon.GetName() + "!" + " ( " + playerRollWithBonus + " > " + gymRollWithBonus + " )");
        }
        else
        {
            encounterMessage.text = pokemon.GetName() + " lost to " + gymLeaderName + "'s " + gymPokemon.GetName() + "!";
            chatManager.CmdSendMessage(pokemon.GetName() + " lost to " + gymLeaderName + "'s " + gymPokemon.GetName() + "!" + " ( " + playerRollWithBonus + " < " + gymRollWithBonus + " )");
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
        int pokemonRarity = GetRarity();
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
    private int GetRarity(){
        int numBadges = badges.GetBadges();
        int random = UnityEngine.Random.Range(0, 100) + 1; // 1 - 100
        // Four tiers of pokemon rarity
        if (numBadges < 2){ // 0 or 1 - only tier 1
            return 1;
        }
        else if (numBadges < 4){ // 2 or 3 - 80% tier 1, 20% tier 2
            if (random <= 80){
                return 1;
            }
            else {
                return 2;
            }
        }
        else if (numBadges < 6){ // 4 or 5 - 50% tier 1, 40% tier 2, 10% tier 3
            if (random <= 50){
                return 1;
            }
            else if (random <= 90){
                return 2;
            }
            else {
                return 3;
            }
        }
        else if (numBadges < 8){ // 6 or 7 - 40% tier 1, 40% tier 2, 15% tier 3, 5% tier 4
            if (random <= 40){
                return 1;
            }
            else if (random <= 80){
                return 2;
            }
            else if (random <= 95){
                return 3;
            }
            else {
                return 4;
            }
        }
        else { // 8 - 35% tier 1, 35% tier 2, 20% tier 3, 10% tier 4
            if (random <= 35){
                return 1;
            }
            else if (random <= 70){
                return 2;
            }
            else if (random <= 90){
                return 3;
            }
            else {
                return 4;
            }
        }
    }
    public void Catch(int playerRoll)
    {
        if(!inGrass)
        {
            return;
        }
        int pokemonRoll = UnityEngine.Random.Range(1, catchDifficulty + 1);
        if (playerRoll >= pokemonRoll)
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
            chatManager.CmdSendMessage("Failed to catch the " + pokemonName + ". (" + pokemonRoll.ToString() + " > " + playerRoll.ToString() + ")");
        }
        StartCoroutine(WaitForText());
    }
    // Test function
    public void AddRandomPokemonToParty(){
        pokemon = pokemonController.GetRandomPokemon(UnityEngine.Random.Range(1, 4));
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
        chatManager.CmdSendMessage("Ran away from " + pokemonName + " like a coward. They must take a drink to get back to their senses.");
        dice.ResetRolls(0);
        StartCoroutine(WaitForText());
        playerInfo.ToggleGameStateImage(false);
    }
    private double CheckForEffectiveness(string attackType, string defenseType){
        // 0 = no effect, 0.5 = not effective, 1 = neutral, 2 = super effective
        switch (attackType){
            case "Normal":
                switch (defenseType){
                    case "Rock":
                        return 0.5;
                    case "Ghost":
                        return 0;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "fire":
                switch (defenseType){
                    case "Fire":
                        return 0.5;
                    case "Water":
                        return 0.5;
                    case "Grass":
                        return 2;
                    case "Ice":
                        return 2;
                    case "Bug":
                        return 2;
                    case "Rock":
                        return 0.5;
                    case "Dragon":
                        return 0.5;
                    case "Steel":
                        return 2;
                    default:
                        return 1;
                }
            case "Water":
                switch (defenseType){
                    case "Fire":
                        return 2;
                    case "Water":
                        return 0.5;
                    case "Grass":
                        return 0.5;
                    case "Ground":
                        return 2;
                    case "Rock":
                        return 2;
                    case "Dragon":                      
                        return 0.5;
                    default:
                        return 1;
                }
            case "Electric":
                switch (defenseType){
                    case "Water":
                        return 2;
                    case "Electric":
                        return 0.5;
                    case "Grass":
                        return 0.5;
                    case "Ground":
                        return 0;
                    case "Flying":
                        return 2;
                    case "Dragon":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Grass":
                switch (defenseType){
                    case "Fire":
                        return 0.5;
                    case "Water":
                        return 2;
                    case "Grass":
                        return 0.5;
                    case "Ground":
                        return 2;
                    case "Flying":
                        return 0.5;
                    case "Poison":
                        return 0.5;
                    case "Bug":
                        return 0.5;
                    case "Rock":
                        return 2;
                    case "Dragon":
                        return 0.5;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Ice":
                switch (defenseType){
                    case "Fire":
                        return 0.5;
                    case "Water":
                        return 0.5;
                    case "Grass":
                        return 2;
                    case "Ice":
                        return 0.5;
                    case "Ground":
                        return 2;
                    case "Flying":
                        return 2;
                    case "Dragon":
                        return 2;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Fighting":
                switch (defenseType){
                    case "Normal":
                        return 2;
                    case "Ice":
                        return 2;
                    case "Rock":
                        return 2;
                    case "Poison":
                        return 0.5;
                    case "Flying":
                        return 0.5;
                    case "Psychic":
                        return 0.5;
                    case "Bug":
                        return 0.5;
                    case "Ghost":
                        return 0;
                    case "Steel":
                        return 2;
                    default:
                        return 1;
                }
            case "Poison":
                switch (defenseType){
                    case "Grass":
                        return 2;
                    case "Poison":
                        return 0.5;
                    case "Ground":
                        return 0.5;
                    case "Rock":
                        return 0.5;
                    case "Ghost":
                        return 0.5;
                    case "Steel":
                        return 0;
                    default:
                        return 1;
                }
            case "Ground":
                switch (defenseType){
                    case "Fire":
                        return 2;
                    case "Electric":
                        return 2;
                    case "Grass":
                        return 0.5;
                    case "Poison":
                        return 2;
                    case "Flying":
                        return 0;
                    case "Bug":
                        return 0.5;
                    case "Rock":
                        return 2;
                    case "Steel":
                        return 2;
                    default:
                        return 1;
                }
            case "Flying":
                switch (defenseType){
                    case "Electric":
                        return 0.5;
                    case "Grass":
                        return 2;
                    case "Fighting":
                        return 2;
                    case "Bug":
                        return 2;
                    case "Rock":
                        return 0.5;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Psychic":
                switch (defenseType){
                    case "Fighting":
                        return 2;
                    case "Poison":
                        return 2;
                    case "Psychic":
                        return 0.5;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Bug":
                switch (defenseType){
                    case "Fire":
                        return 0.5;
                    case "Grass":
                        return 2;
                    case "Fighting":
                        return 0.5;
                    case "Poison":
                        return 0.5;
                    case "Flying":
                        return 0.5;
                    case "Psychic":
                        return 2;
                    case "Ghost":
                        return 0.5;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Rock":
                switch (defenseType){
                    case "Fire":
                        return 2;
                    case "Ice":
                        return 2;
                    case "Fighting":
                        return 0.5;
                    case "Ground":
                        return 0.5;
                    case "Flying":
                        return 2;
                    case "Bug":
                        return 2;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Ghost":
                switch (defenseType){
                    case "Normal":
                        return 0;
                    case "Psychic":
                        return 2;
                    case "Ghost":
                        return 2;
                    case "Dark":
                        return 0.5;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Dragon":
                switch (defenseType){
                    case "Dragon":
                        return 2;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Dark":
                switch (defenseType){
                    case "Fighting":
                        return 0.5;
                    case "Dark":
                        return 2;
                    case "Ghost":
                        return 2;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            case "Steel":
                switch (defenseType){
                    case "Fire":
                        return 0.5;
                    case "Water":
                        return 0.5;
                    case "Electric":
                        return 0.5;
                    case "Ice":
                        return 0.5;
                    case "Rock":
                        return 2;
                    case "Steel":
                        return 0.5;
                    default:
                        return 1;
                }
            default:
                Debug.Log("Error: No Attack Type");
                return 1;
        }
    }
}
