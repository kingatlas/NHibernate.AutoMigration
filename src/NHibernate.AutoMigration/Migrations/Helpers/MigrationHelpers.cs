
namespace NHibernate.AutoMigration
{
    public class MigrationHelpers
    {
        public static string ScriptForCreateTable(MigrationTable table)
        {
            var script = Script.New();

            script.AppendLine("Create.Table(\"{0}\")", table.Name);

            // columns
            foreach (var column in table.Columns)
            {
                script.AppendLine("      .WithColumn(\"{0}\"){1}", column.Name, column.ScriptActions());
            }

            script.AppendSemicolon();
            script.AppendEmptyLine();

            // foreign keys
            foreach (var column in table.ForeignKeyColumns)
            {
                script.AppendLine("Create{0}", column.ForeignKeyActions());
                script.AppendSemicolon();
            }


            return script.ToString();
        }

        public static string ScriptForDropTable(MigrationTable table)
        {
            var script = Script.New();

            script.Append("Drop.Table(\"{0}\")", table.Name);
            script.AppendSemicolon();

            return script.ToString();
        }

        public static string ScriptForCreateColumn(MigrationColumn column)
        {
            var script = Script.New();

            script.AppendLine("Create.Column(\"{0}\").OnTable(\"{1}\"){2}", column.Name, column.OwnerTable.Name, column.ScriptActions());
            script.AppendSemicolon();

            
            // if foreign key
            var foreignkeyColumn = column as ForeignKeyColumn;
            if (foreignkeyColumn != null)
            {
                script.AppendEmptyLine();
                script.Append(ScriptForCreateForeignKey(foreignkeyColumn));
            }


            return script.ToString();
        }

        public static string ScriptForCreateForeignKey(ForeignKeyColumn foreignkeyColumn)
        {
            var script = Script.New();

            script.AppendLine("Create{0}", foreignkeyColumn.ForeignKeyActions());
            script.AppendSemicolon();

            return script.ToString();
        }

        public static string ScriptForDeleteForeignKey(ForeignKeyColumn foreignkeyColumn)
        {
            var script = Script.New();

            script.AppendLine("Delete{0}", foreignkeyColumn.ForeignKeyActions());
            script.AppendSemicolon();

            return script.ToString();
        }

        public static string ScriptForDropColumn(MigrationColumn column)
        {
            var script = Script.New();

            // if foreign key
            var foreignkeyColumn = column as ForeignKeyColumn;
            if (foreignkeyColumn != null)
            {
                script.Append(ScriptForDeleteForeignKey(foreignkeyColumn));
                script.AppendEmptyLine();
            }

            script.AppendLine("Delete.Column(\"{0}\").FromTable(\"{1}\")", column.Name, column.OwnerTable.Name);
            script.AppendSemicolon();


            return script.ToString();
        }

        public static string ScriptForAlterColumn(MigrationColumn oldColumn, MigrationColumn newColumn)
        {
            var script = Script.New();

            var oldforeignkeyColumn = oldColumn as ForeignKeyColumn;
            var newforeignkeyColumn = newColumn as ForeignKeyColumn;
            if ((oldforeignkeyColumn != null && newforeignkeyColumn == null) // the column is no more a foreign key
               || (oldforeignkeyColumn != null && newforeignkeyColumn != null && oldforeignkeyColumn.ReferencedTableName != newforeignkeyColumn.ReferencedTableName))  // the foreign key has changed target
            
            {
                script.Append(ScriptForDeleteForeignKey(oldforeignkeyColumn));
                script.AppendEmptyLine();
            }

            script.AppendLine("// WARNING ! this column alteration might not work properly without specific data migration code");
            script.AppendLine("Alter.Column(\"{0}\").OnTable(\"{1}\"){2}", newColumn.Name, newColumn.OwnerTable.Name, newColumn.ScriptActions());
            script.AppendSemicolon();

            if ((oldforeignkeyColumn == null && newforeignkeyColumn != null) // the column has become a foreign key
                || (oldforeignkeyColumn != null && newforeignkeyColumn != null && oldforeignkeyColumn.ReferencedTableName != newforeignkeyColumn.ReferencedTableName))  // the foreign key has changed target
            
            {
                script.AppendEmptyLine();
                script.Append(ScriptForCreateForeignKey(newforeignkeyColumn));
                script.AppendEmptyLine();
            }

            return script.ToString();
        }
    }
}
