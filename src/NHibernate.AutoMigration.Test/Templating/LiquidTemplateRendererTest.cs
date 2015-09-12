using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.AutoMigration.Templating;
using System.IO;
using System.Reflection;

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
            public IEnumerable<string> Migrations { get; set; }

            public string Namespace { get; set; }
        }

        private readonly Sample1Model sample1Model;

        public LiquidTemplateRendererTest()
        {
            this.sample1Model = new Sample1Model()
            {
                Migrations = new [] { "//migration 3", "//migration 2", "//migration 1" },
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
            

            var templateContent = Templating.Templates.FluentMigrator.Resources.Template1;
            var expectedContent = Templating.Templates.FluentMigrator.Resources.Sample1;

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

            var templateContent = Templating.Templates.FluentMigrator.Resources.Template1;
            var expectedContent = Templating.Templates.FluentMigrator.Resources.Sample1;
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
