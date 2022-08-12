using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymEncounter : MonoBehaviour
{
    [SerializeField] private int GymID;
    [SerializeField] private Sprite gymBadgeSprite;
    private Pokemon[] pokemonParty = new Pokemon[3];
    private PokemonController pokemonController;
    private string gymLeaderName;
    void Start(){
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
        SetPokemonParty(GymID);
    }
    public Pokemon[] GetPokemonParty(){
        return pokemonParty;
    }
    public string GetGymLeaderName(){
        return gymLeaderName;
    }
    // @TODO: Set Pokemon Parties to the correct Pokemon Party for the Gym
    private void SetPokemonParty(int id){
        switch (id){
            case 1:
                gymLeaderName = "Brock";
                pokemonParty[0] = pokemonController.GetPokemon(74 - 1); // Geodude
                pokemonParty[1] = pokemonController.GetPokemon(95 - 1); // Onix
                pokemonParty[2] = pokemonController.GetPokemon(95 - 1); // Onix
                break;
            case 2:
                gymLeaderName = "Misty";
                pokemonParty[0] = pokemonController.GetPokemon(120 - 1); // Staryu
                pokemonParty[1] = pokemonController.GetPokemon(121 - 1); // Starmie
                pokemonParty[2] = pokemonController.GetPokemon(121 - 1); // Starmie
                break;
            case 3:
                gymLeaderName = "Lt. Surge";
                pokemonParty[0] = pokemonController.GetPokemon(100); // Voltorb
                pokemonParty[1] = pokemonController.GetPokemon(25); // Pikachu
                pokemonParty[2] = pokemonController.GetPokemon(26); // Raichu
                break;
            case 4:
                gymLeaderName = "Erika";
                pokemonParty[0] = pokemonController.GetPokemon(71 - 1); // Victreebel
                pokemonParty[1] = pokemonController.GetPokemon(114 - 1); // Tangela
                pokemonParty[2] = pokemonController.GetPokemon(45 - 1); // Vileplume
                break;
            case 5:
                gymLeaderName = "Koga";
                pokemonParty[0] = pokemonController.GetPokemon(109 - 1); // Koffing
                pokemonParty[1] = pokemonController.GetPokemon(89 - 1); // Muk
                pokemonParty[2] = pokemonController.GetPokemon(110 - 1); // Weezing
                break;
            case 6:
                gymLeaderName = "Sabrina";
                pokemonParty[0] = pokemonController.GetPokemon(49 - 1); // Venomoth
                pokemonParty[1] = pokemonController.GetPokemon(122 - 1); // Mr. Mime
                pokemonParty[2] = pokemonController.GetPokemon(65 - 1); // Alakazam
                break;
            case 7:
                gymLeaderName = "Blaine";
                pokemonParty[0] = pokemonController.GetPokemon(59 - 1); // Arcanine
                pokemonParty[1] = pokemonController.GetPokemon(78 - 1); // Rapidash
                pokemonParty[2] = pokemonController.GetPokemon(6 - 1); // Charizard
                break;
            case 8:
                gymLeaderName = "Giovanni";
                pokemonParty[0] = pokemonController.GetPokemon(31 - 1); // Nidoqueen
                pokemonParty[1] = pokemonController.GetPokemon(34 - 1); // Nidoking
                pokemonParty[2] = pokemonController.GetPokemon(112 - 1); // Rydon
                break;
        }
    }
    public Sprite GetGymBadgeSprite(){
        return gymBadgeSprite;
    }
    public int GetGymID(){
        return GymID;
    }
}
