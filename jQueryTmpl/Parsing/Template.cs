﻿using System;
using System.Collections.Generic;
using System.Linq;

using jQueryTmpl.Rendering;
using jQueryTmpl.Tags;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class Template
    {
        public string Value { get; private set; }
        public IList<Template> Children { get; private set; }
        public ITagDescriptor Descriptor { get; private set; }

        public Template()
        {
            Value = String.Empty;
            Descriptor = new LiteralTagDescriptor();
            Children = new List<Template>();
        }

        public Template(Token token)
        {
            Value = token.Value;
            Descriptor = token.Descriptor;
            Children = new List<Template>();
        }

        public void AddChild(Template template)
        {
            Children.Add(template);
            Value += template.Value;
        }

        public void Close(Token token)
        {
            Value += token.Value;
        }

        public virtual string Render(object item)
        {
            return Render(Children, item);
        }

        protected string GetDataProperty()
        {
            return Descriptor.Pattern.Match(Value).Groups["data"].Value;
        }

        protected object GetDataValue(object item)
        {
            var propertyName = GetDataProperty();
            object o = item;
            PropertyInfo property = null;
            Type type = item.GetType();	
            String[] parts = propertyName.Split(new char[]{'.'});

            foreach (String part in parts) {

                if (property != null) {
								    o = property.GetValue(o, null);
                }
                
                property = type.GetProperty(part);

                if (property == null)
                    throw new TemplateRenderingException("The provided data object does not have a property '" + propertyName + "'.");
							
                type = property.PropertyType;
            }

            return property.GetValue(o, null);
        }

        protected string Render(IEnumerable<Template> templates, object item)
        {
            return String.Join(String.Empty, templates.Select(t => t.Render(item)));
        }

        protected string Render(IEnumerable<Template> templates, IEnumerable<object> items)
        {
            return String.Join(String.Empty, items.Select(item => Render(templates, item)));
        }
    }
}