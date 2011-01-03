﻿namespace jQueryTmpl.Templates
{
    public class NestedTemplate : Template
    {
        public Expression Data { get; set; }
        public Expression Options { get; set; }
        public Template Template { get; set; }

        public override string Render(TemplateItem item)
        {
            return Template.Render(new TemplateItem {
                Parent = item,
                Data = new ExpressionEvaluator(item).Evaluate(Data).Value,
                Options = new ExpressionEvaluator(item).Evaluate(Options).Value,
            });
        }
    }
}