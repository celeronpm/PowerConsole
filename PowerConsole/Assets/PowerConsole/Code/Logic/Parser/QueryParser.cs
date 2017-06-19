﻿using ProceduralLevel.Common.Parsing;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryParser: AParser<List<Query>>
	{
		public QueryParser() : base(new QueryTokenizer())
		{
		}

		protected override void Reset()
		{
			base.Reset();
		}

		protected override List<Query> Parse()
		{
			List<Query> queries = new List<Query>();

			while(HasTokens())
			{
				Query query = ParseQuery();
				queries.Add(query);
			}


			return queries;
		}

		private Query ParseQuery()
		{
			Query query = new Query();

			bool quoted = false;
			QueryParam param = null;
			Token token = null;

			while(HasTokens())
			{
				token = ConsumeToken();
				switch(token.Value)
				{
					case ParserConst.SPACE:
						AssertNamedParamValue(param, token);
						param = null;
						break;
					case ParserConst.QUOTE:
						quoted = !quoted;
						break;
					case ParserConst.SEPARATOR:
						AssertNamedParamValue(param, token);
						return query;
					case ParserConst.ASSIGN:
						if(param == null)
						{
							throw new QueryParserException(EQueryError.NamedParam_NoName, token);
						}
						param.Name = param.Value;
						param.Value = null;
						break;
					default:
						if(param == null)
						{
							param = new QueryParam();
							query.Params.Add(param);
						}
						param.Value = token.Value;
						break;
				}
				query.RawQuery += token.Value;
			}

			AssertNamedParamValue(param, token);
			if(quoted)
			{
				throw new QueryParserException(EQueryError.Quote_Mismatch, token);
			}
			return query;
		}

		private void AssertNamedParamValue(QueryParam param, Token token)
		{
			if(param != null && param.Value == null)
			{
				throw new QueryParserException(EQueryError.NamedParam_NoValue, token);
			}
		}
	}
}
