using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Common.Helpers
{
    /// <summary>
    /// Encapsulates functions for helping work with XML data.
    /// </summary>
    public static partial class XmlHelper
    {
        /// <summary>
        /// Transforms and returns an XML (<paramref name="xmlString"/>) by using an XSLT file from <paramref name="xsltPath"/>.
        /// </summary>
        /// <returns>Transformed XML string.</returns>
        /// <remarks>
        /// For more information about XML Transformation, read:
        ///   - http://www.w3.org/TR/xslt
        ///   - http://www.w3schools.com/xsl
        ///   - http://msdn.microsoft.com/en-us/library/ms256069(v=vs.100).aspx
        ///   - http://forums.asp.net/t/1256636.aspx?Generating+HTML+Reports+in+C+with+XML+Data+
        /// </remarks>
        public static string Transform(string xmlString, string xsltPath)
        {
            string result = null;

            if (string.IsNullOrEmpty(xmlString))
            {
                throw new ArgumentNullException(nameof(xmlString));
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(xsltPath);

                    try
                    {
                        //LogManager.Log(LogLevel.Debug, message: string.Format("Trying to transform an XML String by using {0}. XML string is: {1}", xsltPath, xmlString));

                        using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString)))
                        {
                            transform.Transform(xmlReader, writer);
                        }
                    }
                    catch (Exception exception)
                    {
                        //LogManager.Log(LogLevel.Error, exception);
                        throw new XmlTransformationException(xmlString, xsltPath, exception);
                    }
                }

                result = stringWriter.ToString();
                //LogManager.Log(LogLevel.Debug, message: string.Format("XML transformation result: {0}", result));
            }

            return result;
        }
    }
}
