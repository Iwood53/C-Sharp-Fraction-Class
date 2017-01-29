using System;
using System.Collections.Generic;
using System.Linq;

namespace Fraction_Class
{
	public class Fraction
	{
		public int integer { get; private set; }
		public int numerator { get; private set; }
		public int denominator { get; private set; }
		public string fractionText { get; private set; }
		public decimal fractionDecimal { get; private set; }

		public override string ToString() { return this.fractionText; }
		public decimal ToDecimal() { return this.fractionDecimal; }

		public Fraction()
		{
			this.numerator = 1;
			this.denominator = 1;
			this.fractionText = "1";
			this.fractionDecimal = 1;
			this.integer = 0;
			GenerateFractionText();
		}

		/*
		public Fraction(string fractionText)
		{
			char[] delimiters = new char[] { '/', '|', '\\', '-' };
			string[] nums = fractionText.Split(delimiters);

			// TryParse returns a bool so use that for integrity checks
			int wholeNum, numer, denom;

			if (nums.Count() == 2)
			{
				wholeNum = 0;
				int.TryParse(nums[0], out numer);
				int.TryParse(nums[1], out denom);
			}

			else if (nums.Count() == 3)
			{
				int.TryParse(nums[0], out wholeNum);
				int.TryParse(nums[1], out numer);
				int.TryParse(nums[2], out denom);
			}
			else
			{
				wholeNum = 0;
				numer = 1;
				denom = 1;
			}

			if (denom == 0) throw new ArgumentException("Denominator cannot be 0");
			else
			{
				this.wholeNumber = wholeNum;
				this.numerator = numer;
				this.denominator = denom;
				this.fractionDecimal = Decimal.Divide(numerator, denominator);
				generateFractionText();
			}
		}
		*/

		public Fraction(int numerator, int denominator)
		{
			if (denominator == 0) throw new ArgumentException("Denominator cannot be 0");
			else
			{
				this.integer = 0;
				this.numerator = numerator;
				this.denominator = denominator;
				this.fractionDecimal = Decimal.Divide(numerator, denominator);
				GenerateFractionText();
			}
		}

		public Fraction(int wholeNumber, int numerator, int denominator)
		{
			if (denominator == 0) throw new ArgumentException("Denominator cannot be 0");
			else
			{
				this.integer = wholeNumber;
				this.numerator = numerator;
				this.denominator = denominator;
				this.fractionDecimal = Decimal.Divide(numerator, denominator);
				GenerateFractionText();
			}
		}

		private void GenerateFractionText()
		{
			string text;

			if (this.numerator == this.denominator) { text = "1"; }

			if (integer == 0)
			{
				text = numerator.ToString() + "/" + denominator.ToString();
			}
			else
			{
				if (this.numerator == 0) text = integer.ToString();
				else text = integer.ToString() + "~" + numerator.ToString() + "/" + denominator.ToString();
			}

			this.fractionText = text;
		}

		public static Fraction Add(Fraction firstFraction, Fraction secondFraction)
		{
			List<Fraction> myFractions = new List<Fraction>() { firstFraction, secondFraction };
			return Add(myFractions);

		}

		public static Fraction Add(List<Fraction> fractionList)
		{
			List<Fraction> lcdFractionList = Fraction.FindLeastCommonDenominator(fractionList);

			int? newNumerator = null;
			foreach (Fraction currentFraction in lcdFractionList)
			{
				Fraction cleanFraction = Fraction.RemoveInteger(currentFraction);
				if (newNumerator == null) newNumerator = cleanFraction.numerator;
				else newNumerator += cleanFraction.numerator;
			}

			return Fraction.Reduce(new Fraction(Convert.ToInt32(newNumerator), lcdFractionList[0].denominator));
		}

		public static Fraction Subtract(Fraction firstFraction, Fraction secondFraction)
		{
			List<Fraction> myFractions = new List<Fraction>() { firstFraction, secondFraction };
			return Subtract(myFractions);

		}

		public static Fraction Subtract(List<Fraction> fractionList)
		{
			List<Fraction> lcdFractionList = Fraction.FindLeastCommonDenominator(fractionList);

			int? newNumerator = null;
			foreach (Fraction currentFraction in lcdFractionList)
			{
				Fraction cleanFraction = Fraction.RemoveInteger(currentFraction);
				if (newNumerator == null) newNumerator = cleanFraction.numerator;
				else newNumerator -= cleanFraction.numerator;
			}

			return Fraction.Reduce(new Fraction(Convert.ToInt32(newNumerator), lcdFractionList[0].denominator));
		}

		public static Fraction Multiply(Fraction firstFraction, Fraction secondFraction)
		{
			List<Fraction> myFractions = new List<Fraction>() { firstFraction, secondFraction };
			return Multiply(myFractions);
		}

		public static Fraction Multiply(List<Fraction> fractionList)
		{
			int newNumerator = 1;
			int newDenominator = 1;
			foreach (Fraction currentFraction in fractionList)
			{
				Fraction cleanFraction = Fraction.RemoveInteger(currentFraction);
				newNumerator *= cleanFraction.numerator;
				newDenominator *= cleanFraction.denominator;	                                 
			}

			return Fraction.Reduce(new Fraction(newNumerator, newDenominator));				
		}

		public static Fraction Divide(Fraction firstFraction, Fraction secondFraction)
		{
			List<Fraction> myFractions = new List<Fraction>() { firstFraction, secondFraction };
			return Divide(myFractions);
		}

		public static Fraction Divide(List<Fraction> fractionList)
		{
			int? newNumerator = null;
			int? newDenominator = null;

			foreach (Fraction currentFraction in fractionList)
			{
				Fraction cleanFraction = Fraction.RemoveInteger(currentFraction);

				if (newNumerator == null)
				{
					newNumerator = cleanFraction.numerator;
					newDenominator = cleanFraction.denominator;
				}
				else
				{
					newNumerator *= cleanFraction.denominator;
					newDenominator *= cleanFraction.numerator;
				}
			}

			return Fraction.Reduce(new Fraction(Convert.ToInt32(newNumerator), Convert.ToInt32(newDenominator)));
		}

		public static List<Fraction> FindLeastCommonDenominator(List<Fraction> fractionList)
		{
			List<Fraction> finishedList = new List<Fraction>();
			List<List<int>> primeFactorList = new List<List<int>>();

			foreach (Fraction currentFraction in fractionList)
			{
				Fraction finalFraction;
				if (currentFraction.integer != 0)
				{
					finalFraction = new Fraction(currentFraction.denominator * currentFraction.integer
												   + currentFraction.numerator, currentFraction.denominator);
				}
				else finalFraction = currentFraction;

				primeFactorList.Add(Fraction.FindFactors(finalFraction.denominator));
			}

			int leastCommonDenominator = 1;
			foreach (List<int> currentFraction1 in primeFactorList)
			{
				foreach (int x in currentFraction1)
				{
					leastCommonDenominator *= x;
				}
			}

			foreach (Fraction startFraction in fractionList)
			{
				int newNumerator = ((leastCommonDenominator / startFraction.denominator) * startFraction.numerator);
				finishedList.Add(new Fraction(newNumerator, leastCommonDenominator));
			}

			return finishedList;
		}

		private static List<int> FindFactors(int num)
		{
			List<int> result = new List<int>();

			while (num % 2 == 0)
			{
				result.Add(2);
				num /= 2;
			}

			int factor = 3;
			while (factor * factor <= num)
			{
				if (num % factor == 0)
				{
					result.Add(factor);
					num /= factor;
				}
				else
				{
					factor += 2;
				}
			}

			if (num > 1) result.Add(num);

			return result;
		}

		public static Fraction RemoveInteger(Fraction myFraction)
		{
			if (myFraction.integer != 0)
			{
				int newNumerator = myFraction.denominator * myFraction.integer + myFraction.numerator;
				return new Fraction(newNumerator, myFraction.denominator);
			}
			else return myFraction;
		}

		public static Fraction Reduce(Fraction myFraction)
		{
			List<int> numeratorFactors = new List<int>();
			List<int> denominatorFactors = new List<int>();
			int numeratorMax = (int)Math.Sqrt(myFraction.numerator);
			int denominatorMax = (int)Math.Sqrt(myFraction.denominator);

			if (myFraction.numerator > myFraction.denominator)
			{
				int wholeNum = myFraction.numerator / myFraction.denominator;
				int newNumerator = myFraction.numerator % myFraction.denominator;
				myFraction = new Fraction(wholeNum, newNumerator, myFraction.denominator);
			}

			for (int factor = 1; factor <= numeratorMax; factor++)
			{
				if (myFraction.numerator % factor == 0)
				{
					numeratorFactors.Add(myFraction.numerator / factor);
				}
			}

			for (int factor = 1; factor <= denominatorMax; factor++)
			{
				if (myFraction.denominator % factor == 0)
				{
					denominatorFactors.Add(myFraction.denominator / factor);
				}
			}

			var commonFactors = numeratorFactors.Intersect(denominatorFactors);
			Fraction tempFrac;
			if (commonFactors.Count() == 0)
			{
				tempFrac = myFraction;
			}
			else
			{
				int highestCommonFactor = commonFactors.Max();
				tempFrac = new Fraction(myFraction.numerator / highestCommonFactor, myFraction.denominator / highestCommonFactor);
			}

			if (tempFrac.numerator == 2 && tempFrac.denominator % 2 == 0)
			{
				return new Fraction(1, tempFrac.denominator / 2);
			}
			else return tempFrac;
		}


		public static DetailClosestFraction FindClosestFraction(Fraction precision, Decimal targetDecimal)
		{
			// Creates obects for the Fraction above and below target decimal
			// Creates placeholder for final closest fraction
			Fraction topFraction = new Fraction();
			Fraction bottomFraction = new Fraction();
			for (int i = 1; i <= precision.denominator; i++)
			{
				// starting at 1/targetprecison loops until it finds fraction with decimal above target decimal
				// assings top and bottom fraction
				if (Decimal.Divide(i, precision.denominator) > targetDecimal)
				{
					topFraction = new Fraction(i, precision.denominator);
					bottomFraction = new Fraction(i - 1, precision.denominator);
					break;
				}
			}

			DetailClosestFraction closestFraction = new DetailClosestFraction();

			closestFraction.aboveFraction = topFraction;
			closestFraction.belowFraction = bottomFraction;

			// calculates distances to and from target decimal
			closestFraction.differenceFromBelowFraction = Convert.ToDecimal(Math.Abs(targetDecimal - bottomFraction.fractionDecimal));
			closestFraction.differenceFromaboveFraction = Convert.ToDecimal(Math.Abs(targetDecimal - topFraction.fractionDecimal));

			//finds closest fraction
			if (closestFraction.differenceFromBelowFraction > closestFraction.differenceFromaboveFraction)
			{
				closestFraction.closestFraction = topFraction;
				closestFraction.aboveTargetDecimal = true;
			}
			else if (closestFraction.differenceFromBelowFraction < closestFraction.differenceFromaboveFraction)
			{
				closestFraction.closestFraction = bottomFraction;
				closestFraction.aboveTargetDecimal = false;
			}
			else
			{
				closestFraction.closestFraction = topFraction;
				closestFraction.aboveTargetDecimal = true;
			}

			return closestFraction;
		}
	}


	public struct DetailClosestFraction
	{
		public Fraction closestFraction;
		public Fraction aboveFraction;
		public Fraction belowFraction;
		public Decimal differenceFromBelowFraction;
		public Decimal differenceFromaboveFraction;
		public bool aboveTargetDecimal;
	}
}
