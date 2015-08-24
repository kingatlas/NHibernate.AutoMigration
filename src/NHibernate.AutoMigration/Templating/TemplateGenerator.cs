using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AutoMigration.Templating
{
    /// <summary>
    /// Templating class
    /// </summary>
    public class TemplateGenerator
    {
        /// <summary>
        /// Renders the specified template content.
        /// </summary>
        /// <param name="templateContent">Content of the template.</param>
        /// <param name="model">The model.</param>
        /// <param name="output">The output stream</param>
        public void Render(string templateContent, object model, Stream output)
        {
            var template = Template.Parse(templateContent);
            template.Render(output, TemplateGenerator.BuildRenderParameters(model));

        }

        /// <summary>
        /// Renders the specified template conten used the given model
        /// </summary>
        /// <param name="templateContent">Content of the template.</param>
        /// <param name="model">The model.</param>
        /// <returns>generated content</returns>
        public string Render(string templateContent, object model)
        {
            var template = Template.Parse(templateContent);
            return template.Render(TemplateGenerator.BuildRenderParameters(model));
        }

        /// <summary>
        /// Builds the render parameters.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>render parameters</returns>
        private static RenderParameters BuildRenderParameters(object model)
        {
            return new RenderParameters
            {
                LocalVariables = DotLiquid.Hash.FromAnonymousObject(new { model = model })
            };
        }
    }
}
