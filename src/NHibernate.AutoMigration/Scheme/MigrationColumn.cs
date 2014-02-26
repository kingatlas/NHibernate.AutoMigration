using NHibernate.Mapping;
using System.Linq;


namespace NHibernate.AutoMigration
{
    public enum MigrationColumnType
    {
        Integer,
        Short,
        Guid,
        Decimal,
        String,
        Blob,
        Bool,
        DateTime,
    }

    public abstract class MigrationColumn
    {
        public virtual string Name { get; protected set; }
        public virtual bool IsNullable { get; protected set; }
        public virtual bool IsPrimaryKey { get; protected set; }
        public virtual MigrationColumnType Type { get; protected set; }
        public MigrationTable OwnerTable { get; set; }

        public MigrationColumn()
        {
            
        }

        public MigrationColumn(Column column, MigrationColumnType type)
        {
            Name = column.Name;
            IsNullable = column.IsNullable;
            Type = type;
            IsPrimaryKey = false;
        }

        public MigrationColumn(string columnName, MigrationColumnType type, bool isPrimaryKey, bool isNullable)
        {
            Name = columnName;
            Type = type;
            IsNullable = isNullable;
            IsPrimaryKey = isPrimaryKey;
        }

        public void SetAsPrimaryKey()
        {
            IsPrimaryKey = true;
        }

        public abstract string TypeAction();

        public virtual string AttributesAction()
        {
            var result = "";

            if (IsNullable)
                result += ".Nullable()";
            else
                result += ".NotNullable()";

            if (IsPrimaryKey)
                result += ".PrimaryKey()";

            return result;
        }

        public virtual string ScriptActions()
        {
            return  TypeAction() + AttributesAction();
        }

        public virtual bool IsSimilarTo(MigrationColumn column)
        {
            if (column.GetType() != this.GetType())
                return false;

            if (column.Name != this.Name)
                return false;

            if (column.Type != this.Type)
                return false;

            if (column.IsNullable != this.IsNullable)
                return false;

            if (column.IsPrimaryKey != this.IsPrimaryKey)
                return false;

            return true;
        }

    }

    public class IntegerMigrationColumn : MigrationColumn
    {
        public IntegerMigrationColumn()
        {
            
        }

        public IntegerMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Integer, isPrimaryKey, isNullable)
        {
        }

        public IntegerMigrationColumn(Column column)
            :base(column, MigrationColumnType.Integer)
        {
            
        }

        public override string TypeAction()
        {
            return ".AsInt32()";
        }
    }

    public class ForeignKeyColumn : IntegerMigrationColumn
    {
        public MigrationTable ReferencedTable { get; set; }

        public string ReferencedTableName { get; set; }

        public ForeignKeyColumn()
        {
            
        }

        public ForeignKeyColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, isPrimaryKey, isNullable)
        {
        }


        public ForeignKeyColumn(ForeignKey foreignKey)
            : base(foreignKey.Columns.First())
        {
            ReferencedTableName = foreignKey.ReferencedTable.Name;
        }

        public string ForeignKeyActions()
        {

            return string.Format(".ForeignKey().FromTable(\"{0}\").ForeignColumn(\"{1}\").ToTable(\"{2}\").PrimaryColumn(\"{3}\")", OwnerTable.Name, Name, ReferencedTable.Name, ReferencedTable.PrimaryKeyColumn.Name);

        }

        public override bool IsSimilarTo(MigrationColumn column)
        {
            var result = base.IsSimilarTo(column);
            
            if (!result)
                return false;

            var fk = column as ForeignKeyColumn;
            
            if (fk == null)
                return false;

            if (fk.ReferencedTableName != this.ReferencedTableName)
                return false;

            return true;
        }
    }

    public class ShortMigrationColumn : MigrationColumn
    {
        public ShortMigrationColumn()
        {
            
        }

        public ShortMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Short, isPrimaryKey, isNullable)
        {
        }

        public ShortMigrationColumn(Column column)
            : base(column, MigrationColumnType.Short)
        {

        }

        public override string TypeAction()
        {
            return ".AsInt16()";
        }
    }

    public class GuidMigrationColumn : MigrationColumn
    {
        public GuidMigrationColumn()
        {
            
        }

        public GuidMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Guid, isPrimaryKey, isNullable)
        {
        }

        public GuidMigrationColumn(Column column)
            : base(column, MigrationColumnType.Guid)
        {

        }

        public override string TypeAction()
        {
            return ".AsGuid()";
        }
    }

    public class DecimalMigrationColumn : MigrationColumn
    {
        public virtual int Precision { get; protected set; }
        public virtual int Scale { get; protected set; }

        public DecimalMigrationColumn()
        {
            
        }

        public DecimalMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Decimal, isPrimaryKey, isNullable)
        {
        }

        public DecimalMigrationColumn(Column column)
            : base(column, MigrationColumnType.Decimal)
        {
            Precision = column.Precision;
            Scale = column.Scale;
        }

        public override string TypeAction()
        {
            return ".AsDecimal("+Precision+","+Scale+")";
        }

        public override bool IsSimilarTo(MigrationColumn column)
        {
            var result = base.IsSimilarTo(column);

            if (!result)
                return false;

            var dc = column as DecimalMigrationColumn;

            if (dc == null)
                return false;

            if (dc.Precision != this.Precision)
                return false;

            if (dc.Scale != this.Scale)
                return false;

            return true;
        }
    }

    public class StringMigrationColumn : MigrationColumn
    {
        public virtual int Length { get; protected set; }

        public StringMigrationColumn()
        {
            
        }

        public StringMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.String, isPrimaryKey, isNullable)
        {
        }


        public StringMigrationColumn(Column column)
            :base(column, MigrationColumnType.String)
        {
            Length = column.Length;
        }


        public override string TypeAction()
        {
            return ".AsString("+Length+")";
        }

        public override bool IsSimilarTo(MigrationColumn column)
        {
            var result = base.IsSimilarTo(column);

            if (!result)
                return false;

            var sc = column as StringMigrationColumn;

            if (sc == null)
                return false;

            if (sc.Length != this.Length)
                return false;


            return true;
        }
    }

    public class DateTimeMigrationColumn : MigrationColumn
    {

        public DateTimeMigrationColumn()
        {
            
        }

        public DateTimeMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.DateTime, isPrimaryKey, isNullable)
        {
        }

        public DateTimeMigrationColumn(Column column)
            : base(column, MigrationColumnType.DateTime)
        {
        }

        public override string TypeAction()
        {
            return ".AsDateTime()";
        }
    }

    public class BoolMigrationColumn : MigrationColumn
    {
        public BoolMigrationColumn()
        {
            
        }

        public BoolMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Bool, isPrimaryKey, isNullable)
        {
        }

        public BoolMigrationColumn(Column column)
            : base(column, MigrationColumnType.Bool)
        {
        }

        public override string TypeAction()
        {
            return ".AsBoolean()";
        }
    }

    public class BlobMigrationColumn : MigrationColumn
    {
        public BlobMigrationColumn()
        {
            
        }


        public BlobMigrationColumn(string columnName, bool isPrimaryKey, bool isNullable)
            : base(columnName, MigrationColumnType.Blob, isPrimaryKey, isNullable)
        {
        }

        public BlobMigrationColumn(Column column)
            : base(column, MigrationColumnType.Blob)
        {
        }

        public override string TypeAction()
        {
            return ".AsBinary()";
        }
    }


}
