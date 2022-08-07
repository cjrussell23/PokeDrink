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
    [SerializeField] private GameObject pokemonPrefab;
    [SerializeField] private GameObject pokemonInventory;
    // Start is called before the first frame update
    void Start()
    {
        chatManager = gameObject.GetComponent<ChatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                break;
            }
        }
    }
    private void ReplacePokemon(Pokemon pokemon){
        int pokemonToReplace = FindWeakestPokemon();
        if (pokemonParty[pokemonToReplace].GetCatchDifficulty() < pokemon.GetCatchDifficulty())
        {
            Debug.Log("Replacing pokemon " + pokemonParty[pokemonToReplace].GetName() + " with " + pokemon.GetName());
            chatManager.CmdSendMessage("Replaced their " + pokemonParty[pokemonToReplace].GetName() + " with " + pokemon.GetName());
            pokemonParty[pokemonToReplace] = pokemon;
            pokemonPartyUI[pokemonToReplace].transform.GetChild(0).GetComponent<Image>().sprite = pokemon.GetSprite();
        }
        else {
            chatManager.CmdSendMessage("Could not add " + pokemon.GetName() + " to thier party because it is too weak :(");
            Debug.Log("Pokemon " + pokemon.GetName() + " is not strong enough to replace " + pokemonParty[pokemonToReplace].GetName());
        }
    }
    private int FindWeakestPokemon(){
        int weakestPokemonIndex = 0;
        for (int i = 1; i < pokemonParty.Length; i++)
        {
            if (pokemonParty[i] != null)
            {
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
}
