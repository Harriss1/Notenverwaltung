namespace Notenverwaltung {
    internal class BranchOfStudy : Entity {
        public string label { get; set; }
        public BranchOfStudy() : base() { }
        public BranchOfStudy(string label) { 
            this.label = label;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNames.BranchOfStudyAttr.branchOfStudyId, this.id);
            attributeList.AddStringAttribute(TableNames.BranchOfStudyAttr.label, label);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNames.branchOfStudy;
        }

        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;
            this.label =  (string)attributeToValuesDescription.GetValue(TableNames.BranchOfStudyAttr.label);
        }
    }
}