using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    private int nextPokemonToRollWith;
    private IEnumerator blinkingImage;
    [SerializeField] private int pokemonInParty = 0;
    private Pokemon[] pokemonParty = new Pokemon[6];
    private GameObject[] pokemonPartyUI = new GameObject[6];
    private ChatManager chatManager;
    private PokemonController pokemonController;
    [SerializeField] private GameObject pokemonPrefab;
    [SerializeField] private GameObject pokemonInventory;
    [SerializeField] private GameObject starterSelectionCanvas;
    private GamePlayer gamePlayer;
    // Start is called before the first frame update
    void Start()
    {
        nextPokemonToRollWith = 0;
        gamePlayer = GetComponent<GamePlayer>();
        starterSelectionCanvas.gameObject.SetActive(true);
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
        chatManager = gameObject.GetComponent<ChatManager>();
    }
    private void DisableStarterPokemonSelectionCanvas(){
        starterSelectionCanvas.SetActive(false);
    }
    public void StarterCharmander(){
        Pokemon starter = pokemonController.GetPokemon(3);
        AddPokemon(starter);
        DisableStarterPokemonSelectionCanvas();
        Image imageToBlink = pokemonPartyUI[nextPokemonToRollWith].transform.GetChild(3).GetComponent<Image>();
        blinkingImage = BlinkImage(imageToBlink);
        StartCoroutine(blinkingImage);
    }
    public void StarterSquirtle(){
        Pokemon starter = pokemonController.GetPokemon(6);
        AddPokemon(starter);
        DisableStarterPokemonSelectionCanvas();nextPokemonToRollWith = 0;
        Image imageToBlink = pokemonPartyUI[nextPokemonToRollWith].transform.GetChild(3).GetComponent<Image>();
        blinkingImage = BlinkImage(imageToBlink);
        StartCoroutine(blinkingImage);
    }
    public void StarterBulbasaur(){
        Pokemon starter = pokemonController.GetPokemon(0);
        AddPokemon(starter);
        DisableStarterPokemonSelectionCanvas();
        Image imageToBlink = pokemonPartyUI[nextPokemonToRollWith].transform.GetChild(3).GetComponent<Image>();
        blinkingImage = BlinkImage(imageToBlink);
        StartCoroutine(blinkingImage);
    }
    public void AddPokemon(Pokemon pokemon)
    {
        if (pokemonInParty < pokemonParty.Length)
        {
            AddPokemonToOpenSlot(pokemon);
        }
        else {
            ReplacePokemon(pokemon);
        }
        
    }
    private void AddPokemonToOpenSlot(Pokemon pokemon){
        for (int i = 0; i < pokemonParty.Length; i++)
        {
            if (pokemonParty[i] == null)
            {
                pokemonInParty += 1;
                pokemonParty[i] = pokemon;
                GameObject newPokemon = Instantiate(pokemonPrefab, pokemonInventory.transform);
                pokemonPartyUI[i] = newPokemon;
                newPokemon.transform.GetChild(0).GetComponent<Image>().sprite = pokemon.GetSprite();
                newPokemon.transform.GetChild(1).GetComponentInChildren<Text>().text = pokemon.GetCatchDifficulty().ToString();
                newPokemon.GetComponent<InventoryItem>().SetIndex(i);
                // Send message to server to add pokemon to party
                gamePlayer.CmdAddPokemon(pokemon.GetId());
                break;
            }
        }
    }
    private void ReplacePokemon(Pokemon pokemon){
        int pokemonToReplace = FindWeakestPokemon();
        if (pokemonToReplace == -1)
        {   
            chatManager.CmdSendMessage("Could not add " + pokemon.GetName() + "(" + pokemon.GetCatchDifficulty().ToString() + ")" +" to thier party because it is too weak :(");
            return;
        }
        if (pokemonParty[pokemonToReplace].GetCatchDifficulty() < pokemon.GetCatchDifficulty())
        {
            Debug.Log("Replacing pokemon " + pokemonParty[pokemonToReplace].GetName() + " with " + pokemon.GetName());
            chatManager.CmdSendMessage("Replaced their " + pokemonParty[pokemonToReplace].GetName() + " with " + pokemon.GetName());
            pokemonParty[pokemonToReplace] = pokemon;
            pokemonPartyUI[pokemonToReplace].transform.GetChild(0).GetComponent<Image>().sprite = pokemon.GetSprite();
            pokemonPartyUI[pokemonToReplace].transform.GetChild(1).GetComponentInChildren<Text>().text = pokemon.GetCatchDifficulty().ToString();
            // Send message to server to replace pokemon
            gamePlayer.CmdSetPokemonAt(pokemonToReplace, pokemon.GetId());
        }
        else {
            chatManager.CmdSendMessage("Could not add " + pokemon.GetName() + "(" + pokemon.GetCatchDifficulty().ToString() + ")" +" to thier party because it is too weak :(");
            Debug.Log("Pokemon " + pokemon.GetName() + " is not strong enough to replace " + pokemonParty[pokemonToReplace].GetName());
        }
    }
    private int FindWeakestPokemon(){
        int weakestPokemonIndex = -1;
        for (int i = 0; i < pokemonParty.Length; i++)
        {
            if (pokemonParty[i] != null && !pokemonParty[i].GetIsLocked())
            {
                if (weakestPokemonIndex == -1){
                    weakestPokemonIndex = i;
                }
                if (pokemonParty[i].GetCatchDifficulty() < pokemonParty[weakestPokemonIndex].GetCatchDifficulty())
                {
                    weakestPokemonIndex = i;
                }
            }
        }
        return weakestPokemonIndex;
    }
    public Pokemon GetRandomPokemon(){
        if (pokemonInParty == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, pokemonInParty);
        Debug.Log("Random index is " + randomIndex);
        AddExperience(randomIndex, 1);
        return pokemonParty[randomIndex];
    }
    public IEnumerator BlinkImage(Image image){
        while (true)
        {
            image.enabled = !image.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public Pokemon GetNextPokemonToRollWith(){
        // Get next pokemon to roll with, Add experience to that pokemon, and return it
        Pokemon pokemonToReturn = pokemonParty[nextPokemonToRollWith];
        AddExperience(nextPokemonToRollWith, 1);

        // Increment next pokemon to roll with
        // Disable the current pokemons active icon
        StopCoroutine(blinkingImage);
        pokemonPartyUI[nextPokemonToRollWith].transform.GetChild(3).GetComponent<Image>().enabled = false;
        // Increment the next pokemon to roll with
        if (nextPokemonToRollWith + 1 > pokemonInParty - 1)
        {
            nextPokemonToRollWith = 0;
        }
        else {
            nextPokemonToRollWith += 1;
        }
        // Enable the next pokemon to roll with active icon
        Image imageToBlink = pokemonPartyUI[nextPokemonToRollWith].transform.GetChild(3).GetComponent<Image>();
        blinkingImage = BlinkImage(imageToBlink);
        StartCoroutine(blinkingImage);
        return pokemonToReturn;
    }
    public void LockPokemon(int index, bool lockState){
        pokemonParty[index].SetIsLocked(lockState);
        Debug.Log(pokemonParty[index].GetName() + " lockState set to " + lockState);
    }
    public void AddExperience (int index, int expAmount){
        int currentExp = pokemonParty[index].GetExperience();
        if (currentExp + expAmount > pokemonParty[index].GetExpToLevel()){
            EvolvePokemon(index);
        }
        else {
            pokemonParty[index].SetExperience(currentExp + expAmount);
        }
    }
    public void EvolvePokemon(int index){
        Pokemon oldPokemon = pokemonParty[index];
        if (oldPokemon.GetId() == 133){ // Eevee
            // Evolve into random eeveelution
            int randomEeveelution = Random.Range(134, 137);
            Pokemon newEeveelution = pokemonController.GetPokemon(randomEeveelution);
            pokemonParty[index] = newEeveelution;
            pokemonPartyUI[index].transform.GetChild(0).GetComponent<Image>().sprite = newEeveelution.GetSprite();
            pokemonPartyUI[index].transform.GetChild(1).GetComponentInChildren<Text>().text = newEeveelution.GetCatchDifficulty().ToString();
            // Send message to server to evolve pokemon
            gamePlayer.CmdSetPokemonAt(index, newEeveelution.GetId());
            chatManager.CmdSendMessage("Evolved " + oldPokemon.GetName() + " into " + newEeveelution.GetName());
        }
        else {
            // Check if pokemon has an evolution
            if (oldPokemon.GetEvolution() == null){
                return;
            }
            Pokemon newPokemon = pokemonController.GetPokemon(oldPokemon.GetEvolution().GetId() - 1);
            if (newPokemon != null){
                pokemonParty[index] = newPokemon;
                pokemonPartyUI[index].transform.GetChild(0).GetComponent<Image>().sprite = newPokemon.GetSprite();
                pokemonPartyUI[index].transform.GetChild(1).GetComponentInChildren<Text>().text = newPokemon.GetCatchDifficulty().ToString();
                // Send message to server to evolve pokemon
                gamePlayer.CmdSetPokemonAt(index, newPokemon.GetId());
                chatManager.CmdSendMessage("Evolved " + oldPokemon.GetName() + " into " + newPokemon.GetName());
            }
        }
    }
}
