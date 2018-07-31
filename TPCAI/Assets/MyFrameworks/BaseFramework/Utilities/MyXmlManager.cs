using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BaseFramework.Testing
{
    public class MyXmlManager : MonoBehaviour
    {
        public static MyXmlManager thisInstance;

        private void Awake()
        {
            thisInstance = this;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    Debug.Log("Saving Items");
            //    SaveItems();
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    Debug.Log("Loading Items");
            //    LoadItems();
            //}
        }

        //List of Items
        public ItemDatabase itemDB;

        //Save Function
        public void SaveItems()
        {
            //Open a new XML File
            var _serializer = new XmlSerializer(typeof(ItemDatabase));
            //dataPath = editor save
            //persistentDataPath = game save
            using (FileStream _stream = new FileStream(Application.dataPath +
                "/StreamingAssets/XML/item_data.xml", FileMode.Create))
            {
                _serializer.Serialize(_stream, itemDB);
            }

        }

        //Load Function
        public void LoadItems()
        {
            //Open a new XML File
            var _serializer = new XmlSerializer(typeof(ItemDatabase));
            //dataPath = editor save
            //persistentDataPath = game save
            string _fpath = Application.dataPath + "/StreamingAssets/XML/item_data.xml";
            if (File.Exists(_fpath))
            {
                using (FileStream _stream = new FileStream(_fpath, FileMode.Open))
                {
                    itemDB = _serializer.Deserialize(_stream) as ItemDatabase;
                }
            }
        }
    }
}

namespace BaseFramework.Testing
{
    /// <summary>
    /// Temp Class For Tutorial
    /// </summary>
    [System.Serializable]
    public class ItemEntry
    {
        public string itemName;
        public EMyTestMaterial MyMaterial;
        public int itemValue;
    }
    
    [System.Serializable]
    public class ItemDatabase
    {
        [XmlArray("CombatEquipment")]
        public List<ItemEntry> ItemEntryList = new List<ItemEntry>();
    }

    public enum EMyTestMaterial
    {
        Wood,
        Copper,
        Iron,
        Steel,
        Obsidian
    }
}
