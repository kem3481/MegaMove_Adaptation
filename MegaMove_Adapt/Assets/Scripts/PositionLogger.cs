using UnityEngine;
using System;
using System.IO;
using System.Text;
using PupilLabs;

public class PositionLogger : MonoBehaviour
{
    public string FolderName = "C:\\Users\\kem3481\\MEGAMOVE";
    public string FileName = "Positions";
    private string OutputDir;

    //Things you want to write out, set them in the inspector
    public GameObject controller;
    ////public GameObject gaze;

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

    GazeListener gazeListener = null;

    Vector3 pl_gazedirection_wrtHead_xyz;
    Vector3 pl_E0_Norm_wrtHead_xyz;
    Vector3 pl_E1_Norm_wrtHead_xyz;

    float plConfidence;
    string plTimeStamp;
    float pl_gazedistance;

    int mode;

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
        "The file contains frame by frame data of location for 2 tracked objects " + Environment.NewLine +
        "The coordinate system is in Unity world coordinates." + Environment.NewLine
        );
        stringBuilder.Append("-------------------------------------------------" +
            Environment.NewLine
            );
        //add column names
        stringBuilder.Append(
            "FrameNumber\t" + 
            "StartTime\t" + 
            "pl_Confidence\t" + 
            "pl_gazedirection_wrtHead_X\t" +
            "pl_gazedirection_wrtHead_Y\t" +
            "pl_gazedirection_wrtHead_Z\t" +
            "pl_gazedistance_wrtHead\t" +
            "HandX\t" + 
            "HandY\t" + 
            "HandZ\t" + 
            Environment.NewLine);


        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);

    }

    void ReceiveGaze(GazeData gazeData)
    {

        if (gazeData.MappingContext != GazeData.GazeMappingContext.Binocular)
        {
            plConfidence = float.NaN;
            plTimeStamp = "NaN";
            pl_gazedirection_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);

            pl_E0_Norm_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);
            pl_E1_Norm_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);

            pl_gazedistance = float.NaN;

            mode = 0;
        }
        if (gazeData.MappingContext == GazeData.GazeMappingContext.Binocular)
        {
            plConfidence = gazeData.Confidence;
            plTimeStamp = DateTime.UtcNow.ToString("HH:mm:ss"); ;
            pl_gazedirection_wrtHead_xyz = gazeData.GazeDirection;

            pl_E0_Norm_wrtHead_xyz = gazeData.GazeNormal0;
            pl_E1_Norm_wrtHead_xyz = gazeData.GazeNormal1;

            pl_gazedistance = gazeData.GazeDistance;

            mode = 1;
        }
    }

    void WriteFile()
    {
        controller = controlLevel.gamecontroller;

        stringBuilder.Length = 0;
        stringBuilder.Append(
                    Time.frameCount + "\t"
                    + Time.time * 1000 + "\t"
                    + plConfidence.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.x.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.y.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.z.ToString() + "\t"
                    + pl_gazedistance.ToString() + "\t" 
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