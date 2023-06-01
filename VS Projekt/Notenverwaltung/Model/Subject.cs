using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Subject : Entity {
        public string label;
        public string acronym;
        public Subject() : base() { }
        public Subject(string label, string acronym) {
            this.label = label;
            this.acronym = acronym;
        }
        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.SubjectAttr.subjectId, this.id);
            attributeList.AddStringAttribute(TableNotation.SubjectAttr.label, label);
            attributeList.AddStringAttribute(TableNotation.SubjectAttr.acronym, acronym);

            return attributeList;
        }

        public override string ToTableName() {
            return TableNotation.subject;
        }
        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;

            label = (string)attributeToValuesDescription.GetValue(TableNotation.SubjectAttr.label);
            acronym = (string)attributeToValuesDescription.GetValue(TableNotation.SubjectAttr.acronym);

        }
    }
}
