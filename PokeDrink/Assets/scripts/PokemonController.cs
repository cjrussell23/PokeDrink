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
        8,
        // "Doduo",
        6,
        // "Dodrio",
        10,
        // "Seel",
        6,
        // "Dewgong",
        12,
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
        10,
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
        18,
        // "Ditto",
        10,
        // "Eevee",
        8,
        // "Vaporeon",
        16,
        // "Jolteon",
        16,
        // "Flareon",
        16,
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
        16,
        // "Snorlax",
        18,
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
    private int[] evolutions = {
        // 1 Bulbasaur
        2,
        // 2 Ivysaur
        3,
        // 3 Venusaur
        0,
        // 4 Charmander
        5,
        // 5 Charmeleon
        6,
        // 6 Charizard
        0,
        // 7 Squirtle
        8,
        // 8 Wartortle
        9,
        // 9 Blastoise
        0,
        // 10 Caterpie
        11,
        // 11 Metapod
        12,
        // 12 Butterfree
        0,
        // 13 Weedle
        14,
        // 14 Kakuna
        15,
        // 15 Beedrill
        0,
        // 16 Pidgey
        17,
        // 17 Pidgeotto
        18,
        // 18 Pidgeot
        0,
        // 19 Rattata
        20,
        // 20 Raticate
        0,
        // 21 Spearow
        22,
        // 22 Fearow
        0,
        // 23 Ekans
        24,
        // 24 Arbok
        0,
        // 25 Pikachu
        26,
        // 26 Raichu
        0,
        // 27 Sandshrew
        28,
        // 28 Sandslash
        0,
        // 29 Nidoran♀
        30,
        // 30 Nidorina
        31,
        // 31 Nidoqueen
        0,
        // 32 Nidoran♂
        33,
        // 33 Nidorino
        34,
        // 34 Nidoking
        0,
        // 35 Clefairy
        36,
        // 36 Clefable
        0,
        // 37 Vulpix
        38,
        // 38 Ninetales
        0,
        // 39 Jigglypuff
        40,
        // 40 Wigglytuff
        0,
        // 41 Zubat
        42,
        // 42 Golbat
        43,
        // 43 Oddish
        44,
        // 44 Gloom
        45,
        // 45 Vileplume
        0,
        // 46 Paras
        47,
        // 47 Parasect
        0,
        // 48 Venonat
        49,
        // 49 Venomoth
        0,
        // 50 Diglett
        51,
        // 51 Dugtrio
        0,
        // 52 Meowth
        53,
        // 53 Persian
        0,
        // 54 Psyduck
        55,
        // 55 Golduck
        0,
        // 56 Mankey
        57,
        // 57 Primeape
        0,
        // 58 Growlithe
        59,
        // 59 Arcanine
        0,
        // 60 Poliwag
        61,
        // 61 Poliwhirl
        62,
        // 62 Poliwrath
        0,
        // 63 Abra
        64,
        // 64 Kadabra
        65,
        // 65 Alakazam
        0,
        // 66 Machop
        67,
        // 67 Machoke
        68,
        // 68 Machamp
        0,
        // 69 Bellsprout
        70,
        // 70 Weepinbell
        71,
        // 71 Victreebel
        0,
        // 72 Tentacool
        73,
        // 73 Tentacruel
        0,
        // 74 Geodude
        75,
        // 75 Graveler
        76,
        // 76 Golem
        0,
        // 77 Ponyta
        78,
        // 78 Rapidash
        0,
        // 79 Slowpoke
        80,
        // 80 Slowbro
        0,
        // 81 Magnemite
        82,
        // 82 Magneton
        0,
        // 83 Farfetch'd
        0,
        // 84 Doduo
        85,
        // 85 Dodrio
        0,
        // 86 Seel
        87,
        // 87 Dewgong
        0,
        // 88 Grimer
        89,
        // 89 Muk
        0,
        // 90 Shellder
        91,
        // 91 Cloyster
        0,
        // 92 Gastly
        93,
        // 93 Haunter
        94,
        // 94 Gengar
        0,
        // 95 Onix
        0,
        // 96 Drowzee
        97,
        // 97 Hypno
        0,
        // 98 Krabby
        99,
        // 99 Kingler
        0,
        // 100 Voltorb
        101,
        // 101 Electrode
        0,
        // 102 Exeggcute
        103,
        // 103 Exeggutor
        0,
        // 104 Cubone
        105,
        // 105 Marowak
        0,
        // 106 Hitmonlee
        107,
        // 107 Hitmonchan
        0,
        // 108 Lickitung
        0,
        // 109 Koffing
        110,
        // 110 Weezing
        0,
        // 111 Rhyhorn
        112,
        // 112 Rhydon
        0,
        // 113 Chansey
        0,
        // 114 Tangela
        0,
        // 115 Kangaskhan
        0,
        // 116 Horsea
        117,
        // 117 Seadra
        0,
        // 118 Goldeen
        119,
        // 119 Seaking
        0,
        // 120 Staryu
        121,
        // 121 Starmie
        0,
        // 122 Mr. Mime
        0,
        // 123 Scyther
        0,
        // 124 Jynx
        0,
        // 125 Electabuzz
        0,
        // 126 Magmar
        0,
        // 127 Pinsir
        0,
        // 128 Tauros
        0,
        // 129 Magikarp
        130,
        // 130 Gyarados
        0,
        // 131 Lapras
        0,
        // 132 Ditto
        0,
        // 133 Eevee
        0,
        // 134 Vaporeon
        0,
        // 135 Jolteon
        0,
        // 136 Flareon
        0,
        // 137 Porygon
        0,
        // 138 Omanyte
        0,
        // 139 Omastar
        0,
        // 140 Kabuto
        0,
        // 141 Kabutops
        0,
        // 142 Aerodactyl
        0,
        // 143 Snorlax
        0,
        // 144 Articuno
        0,
        // 145 Zapdos
        0,
        // 146 Moltres
        0,
        // 147 Dratini
        0,
        // 148 Dragonair
        0,
        // 149 Dragonite
        0,
        // 150 Mewtwo
        0,
        // 151 Mew
        0,
    };
    private string[] types = {
        // 1 Bulbasaur
        "Grass",
        // 2 Ivysaur
        "Grass",
        // 3 Venusaur
        "Grass",
        // 4 Charmander
        "Fire",
        // 5 Charmeleon
        "Fire",
        // 6 Charizard
        "Fire",
        // 7 Squirtle
        "Water",
        // 8 Wartortle
        "Water",
        // 9 Blastoise
        "Water",
        // 10 Caterpie
        "Bug",
        // 11 Metapod
        "Bug",
        // 12 Butterfree
        "Bug",
        // 13 Weedle
        "Bug",
        // 14 Kakuna
        "Bug",
        // 15 Beedrill
        "Bug",
        // 16 Pidgey
        "Flying",
        // 17 Pidgeotto
        "Flying",
        // 18 Pidgeot
        "Flying",
        // 19 Rattata
        "Normal",
        // 20 Raticate
        "Normal",
        // 21 Spearow
        "Flying",
        // 22 Fearow
        "Flying",
        // 23 Ekans
        "Poison",
        // 24 Arbok
        "Poison",
        // 25 Pikachu
        "Electric",
        // 26 Raichu
        "Electric",
        // 27 Sandshrew
        "Ground",
        // 28 Sandslash
        "Ground",
        // 29 Nidoran♀
        "Poison",
        // 30 Nidorina
        "Poison",
        // 31 Nidoqueen
        "Poison",
        // 32 Nidoran♂
        "Poison",
        // 33 Nidorino
        "Poison",
        // 34 Nidoking
        "Poison",
        // 35 Clefairy
        "Normal",
        // 36 Clefable
        "Normal",
        // 37 Vulpix
        "Fire",
        // 38 Ninetales
        "Fire",
        // 39 Jigglypuff
        "Normal",
        // 40 Wigglytuff
        "Normal",
        // 41 Zubat
        "Flying",
        // 42 Golbat
        "Flying",
        // 43 Oddish
        "Grass",
        // 44 Gloom
        "Grass",
        // 45 Vileplume
        "Grass",
        // 46 Paras
        "Bug",
        // 47 Parasect
        "Bug",
        // 48 Venonat
        "Bug",
        // 49 Venomoth
        "Bug",
        // 50 Diglett
        "Ground",
        // 51 Dugtrio
        "Ground",
        // 52 Meowth
        "Normal",
        // 53 Persian
        "Normal",
        // 54 Psyduck
        "Water",
        // 55 Golduck
        "Water",
        // 56 Mankey
        "Fighting",
        // 57 Primeape
        "Fighting",
        // 58 Growlithe
        "Fire",
        // 59 Arcanine
        "Fire",
        // 60 Poliwag
        "Water",
        // 61 Poliwhirl
        "Water",
        // 62 Poliwrath
        "Water",
        // 63 Abra
        "Psychic",
        // 64 Kadabra
        "Psychic",
        // 65 Alakazam
        "Psychic",
        // 66 Machop
        "Fighting",
        // 67 Machoke
        "Fighting",
        // 68 Machamp
        "Fighting",
        // 69 Bellsprout
        "Grass",
        // 70 Weepinbell
        "Grass",
        // 71 Victreebel
        "Grass",
        // 72 Tentacool
        "Water",
        // 73 Tentacruel
        "Water",
        // 74 Geodude
        "Rock",
        // 75 Graveler
        "Rock",
        // 76 Golem
        "Rock",
        // 77 Ponyta
        "Fire",
        // 78 Rapidash
        "Fire",
        // 79 Slowpoke
        "Water",
        // 80 Slowbro
        "Water",
        // 81 Magnemite
        "Electric",
        // 82 Magneton
        "Electric",
        // 83 Farfetch'd
        "Flying",
        // 84 Doduo
        "Flying",
        // 85 Dodrio
        "Flying",
        // 86 Seel
        "Water",
        // 87 Dewgong
        "Water",
        // 88 Grimer
        "Poison",
        // 89 Muk
        "Poison",
        // 90 Shellder
        "Water",
        // 91 Cloyster
        "Water",
        // 92 Gastly
        "Ghost",
        // 93 Haunter
        "Ghost",
        // 94 Gengar
        "Ghost",
        // 95 Onix
        "Rock",
        // 96 Drowzee
        "Psychic",
        // 97 Hypno
        "Psychic",
        // 98 Krabby
        "Water",
        // 99 Kingler
        "Water",
        // 100 Voltorb
        "Electric",
        // 101 Electrode
        "Electric",
        // 102 Exeggcute
        "Grass",
        // 103 Exeggutor
        "Grass",
        // 104 Cubone
        "Ground",
        // 105 Marowak
        "Ground",
        // 106 Hitmonlee
        "Fighting",
        // 107 Hitmonchan
        "Fighting",
        // 108 Lickitung
        "Normal",
        // 109 Koffing
        "Poison",
        // 110 Weezing
        "Poison",
        // 111 Rhyhorn
        "Ground",
        // 112 Rhydon
        "Ground",
        // 113 Chansey
        "Normal",
        // 114 Tangela
        "Grass",
        // 115 Kangaskhan
        "Normal",
        // 116 Horsea
        "Water",
        // 117 Seadra
        "Water",
        // 118 Goldeen
        "Water",
        // 119 Seaking
        "Water",
        // 120 Staryu
        "Water",
        // 121 Starmie
        "Water",
        // 122 Mr. Mime
        "Psychic",
        // 123 Scyther
        "Bug",
        // 124 Jynx
        "Ice",
        // 125 Electabuzz
        "Electric",
        // 126 Magmar
        "Fire",
        // 127 Pinsir
        "Bug",
        // 128 Tauros
        "Normal",
        // 129 Magikarp
        "Water",
        // 130 Gyarados
        "Water",
        // 131 Lapras
        "Ice",
        // 132 Ditto
        "Normal",
        // 133 Eevee
        "Normal",
        // 134 Vaporeon
        "Water",
        // 135 Jolteon
        "Electric",
        // 136 Flareon
        "Fire",
        // 137 Porygon
        "Normal",
        // 138 Omanyte
        "Rock",
        // 139 Omastar
        "Rock",
        // 140 Kabuto
        "Rock",
        // 141 Kabutops
        "Rock",
        // 142 Aerodactyl
        "Rock",
        // 143 Snorlax
        "Normal",
        // 144 Articuno
        "Ice",
        // 145 Zapdos
        "Electric",
        // 146 Moltres
        "Fire",
        // 147 Dratini
        "Dragon",
        // 148 Dragonair
        "Dragon",
        // 149 Dragonite
        "Dragon",
        // 150 Mewtwo
        "Psychic",
        // 151 Mew
        "Psychic",
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
            pokemon[i] = new Pokemon(pokemonNames[i], pokemonSprites[i], pokemonCatchDifficulty[i], types[i], i+1);
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
                p.SetExpToLevel(15);
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
        for (int index = 0; index < pokemon.Length; index++)
        {
            if (evolutions[index] != 0)
            {
                pokemon[index].SetEvolution(pokemon[evolutions[index] - 1]);
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
    public Pokemon GetPokemon(int index)
    {
        if (index >= 0 && index <= 151)
        {
            return pokemon[index];
        }
        else
        {
            Debug.Log("Invalid pokemon index");
            return null;
        }
    }
    public int Length()
    {
        return pokemon.Length;
    }
}
public class Pokemon {
    private int id;
    private string name;
    private Sprite sprite;
    private int catchDifficulty;
    private bool isLocked;
    private int experience;
    private int expToLevel;
    private string type;
    private Pokemon evolution;
    public Pokemon (string name, Sprite sprite, int catchDifficulty, string type, int id, int expToLevel = 10) {
        this.name = name;
        this.sprite = sprite;
        this.catchDifficulty = catchDifficulty;
        this.isLocked = false;
        this.id = id;
        this.experience = 0;
        this.expToLevel = expToLevel;
        this.evolution = null;
        this.type = type;
    }
    public void SetEvolution(Pokemon evolution)
    {
        this.evolution = evolution;
    }
    public Pokemon GetEvolution()
    {
        return evolution;
    }
    public int GetId()
    {
        return id;
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
    public int GetExperience()
    {
        return experience;
    }
    public void SetExperience(int experience)
    {
        this.experience = experience;
    }
    public void SetExpToLevel(int expToLevel)
    {
        this.expToLevel = expToLevel;
    }
    public int GetExpToLevel(){
        return expToLevel;
    }
    public string GetPokemonType()
    {
        return type;
    }
}
