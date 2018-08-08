using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTreesUtility : MonoBehaviour {

    //Array with trees we are going to combine
    public GameObject[] treesArray;
    //The object that is going to hold the combined mesh
    public GameObject combinedObj;

    void Start()
    {
        CombineTrees();
    }

    //Similar to Unity's reference, but with different materials
    //http://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
    void CombineTrees()
    {
        //Lists that holds mesh data that belongs to each submesh
        List<CombineInstance> woodList = new List<CombineInstance>();
        List<CombineInstance> leafList = new List<CombineInstance>();

        //Loop through the array with trees
        for (int i = 0; i < treesArray.Length; i++)
        {
            GameObject currentTree = treesArray[i];

            //Deactivate the tree 
            currentTree.SetActive(false);

            //Get all meshfilters from this tree, true to also find deactivated children
            MeshFilter[] meshFilters = currentTree.GetComponentsInChildren<MeshFilter>(true);

            //Loop through all children
            for (int j = 0; j < meshFilters.Length; j++)
            {
                MeshFilter meshFilter = meshFilters[j];

                CombineInstance combine = new CombineInstance();

                //Is it wood or leaf?
                MeshRenderer meshRender = meshFilter.GetComponent<MeshRenderer>();

                //Modify the material name, because Unity adds (Instance) to the end of the name
                string materialName = meshRender.material.name.Replace(" (Instance)", "");

                if (materialName == "Leaf")
                {
                    combine.mesh = meshFilter.mesh;
                    combine.transform = meshFilter.transform.localToWorldMatrix;

                    //Add it to the list of leaf mesh data
                    leafList.Add(combine);
                }
                else if (materialName == "Wood")
                {
                    combine.mesh = meshFilter.mesh;
                    combine.transform = meshFilter.transform.localToWorldMatrix;

                    //Add it to the list of wood mesh data
                    woodList.Add(combine);
                }
            }
        }


        //First we need to combine the wood into one mesh and then the leaf into one mesh
        Mesh combinedWoodMesh = new Mesh();
        combinedWoodMesh.CombineMeshes(woodList.ToArray());

        Mesh combinedLeafMesh = new Mesh();
        combinedLeafMesh.CombineMeshes(leafList.ToArray());

        //Create the array that will form the combined mesh
        CombineInstance[] totalMesh = new CombineInstance[2];

        //Add the submeshes in the same order as the material is set in the combined mesh
        totalMesh[0].mesh = combinedLeafMesh;
        totalMesh[0].transform = combinedObj.transform.localToWorldMatrix;
        totalMesh[1].mesh = combinedWoodMesh;
        totalMesh[1].transform = combinedObj.transform.localToWorldMatrix;

        //Create the final combined mesh
        Mesh combinedAllMesh = new Mesh();

        //Make sure it's set to false to get 2 separate meshes
        combinedAllMesh.CombineMeshes(totalMesh, false);
        combinedObj.GetComponent<MeshFilter>().mesh = combinedAllMesh;
    }
}
