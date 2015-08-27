using System.IO;

namespace NHibernate.AutoMigration.Templating
{
    public interface ITemplateRenderer
    {
        string Render(string templateContent, object model);
        void Render(string templateContent, object model, Stream output);
    }
}