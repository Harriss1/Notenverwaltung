using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Course :Entity {
        public string label { get; set; }
        public DateTime startDate;
        public DateTime endDate;
        public Subject subject;
        private int tempForeignKeySubjectId;

        public Course() : base() { }
        public Course(string label, DateTime startDate, DateTime endDate, Subject subject) {
            this.label = label;
            this.startDate = startDate;
            this.endDate = endDate;
            this.subject = subject;
            this.tempForeignKeySubjectId = subject.id;
        }
        public Course(string label, string startDate, string endDate, Subject subject)  
            : this (label, (new SimpleDate(startDate)).ToDateTime, (new SimpleDate(endDate)).ToDateTime, subject)
        {}

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.CourseAttr.courseId, this.id);
            attributeList.AddStringAttribute(TableNotation.CourseAttr.label, label);
            attributeList.AddDateTimeAttribute(TableNotation.CourseAttr.startDate, startDate);
            attributeList.AddDateTimeAttribute(TableNotation.CourseAttr.endDate, endDate);

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.subject,
                TableNotation.SubjectAttr.subjectId,
                TableNotation.CourseAttr.subjectId,
                this.tempForeignKeySubjectId
                ));

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.course;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;

            label = (string)attributeToValuesDescription.GetValue(TableNotation.CourseAttr.label);
            startDate = (DateTime)attributeToValuesDescription.GetValue(TableNotation.CourseAttr.startDate);
            endDate = (DateTime)attributeToValuesDescription.GetValue(TableNotation.CourseAttr.endDate);

            // Viele-zu-eins Beziehungen
            Subject subjectRelationship = new Subject();
            OneToXRelationKeyValue relation = attributeToValuesDescription.GetOneToXRelation(TableNotation.subject);
            tempForeignKeySubjectId = relation.GetForeignId();
            if (subjectRelationship.FindById(tempForeignKeySubjectId) == null) {
                throw new ArgumentException("Person mit ID=" + tempForeignKeySubjectId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.subject = subjectRelationship;
        }
    }
}
