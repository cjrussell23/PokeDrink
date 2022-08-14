using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInventoriesManager : MonoBehaviour
{
    // Badges
    [SerializeField] private Sprite badge1;
    [SerializeField] private Sprite badge2;
    [SerializeField] private Sprite badge3;
    [SerializeField] private Sprite badge4;
    [SerializeField] private Sprite badge5;
    [SerializeField] private Sprite badge6;
    [SerializeField] private Sprite badge7;
    [SerializeField] private Sprite badge8;
    private Sprite[] badges = new Sprite[8];
    [SerializeField] private GameObject badgePrefab;
    [SerializeField]
    private GameObject playerInventories;

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private GameObject inventoryItemPrefab;
    private List<GameObject> listOfInventories = new List<GameObject>();
    private PokemonController pokemonController;
    [SerializeField] private GameObject pokemonNetworkItemPrefab;
    private MyNetworkManager game;
    private MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    void Start()
    {
        badges[0] = badge1;
        badges[1] = badge2;
        badges[2] = badge3;
        badges[3] = badge4;
        badges[4] = badge5;
        badges[5] = badge6;
        badges[6] = badge7;
        badges[7] = badge8;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(playerInventories);
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
        SceneManager.activeSceneChanged += SceneChanged;
    }

    void SceneChanged(Scene current, Scene next)
    {
        if (next.name == "Scene_SteamworksGame")
        {
            // When the game scene is loaded, Spawn inventory prefabs for each player
            Debug.Log("Scene changed to game scene");
            if (listOfInventories.Count == 0)
            {
                SpawnInventoryPrefabs();
            }
        }
    }
    private void SpawnInventoryPrefabs()
    {
        Debug.Log("Spawning inventory prefabs");
        foreach (GamePlayer player in Game.GamePlayers)
        {
            Debug.Log("Spawning inventory prefab for player: " + player.playerName);
            GameObject inventory = Instantiate(inventoryItemPrefab, inventoryPanel.transform);
            inventory.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player.playerName;
            inventory.transform.GetChild(0).GetComponent<Image>().color = player.gameObject.GetComponent<PlayerInfo>().playerColor;
            listOfInventories.Add(inventory);
        }
    }
    public void UpdateInventories(){
        int index = 0;
        foreach (GamePlayer player in Game.GamePlayers)
        {
            // Update player name
            listOfInventories[index].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player.playerName;
            // Update player Color
            listOfInventories[index].transform.GetChild(0).GetComponent<Image>().color = player.gameObject.GetComponent<PlayerInfo>().playerColor;
            // Destroy old pokemonNetworkItems
            foreach (Transform child in listOfInventories[index].transform.GetChild(1))
            {
                if (child.gameObject.tag == "PokemonNetworkItem")
                {
                    Destroy(child.gameObject);
                }
            }
            // Spawn new pokemonNetworkItems
            foreach (int pokemonID in player.playerPokemon)
            {
                Pokemon pokemon = pokemonController.GetPokemon(pokemonID - 1);
                GameObject pokemonItem = Instantiate(pokemonNetworkItemPrefab, listOfInventories[index].transform.GetChild(1));
                pokemonItem.transform.GetChild(0).GetComponent<Image>().sprite = pokemon.GetSprite();
                pokemonItem.transform.GetChild(1).GetComponentInChildren<Text>().text = pokemon.GetCatchDifficulty().ToString();
            }
            index++;
        }
    }
    public void UpdateBadges(){
        int index = 0;
        foreach (GamePlayer player in Game.GamePlayers)
        {
            // Destroy old badges
            foreach (Transform child in listOfInventories[index].transform.GetChild(2))
            {
                if (child.gameObject.tag == "Badge")
                {
                    Destroy(child.gameObject);
                }
            }
            // Spawn new badges
            int badgeCount = player.GetPlayerBadgeCount();
            for(int i = 0; i < badgeCount; i++){
                GameObject badge = Instantiate(badgePrefab, listOfInventories[index].transform.GetChild(2));
                badge.transform.GetChild(0).GetComponent<Image>().sprite = badges[i];
            }
            index++;
        }
    }
}
