using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Student : Entity {
        public Person person;

        public Student() : base() { }
        public Student(Person person) {
            this.person = person;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNames.StudentAttr.studentId, this.id);
            attributeList.AddRelation(TableNames.StudentAttr.personId, person.id);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNames.student;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            Person personRelationship = new Person();
            int personId = attributeToValuesDescription.GetRelationId(TableNames.StudentAttr.personId);
            if (personRelationship.FindById(personId) == null) {
                throw new ArgumentException("Person mit ID=" + personId + " existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.person = personRelationship;
        }
    }
}
