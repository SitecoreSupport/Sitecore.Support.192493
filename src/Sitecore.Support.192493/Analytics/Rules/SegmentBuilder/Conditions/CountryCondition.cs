using System;
using System.Linq.Expressions;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.Rules.Conditions;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Sitecore.Support.Analytics.Rules.SegmentBuilder.Conditions
{
  public class CountryCondition<T> : TypedQueryableStringOperatorCondition<T, IndexedContact> where T : VisitorRuleContext<IndexedContact>
  {
    protected override Expression<Func<IndexedContact, bool>> GetResultPredicate(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      //var contextItem = ruleContext.Item;
      //if (contextItem == null)
      //{
      //  Log.Warn("Country condition ran without context item.", this);
      //  return c => false;
      //}

      var value = this.Value;
      if (string.IsNullOrEmpty(value))
      {
        return c => false;
      }

      var item = Database.GetDatabase("master").GetItem(value);
      if (item == null)
      {
        Log.Warn("Country not found in the item database: " + value, this);
        return c => false;
      }

      value = item["Country Code"];

      return this.GetCompareExpression(c => c["contact.preferredaddress.country"], value);
    }
  }
}