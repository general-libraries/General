using System;

namespace Common.Helpers
{
    /// <summary>
    /// Xml Transformation Exception.
    /// </summary>
    public class XmlTransformationException : Exception, IException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="XmlTransformationException"/>
        /// </summary>
        /// <param name="xmlString">XML in string format.</param>
        /// <param name="xsltPath">Physical path to XSLT file.</param>
        /// <param name="innerException">Inner exception, is exists.</param>
        public XmlTransformationException(string xmlString, string xsltPath, Exception innerException = null)
            : base(BuildErrorMessage(xmlString, xsltPath), innerException)
        { }

        /// <summary>
        /// Builds and returns exception message for an <see cref="XmlTransformationException"/> instance.
        /// </summary>
        /// <param name="xmlString">XML in string format.</param>
        /// <param name="xsltPath">Physical path to XSLT file.</param>
        /// <returns>Exception message for an <see cref="XmlTransformationException"/> instance.</returns>
        protected static string BuildErrorMessage(string xmlString, string xsltPath)
        {
            if (!string.IsNullOrEmpty(xmlString) && xmlString.Length > 100)
            {
                xmlString = xmlString.Substring(0, 100);
            }

            return string.Format(ExceptionMessages.XmlTransformationException, xmlString, xsltPath);
        }
    }
}
