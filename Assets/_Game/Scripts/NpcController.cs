using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private Npc npcPrefab;
    public void CreateNpc()
    {
        var a = Instantiate(npcPrefab);
        
    }
}
