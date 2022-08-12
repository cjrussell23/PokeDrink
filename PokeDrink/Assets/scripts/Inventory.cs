using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
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
    }
    public void StarterSquirtle(){
        Pokemon starter = pokemonController.GetPokemon(6);
        AddPokemon(starter);
        DisableStarterPokemonSelectionCanvas();
    }
    public void StarterBulbasaur(){
        Pokemon starter = pokemonController.GetPokemon(0);
        AddPokemon(starter);
        DisableStarterPokemonSelectionCanvas();
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
        return pokemonParty[randomIndex];
    }
    public void LockPokemon(int index, bool lockState){
        pokemonParty[index].SetIsLocked(lockState);
        Debug.Log(pokemonParty[index].GetName() + " lockState set to " + lockState);
    }
}
