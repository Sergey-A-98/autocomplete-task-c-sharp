using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete;

internal class AutocompleteTask
{
	/// <returns>
	/// Возвращает первую фразу словаря, начинающуюся с prefix.
	/// </returns>
	/// <remarks>
	/// Эта функция уже реализована, она заработает, 
	/// как только вы выполните задачу в файле LeftBorderTask
	/// </remarks>
	public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
		var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;

		if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
		{
            return phrases[index];
        }
		else
		{
            return null;
        }
	}

	/// <returns>
	/// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
	/// элементов словаря, начинающихся с prefix.
	/// </returns>
	/// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
	public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
	{
		var phrasesCount = phrases.Count;
		var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrasesCount) + 1;

		if (leftBorder == phrasesCount)
		{
			return new string[0];
		}

		var actualCount = Math.Min(count, phrasesCount - leftBorder);

		var result = new List<string>();
		var nextPhraseIndex = 0;

		for (var i = 0; i < actualCount; i++)
		{
			nextPhraseIndex = leftBorder + i;

			if (!phrases[nextPhraseIndex].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
				break;

			result.Add(phrases[nextPhraseIndex]);
		}

		return result.ToArray();
	}

	/// <returns>
	/// Возвращает количество фраз, начинающихся с заданного префикса
	/// </returns>
	public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
		var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
		var right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
		var result = right - left - 1;

		return (result <= 0) ? 0 : result;
	}
}

[TestFixture]
public class AutocompleteTests
{
	[Test]
	public void TopByPrefix_IsEmpty_WhenNoPhrases_AndCountIsZero()
	{
		var phrases = new List<string>();
		var result = AutocompleteTask.GetTopByPrefix(phrases, "q", 0);
		CollectionAssert.IsEmpty(result);
	}

	public void TopByPrefix_IsEmpty_WhenNoPhrases_AndCountIsGreaterThenZero()
	{
        var phrases = new List<string>();
        var result = AutocompleteTask.GetTopByPrefix(phrases, "q", 0);
        CollectionAssert.IsEmpty(result);
    }

	public void TopByPrefix_IsEmpty_WhenPhrasesNotContainPrefix_AndCountIsZero()
	{
		var phrases = new List<string>() { "can", "cand", "candy" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "a", 0);
        CollectionAssert.IsEmpty(result);
    }

    public void TopByPrefix_IsEmpty_WhenPhrasesNotContainPrefix_AndCountIsGreaterThenZero()
    {
        var phrases = new List<string>() { "can", "cand", "candy" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "a", phrases.Count);
        CollectionAssert.IsEmpty(result);
    }

    public void TopByPrefix_IsEmpty_WhenPhrasesContainPrefix_AndCountIsZero()
    {
        var phrases = new List<string>() { "can", "cand", "candy" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "c", 0);
        CollectionAssert.IsEmpty(result);
    }

    public void TopByPrefix_IsCCount_WhenPhrasesContainPrefix_AndCountIsC()
    {
        var phrases = new List<string>() { "can", "candy", "candys", "candyshop" };
        var expected = new List<string>() { "can", "candy" };
        var c = expected.Count;
        var result = AutocompleteTask.GetTopByPrefix(phrases, "ca", c);
        Assert.AreEqual(c, result.Length);
        CollectionAssert.AreEqual(expected, result);
    }
    [Test]
	public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
	{
		var phrases = new List<string>() { "i", "love", "programming" };
		var totalCount = phrases.Count;
		var result = AutocompleteTask.GetCountByPrefix(phrases, "");
		Assert.AreEqual(totalCount, result);
	}

    public void CountByPrefix_IsTotalCount_WhenAllPhrasesContainPrefix()
    {
        var phrases = new List<string>() { "he", "hell", "hello", "help" };
        var totalCount = phrases.Count;
        var result = AutocompleteTask.GetCountByPrefix(phrases, "he");
        Assert.AreEqual(totalCount, result);
    }
}