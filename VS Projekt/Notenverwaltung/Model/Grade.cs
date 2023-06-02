using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Grade : Entity {

        public Participant participant;
        private int tempParticipantForeignId = -1;
        public string note;
        public DateTime date;
        int valueInPercent;

        public Grade() : base() { }
        public Grade(Participant participant, string note, DateTime date, int valueInPercent) {
            this.participant = participant;
            tempParticipantForeignId = participant.id;
            this.note = note;
            this.date = date;
            this.valueInPercent = valueInPercent;
        }
        public Grade(Participant participant, string note, string date, int valueInPercent)
           : this(participant, note, (new SimpleDate(date)).ToDateTime, valueInPercent) { }

        public override string ToTableName() {
            return TableNotation.grade;
        }

        public override AttributeToValuesDescription ToAttributeValueDescription() {
            AttributeToValuesDescription attributeList
                = new AttributeToValuesDescription(TableNotation.GradeAttr.gradeId, this.id);

            attributeList.AddStringAttribute(TableNotation.GradeAttr.note, note);
            attributeList.AddIntegerAttribute(TableNotation.GradeAttr.valueInPercent, valueInPercent);
            attributeList.AddDateTimeAttribute(TableNotation.GradeAttr.date, date);

            attributeList.AddOneToXRelation(new OneToXRelationKeyValue(
                TableNotation.participant,
                TableNotation.ParticipantAttr.participantId,
                TableNotation.GradeAttr.participantId,
                this.tempParticipantForeignId
                ));

            return attributeList;
        }
        protected override void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription) {
            this.id = attributeToValuesDescription.primaryKeyValue;

            note = (string)attributeToValuesDescription.GetValue(TableNotation.GradeAttr.note);
            date = (DateTime)attributeToValuesDescription.GetValue(TableNotation.GradeAttr.date);
            valueInPercent = (int)attributeToValuesDescription.GetValue(TableNotation.GradeAttr.valueInPercent);

            // Viele-zu-eins Beziehung
            Participant participantRelationship = new Participant();
            OneToXRelationKeyValue relation = attributeToValuesDescription.GetOneToXRelation(TableNotation.participant);
            tempParticipantForeignId = relation.GetForeignId();
            if (participantRelationship.FindById(tempParticipantForeignId) == null) {
                throw new ArgumentException("Person mit ID=" + tempParticipantForeignId +
                    "existiert nicht, Beziehung kann nicht erstellt werden");
            }
            this.participant = participantRelationship;

        }
    }
}
