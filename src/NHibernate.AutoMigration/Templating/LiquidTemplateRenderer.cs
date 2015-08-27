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
    public class LiquidTemplateRenderer : ITemplateRenderer
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
            template.Render(output, LiquidTemplateRenderer.BuildRenderParameters(model));

        }

        /// <summary>
        /// Renders the specified template conten used the given model
        /// </summary>
        /// <param name="templateContent">Content of the template.</param>
        /// <param name="model">The model.</param>
        /// <returns>generated content</returns>
        public string Render(string templateContent, object model)
        {
            using (var output = new MemoryStream())
            {
                this.Render(templateContent, model, output);
                output.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(output, true))
                {
                    return streamReader.ReadToEnd();
                }
            }
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
                LocalVariables = DotLiquid.Hash.FromAnonymousObject(model)
            };
        }
    }
}
