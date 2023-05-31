namespace Notenverwaltung {
    internal class BranchOfStudy : Entity {
        public string label { get; set; }
        public BranchOfStudy() : base() { }
        public BranchOfStudy(string label) { 
            this.label = label;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.BranchOfStudyAttr.branchOfStudyId, this.id);
            attributeList.AddStringAttribute(TableNotation.BranchOfStudyAttr.label, label);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.branchOfStudy;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            this.label =  (string)attributeToValuesDescription.GetValue(TableNotation.BranchOfStudyAttr.label);
        }
    }
}