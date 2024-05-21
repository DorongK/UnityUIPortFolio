using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;

/// <summary>
/// FileŬ������ Stream ��ü�� ����� �а� ����
/// ������ �����ʹ� FileŬ������, ũ�� ������ �����Ͷ�� Stream��ü�� ����ϴ°��� �̵��̴�.
/// </summary>
public class DataManager : MonoBehaviour,IManager
{
    private string _state;
    private string _dataPath;
    private string _textFile;
    private string _streamingTextFile;
    private string _xmlLevelProgress;

    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);
        _textFile = _dataPath + "Save_Data.txt";
        _streamingTextFile = _dataPath + "Streaming_Save_Date.txt";
        _xmlLevelProgress = _dataPath + "Progress_Data.xml";
    }
    //xml ������ ����
    public void WriteToXML(string filename)
    {
        if(!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("level.progress");
            for(int i=1; i<5;i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }

    //Stream ��ü�� �а� ����
    public void WriteToStream(string filename)
    {
        if(!File.Exists(filename))
        {
            StreamWriter newStream = File.CreateText(filename);
            newStream.WriteLine("<Save Data> for Tutorial \n");
            newStream.Close();//��Ʈ���� ����� ���� ������ ���α׷����� �� ���Ҹ��� ��� ����Ѵ�.
            Debug.Log("New file created with StreamWriter!");
        }
        StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.WriteLine("Game ended: " + DateTime.Now);
        streamWriter.Close();
        Debug.Log("file updated with StreamWriter!");
        //����Ʈ���� using������ ���εȴ� �Ʒ���������ϸ� close�� ��������� �����ʾƵ� �ݾ��ִ� ������up
        using (StreamWriter newStream = File.CreateText(filename))
        {
            //��羲������ �߰�ȣ�ȿ�
            newStream.WriteLine("<Save Data> for Tutorial \n");

        }
    }

    public void ReadFromStream(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("file doesn't exist...");
            return;
        }
        StreamReader streamReader = new StreamReader(filename);
        Debug.Log(streamReader.ReadToEnd());
    }


    //FileŬ���� �а� ����
    public void NewDirectory()
    {
        if(Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists!!");
            return;
        }
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New Directory Created!");
    }

    public void DeleteDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist..");
            return;
        }
        Directory.Delete(_dataPath, true);
        Debug.Log("Successfully deleted!");
    }

    public void NewTextFile()
    {
        if(File.Exists(_textFile))
        {
            Debug.Log("File already exists..");
            return;
        }
        File.WriteAllText(_textFile, "<SAVE DATA>\n");
        Debug.Log("New file created!");
    }

    public void UpdateTextFile()
    {
        if(!File.Exists(_textFile))
        {
            Debug.Log("File doesn't exists..");
            return;
        }
        File.AppendAllText(_textFile, $"Game started: {DateTime.Now}\n");
        Debug.Log("File Updated Successfully!");
    }

    public void ReadFromFile(string filename)
    {
        if(!File.Exists(filename))
        {
            Debug.Log("File doesn't exists..");
            return;
        }
        Debug.Log(File.ReadAllText(filename));
    }

    public void DeleteFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exists..");
            return;
        }
        File.Delete(filename);
        Debug.Log("File SuccessFully Deleted!");
    }



    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    void Start()
    {
       Initialize();
    }

    public void Initialize()
    {
        _state = "Date Manager Initialized";
        Debug.Log(_state);
        FileSystemInfo();
        NewDirectory();
        //NewTextFile();
        //UpdateTextFile();
        //ReadFromFile(_textFile);
        //WriteToStream(_streamingTextFile);
        WriteToXML(_xmlLevelProgress);
        ReadFromStream(_xmlLevelProgress);
    }

    public void FileSystemInfo()
    {
        Debug.LogFormat("Path separator character:{0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character:{0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directiory :{0}", Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path :{0}", Path.GetTempPath());
    }

}
