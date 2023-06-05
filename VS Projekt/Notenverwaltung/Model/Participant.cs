using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Participant : Entity {

        public Student student;
        private int tempForeignStudentId = -1;
        public Course course;
        private int tempForeignCourseId = -1;
        // Alle Noten eines Teilnehmers abfragen fehlt. Dh. Grades-Relation müsste man hier nachpflegen.

        public Participant() : base() { }
        public Participant(Student student, Course course) {
            this.student = student;
            this.tempForeignStudentId = student.id;
            this.course = course;
            this.tempForeignCourseId = course.id;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.ParticipantAttr.participantId, this.id);

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.student,
                TableNotation.StudentAttr.studentId,
                TableNotation.ParticipantAttr.studentId,
                this.tempForeignStudentId
                ));

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.course,
                TableNotation.CourseAttr.courseId,
                TableNotation.ParticipantAttr.courseId,
                this.tempForeignCourseId
                ));

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.participant;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            // Viele-Zu-Eins Beziehungen
            Student student = new Student();
            OneToXRelationKeyValue studentRelation = attributeToValuesDescription.GetOneToXRelation(TableNotation.student);
            tempForeignStudentId = studentRelation.GetForeignId();
            if (student.FindById(tempForeignStudentId) == null) {
                throw new ArgumentException("Student mit ID=" + tempForeignStudentId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.student = student;

            Course course = new Course();
            OneToXRelationKeyValue courseRelation = attributeToValuesDescription.GetOneToXRelation(TableNotation.course);
            tempForeignCourseId = courseRelation.GetForeignId();
            if (course.FindById(tempForeignCourseId) == null) {
                throw new ArgumentException("Kurs mit ID=" + tempForeignCourseId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.course = course;
        }
    }
}
