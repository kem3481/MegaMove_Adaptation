using UnityEngine;
using System;
using System.IO;
using System.Text;


public class PositionLogger : MonoBehaviour
{
    public string FolderName = "This PC//C://Desktop//MegaMove";
    public string FileName = "FramesData";
    private string OutputDir;

    //Things you want to write out, set them in the inspector
    public GameObject controller;

    //Gives user control over when to start and stop recording, trigger this with spacebar;
    public bool startWriting;

    //Initialize some containers
    FileStream streams;
    FileStream trialStreams;
    StringBuilder stringBuilder = new StringBuilder();
    String writeString;
    Byte[] writebytes;

    private ControlLevel_Trials controlLevel;
    public GameObject manager;

    void Start()
    {
        controlLevel = manager.GetComponent<ControlLevel_Trials>();

        // create a folder 
        string OutputDir = Path.Combine(FolderName, string.Concat(DateTime.Now.ToString("MM-dd-yyyy"), FileName));
        Directory.CreateDirectory(OutputDir);

        // create a file to record data
        String trialOutput = Path.Combine(OutputDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "_Results.txt");
        trialStreams = new FileStream(trialOutput, FileMode.Create, FileAccess.Write);


        //Call the function below to write the column names
        WriteHeader();
    }


    void WriteHeader()
    {

        //output file-- order of column names here should match the order you use when writing out each value 
        stringBuilder.Length = 0;
        //add header info
        stringBuilder.Append(
        DateTime.Now.ToString() + "\t" +
        "The file contains frame by frame data of location for the controller position " + Environment.NewLine +
        "The coordinate system is in Unity world coordinates." + Environment.NewLine
        );
        stringBuilder.Append("-------------------------------------------------" +
            Environment.NewLine
            );
        //add column names
        stringBuilder.Append(
            "FrameNumber\t" + "StartTime\t" + "HandX\t" + "HandY\t" + "HandZ\t" + Environment.NewLine
                        );


        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);

    }

    void WriteFile()
    {
        controller = controlLevel.gamecontroller;

        stringBuilder.Length = 0;
        stringBuilder.Append(
                    Time.frameCount + "\t"
                    + Time.time * 1000 + "\t"
                    + controller.transform.position.x.ToString() + "\t"
                    + controller.transform.position.y.ToString() + "\t"
                    + controller.transform.position.z.ToString() + "\t" +
                    Environment.NewLine
                );
        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);
    }

    public void Update()
    {
        //Use spacebar to initiate/stop recording values, you can change this if you want 
        if (controller != null)
        {
            WriteFile();
        }


    }

    public void OnApplicationQuit()
    {
        trialStreams.Close();

    }
}