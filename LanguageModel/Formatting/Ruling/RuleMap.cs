﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageService.Formatting.Ruling
{

    internal class RuleMap
    {
        internal RuleBucket[,] map;

        internal RuleMap()
        {
            if (map != null)
            {
                map = null;
            }

            int length = Enum.GetNames(typeof(SyntaxKind)).Length;

            this.map = new RuleBucket[length, length];

        }

        internal void AddRule(Rule rule)
        {
            foreach (SyntaxKind typeLeft in rule.RuleDescriptor.TokenRangeLeft)
            {
                foreach (SyntaxKind typeRight in rule.RuleDescriptor.TokenRangeRight)
                {
                    int column = (int)typeLeft;
                    int row = (int)typeRight;

                    RuleBucket bucket = this.map[column, row];
                    if (bucket == null)
                    {
                        bucket = new RuleBucket();
                    }

                    bucket.Add(rule);
                    this.map[column, row] = bucket;

                }
            }
        }

        internal Rule GetRule(FormattingContext formattingContext)
        {
            int column = (int)formattingContext.CurrentToken.Token.Kind;
            int row = (int)formattingContext.NextToken.Token.Kind;

            RuleBucket ruleBucket = this.map[column,row];
            
            if (ruleBucket != null)
            {
                return ruleBucket.Get(formattingContext);
            }

            return null;

        }


    }
}
