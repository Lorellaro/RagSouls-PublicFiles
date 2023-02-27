using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRandomiser : MonoBehaviour
{
    [SerializeField] List<Mesh> possibleMeshes;


    private void Awake()
    {
        //Randomise Mesh
        GetComponent<MeshFilter>().mesh = possibleMeshes[Random.Range(0, possibleMeshes.Count)];
    }

}
