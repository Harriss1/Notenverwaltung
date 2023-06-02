using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Student : Entity {
        public Person person;
        private int tempForeignKeyPersonId = -1;

        // Many-To-Many Beziehungen
        public List<Class> classes = new List<Class>();
        private List<int> tempClassForeignIds = new List<int>(); 
        public List<Course> courses = new List<Course>();
        private List<int> tempCourseForeignIds = new List<int>();
        public Student() : base() { }
        public Student(Person person) {
            this.person = person;
            this.tempForeignKeyPersonId = person.id;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.StudentAttr.studentId, this.id);

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.person,
                TableNotation.PersonAttr.personId,
                TableNotation.StudentAttr.personId,
                this.tempForeignKeyPersonId
                ));

            // Klassenmitglied (Viele zu Viele Beziehung)
            ManyToManyKeyValue studentHasClassRelation = new ManyToManyKeyValue(
                    TableNotation.studentHasClass,
                    TableNotation.StudentHasClassAttr.studentId,
                    TableNotation.StudentHasClassAttr.classId,
                    tempClassForeignIds);
            attributeList.AddManyToManyRelation(studentHasClassRelation);

            // Kursteilnehmer (Viele-zu-Viele Beziehung)
            ManyToManyKeyValue participantRelation = new ManyToManyKeyValue(
                    TableNotation.participant,
                    TableNotation.ParticipantAttr.studentId,
                    TableNotation.ParticipantAttr.courseId,
                    tempCourseForeignIds);
            attributeList.AddManyToManyRelation(participantRelation);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.student;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            
            // Viele-zu-eins Beziehungen
            Person personRelationship = new Person();
            OneToXRelationKeyValue relation = attributeToValuesDescription.GetOneToXRelation(TableNotation.person);
            tempForeignKeyPersonId = relation.GetForeignId();
            System.Console.WriteLine("Rufe FindById auf, id=" + tempForeignKeyPersonId);
            if (personRelationship.FindById(tempForeignKeyPersonId) == null) {
                throw new ArgumentException("Person mit ID=" + tempForeignKeyPersonId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.person = personRelationship;

            //Klassenmitglied
            //TODO Endlosschleife: Student sucht seine Klasse mittels FindById, und Klasse sucht seine Studenten
            // mittels FindById
            ManyToManyKeyValue studentHasClassRelation =
                attributeToValuesDescription.GetManyToManyRelation(TableNotation.studentHasClass);
            if (studentHasClassRelation != null) {
                classes.Clear();
                tempClassForeignIds = studentHasClassRelation.GetForeignIds();
                foreach (int classId in tempClassForeignIds) {
                    Class relatedClass = new Class();
                    if (attributeToValuesDescription.recursionLevel >= recursionMaxDepth) {
                        //System.Console.WriteLine("Klasse mit ID =" + classId + " ==> Rekursionstiefe erreicht");
                    }
                    else {
                        if (relatedClass.FindById(classId, attributeToValuesDescription.recursionLevel) == null) {
                            throw new ArgumentException("Klasse mit ID=" + classId + " existiert nicht, Beziehung kann nicht erstellt werden");

                        }
                        else {
                            classes.Add(relatedClass);
                            relatedClass.Print();
                        }
                    }
                }
            }

            // Kursteilnehmer
            //ManyToManyKeyValue participantDescription =
            //    attributeToValuesDescription.GetManyToManyRelation(TableNotation.participant);
            //if (participantDescription != null) {
            //    courses.Clear();
            //    tempCourseForeignIds = participantDescription.GetForeignIds();
            //    foreach (int courseId in tempCourseForeignIds) {
            //        Course relatedCourse = new Course();
            //        if (relatedCourse.FindById(courseId) == null) {
            //            throw new ArgumentException("Klasse mit ID=" + courseId + " existiert nicht, Beziehung kann nicht erstellt werden");
            //        }
            //        else {
            //            courses.Add(relatedCourse);
            //            relatedCourse.Print();
            //        }
            //    }
            //}

        }

        public void AddToClass(Class newClass) {
            isNewManyToManyRelationAdded = true;
            classes.Add(newClass);
            tempClassForeignIds.Add(newClass.id);
        }
        public void AddToCourse(Course newCourse) {
            isNewManyToManyRelationAdded = true;
            courses.Add(newCourse);
            tempCourseForeignIds.Add(newCourse.id);
        }
    }
}
