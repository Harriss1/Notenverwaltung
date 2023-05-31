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

        public BranchOfStudy branchOfStudy;
        public Class() : base() { }
        public Class(string label, DateTime startDate, DateTime endDate, BranchOfStudy branchOfStudy) {
            this.label = label;
            this.startDate = startDate;
            this.endDate = endDate;
            this.branchOfStudy = branchOfStudy;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNames.ClassAttr.classId, this.id);
            attributeList.AddStringAttribute(TableNames.ClassAttr.label, label);
            attributeList.AddDateTimeAttribute(TableNames.ClassAttr.startDate, startDate);
            attributeList.AddDateTimeAttribute(TableNames.ClassAttr.endDate, endDate);
            attributeList.AddRelation(TableNames.ClassAttr.branchOfStudyId, branchOfStudy.id);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNames.class_tablename;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;

            label = (string)attributeToValuesDescription.GetValue(TableNames.ClassAttr.label);
            startDate = (DateTime)attributeToValuesDescription.GetValue(TableNames.ClassAttr.startDate);
            endDate = (DateTime)attributeToValuesDescription.GetValue(TableNames.ClassAttr.endDate);
            BranchOfStudy branchRelationship = new BranchOfStudy();
            int branchId = attributeToValuesDescription.GetRelationId(TableNames.ClassAttr.branchOfStudyId);
            if (branchRelationship.FindById(branchId) == null) {
                throw new ArgumentException("Bildungsgang mit ID=" + branchId + " existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.branchOfStudy = branchRelationship;
        }
    }
}
