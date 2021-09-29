using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using System.IO;


public class DialogueObject : MonoBehaviour
{
    [SerializeField] private GameObject tag;
    public TextAsset textAsset;
    public Dialog dialog;
    private Node nodes;

    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] private GameObject button, dialogue;
    private string[] sentense = new string[99];
    private string[] answers = new string[99];
    private int index = 0;
    private bool[] activeButton = new bool[99];
    private bool active;

    private void Start()
    {
        LoadData();
        SentenseControl();
        if (activeButton[index])
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }

        active = !dialogue.active;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            NextSentens();
        }
    }

    private void LoadData()
    {
        dialog = Dialog.Load(textAsset);
        for (int i = 0; i < dialog.nodes.Length; i++)
        {
            Node node = dialog.nodes[i];
            sentense[i] = node.npc;
            
            if (node.answers.Length != 0)
            {
                activeButton[i] = true;
                for (int t = 0; t < node.answers.Length; t++)
                {
                    Answers answers = node.answers[t];
                    this.answers[t] = answers.text;
                }
            }
        }  
    } 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NewPlayer>())
        {
            tag.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NewPlayer>())
        {
            tag.SetActive(false);
        }
    } 
    public void NextSentens()
    {
        if (!active)
        {
            if (index <= dialog.nodes.Length - 2)
            {
                index++;
                Debug.Log(dialog.nodes.Length + " sentens  index = " + index);
            }
            else
            {
                NewPlayer.Instance.state = false;
                dialogue.SetActive(false);
                active = true;
                index = 0;
            }

            if (activeButton[index])
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
            SentenseControl();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogue.SetActive(true);
                active = !dialogue.active;
                NewPlayer.Instance.state = true;
            }
        }
    }

    private void SentenseControl()
    {
        textMesh.text = sentense[index];
    }

     IEnumerator Type()
    {
        foreach (char latter in sentense[index].ToCharArray())
        {
            textMesh.text += latter;
            yield return new WaitForSeconds(0.1f);
        }
       
    }
}

[XmlRoot("dialogue")]
public class Dialog
{
    [XmlElement("text")]
    public string text;
    [XmlElement("node")]
    public Node[] nodes;
    

    public static Dialog Load(TextAsset xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialog));
        StringReader reader = new StringReader(xml.text);
        Dialog dialog = serializer.Deserialize(reader) as Dialog;
        return dialog;
    }
}

[System.Serializable]
public class Node
{
    [XmlElement("npc")]
    public string npc;
    [XmlArray("answers")]
    [XmlArrayItem("answer")]
    public Answers[] answers;
   
   
}

[System.Serializable]
public class Answers
{
    [XmlAttribute("tonode")]
    public int nextNode;
    [XmlElement("txt")]
    public string text;
}


