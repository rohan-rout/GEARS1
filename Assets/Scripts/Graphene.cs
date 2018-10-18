using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class Graphene : MonoBehaviour
{
    public Transform carbonAtom;

    public string filePath;
    public int no_files;
    public double[] newCenter = new double[3];
     double[] scaleFactor = new double[3];

    double[] l = new double[3];
    private double[] angle = new double[3];
    private List<double[]> positions = new List<double[]>();

    private double[] maxPos = new double[3];
    private double[] minPos = new double[3];

    private List<Transform> atoms = new List<Transform>();

    int numAtoms;
    int filenumber = 0;
    public void RenderAtoms()
    {
        Readfile(filePath + filenumber.ToString("D9") + ".xyz");
        computeDisplacement();
        filenumber++;
        for (int atomIndex = 0; atomIndex < numAtoms; atomIndex++)
        {
            double[] rr = positions[atomIndex];

            Transform atom = Instantiate(carbonAtom);

            atom.transform.parent = this.gameObject.transform;

            atom.transform.position = new Vector3(
                (float)rr[0],
                (float)rr[1],
                (float)rr[2]);
            
            atom.transform.localScale = new Vector3(
                        atom.transform.localScale.x * (float)scaleFactor[0],
                        atom.transform.localScale.y * (float)scaleFactor[0],
                        atom.transform.localScale.z * (float)scaleFactor[0]);

            atoms.Add(atom);

        }
    }

    // Use this for initialization
    void Start()
    {
        RenderAtoms();
        //RenderBonds();
    }

    void FixedUpdate()
    {
        if (filenumber < no_files)
        {
            Debug.Log(filePath + filenumber.ToString("D9") + ".xyz");
            string path = filePath + filenumber.ToString("D9") + ".xyz";
            Readfile(path);
            computeDisplacement();
            filenumber++;
            using (var pos = positions.GetEnumerator())
            using (var atom = atoms.GetEnumerator())
            {
                while (pos.MoveNext() && atom.MoveNext())
                {
                    double[] atom_pos = pos.Current;
                    var atom_current = atom.Current;

                    Vector3 position = new Vector3((float)atom_pos[0], (float)atom_pos[1], (float)atom_pos[2]);
                    atom_current.transform.position = position;
                    atom_current.transform.localScale = new Vector3(
                        atom_current.transform.localScale.x * (float)scaleFactor[0],
                        atom_current.transform.localScale.y * (float)scaleFactor[0],
                        atom_current.transform.localScale.z * (float)scaleFactor[0]);

                }
            }
        }

    }

    public void computeDisplacement()
    {
        double[] originalCenter = new double[3];


        for (int index = 0; index < 3; index++)
        {
            originalCenter[index] = (maxPos[index] + minPos[index]) / 2.0;
            scaleFactor[index] = newCenter[index] / originalCenter[index];
        }
      // Debug.Log("Original Center: " + originalCenter[0] + " " + originalCenter[1] + " " + originalCenter[2]);
        for (int atomIndex = 0; atomIndex < numAtoms; atomIndex++)
        {
            for (int i = 0; i < 3; i++)
            {
                double[] rr = positions[atomIndex];
                rr[i] = scaleFactor[i] * (rr[i] - originalCenter[i]) + newCenter[i];
            }
        }
        //Debug.Log("New Center: " + newCenter[0] + " " + newCenter[1] + " " + newCenter[2]);

    }

    void Readfile(string filePath)
    {
        Debug.Log("inside read" + filePath);
        Regex reg = new Regex(@"\s+");
        int atomIndex = 0;

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = sr.ReadLine().Trim();
            //Debug.Log("Line: " + line + "\n");
            string[] cdnt = reg.Split(line);
            //Debug.Log("FirstLine: " + cdnt+"\n");
            numAtoms = int.Parse(cdnt[0]);
            

            //Get information about the box
            line = sr.ReadLine().Trim();
            cdnt = reg.Split(line);
            /*
            Debug.Log("Second Line: " + line);
            for(int index = 0;index < cdnt.Length; index+=1)
                Debug.Log("Second Line: " + index +"    "+cdnt[index]);
            */
            for (int index = 0; index < 3; index++)
            {
                l[index] = float.Parse(cdnt[index]);
                angle[index] = float.Parse(cdnt[index + 3]);
            }
            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                cdnt = reg.Split(line);
                if (cdnt.Length < 4)
                    continue;
                else
                {
                    double[] rr = new double[3] { float.Parse(cdnt[1]), float.Parse(cdnt[2]), float.Parse(cdnt[3]) };
                    positions.Add(rr);

                    for (int j = 0; j < 3; j++)
                    {
                        if ((double.Parse(cdnt[j + 1]) < minPos[j]) || (atomIndex == 0))
                            minPos[j] = double.Parse(cdnt[j + 1]);
                        if ((double.Parse(cdnt[j + 1]) > maxPos[j]) || (atomIndex == 0))
                            maxPos[j] = double.Parse(cdnt[j + 1]);
                    }
                    atomIndex++;
                    //Debug.Log(atoms[atomIndex].iatom+" , "+atoms[atomIndex].rr0[0] + " , " + atoms[atomIndex].rr0[1] + " , " + atoms[atomIndex].rr0[2]);
                }
            }
        }

    }
    
}
