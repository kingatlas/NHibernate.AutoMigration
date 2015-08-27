using System;
using NUnit.Framework;
using NHibernate.AutoMigration.Templating;
using System.IO;
using System.Reflection;
using DotLiquid;

namespace NHibernate.AutoMigration.Test.Templating
{
    [TestFixture]
    public class LiquidTemplateRendererTest
    {
        /// <summary>
        /// Template model calss
        /// </summary>
        public class Sample1Model 
        {
            public bool IsInherited { get; set; }

            public string ClassAttribute { get; set; }

            public string ClassName { get; set; }

            public string BaseClassName { get; set; }

            public string UpCode { get; set; }

            public string DownCode { get; set; }

            public string Namespace { get; set; }
        }

        private readonly Sample1Model sample1Model;

        public LiquidTemplateRendererTest()
        {
            this.sample1Model = new Sample1Model()
            {
                IsInherited = true,
                ClassAttribute = "[Attr1]",
                ClassName = "C1",
                BaseClassName = "C0",
                UpCode = @"var a=""up"";",
                DownCode = @"var a=""down"";",
                Namespace = "A.B.C"
            };
        }

        /// <summary>
        /// Test on LiquidTemplateRenderer Render to string method 
        /// </summary>
        [Test]
        public void MigrationFileRenderStringTest()
        {
            var liquidTemplateRenderer = new LiquidTemplateRenderer();
            

            var templateContent = NHibernate.AutoMigration.Templating.Templates.FluentMigrator.Resources.MigrationFile;
            var expectedContent = Templating.Templates.FluentMigrator.Resources.MigrationFile1;

            var content = liquidTemplateRenderer.Render(templateContent, sample1Model);

            Assert.AreEqual(expectedContent, content);

        }

        /// <summary>
        /// Test on LiquidTemplateRenderer Render to stream method 
        /// </summary>
        [Test]
        public void MigrationFileRenderStreamTest()
        {
            var liquidTemplateRenderer = new LiquidTemplateRenderer();

            var templateContent = NHibernate.AutoMigration.Templating.Templates.FluentMigrator.Resources.MigrationFile;
            var expectedContent = Templating.Templates.FluentMigrator.Resources.MigrationFile1;
            string content = null;

            using (var output = new MemoryStream())
            {
                liquidTemplateRenderer.Render(templateContent, sample1Model);
                output.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(output, true))
                {
                    content =  streamReader.ReadToEnd();
                }
            }

            content = liquidTemplateRenderer.Render(templateContent, sample1Model);

            Assert.AreEqual(expectedContent, content);

        }
    }
}
