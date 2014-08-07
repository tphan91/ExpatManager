using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using ExpatManager.Models;
using ExpatManager.Helper;

namespace ExpatManager.DAL
{
    public partial class EntitiesExtension
    {
        public string UserName { get; set; }
        List<Audit> auditTrailList = new List<Audit>();

        partial void OnContextCreated()
        {
            ObjectContext.SavingChanges += new EventHandler(Entities_SavingChanges);
        } 

        void Entities_SavingChanges(object sender, EventArgs e)
        {
            IEnumerable<ObjectStateEntry> changes = ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified);
            foreach (ObjectStateEntry stateEntryEntity in changes) 
            { 
                if (!stateEntryEntity.IsRelationship &&
                    stateEntryEntity.Entity != null &&
                    !(stateEntryEntity.Entity is Audit))
                { //is a normal entry, not a relationship 
                    Audit audit = this.AuditTrailFactory(stateEntryEntity, UserName);
                    auditTrailList.Add(audit);
                }
            }

            if (auditTrailList.Count > 0)
            {
                foreach (var audit in auditTrailList)
                {
                    AddToAudit(audit);
                }
            }
        }

        private Audit AuditTrailFactory(ObjectStateEntry entry, string UserName)
        {
            Audit audit = new Audit();
            audit.AuditId = Guid.NewGuid().ToString();
            audit.RevisionStamp = DateTime.Now;
            audit.TableName = entry.EntitySet.Name;
            audit.UserName = UserName;

            if (entry.State == EntityState.Added)
            { //entry is added
                audit.NewData = GetEntryValueInString(entry, false);
                audit.Action = Enums.AuditAction.I.ToString();
            }
            else if (entry.State == EntityState.Deleted)
            {
                audit.OldData = GetEntryValueInString(entry, true);
                audit.Action = Enums.AuditAction.D.ToString();
            }
            else
            {
                audit.OldData = GetEntryValueInString(entry, true); 
                audit.NewData = GetEntryValueInString(entry, false);
                audit.Action = Enums.AuditAction.U.ToString();

                IEnumerable<string> modifiedProperties = entry.GetModifiedProperties();
                //assing collection of mismatched column name as serialized string
                audit.ChangedColumn = XMLSerializationHelper.XmlSerialize(modifiedProperties.ToArray());
            }
            return audit;
        }

        private string GetEntryValueInString(ObjectStateEntry entry, bool isOriginal)
        {
            if (entry.Entity is EntityObject)
            {
                object target = CloneEntity((EntityObject)entry.Entity);
                foreach (string propName in entry.GetModifiedProperties())
                {
                    object setterValue = null;
                    if (isOriginal)
                    {
                        //Get orginal value
                        setterValue = entry.OriginalValues[propName];
                    }
                    else
                    {
                        //Get orginal value
                        setterValue = entry.CurrentValues[propName];
                    }

                    //Find property to update
                    PropertyInfo propInfo = target.GetType().GetProperty(propName);
                    //Update property with original value

                    if (setterValue == DBNull.Value)
                    {
                        setterValue = null;
                    }
                    propInfo.SetValue(target, setterValue, null);
                }

                XmlSerializer formatter = new XmlSerializer(target.GetType());
                XDocument document = new XDocument();

                using (XmlWriter xmlWriter = document.CreateWriter())
                {
                    formatter.Serialize(xmlWriter, target);
                }
                return document.Root.ToString();
            }
            return null;
        }

        public EntityObject CloneEntity(EntityObject obj)
        {
            System.Runtime.Serialization.DataContractSerializer dcSer = new System.Runtime.Serialization.DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
             
            dcSer.WriteObject(memoryStream, obj); 
            memoryStream.Position = 0;
             
            EntityObject newObject = (EntityObject)dcSer.ReadObject(memoryStream);
            return newObject;
        }

        public void AddToAudit(Audit audit)
        {
           
        }
    }
}
