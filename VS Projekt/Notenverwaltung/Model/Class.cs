using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Class : Entity {
        public string label { get; set; }
        public DateTime startDate;
        public DateTime endDate;
        public List<Student> enrolledStudents = new List<Student>();
        private List<int> tempForeignStudentIds = new List<int>();

        public BranchOfStudy branchOfStudy;
        private int tempForeignBranchOfStudyId;
        public Class() : base() { }
        public Class(string label, DateTime startDate, DateTime endDate, BranchOfStudy branchOfStudy) {
            this.label = label;
            this.startDate = startDate;
            this.endDate = endDate;
            this.branchOfStudy = branchOfStudy;
            this.tempForeignBranchOfStudyId = branchOfStudy.id;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.ClassAttr.classId, this.id);

            attributeList.AddStringAttribute(TableNotation.ClassAttr.label, label);
            attributeList.AddDateTimeAttribute(TableNotation.ClassAttr.startDate, startDate);
            attributeList.AddDateTimeAttribute(TableNotation.ClassAttr.endDate, endDate);
            
            OneToXRelationKeyValue branchOfStudyRelation = new OneToXRelationKeyValue(
                TableNotation.branchOfStudy,
                TableNotation.BranchOfStudyAttr.branchOfStudyId,
                TableNotation.ClassAttr.branchOfStudyId,
                tempForeignBranchOfStudyId
                );
            attributeList.AddOneToXRelation(branchOfStudyRelation);

            ManyToManyKeyValue studentHasClassRelation = new ManyToManyKeyValue(
                    TableNotation.studentHasClass,
                    TableNotation.StudentHasClassAttr.classId,
                    TableNotation.StudentHasClassAttr.studentId,
                    this.tempForeignStudentIds);
            attributeList.AddManyToManyRelation(studentHasClassRelation);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.class_tablename;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;

            label = (string)attributeToValuesDescription.GetValue(TableNotation.ClassAttr.label);
            startDate = (DateTime)attributeToValuesDescription.GetValue(TableNotation.ClassAttr.startDate);
            endDate = (DateTime)attributeToValuesDescription.GetValue(TableNotation.ClassAttr.endDate);

            // Viele-1-Beziehung (Ein Bildungsgang hat mehrere Klassen, eine Klasse hat einen Bildungsgang)
            BranchOfStudy branchRelationship = new BranchOfStudy();
            OneToXRelationKeyValue relation = attributeToValuesDescription.GetOneToXRelation(TableNotation.branchOfStudy);
            tempForeignBranchOfStudyId = relation.GetForeignId();
            System.Console.WriteLine("Rufe FindById auf, id=" + tempForeignBranchOfStudyId);
            if (branchRelationship.FindById(tempForeignBranchOfStudyId) == null) {
                throw new ArgumentException("Bildungsgang mit ID=" + tempForeignBranchOfStudyId + " existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.branchOfStudy = branchRelationship;

            // Klassen setzen Viele-Zu-Viele (Ein Schüler kann auch mehrere Klassen haben)
            //enrolledStudents.Clear();
            //foreach (ManyToManyKeyValue relationDescription
            //    in attributeToValuesDescription.GetAllManyToManyRelations()) {
            //    Student student = new Student();
            //    int studentId = relationDescription.GetForeignId();
            //    if (student.FindById(relationDescription.GetForeignId()) == null) {
            //        throw new ArgumentException("Schüler mit ID=" + studentId + " existiert nicht, " +
            //            "Beziehung kann nicht erstellt werden");
            //    }
            //    else {
            //        enrolledStudents.Add(student);
            //        student.Print();
            //    }
            //}
        }
    }
}
