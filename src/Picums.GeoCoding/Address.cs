﻿using System.Collections.Generic;
using System.Linq;

namespace Picums.GeoCoding
{
    public sealed class Address
    {
        public Address(
                string houseNumber = ""
                , string streetName = ""
                , string district = ""
                , string city = ""
                , string postCode = ""
                , string province = ""
                , string country = "")
        {
            this.HouseNumber = houseNumber;
            this.StreetName = streetName;
            this.District = district;
            this.PostCode = postCode;
            this.City = city;
            this.Province = province;
            this.Country = country;
        }

        public string HouseNumber { get; }

        public string StreetName { get; }

        public string District { get; }

        public string City { get; }

        public string Province { get; }

        public string Country { get; }

        public string PostCode { get; }

        public override string ToString()
            => string.Join(" ", FilterEmptyValues());

        private IEnumerable<string> AsEnumerable()
                    => new[]
            {
                HouseNumber,
                StreetName,
                District,
                City,
                Province,
                Country,
                PostCode,
            };

        private IEnumerable<string> FilterEmptyValues()
            => this.AsEnumerable().Where(x => !string.IsNullOrEmpty(x));
    }
}