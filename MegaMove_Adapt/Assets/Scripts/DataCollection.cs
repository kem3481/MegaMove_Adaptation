using UnityEngine;
using System;
using System.IO;
using System.Text;


/// <summary>
/// This script records data each frame in a text file in the following tab-delimited format
/// Frame   Start		HeadX	HeadY	HeadZ	HandX   HandY   HandZ				
///------------------------------------------------------------------------------
/// 1       1241.806	float	float	float   float	float	float	
/// 2       4619.335	float	float	float   float	float	float	
/// </remark>
public class DataCollection : MonoBehaviour
{
    public string FolderName = "C://Users//performlab-admin//Desktop//MegaMove_Adapt";
    public string FileName = "EndPointsData";
    private string OutputDir;

    //Things you want to write out, set them in the inspector
    [System.NonSerialized]
    public int trialNumber;
    [System.NonSerialized]
    public Vector3 triggerPosition, targetPosition;
    [System.NonSerialized]
    public string startTime, endTime;
    [System.NonSerialized]
    public GameObject testObject;
    [System.NonSerialized]
    public float angle, radius;
    [System.NonSerialized]
    public ControlLevel_Trials controlLevel;
    [System.NonSerialized]
    public TriggerPull triggerPull;

    public GameObject manager;
    
   
    //Gives user control over when to start and stop recording, trigger this with spacebar;
    public bool startWriting;

    //Initialize some containers
    FileStream streams;
    FileStream trialStreams;
    StringBuilder stringBuilder = new StringBuilder();
    String writeString;
    Byte[] writebytes;


    void Start()
    {
        controlLevel = manager.GetComponent<ControlLevel_Trials>();
        triggerPull = controlLevel.test.GetComponent<TriggerPull>();

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
        "The file contains end point data for each trial" + Environment.NewLine +
        "The coordinate system is in Unity world coordinates." + Environment.NewLine
        );
        stringBuilder.Append("-------------------------------------------------" +
            Environment.NewLine
            );
        //add column names
        stringBuilder.Append(
            "Trial Number\t" + "Target Position\t\t" + "Trigger Position\t" + "Start Time\t" + "Overlap Type\t" + "Presentation Radius\t" + "Presentation Angle\t" + Environment.NewLine
                        );


        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);

    }

    void WriteFile()
    {
        trialNumber = controlLevel.trials;
        triggerPosition = new Vector3(controlLevel.trigger_x, controlLevel.trigger_y, controlLevel.trigger_z);
        targetPosition = new Vector3(controlLevel.target_x, controlLevel.target_y, controlLevel.target_z);
        startTime = controlLevel.startTime;
        endTime = controlLevel.endTime;
        testObject = controlLevel.testobject;
        radius = controlLevel.radius;
        angle = controlLevel.angle;

        stringBuilder.Length = 0;
        stringBuilder.Append(
                    trialNumber.ToString() + "\t\t"
                    + targetPosition.ToString() + "\t" 
                    + triggerPosition.ToString() + "\t"
                    + startTime.ToString() + "\t"
                    + testObject.ToString() + "\t"
                    + radius.ToString() + "\t"
                    + angle.ToString() + "\t" +
                    Environment.NewLine
                );
        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);
    }

    public void Update()
    {
        if (triggerPull.trigger == true)
        {
            WriteFile();
        }

    }

    public void OnApplicationQuit()
    {
        trialStreams.Close();
    }
}