using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;

namespace A.B.C
{
    [Attr1]
    public class C1 : C0 
    {
        public override void Up()
        {
            var a="up";
        }

        public override void Down()
        {
            var a="down";
        }
    }
}