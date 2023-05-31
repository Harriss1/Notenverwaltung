using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Person : Entity {
        
        public string firstname { get; set; }
        public string lastname;

        public DateTime birthdate;
        public string birthplace;

        public string username;
        public string password;
        public Person() : base() { }
        public Person(string firstname, string lastname, DateTime birthdate, string birthplace, string username, string password) {
            this.firstname = firstname;
            this.lastname = lastname;

            this.birthdate = birthdate;
            this.birthplace = birthplace;

            this.username = username;
            this.password = password;
        }
        public Person(string firstname, string lastname, string birthdate, string birthplace, string username, string password) {
            this.firstname = firstname;
            this.lastname = lastname;

            this.birthdate = (new SimpleDate(birthdate)).ToDateTime;
            this.birthplace = birthplace;

            this.username = username;
            this.password = password;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList 
                = new AttributeToValuesDescription(TableNotation.PersonAttr.personId, this.id);
            attributeList.AddStringAttribute(TableNotation.PersonAttr.firstname, firstname);
            attributeList.AddStringAttribute(TableNotation.PersonAttr.lastname, lastname);

            attributeList.AddStringAttribute(TableNotation.PersonAttr.birthplace, birthplace);
            attributeList.AddDateTimeAttribute(TableNotation.PersonAttr.birthdate, birthdate);

            attributeList.AddStringAttribute(TableNotation.PersonAttr.username, username);
            attributeList.AddStringAttribute(TableNotation.PersonAttr.password, password);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.person;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id=attributeToValuesDescription.primaryKeyValue;

            firstname=(string)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.firstname);
            lastname= (string)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.lastname);

            birthplace= (string)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.birthplace);
            birthdate= (DateTime)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.birthdate);
            
            username= (string)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.username);
            password= (string)attributeToValuesDescription.GetValue(TableNotation.PersonAttr.password);
        }
    }
}
