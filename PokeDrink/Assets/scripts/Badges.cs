using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badges : MonoBehaviour
{
    private int badges;
    private GamePlayer gamePlayer;
    void Start() {
        gamePlayer = GetComponent<GamePlayer>();
        badges = 0;
    }
    public void AddBadge(){
        badges += 1;
        gamePlayer.CmdSetPlayerBadgeCount(badges);
    }
    public int GetBadges(){
        return badges;
    }
}
