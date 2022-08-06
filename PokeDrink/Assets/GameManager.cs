using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    private void Start() {
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("There are " + players.Length + " players in the game.");
    }
}
