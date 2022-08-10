using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonController : MonoBehaviour
{
    public static PokemonController instance;
    private string[] pokemonNames =
    {
        "Bulbasaur",
        "Ivysaur",
        "Venusaur",
        "Charmander",
        "Charmeleon",
        "Charizard",
        "Squirtle",
        "Wartortle",
        "Blastoise",
        "Caterpie",
        "Metapod",
        "Butterfree",
        "Weedle",
        "Kakuna",
        "Beedrill",
        "Pidgey",
        "Pidgeotto",
        "Pidgeot",
        "Rattata",
        "Raticate",
        "Spearow",
        "Fearow",
        "Ekans",
        "Arbok",
        "Pikachu",
        "Raichu",
        "Sandshrew",
        "Sandslash",
        "Nidoran♀",
        "Nidorina",
        "Nidoqueen",
        "Nidoran♂",
        "Nidorino",
        "Nidoking",
        "Clefairy",
        "Clefable",
        "Vulpix",
        "Ninetales",
        "Jigglypuff",
        "Wigglytuff",
        "Zubat",
        "Golbat",
        "Oddish",
        "Gloom",
        "Vileplume",
        "Paras",
        "Parasect",
        "Venonat",
        "Venomoth",
        "Diglett",
        "Dugtrio",
        "Meowth",
        "Persian",
        "Psyduck",
        "Golduck",
        "Mankey",
        "Primeape",
        "Growlithe",
        "Arcanine",
        "Poliwag",
        "Poliwhirl",
        "Poliwrath",
        "Abra",
        "Kadabra",
        "Alakazam",
        "Machop",
        "Machoke",
        "Machamp",
        "Bellsprout",
        "Weepinbell",
        "Victreebel",
        "Tentacool",
        "Tentacruel",
        "Geodude",
        "Graveler",
        "Golem",
        "Ponyta",
        "Rapidash",
        "Slowpoke",
        "Slowbro",
        "Magnemite",
        "Magneton",
        "Farfetch'd",
        "Doduo",
        "Dodrio",
        "Seel",
        "Dewgong",
        "Grimer",
        "Muk",
        "Shellder",
        "Cloyster",
        "Gastly",
        "Haunter",
        "Gengar",
        "Onix",
        "Drowzee",
        "Hypno",
        "Krabby",
        "Kingler",
        "Voltorb",
        "Electrode",
        "Exeggcute",
        "Exeggutor",
        "Cubone",
        "Marowak",
        "Hitmonlee",
        "Hitmonchan",
        "Lickitung",
        "Koffing",
        "Weezing",
        "Rhyhorn",
        "Rhydon",
        "Chansey",
        "Tangela",
        "Kangaskhan",
        "Horsea",
        "Seadra",
        "Goldeen",
        "Seaking",
        "Staryu",
        "Starmie",
        "Mr. Mime",
        "Scyther",
        "Jynx",
        "Electabuzz",
        "Magmar",
        "Pinsir",
        "Tauros",
        "Magikarp",
        "Gyarados",
        "Lapras",
        "Ditto",
        "Eevee",
        "Vaporeon",
        "Jolteon",
        "Flareon",
        "Porygon",
        "Omanyte",
        "Omastar",
        "Kabuto",
        "Kabutops",
        "Aerodactyl",
        "Snorlax",
        "Articuno",
        "Zapdos",
        "Moltres",
        "Dratini",
        "Dragonair",
        "Dragonite",
        "Mewtwo",
        "Mew"
    };
    private int[] pokemonCatchDifficulty = {
        // "Bulbasaur",
        4,
        // "Ivysaur",
        10,
        // "Venusaur",
        20,
        // "Charmander",
        4,
        // "Charmeleon",
        10,
        // "Charizard",
        20,
        // "Squirtle",
        4,
        // "Wartortle",
        10,
        // "Blastoise",
        20,
        // "Caterpie",
        4,
        // "Metapod",
        8,
        // "Butterfree",
        12,
        // "Weedle",
        4,
        // "Kakuna",
        8,
        // "Beedrill",
        12,
        // "Pidgey",
        4,
        //  "Pidgeotto",
        8,
        // "Pidgeot",
        12,
        // "Rattata",
        4,
        // "Raticate",
        10,
        // "Spearow",
        4,
        // "Fearow",
        10,
        // "Ekans",
        4,
        // "Arbok",
        10,
        // "Pikachu",
        6,
        // "Raichu",
        10,
        // "Sandshrew",
        6,
        // "Sandslash",
        10,
        // "Nidoran♀",
        6,
        // "Nidorina",
        10,
        // "Nidoqueen",
        20,
        // "Nidoran♂",
        6,
        // "Nidorino",
        10,
        // "Nidoking",
        20,
        // "Clefairy",
        6,
        // "Clefable",
        10,
        // "Vulpix",
        6,
        // "Ninetales",
        10,
        // "Jigglypuff",
        6,
        // "Wigglytuff",
        10,
        // "Zubat",
        4,
        // "Golbat",
        10,
        // "Oddish",
        6,
        // "Gloom",
        10,
        // "Vileplume",
        20,
        // "Paras",
        6,
        // "Parasect",
        10,
        // "Venonat",
        6,
        // "Venomoth",
        10,
        // "Diglett",
        6,
        // "Dugtrio",
        10,
        // "Meowth",
        6,
        // "Persian",
        10,
        // "Psyduck",
        6,
        // "Golduck",
        10,
        // "Mankey",
        6,
        // "Primeape",
        10,
        // "Growlithe",
        6,
        // "Arcanine",
        12,
        // "Poliwag",
        6,
        // "Poliwhirl",
        10,
        // "Poliwrath",
        20,
        // "Abra",
        6,
        // "Kadabra",
        10,
        // "Alakazam",
        20,
        // "Machop",
        6,
        // "Machoke",
        10,
        // "Machamp",
        20,
        // "Bellsprout",
        6,
        // "Weepinbell",
        10,
        // "Victreebel",
        20,
        // "Tentacool",
        6,
        // "Tentacruel",
        10,
        // "Geodude",
        6,
        // "Graveler",
        10,
        // "Golem",
        20,
        // "Ponyta",
        6,
        // "Rapidash",
        10,
        // "Slowpoke",
        6,
        // "Slowbro",
        10,
        // "Magnemite",
        6,
        // "Magneton",
        10,
        // "Farfetch'd",
        20,
        // "Doduo",
        6,
        // "Dodrio",
        10,
        // "Seel",
        6,
        // "Dewgong",
        10,
        // "Grimer",
        6,
        // "Muk",
        10,
        // "Shellder",
        6,
        // "Cloyster",
        10,
        // "Gastly",
        6,
        // "Haunter",
        10,
        // "Gengar",
        20,
        // "Onix",
        6,
        // "Drowzee",
        6,
        // "Hypno",
        10,
        // "Krabby",
        6,
        // "Kingler",
        10,
        // "Voltorb",
        6,
        // "Electrode",
        10,
        // "Exeggcute",
        6,
        // "Exeggutor",
        10,
        // "Cubone",
        6,
        // "Marowak",
        10,
        // "Hitmonlee",
        10,
        // "Hitmonchan",
        10,
        // "Lickitung",
        10,
        // "Koffing",
        6,
        // "Weezing",
        10,
        // "Rhyhorn",
        8,
        // "Rhydon",
        12,
        // "Chansey",
        10,
        // "Tangela",
        10,
        // "Kangaskhan",
        10,
        // "Horsea",
        6,
        // "Seadra",
        10,
        // "Goldeen",
        6,
        // "Seaking",
        10,
        // "Staryu",
        6,
        // "Starmie",
        10,
        // "Mr. Mime",
        10,
        // "Scyther",
        10,
        // "Jynx",
        10,
        // "Electabuzz",
        10,
        // "Magmar",
        10,
        // "Pinsir",
        10,
        // "Tauros",
        10,
        // "Magikarp",
        4,
        // "Gyarados",
        20,
        // "Lapras",
        16,
        // "Ditto",
        10,
        // "Eevee",
        6,
        // "Vaporeon",
        12,
        // "Jolteon",
        12,
        // "Flareon",
        12,
        // "Porygon",
        10,
        // "Omanyte",
        6,
        // "Omastar",
        12,
        // "Kabuto",
        6,
        // "Kabutops",
        12,
        // "Aerodactyl",
        12,
        // "Snorlax",
        16,
        // "Articuno",
        30,
        // "Zapdos",
        30,
        // "Moltres",
        30,
        // "Dratini",
        8,
        // "Dragonair",
        12,
        // "Dragonite",
        22,
        // "Mewtwo",
        40,
        // "Mew"
        30
    };
    private Pokemon[] pokemon = new Pokemon[151];
    private ArrayList commonPokemon = new ArrayList();
    private ArrayList uncommonPokemon = new ArrayList();
    private ArrayList rarePokemon = new ArrayList();
    private ArrayList legendaryPokemon = new ArrayList();
    private Sprite[] pokemonSprites;
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        pokemonSprites = Resources.LoadAll<Sprite>("Pokemon/");
        for (int i = 0; i < 151; i++)
        {
            pokemon[i] = new Pokemon(pokemonNames[i], pokemonSprites[i], pokemonCatchDifficulty[i]);
        }
        foreach (Pokemon p in pokemon)
        {
            if (p.GetCatchDifficulty() <= 6)
            {
                commonPokemon.Add(p);
            }
            else if (p.GetCatchDifficulty() <= 10)
            {
                uncommonPokemon.Add(p);
            }
            else if (p.GetCatchDifficulty() <= 20)
            {
                rarePokemon.Add(p);
            }
            else
            {
                legendaryPokemon.Add(p);
            }
        }
    }
    public Pokemon GetRandomPokemon(int rarity)
    {
        if (rarity == 1)
        {
            return (Pokemon)commonPokemon[Random.Range(0, commonPokemon.Count)];
        }
        else if (rarity == 2)
        {
            return (Pokemon)uncommonPokemon[Random.Range(0, uncommonPokemon.Count)];
        }
        else if (rarity == 3)
        {
            return (Pokemon)rarePokemon[Random.Range(0, rarePokemon.Count)];
        }
        else if (rarity == 4)
        {
            return (Pokemon)legendaryPokemon[Random.Range(0, legendaryPokemon.Count)];
        }
        else
        {
            return null;
        }
    }
    public int Length()
    {
        return pokemon.Length;
    }
}
public class Pokemon {
    private string name;
    private Sprite sprite;
    private int catchDifficulty;
    private bool isLocked;
    public Pokemon (string name, Sprite sprite, int catchDifficulty) {
        this.name = name;
        this.sprite = sprite;
        this.catchDifficulty = catchDifficulty;
        this.isLocked = false;
    }
    public string GetName()
    {
        return name;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public int GetCatchDifficulty()
    {
        return catchDifficulty;
    }
    public bool GetIsLocked()
    {
        return isLocked;
    }
    public void SetIsLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }
}
