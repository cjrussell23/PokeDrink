using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymEncounter : MonoBehaviour
{
    [SerializeField] private int GymID;
    private Pokemon[] pokemonParty = new Pokemon[6];
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
                pokemonParty[0] = pokemonController.GetPokemon(73);
                pokemonParty[1] = pokemonController.GetPokemon(73);
                pokemonParty[2] = pokemonController.GetPokemon(73);
                pokemonParty[3] = pokemonController.GetPokemon(94);
                pokemonParty[4] = pokemonController.GetPokemon(94);
                pokemonParty[5] = pokemonController.GetPokemon(94);
                break;
            case 2:
                gymLeaderName = "Misty";
                pokemonParty[0] = pokemonController.GetPokemon(120);
                pokemonParty[1] = pokemonController.GetPokemon(120);
                pokemonParty[2] = pokemonController.GetPokemon(120);
                pokemonParty[3] = pokemonController.GetPokemon(119);
                pokemonParty[4] = pokemonController.GetPokemon(119);
                pokemonParty[5] = pokemonController.GetPokemon(119);
                break;
            case 3:
                gymLeaderName = "Lt. Surge";
                pokemonParty[0] = pokemonController.GetPokemon(12);
                pokemonParty[1] = pokemonController.GetPokemon(13);
                pokemonParty[2] = pokemonController.GetPokemon(14);
                pokemonParty[3] = pokemonController.GetPokemon(15);
                pokemonParty[4] = pokemonController.GetPokemon(16);
                pokemonParty[5] = pokemonController.GetPokemon(17);
                break;
            case 4:
                gymLeaderName = "Erika";
                pokemonParty[0] = pokemonController.GetPokemon(18);
                pokemonParty[1] = pokemonController.GetPokemon(19);
                pokemonParty[2] = pokemonController.GetPokemon(20);
                pokemonParty[3] = pokemonController.GetPokemon(21);
                pokemonParty[4] = pokemonController.GetPokemon(22);
                pokemonParty[5] = pokemonController.GetPokemon(23);
                break;
            case 5:
                gymLeaderName = "Koga";
                pokemonParty[0] = pokemonController.GetPokemon(24);
                pokemonParty[1] = pokemonController.GetPokemon(25);
                pokemonParty[2] = pokemonController.GetPokemon(26);
                pokemonParty[3] = pokemonController.GetPokemon(27);
                pokemonParty[4] = pokemonController.GetPokemon(28);
                pokemonParty[5] = pokemonController.GetPokemon(29);
                break;
            case 6:
                gymLeaderName = "Sabrina";
                pokemonParty[0] = pokemonController.GetPokemon(30);
                pokemonParty[1] = pokemonController.GetPokemon(31);
                pokemonParty[2] = pokemonController.GetPokemon(32);
                pokemonParty[3] = pokemonController.GetPokemon(33);
                pokemonParty[4] = pokemonController.GetPokemon(34);
                pokemonParty[5] = pokemonController.GetPokemon(35);
                break;
            case 7:
                gymLeaderName = "Blaine";
                pokemonParty[0] = pokemonController.GetPokemon(36);
                pokemonParty[1] = pokemonController.GetPokemon(37);
                pokemonParty[2] = pokemonController.GetPokemon(38);
                pokemonParty[3] = pokemonController.GetPokemon(39);
                pokemonParty[4] = pokemonController.GetPokemon(40);
                pokemonParty[5] = pokemonController.GetPokemon(40);
                break;
            case 8:
                gymLeaderName = "Giovanni";
                pokemonParty[0] = pokemonController.GetPokemon(110);
                pokemonParty[1] = pokemonController.GetPokemon(50);
                pokemonParty[2] = pokemonController.GetPokemon(30);
                pokemonParty[3] = pokemonController.GetPokemon(33);
                pokemonParty[4] = pokemonController.GetPokemon(111);
                pokemonParty[5] = pokemonController.GetPokemon(111);
                break;
        }
    }
}
