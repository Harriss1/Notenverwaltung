using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Teacher : Entity {
        
        public Person person;
        public List<Course> courses = new List<Course>();

        public Teacher() : base() { }
        public Teacher(Person person) {
            this.person = person;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.TeacherAttr.teacherId, this.id);
            attributeList.AddRelation(TableNotation.TeacherAttr.personId, person.id);
            ManyToManyKeyValue lecturer = new ManyToManyKeyValue(
                    TableNotation.lecturer,
                    TableNotation.LecturerAttr.teacherId,
                    TableNotation.LecturerAttr.courseId,
                    this.id);
            attributeList.AddManyToManyRelation(lecturer);
            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.teacher;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            Person personRelationship = new Person();
            int personId = attributeToValuesDescription.GetRelationId(TableNotation.TeacherAttr.personId);
            if(personRelationship.FindById(personId) == null) {
                throw new ArgumentException("Person mit ID=" + personId + " " +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.person = personRelationship;

            // Klassen setzen Viele-Zu-Viele (Ein Schüler kann auch mehrere Klassen haben)
            courses.Clear();
            foreach (ManyToManyKeyValue relationDescription
                in attributeToValuesDescription.GetAllManyToManyRelations()) {
                Course relatedCourse = new Course();
                int courseId = relationDescription.GetForeignId();
                if (relatedCourse.FindById(relationDescription.GetForeignId()) == null) {
                    throw new ArgumentException("Kurs mit ID=" + courseId + " existiert nicht, Beziehung kann nicht erstellt werden");
                }
                else {
                    courses.Add(relatedCourse);
                    relatedCourse.Print();
                }
            }
        }
    }
}
