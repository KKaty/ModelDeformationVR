using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord;
using System.IO;
using System;
using Accord.Math;
using System.Linq;


public class ReadFile : MonoBehaviour
{
    //public ReadCoord ReadCoord;
    public string modelFileName;
    public string cageFileName;
    public string barCoordFileName;


    void Start() 
    {
        var modelMatrices = ReadMatrixFromFile(modelFileName, false);
        Debug.Log("load matrix 1");

        var cageMatrices = ReadMatrixFromFile(cageFileName, false);
        Debug.Log("load matrix 2");

        var barMatrices = ReadMatrixFromFile(barCoordFileName, true);
        Debug.Log("load matrix 3");

        var produit = Matrix.Dot(barMatrices, cageMatrices);
        for (int i = 0; i < Matrix.Rows(produit); i++)
        {
            for (int j = 0; j < 3; j++)
            { Debug.Log("produit" + produit[i, j]); }
        }
    }


    public StreamReader readFile(string fileName)
    {
        StreamReader r;

        string path = (Application.streamingAssetsPath + "/" + fileName + ".txt");
        FileInfo fInfo = new FileInfo(path);
        if (fInfo.Exists)
        {
            r = new StreamReader(path);
        }
        else
        {
            Debug.Log("File does not exist");
            return null;
        }

        return r;
    }
    public double[,] ReadMatrixFromFile(string fileName, bool isCoordinateFile)
    {
        int rowNumber;
        int columnNumber;
        int startingRowIndex;

        var r = readFile(fileName);
        var fileContent = r.ReadToEnd();
        var fileLine = fileContent.Split('\n');
       
        //We set the number of row and column according to the file we are reading.
        if (isCoordinateFile)
        {
            startingRowIndex = 2;
            var firstFileLine = fileLine.Get(1);
            //Read the number of rows and columns from the second line of the file
            string[] matrixData = firstFileLine.Split(' ');
            rowNumber = Int32.Parse(matrixData.Get(0));
            Debug.Log("This is matrix B's row size:" + rowNumber);
            columnNumber = Int32.Parse(matrixData.Get(1));
            Debug.Log("This is matrix B's column size:" + columnNumber);
            double[,] Barycentric = Matrix.Create(new double[rowNumber, columnNumber]);
            for (int i = 2; i < rowNumber + 2; i++)
            {
                var line = fileLine.Get(i);
                int k = i - 1;
                Debug.Log("To see the " + k + "th " + "row:" + line);
                var eachData = line.Split(' ');
                for (int j = 0; j < columnNumber; j++)
                {
                    Barycentric[i - 2, j] = float.Parse(eachData.Get(j));
                    int l = j + 1;
                    Debug.Log("To see [" + k + "," + l + "] element in Matrix B:" + Barycentric[i - 2, j].ToString("F11"));
                }
            }
            return Barycentric;
        }
        else
        {
            startingRowIndex = 0;
            rowNumber = fileLine.Length - 1;
            columnNumber = 3;
        }


        //Initialize a Matrix (size specification)
        var MatrixMandG = Matrix.Create(new double[rowNumber, columnNumber]);
       
        
        //assign the matrix with the data in file
        for (int i = startingRowIndex; i < rowNumber; i++)
        {
            var line = fileLine.Get(i);
            int k = i + 1;
            Debug.Log("To see the " + k + "th " + "row:" + line);
           
            var eachData = line.Split(' ');
       
            for (int j = 0; j < columnNumber; j++)
            {
                MatrixMandG[i, j] = float.Parse(eachData.Get(j));
                int l = j + 1;
                if (fileName.Contains("cage"))
                { Debug.Log("To see [" + k + "," + l + "] element in Matrix G:" + MatrixMandG[i, j].ToString("F6")); }
                //else if (fileName.Contains("coord"))
                //{ }
                else /*if (fileName.Contains("coord")!=true)*/
                {
                  Debug.Log("To see [" + k + "," + l + "] element in Matrix M:" + MatrixMandG[i, j].ToString("F11"));
                }
            }
        }

        return MatrixMandG;
    }


}
