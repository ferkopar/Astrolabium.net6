using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;

namespace Astrolabium.Models
{


    [XmlRoot(ElementName = "RepoText")]
    public class RepoText
    {
        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; }

        [XmlAttribute(AttributeName = "language")]
        public string Language { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ElementText")]
    public class ElementText
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "TextId")]
        public string TextId { get; set; }

        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }

        [XmlElement(ElementName = "RepoText")]
        public List<RepoText> RepoText { get; set; }

        [XmlElement(ElementName = "AwId")]
        public string AwId { get; set; }
    }

    [XmlRoot(ElementName = "AstrolabiumRepository")]
    public class AstrolabiumRepository
    {
        [XmlElement(ElementName = "ElementText")]
        public List<ElementText> ElementText { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }
}


