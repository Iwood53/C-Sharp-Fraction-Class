using System;
using System.Collections.Generic;
using System.Linq;

namespace Fraction_Sandbox
{
	public class Fraction
	{
		// Either dont allow some properties to be changed or recalculate when they do
		public int wholeNumber { get; }
		public int numerator { get; }
		public int denominator { get; }
		public string fractionText { get; private set; }
		public decimal fractionDecimal { get; }

		public Fraction()
		{
			this.numerator = 1;
			this.denominator = 1;
			this.fractionText = "1";
			this.fractionDecimal = 1;
			this.wholeNumber = 0;
		}

		// add whole number processing with negative
		public Fraction(string fractionText)
		{
			// REMOVE THIS AND ADD REAL LOGIC TO CHECK IF POSITIVE
			char[] delimiters = new char[] { '/', '|', '\\', '-' };
			string[] nums = fractionText.Split(delimiters);

			// TryParse returns a bool so use that for integrity checks
			// numer and denom are there becase you cannot pass out directly to a property
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
				numer = 0;
				denom = 0;
			}

			this.numerator = numer;
			this.denominator = denom;
			this.fractionDecimal = Decimal.Divide(numerator, denominator);
			generateFractionText();
		}

		public Fraction(int numerator, int denominator)
		{
			this.wholeNumber = 0;
			this.numerator = numerator;
			this.denominator = denominator;
			this.fractionDecimal = Decimal.Divide(numerator, denominator);
			generateFractionText();
		}

		public Fraction(int wholeNumber, int numerator, int denominator)
		{
			this.wholeNumber = wholeNumber;
			this.numerator = numerator;
			this.denominator = denominator;
			this.fractionDecimal = Decimal.Divide(numerator, denominator);
			generateFractionText();
		}

		public override string ToString() { return this.fractionText; }
		public decimal ToDecimal() { return this.fractionDecimal; }

		void generateFractionText()
		{
			string text;

			if (this.numerator == this.denominator) { text = "1"; }

			if (wholeNumber == 0)
			{
				text = numerator.ToString() + "/" + denominator.ToString();
			}
			else
			{
				text = wholeNumber.ToString() + "~" + numerator.ToString() + "/" + denominator.ToString();
			}
		
			this.fractionText = text;
		}

		public static Fraction reduce(Fraction myFraction)
		{
			List<int> numeratorFactors = new List<int>();
			List<int> denominatorFactors = new List<int>();
			int numeratorMax = (int)Math.Sqrt(myFraction.numerator);
			int denominatorMax = (int)Math.Sqrt(myFraction.denominator);

			for (int factor = 1; factor <= numeratorMax; ++factor)
			{
				if (myFraction.numerator % factor == 0)
				{
					numeratorFactors.Add(myFraction.numerator / factor);
				}
			}

			for (int factor = 1; factor <= denominatorMax; ++factor)
			{
				if (myFraction.denominator % factor == 0)
				{
					denominatorFactors.Add(myFraction.denominator / factor);
				}
			}

			var commonFactors = numeratorFactors.Intersect(denominatorFactors);
			if (commonFactors.Count() == 0)
			{
				return myFraction;
			}
			else
			{
				int highestCommonFactor = commonFactors.Max();
				return new Fraction(myFraction.numerator / highestCommonFactor, myFraction.denominator / highestCommonFactor);
			}
		}


		public static DetailClosestFraction findClosestFraction(Fraction precision, Decimal targetDecimal)
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
