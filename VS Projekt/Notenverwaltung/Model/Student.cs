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

            ManyToManyKeyValue studentHasClassRelation = new ManyToManyKeyValue(
                    TableNotation.studentHasClass,
                    TableNotation.StudentHasClassAttr.studentId,
                    TableNotation.StudentHasClassAttr.classId,
                    tempClassForeignIds);
            attributeList.AddManyToManyRelation(studentHasClassRelation);

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

            //Klassen setzen Viele - Zu - Viele(Ein Schüler kann auch mehrere Klassen haben)
            //classes.Clear();
            //foreach (ManyToManyKeyValue relationDescription
            //    in attributeToValuesDescription.GetAllManyToManyRelations()) {
            //    tempClassForeignIds = relationDescription.GetForeignIds();
            //    foreach (int classId in tempClassForeignIds) {
            //        Class relatedClass = new Class();
            //        if (relatedClass.FindById(classId) == null) {
            //            throw new ArgumentException("Klasse mit ID=" + classId + " existiert nicht, Beziehung kann nicht erstellt werden");
            //        }
            //        else {
            //            classes.Add(relatedClass);
            //            relatedClass.Print();
            //        }
            //    }
            //}

            foreach (ManyToManyKeyValue relationDescription
                in attributeToValuesDescription.GetAllManyToManyRelations()) {
                if (relationDescription.GetTablename().Equals(TableNotation.studentHasClass)) {
                    classes.Clear();
                    tempClassForeignIds = relationDescription.GetForeignIds();
                    foreach (int classId in tempClassForeignIds) {
                        Class relatedClass = new Class();
                        if (relatedClass.FindById(classId) == null) {
                            throw new ArgumentException("Klasse mit ID=" + classId + " existiert nicht, Beziehung kann nicht erstellt werden");
                        }
                        else {
                            classes.Add(relatedClass);
                            relatedClass.Print();
                        }
                    }
                }

                if (relationDescription.GetTablename().Equals(TableNotation.participant)) {
                    courses.Clear();
                    tempCourseForeignIds = relationDescription.GetForeignIds();
                    foreach (int courseId in tempCourseForeignIds) {
                        Course relatedCourse = new Course();
                        if (relatedCourse.FindById(courseId) == null) {
                            throw new ArgumentException("Klasse mit ID=" + courseId + " existiert nicht, Beziehung kann nicht erstellt werden");
                        }
                        else {
                            courses.Add(relatedCourse);
                            relatedCourse.Print();
                        }
                    }
                }
            }

            // Person setzen: deprecated
            //Person personRelationship = new Person();
            //int personId = attributeToValuesDescription.GetRelationId(TableNotation.StudentAttr.personId);
            //if (personRelationship.FindById(personId) == null) {
            //    throw new ArgumentException("Person mit ID=" + personId + " existiert nicht, Beziehung kann nicht erstellt werden");
            //}
            //this.person = personRelationship;


        }

        public void AddToClass(Class icd13) {
            isNewManyToManyRelationAdded = true;
            classes.Add(icd13);
            tempClassForeignIds.Add(icd13.id);
        }
    }
}
