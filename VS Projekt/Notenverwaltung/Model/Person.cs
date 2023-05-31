using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Person : Entity {
        
        public string firstname { get; set; }
        public string lastname;

        public string birthdate;
        public string birthplace;

        public string username;
        public string password;
        public Person() : base() { }
        public Person(string firstname, string lastname, string birthdate, string birthplace, string username, string password) {
            this.firstname = firstname;
            this.lastname = lastname;

            this.birthdate = birthdate;
            this.birthplace = birthplace;

            this.username = username;
            this.password = password;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList 
                = new AttributeToValuesDescription(TableNames.PersonAttr.personId, this.id);
            attributeList.AddStringAttribute(TableNames.PersonAttr.firstname, firstname);
            attributeList.AddStringAttribute(TableNames.PersonAttr.lastname, lastname);

            attributeList.AddStringAttribute(TableNames.PersonAttr.birthplace, birthplace);
            attributeList.AddStringAttribute(TableNames.PersonAttr.birthdate, birthdate);

            attributeList.AddStringAttribute(TableNames.PersonAttr.username, username);
            attributeList.AddStringAttribute(TableNames.PersonAttr.password, password);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNames.person;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id=attributeToValuesDescription.primaryKeyValue;

            firstname=attributeToValuesDescription.GetValue(TableNames.PersonAttr.firstname);
            lastname=attributeToValuesDescription.GetValue(TableNames.PersonAttr.lastname);

            birthplace=attributeToValuesDescription.GetValue(TableNames.PersonAttr.birthplace);
            birthdate=attributeToValuesDescription.GetValue(TableNames.PersonAttr.birthdate);
            
            username=attributeToValuesDescription.GetValue(TableNames.PersonAttr.username);
            password=attributeToValuesDescription.GetValue(TableNames.PersonAttr.password);
        }
    }
}
