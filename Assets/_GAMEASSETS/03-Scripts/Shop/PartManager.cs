using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleSubmarine2
{
    public class PartManager : MonoBehaviour
    {
        [SerializeField] private List<SubmarinePart> bodyParts;
        [SerializeField] private List<SubmarinePart> periscopeParts;

        private void Start()
        {
            if (GameObject.FindGameObjectWithTag(GameTags.PARTMANAGER) == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
                Debug.Log("PartManager loaded");
            }
            else
            {
                Destroy(this.gameObject);
                Debug.Log("PartManager destroyed");
            }
        }

        public SubmarinePart GetBodyByID(int id)
        {
            return bodyParts.Find(item => item.ID == id);
        }
        
        public SubmarinePart GetPeriscopeByID(int id)
        {
            return periscopeParts.Find(item => item.ID == id);
        }

        public List<SubmarinePart> GetPartsList(SubmarinePartType type)
        {
            switch (type)
            {
                default:
                    return bodyParts;
                case SubmarinePartType.PERISCOPE:
                    return periscopeParts;
            }
        }
    }
}