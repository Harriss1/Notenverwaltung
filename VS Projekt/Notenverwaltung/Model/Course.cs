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

        public Course() : base() { }
        public Course(string label, DateTime startDate, DateTime endDate) {
            this.label = label;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.CourseAttr.courseId, this.id);
            attributeList.AddStringAttribute(TableNotation.CourseAttr.label, label);
            attributeList.AddDateTimeAttribute(TableNotation.CourseAttr.startDate, startDate);
            attributeList.AddDateTimeAttribute(TableNotation.CourseAttr.endDate, endDate);

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
        }
    }
}
