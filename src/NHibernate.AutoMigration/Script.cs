using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AutoMigration
{
    public class Script 
    {
        private string Lines = "";

        public static Script New()
        {
            return new Script();
        }

        public void Append(string format, params object[] args)
        {
            Lines += string.Format(format, args);
        }

        public void AppendSemicolon()
        {
            if (!string.IsNullOrEmpty(Lines))
                Lines += ";";
        }

        public void AppendEmptyLine()
        {
                Lines += Environment.NewLine;
        }

        public void AppendLine(string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(Lines))
                Lines += Environment.NewLine;

            Append(string.Format(format, args));
        }

        public override string ToString()
        {
            return Lines;
        }
    }
}
