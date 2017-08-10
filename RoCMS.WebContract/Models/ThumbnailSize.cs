using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    public struct ThumbnailSize
    {
        public ThumbnailSize(int pixels, ImageSide side)
        {
            Pixels = pixels;
            Side = side;
        }

        private const string REGEX_PATTERN = @"^(\d+)(w|h)$";

        public ThumbnailSize(string sizeString)
        {
            var regex = Regex.Match(sizeString, REGEX_PATTERN);
            if (!regex.Success)
                throw new FormatException();
            Pixels = int.Parse(regex.Groups[1].Value);
            Side = regex.Groups[2].Value.Equals("w") ? ImageSide.Width : ImageSide.Height;
        }

        public int Pixels;
        public ImageSide Side; 

        public string SizeString => $"{Pixels}{(Side == ImageSide.Width ? "w" : "h")}";

        public override string ToString()
        {
            return SizeString;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ThumbnailSize other)
        {
            return Pixels == other.Pixels && Side == other.Side;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Pixels * 397) ^ (int) Side;
            }
        }

        public static bool operator ==(ThumbnailSize left, ThumbnailSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ThumbnailSize left, ThumbnailSize right)
        {
            return !left.Equals(right);
        }
    }
}
