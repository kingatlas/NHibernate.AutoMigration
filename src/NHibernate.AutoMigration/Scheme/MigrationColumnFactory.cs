using NHibernate.Mapping;
using System;

namespace NHibernate.AutoMigration
{
    public class MigrationColumnFactory
    {
        public MigrationTable OwnerTable { get; protected set; }
        public MigrationColumnFactory(MigrationTable table)
        {
            OwnerTable = table;
        }

        public MigrationColumn Create(Column column)
        {

            var result = SelectMigrationColumn(column);
            result.OwnerTable = OwnerTable;

            return result;
        }

        private MigrationColumn SelectMigrationColumn(Column column)
        {
            if (column.Value.Type.ReturnedClass == typeof(Guid))
                return new GuidMigrationColumn(column);

            if (column.Value.Type.ReturnedClass == typeof(decimal))
                return new DecimalMigrationColumn(column);

            if (column.Value.Type.ReturnedClass == typeof(string))
                return new StringMigrationColumn(column);

            if (column.Value.Type.ReturnedClass == typeof(DateTime))
                return new DateTimeMigrationColumn(column);

            if (column.Value.Type.ReturnedClass == typeof(bool))
                return new BoolMigrationColumn(column);

            if (column.Value.Type.ReturnedClass == typeof(short))
                return new ShortMigrationColumn(column);

            //TODO Blob

            // if (column.Value.Type.ReturnedClass.IsEnum || column.Value.Type.ReturnedClass == typeof(int))
            return new IntegerMigrationColumn(column);
        }

        public ForeignKeyColumn Create(ForeignKey foreignKey)
        {
            var fk= new ForeignKeyColumn(foreignKey);
            fk.OwnerTable = OwnerTable;
            return fk;
        }
    }
}
