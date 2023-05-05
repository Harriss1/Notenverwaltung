using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    // ":" bedeutet abstracts oder extends BaseClass
    // "," bedeutet implements Interface
    internal class PersonDeprecated : EntityMapperDeprecated {
        
        public string ToTableName() {
            return "Person";
        }

        public List<KeyValue> ToKeyValue() {
            throw new NotImplementedException();
        }

        // person1.foreName.lastName
        // person1.makeTeacher()
        // .setAttributeMap(must Map all Attributes of a Person:Model)
        // .getAttributeMap
        // --> nutzt das Einlesen aller Attribute einer Klasse
        // --> leichter ist eine Klasse welche eine createTable erstellt, und mittels createCol alles setzt.
        // --> eine Klasse die nur setAttributeMap nutzt ist eventuell besser, da wir Create nicht
        // umsetzen können, im Create müssten dann ja auch Beziehungen modelliert sein und automatisiert
        // in die Objekte übertragen werden.
        // Teacher.describeTable{addColumn(this.forename.attributeRef, key, type) : Column.addRelation() : Column.addAutoIncr().addPrimary()}{setTableName}
        // Teacher.Teacher(Person)
        // descriptionList{Column1, Column2}
        // Teacher.save{toKeyValueList{foreach.descriptionList-> {super.foreName}} -> update
        // Teacher:Wrapper.find(Wrapper.lastname) : Teacher {descriptionList.getByAttribute.Lastname -> dbFind}
        // Teacher.findOlderThanAndYoungerThan
        // Teacher.Wrapper.findGreaterThan(key, value)
        // Teacher.Teacher(foreName, lastName)
        // Db.save(Teacher.toKeyValue)
        // Teacher.save() === Wrapper ist abstract class mit privaten Methoden und protected für addColumn()
        // Wrapper bekommt ein DatenbankVerbindungsobjekt injeziert, welches folgende Methoden hat:
        // Verbindungsobjekt zur Datenbank...Wrapper.Init(Db)? > Vererbung wäre falsch.
        // Wrapper(){retrieveDatabase}
        // Wrapper.save(){getDatabase()...Database.save(this)}

        // Database.save(Teacher)


    }
}
