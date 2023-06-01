using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Teacher : Entity {
        
        public Person person;
        public List<Course> courses = new List<Course>();
        private List<int> tempCourseForeignIds = new List<int>(); // Many-To-Many Beziehung
        private int tempForeignKeyPersonId = -1;  // Fremdschlüssel auf Person.personId
                                        // Genutzt in SetAttributesInternally zur Suche der Person

        public Teacher() : base() { }
        public Teacher(Person person) {
            this.person = person;
            this.tempForeignKeyPersonId = person.id;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.TeacherAttr.teacherId, this.id);
            
            // Problem das auftrat: Hier darf kein Objekt verlinkt sein, dieses muss lediglich in
            // SetAttributes befüllt werden. :)
            // Ergo muss der Spaltenname des ForeignKey hier auftauchen, und der foreignKey
            // als temporäre Id zwischengespeichert werden, bis die Person gefunden wurde.
            //attributeList.AddRelation(TableNotation.TeacherAttr.personId, this.tempPersonId);

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.person,
                TableNotation.PersonAttr.personId,
                TableNotation.TeacherAttr.personId,
                this.tempForeignKeyPersonId
                ));

            ManyToManyKeyValue lecturer = new ManyToManyKeyValue(
                    TableNotation.lecturer,
                    TableNotation.LecturerAttr.teacherId,
                    TableNotation.LecturerAttr.courseId,
                    tempCourseForeignIds);
            attributeList.AddManyToManyRelation(lecturer);
            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.teacher;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            // Viele-Zu-Eins Beziehungen
            Person personRelationship = new Person();
            OneToXRelationKeyValue relation = attributeToValuesDescription.GetOneToXRelation(TableNotation.person);
            tempForeignKeyPersonId = relation.GetForeignId();
            if (personRelationship.FindById(tempForeignKeyPersonId) == null) {
                throw new ArgumentException("Person mit ID=" + tempForeignKeyPersonId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.person = personRelationship;


            // Klassen setzen Viele-Zu-Viele (Ein kann auch mehrere Klassen haben)
            //courses.Clear();
            //foreach (ManyToManyKeyValue relationDescription
            //    in attributeToValuesDescription.GetAllManyToManyRelations()) {
            //    Course relatedCourse = new Course();
            //    int courseId = relationDescription.GetForeignId();
            //    if (relatedCourse.FindById(relationDescription.GetForeignId()) == null) {
            //        throw new ArgumentException("Kurs mit ID=" + courseId + " existiert nicht, Beziehung kann nicht erstellt werden");
            //    }
            //    else {
            //        courses.Add(relatedCourse);
            //        relatedCourse.Print();
            //    }
            //}
            //int personId = attributeToValuesDescription.GetRelationId(TableNotation.TeacherAttr.personId);
        }
    }
}
