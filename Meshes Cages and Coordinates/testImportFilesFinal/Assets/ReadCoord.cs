using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord;
using System.IO;
using System;
using Accord.Math;


public class ReadCoord : MonoBehaviour
{
    //public ReadFile ReadFile;
    public string fileName;
    int rowNumber = new int();
    int columnNumber = new int();
    StreamReader r;
    //public struct Matrix { };
    public double[,] TestMatrix = Matrix.Create(new double[10, 10]);

    void Start()
    {
        string path = (Application.streamingAssetsPath + "/" + fileName + ".txt");
        FileInfo fInfo = new FileInfo(path);
        if (fInfo.Exists)
        {
            r = new StreamReader(path);
        }
        else
        {
            Debug.Log("File does not exist");       
        }
        var fileContent = r.ReadToEnd();
        var fileLine = fileContent.Split('\n');
        var matrixDataLine = fileLine.Get(1);
        //Read the number of rows and columns from the second line of the file
        string[] matrixData = matrixDataLine.Split(' ');
        rowNumber = Int32.Parse(matrixData.Get(0));
        Debug.Log("This is matrix B's row size:" + rowNumber);
        columnNumber = Int32.Parse(matrixData.Get(1));
        Debug.Log("This is matrix B's column size:" + columnNumber);
        //Initialize the Barycentric Matrix(size specification)
        double[,] Barycentric = Matrix.Create(new double[rowNumber, columnNumber]);
        //assign the matrix with the data in ".coord" file
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
        Barycentric.Multiply(2);
        TestMatrix.Multiply(2);
        //double[] a = ReadFile.MatrixMandG.Multiply(2);


    }
}
