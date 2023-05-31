using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Student : Entity {
        public Person person;
        public List<Class> classes = new List<Class>();
        public Student() : base() { }
        public Student(Person person) {
            this.person = person;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.StudentAttr.studentId, this.id);
            attributeList.AddRelation(TableNotation.StudentAttr.personId, person.id);
            ManyToManyKeyValue studentHasClassRelation = new ManyToManyKeyValue(
                    TableNotation.studentHasClass, 
                    TableNotation.StudentHasClassAttr.studentId,
                    TableNotation.StudentHasClassAttr.classId,
                    this.id);
            attributeList.AddManyToManyRelation(studentHasClassRelation);
            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.student;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            // Person setzen
            Person personRelationship = new Person();
            int personId = attributeToValuesDescription.GetRelationId(TableNotation.StudentAttr.personId);
            if (personRelationship.FindById(personId) == null) {
                throw new ArgumentException("Person mit ID=" + personId + " existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.person = personRelationship;

            // Klassen setzen Viele-Zu-Viele (Ein Schüler kann auch mehrere Klassen haben)
            classes.Clear();
            foreach(ManyToManyKeyValue relationDescription 
                in attributeToValuesDescription.GetAllManyToManyRelations()) {
                Class relatedClass = new Class();
                int classId = relationDescription.GetForeignId();
                if (relatedClass.FindById(relationDescription.GetForeignId()) == null) {
                    throw new ArgumentException("Klasse mit ID=" + classId + " existiert nicht, Beziehung kann nicht erstellt werden");
                } else {
                    classes.Add(relatedClass);
                    relatedClass.Print();
                }
            }
        }
    }
}
